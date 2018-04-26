using Microsoft.WindowsMediaServices.Interop;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public class CSEventPlugin : IWMSBasicPlugin,
                             IWMSEventNotificationPlugin
{
    [Guid("E1171A34-1F13-46d9-AA8A-63F47E92207C")]
    public class CSEventPlugin : IWMSBasicPlugin, IWMSEventNotificationPlugin
    {
        void IWMSBasicPlugin.InitializePlugin(IWMSContext ServerContext,
                                      WMSNamedValues NamedValues,
                                      IWMSClassObject ClassFactory)
        {
        }

        void IWMSBasicPlugin.ShutdownPlugin()
        {
        }

        void IWMSBasicPlugin.EnablePlugin(ref int lFlags,
                                          ref int lHeartbeatPeriod)
        {
        }

        void IWMSBasicPlugin.DisablePlugin()
        {
        }

        object IWMSBasicPlugin.GetCustomAdminInterface()
        {
            return 0;
        }

        void IWMSBasicPlugin.OnHeartbeat()
        {
        }
        // Overrides the IWMSEventNotificationPlugin.OnEvent method. This 
        // implementation displays a message box when a client either
        // connects or disconnects.
        public void OnEvent(
                            ref WMS_EVENT Event,
                            IWMSContext UserCtx,
                            IWMSContext PresentationCtx,
                            IWMSCommandContext CommandCtx)
        {
            try
            {
                switch (Event.Type)
                {
                    case WMS_EVENT_TYPE.WMS_EVENT_CONNECT:
                        MessageBox.Show("Client connected",
                                        "Event Plug-in",
                                        MessageBoxButtons.OK);
                        break;

                    case WMS_EVENT_TYPE.WMS_EVENT_DISCONNECT:
                        MessageBox.Show("Client disconnected",
                                        "Event Plug-in",
                                        MessageBoxButtons.OK);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                               "Event Plug-in Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        // Overrides the IWMSEventNotificationPlugin.GetHandledEvents method.
        public object GetHandledEvents()
        {
            int[] iHandledEvents = new int[2];
            iHandledEvents[0] = (int)WMS_EVENT_TYPE.WMS_EVENT_CONNECT;
            iHandledEvents[1] = (int)WMS_EVENT_TYPE.WMS_EVENT_DISCONNECT;
            return (iHandledEvents);
        }
        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type t)
        {
            try
            {
                RegistryKey regHKLM = Registry.LocalMachine;
                regHKLM = regHKLM.CreateSubKey("SOFTWARE\\Microsoft\\Windows
            
                          Media\\Server\\RegisteredPlugins\\Event Notification
            
                          and Authorization\\
              { E1171A34 - 1F13 - 46d9 - AA8A - 63F47E92207C}
                ");
              regHKLM.SetValue(null, "Sample CSEvent Notification");

                RegistryKey regHKCR = Registry.ClassesRoot;
                regHKCR = regHKCR.CreateSubKey("CLSID\\
                         { E1171A34 - 1F13 - 46d9 - AA8A - 63F47E92207C}\\
               Properties");
              regHKCR.SetValue("Name", "Sample CSEvent Notification");
                regHKCR.SetValue("Author", "XYZ Corporation");
                regHKCR.SetValue("CopyRight", "Copyright 2002 . 
            
                           All rights reserved");
            
                regHKCR.SetValue("Description", "Enables you to trap the 
            
                           connect and disconnect events");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,
                                "Inside RegisterFunction(). Cannot Register",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }


        [ComUnregisterFunctionAttribute]
        public static void UnRegisterFunction(Type t)
        {
            try
            {
                RegistryKey regHKLM = Registry.LocalMachine;
                regHKLM.DeleteSubKey("SOFTWARE\\Microsoft\\Windows
            
                       Media\\Server\\RegisteredPlugins\\Event Notification and
            
                       Authorization\\{ E1171A34 - 1F13 - 46d9 - AA8A - 63F47E92207C}
                ");
          
    RegistryKey regHKCR = Registry.ClassesRoot;
                regHKCR.DeleteSubKeyTree("CLSID\\{E1171A34-1F13-46d9-AA8A-
            
                                         63F47E92207C}");
            regHKCR.DeleteSubKeyTree("CSEventTest.CSEventPlugin");
        }
  catch(Exception error)
  {
    MessageBox.Show(error.Message,
                    "Cannot delete a subkey.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
  }
}

