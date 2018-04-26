using System;
using System.Runtime.InteropServices;
using SHDocVw;
using mshtml;

namespace ieplugin1
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


    [
         ComVisible(true),
         Guid("1056BA25-DA81-56E3-A671B-D38A9B1B2142"),
         ClassInterface(ClassInterfaceType.None)
     ]
    public class BHO : IObjectWithSite
    {
        private WebBrowser webBrowser;

        public int SetSite(object site)
        {
            if (site != null)
            {
                webBrowser = (WebBrowser)site;
                webBrowser.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
            }
            else
            {
                webBrowser.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
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

        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            HTMLDocument document = (HTMLDocument)webBrowser.Document;


        }
    }
    public void OnDocumentComplete(object pDisp, ref object URL)
    {
        HTMLDocument document = (HTMLDocument)webBrowser.Document;

        IHTMLElement head = (IHTMLElement)((IHTMLElementCollection)document.all.tags("head")).item(null, 0);
        IHTMLScriptElement scriptObject = (IHTMLScriptElement)document.createElement("script");
        scriptObject.type = @"text/javascript";
        scriptObject.text = "function hidediv(){document.getElementById('myOwnUniqueId12345').style.visibility = 'hidden';}";
        ((HTMLHeadElement)head).appendChild((IHTMLDOMNode)scriptObject);

        string div = "<div id=\"myOwnUniqueId12345\" style=\"position:fixed;bottom:0px;right:0px;z-index:9999;width=300px;height=150px;\">" +
            "<div style=\"position:relative;float:right;font-size:9px;\"><a href=\"javascript:hidediv();\">close</a></div>" +
            "My content goes here ...</div>";

        document.body.insertAdjacentHTML("afterBegin", div);
    }

    public void OnDocumentComplete(object pDisp, ref object URL)
    {
        if (URL.ToString().Contains("www.google.com"))
        {
            // Show div in here ...
        }
    }

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
}
