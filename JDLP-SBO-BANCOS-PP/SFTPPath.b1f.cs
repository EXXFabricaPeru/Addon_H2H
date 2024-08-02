using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace JDLP_SBO_BANCOS_PP
{
    [FormAttribute("JDLP_SBO_BANCOS_PP.SFTPPath", "SFTPPath.b1f")]
    class SFTPPath : UserFormBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SFTPPath()
        {
            this.UIAPIRawForm.Left = (Menu.SBO_Application.Desktop.Width / 2) - (UIAPIRawForm.Width / 2);
            this.UIAPIRawForm.Top = (Menu.SBO_Application.Desktop.Height / 2) - ((UIAPIRawForm.Height / 2) + 60);

            buscarSftp();
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.txtServidor = ((SAPbouiCOM.EditText)(this.GetItem("txtSE").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.txtPuerto = ((SAPbouiCOM.EditText)(this.GetItem("txtPU").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.txtUsuario = ((SAPbouiCOM.EditText)(this.GetItem("txtUS").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.txtClave = ((SAPbouiCOM.EditText)(this.GetItem("txtPS").Specific));
            this.btnGrabar = ((SAPbouiCOM.Button)(this.GetItem("btnBU").Specific));
            this.btnGrabar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnGrabar_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText txtServidor;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText txtPuerto;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText txtUsuario;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText txtClave;
        private SAPbouiCOM.Button btnGrabar;

        private void btnGrabar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                Entities.SftpBE bean = new Entities.SftpBE();
                bean.servidor = txtServidor.Value.Trim();
                bean.puerto = txtPuerto.Value.Trim();
                bean.usuario = txtUsuario.Value.Trim();
                bean.clave = txtClave.Value.Trim();

                grabarSftp(bean);

            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

        }

        void grabarSftp(Entities.SftpBE bean)
        {
            try
            {
//
            }catch(Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }
        void buscarSftp()
        {
            try
            {
                // BUSCAR RUTAS Y CLAVE EN BD
            }catch(Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

    }
}
