using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using IE = Interop.SHDocVw;
using AddinExpress.IE;
 
namespace MyIEAddon2
{
    /// <summary>
    /// Add-in Express for Internet Explorer Module
    /// </summary>
    [ComVisible(true), Guid("51E0A956-A987-418F-B3FA-DF05DEBBDC9A")]
    public class IEModule : AddinExpress.IE.ADXIEModule
    {
        public IEModule()
        {
            InitializeComponent();
            //Please write any initialization code in the OnConnect event handler
        }
 
        public IEModule(IContainer container)
        {
            container.Add(this);
 
            InitializeComponent();
            //Please write any initialization code in the OnConnect event handler
        }
 
        #region Component Designer generated code
        /// <summary>
        /// Required by designer
        /// </summary>
        private System.ComponentModel.IContainer components;
 
        /// <summary>
        /// Required by designer support - do not modify
        /// the following method
        /// </summary>
        private void InitializeComponent()
        {
            //
            // IEModule
            //
            this.HandleShortcuts = true;
            this.LoadInMainProcess = false;
            this.ModuleName = "MyIEAddon2";
        }
        #endregion
 
        #region ADX automatic code
 
        // Required by Add-in Express - do not modify
        // the methods within this region
 
        public override System.ComponentModel.IContainer GetContainer()
        {
            if (components == null)
                components = new System.ComponentModel.Container();
            return components;
        }
 
        [ComRegisterFunctionAttribute]
        public static void RegisterIEModule(Type t)
        {
            AddinExpress.IE.ADXIEModule.RegisterIEModuleInternal(t);
        }
 
        [ComUnregisterFunctionAttribute]
        public static void UnregisterIEModule(Type t)
        {
            AddinExpress.IE.ADXIEModule.UnregisterIEModuleInternal(t);
        }
 
        [ComVisible(true)]
        public class IECustomContextMenuCommands :
            AddinExpress.IE.ADXIEModule.ADXIEContextMenuCommandDispatcher
        {
        }
 
        [ComVisible(true)]
        public class IECustomCommands :
            AddinExpress.IE.ADXIEModule.ADXIECommandDispatcher
        {
        }
 
        #endregion
 
        public IE.WebBrowser IEApp
        {
            get
            {
                return (this.IEObj as IE.WebBrowser);
            }
        }
 
        public mshtml.HTMLDocument HTMLDocument
        {
            get
            {
                return (this.HTMLDocumentObj as mshtml.HTMLDocument);
            }
        }

 
    }
}

