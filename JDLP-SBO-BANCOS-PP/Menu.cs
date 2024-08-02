using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using System.IO;

namespace JDLP_SBO_BANCOS_PP
{
   public class Menu
    {
        #region Variables
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static SAPbouiCOM.Application SBO_Application;
        public static SAPbobsCOM.Company SBO_Company;
        private string FileImageName = "bcp.jpg";
        #endregion
        #region FORMULARIOS
        private const string module_id = "-1";
        private const string module_value = "Host to Host";

        private const string frmPaymentsPaths_id= "-2";
        private const string frmPaymentsPaths_value = "Configurar Path Host";

        private const string frmPaymentsConsultar_id = "-3";
        private const string frmPaymentsConsultar_value = "Consultar Planilla";

        private const string frmPaymentsTransferencia_id = "-4";
        private const string frmPaymentsTransferencia_value = "Planilla Transferencia";

        private const string frmPaymentsProveedor_id = "-5";
        private const string frmPaymentsProveedor_value = "Planilla Proveedores";


        private const string frmPaymentsTemplate_id = "-6";
        private const string frmPaymentsTemplate_value = "Generar Archivos";

        #endregion
        public Menu()
        {
            SBO_Application = Application.SBO_Application;
            SBO_Company = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();
            Common.SBO_LOCATE = SBO_Company.CompanyDB;
        }
        private void SetApplication()
        {
            try
            {
                SAPbouiCOM.SboGuiApi SboGuiApi = new SAPbouiCOM.SboGuiApi();
                string sConnectionString = Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));
                logger.Debug($"SetApplication: la cadena de conexion es {sConnectionString}");
                SboGuiApi.Connect(sConnectionString);
               // Utilidades.oApplication = SboGuiApi.GetApplication();
            }
            catch (Exception ex)
            {
                logger.Error("SetApplication", ex);
            }

        }
        private void SetCompany()
        {
            try
            {
               // Utilidades.oCompany = (SAPbobsCOM.Company)Utilidades.oApplication.Company.GetDICompany();
               // logger.Info($"SetCompany: conectado a {Utilidades.oCompany.CompanyName}({Utilidades.oCompany.CompanyDB})");
            }
            catch (Exception ex)
            {
                logger.Error("SetCompany", ex);
            }
        }
        public void AddMenuItems()
        {
            #region MODULO
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = module_id;
            oCreationPackage.String = module_value;
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;
            oCreationPackage.Image = Path.Combine(Environment.CurrentDirectory, @"Resources\images\", FileImageName);

            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {
                //logger.Warn(e.Message);               
            }
            #endregion
            #region MENU
            try
            {
                oMenuItem = Application.SBO_Application.Menus.Item(module_id);
                oMenus = oMenuItem.SubMenus;

                #region Opciones de menu

                //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                //oCreationPackage.UniqueID = frmPaymentsPaths_id;
                //oCreationPackage.String = frmPaymentsPaths_value;
                //oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = frmPaymentsConsultar_id;
                oCreationPackage.String = frmPaymentsConsultar_value;
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = frmPaymentsTransferencia_id;
                oCreationPackage.String = frmPaymentsTransferencia_value;
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = frmPaymentsProveedor_id;
                oCreationPackage.String = frmPaymentsProveedor_value;
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = frmPaymentsTemplate_id;
                oCreationPackage.String = frmPaymentsTemplate_value;
                oMenus.AddEx(oCreationPackage);

                #endregion

                crearcarpetas();
            }
            catch (Exception er)
            {
                Application.SBO_Application.SetStatusBarMessage("[Host To Host cargando...]", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }
            #endregion
        }
        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                //if (pVal.BeforeAction && pVal.MenuUID == frmPaymentsPaths_id)
                //{
                //    SFTPPath activeForm = new SFTPPath();
                //    activeForm.Show();
                //}

                if (pVal.BeforeAction && pVal.MenuUID == frmPaymentsConsultar_id)
                {
                    //PaymentosConsultar
                    PaymentosByPIN activeForm = new PaymentosByPIN();
                    activeForm.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == frmPaymentsTransferencia_id)
                {
                    //PaymentsTransfer
                    PaymentsOVPM activeForm = new PaymentsOVPM();
                    activeForm.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == frmPaymentsProveedor_id)
                {
                    //PaymentsWizard
                    Payments activeForm = new Payments();
                    activeForm.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == frmPaymentsTemplate_id)
                {
                    //PaymentsTemplate
                    PaymentsTemplate activeForm = new PaymentsTemplate();
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                //Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

         private void crearcarpetas()
        {
            Common.crearCarpeta(Common.pathProveedorIN);
            Common.crearCarpeta(Common.pathProveedorOU);
            Common.crearCarpeta(Common.pathTransferenciaIN);
            Common.crearCarpeta(Common.pathTransferenciaOU);

            Common.crearCarpeta(Common.pathSFTPProveedorIN);
            Common.crearCarpeta(Common.pathSFTPProveedorOU);
            Common.crearCarpeta(Common.pathSFTPTransferenciaIN);
            Common.crearCarpeta(Common.pathSFTPTransferenciaOU);
        }
    }
}
//TRANSFERENCIA TXTABC FORMA XYZ
//-- PAGOS EGECTUADOS -> PAGOS INDIVUALES

//PROVEEDORES TXTMNP FORMTO KLN
//-- PAGOS MASIVOS