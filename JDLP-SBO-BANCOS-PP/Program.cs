using System;
using System.Collections.Generic;
using System.Reflection;
using JDLP_SBO_BANCOS_PP.Util;
using Metadata;
using SAPbouiCOM.Framework;

namespace JDLP_SBO_BANCOS_PP
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string PrefijoEXXIS = "EXX";
                string CodigoAddon = "H2H";

                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    oApp = new Application(args[0]);
                }

                MDResources.Messages = mostrarMensajes;

                var version = Assembly.GetExecutingAssembly().GetName().Version;

                if (MDResources.loadMetaData(Assembly.GetExecutingAssembly().GetName().Version, Application.SBO_Application, PrefijoEXXIS, CodigoAddon))
                {
                    Menu MyMenu = new Menu();
                    MyMenu.AddMenuItems();
                    oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                    Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                    oApp.Run();
                }
                else
                {
                    System.Windows.Forms.Application.Exit();
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }

        static void mostrarMensajes(string m, MessageType t)
        {
            switch (t)
            {
                case MessageType.Info:
                    Application.SBO_Application.SetStatusWarningMessage(m);
                    break;
                case MessageType.Success:
                    Application.SBO_Application.SetStatusSuccessMessage(m);
                    break;
                case MessageType.Error:
                    Application.SBO_Application.SetStatusErrorMessage(m);
                    break;
                default:
                    break;
            }
        }
    }
}
