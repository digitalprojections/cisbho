using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SHDocVw;
using mshtml;
using Microsoft.Win32;

namespace IEPlugin
{
    [ComVisible(true),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("D454ADE9-A3FC-4ae6-B9E9-55D1467C22EE")]
    public interface IObjectWithSite
    {
        [PreserveSig]
        int SetSite([In, MarshalAs(UnmanagedType.IUnknown)]object site);

        [PreserveSig]
        int GetSite(ref Guid guid, out IntPtr ppvSite);
    }


    [ComVisible(true),
    ClassInterface(ClassInterfaceType.None),
    Guid("788EA1C3-124A-4d7a-B693-20B3ED8BFD5D")]
    public class BHO : IObjectWithSite
    {

        #region ComREgister/UnRegister Code

        public const string BHO_REGISTRY_KEY_NAME = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHO_REGISTRY_KEY_NAME, true);

            if (registryKey == null)
                registryKey = Registry.LocalMachine.CreateSubKey(BHO_REGISTRY_KEY_NAME);

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
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHO_REGISTRY_KEY_NAME, true);
            string guid = type.GUID.ToString("B");

            if (registryKey != null)
                registryKey.DeleteSubKey(guid, false);
        }

        #endregion



        private WebBrowser oWebbrowser;

        #region IObjectWithSite Members

        public int SetSite(object site)
        {
            if (site != null)
            {
                oWebbrowser = (WebBrowser)site;
                oWebbrowser.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
            }
            else
            {
                oWebbrowser.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
                oWebbrowser = null;
            }

            return 0;
        }

        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject(oWebbrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);
            return hr;
        }

        #endregion

        #region Eventhandlers

        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            //Implement logic to insert Javascript and div elements to the browser

            HTMLDocument oDocument = (HTMLDocument)oWebbrowser.Document;

            IHTMLElement oHead = (IHTMLElement)IHTMLElementCollection)oDocument.all.tags("head")).item(null, 0);

            IHTMLScriptElement scriptObject = (IHTMLScriptElement)oDocument.createElement("script");
            scriptObject.type = @"text/javascript";
            scriptObject.text = "function hidediv(){document.getElementById('mydiv').style.visibility = 'hidden';}";
            ((HTMLHeadElement)oHead).appendChild((IHTMLDOMNode)scriptObject);


            IHTMLElement body = (IHTMLElement)oDocument.body;
            body.insertAdjacentHTML("afterBegin", "&lt;div id=\"mydiv\" style=\"position:absolute;z-index:2000000;top:50%;left:50%;width:300px;height:300px;margin-top:-150px;margin-left:-150px;background:#000;\"/>Hello World
& lt; a href =\"javascript:hidediv();\">close&lt;/a>&lt;/div>");
        }

        #endregion
    }
}