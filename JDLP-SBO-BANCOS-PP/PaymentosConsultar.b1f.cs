using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Data;

namespace JDLP_SBO_BANCOS_PP
{
    [FormAttribute("JDLP_SBO_BANCOS_PP.PaymentosByPIN", "PaymentosConsultar.b1f")]
    class PaymentosByPIN : UserFormBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        SAPbouiCOM.DataTable dtc;

        #region Variables globales
        private const string _Planilla = "Planilla";
        private const string _Nombre = "Nombre";
        private const string _Estado = "Estado";
        private const string _Usuario = "Usuario";
        private const string _ArchivoIN = "Archivo";
        private const string _Generado = "Generado";
        private const string _Marcar = "Marcar";


        #endregion

        public PaymentosByPIN()
        {
            this.UIAPIRawForm.Left = (Menu.SBO_Application.Desktop.Width / 2) - (UIAPIRawForm.Width / 2);
            this.UIAPIRawForm.Top = (Menu.SBO_Application.Desktop.Height / 2) - ((UIAPIRawForm.Height / 2) + 60);

            #region MTX CARGO
            dtc = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            SAPbouiCOM.Column oColumnaCargo;

            dtc.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Planilla, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Nombre, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Estado, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Usuario, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_ArchivoIN, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Generado, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Marcar, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);


            oColumnaCargo = mtxResultado.Columns.Item("#");
            oColumnaCargo.DataBind.Bind("DT_0", "#");

            oColumnaCargo = mtxResultado.Columns.Item("Col_0");
            oColumnaCargo.DataBind.Bind("DT_0", _Planilla);

            oColumnaCargo = mtxResultado.Columns.Item("Col_1");
            oColumnaCargo.DataBind.Bind("DT_0", _Nombre);

            oColumnaCargo = mtxResultado.Columns.Item("Col_2");
            oColumnaCargo.DataBind.Bind("DT_0", _Estado);

            oColumnaCargo = mtxResultado.Columns.Item("Col_3");
            oColumnaCargo.DataBind.Bind("DT_0", _Usuario);

            oColumnaCargo = mtxResultado.Columns.Item("Col_4");
            oColumnaCargo.DataBind.Bind("DT_0", _ArchivoIN);

            oColumnaCargo = mtxResultado.Columns.Item("Col_5");
            oColumnaCargo.DataBind.Bind("DT_0", _Generado);

            oColumnaCargo = mtxResultado.Columns.Item("Col_6");
            oColumnaCargo.DataBind.Bind("DT_0", _Marcar);


            #endregion
        }

        private SAPbouiCOM.EditText txtFechaInicio;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText txtFechaFin;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.Button btnBuscar;
        private SAPbouiCOM.Matrix mtxResultado;
        private SAPbouiCOM.Button txtGrabar;
        private SAPbouiCOM.ComboBox cboProceso;
        private SAPbouiCOM.StaticText StaticText0;

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.txtFechaInicio = ((SAPbouiCOM.EditText)(this.GetItem("1").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.txtFechaFin = ((SAPbouiCOM.EditText)(this.GetItem("2").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.btnBuscar = ((SAPbouiCOM.Button)(this.GetItem("5").Specific));
            this.btnBuscar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnBuscar_ClickBefore);
            this.mtxResultado = ((SAPbouiCOM.Matrix)(this.GetItem("4").Specific));
            this.txtGrabar = ((SAPbouiCOM.Button)(this.GetItem("6").Specific));
            this.txtGrabar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.txtGrabar_ClickBefore);
            this.cboProceso = ((SAPbouiCOM.ComboBox)(this.GetItem("3").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.OnCustomInitialize();

        }

        public override void OnInitializeFormEvents()
        {
        }

    

        private void OnCustomInitialize()
        {

        }

        private void btnBuscar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region Validacion
            BubbleEvent = true;

            if (string.IsNullOrEmpty(txtFechaInicio.Value)) {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar fecha de inicio", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return;
            }

            if (string.IsNullOrEmpty(txtFechaFin.Value))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar fecha fin", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return;
            }

            if (string.IsNullOrEmpty(cboProceso.Value))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe seleccionar un proceso", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return;
            }

            #endregion

            listarPlanillasByFechaInicioAndDechaFin();
        }

        private void listarPlanillasByFechaInicioAndDechaFin()
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("@vFI", Common.formatearFecha(txtFechaInicio.Value));
            parametros.Add("@vFF", Common.formatearFecha(txtFechaFin.Value));
            parametros.Add("@vProceso", cboProceso.Value);

            DataTable dt = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_HOST_BY_FECHA_PROCESO", parametros,Menu.SBO_Company);

            if (dt.Rows.Count > 0)
                cargarMatrixCargo(dt);
            else
                limpiarFormulario();
        }

        private void limpiarFormulario()
        {
            //txtFechaInicio.Value = string.Empty;
            //txtFechaFin.Value = string.Empty;
            //mtxResultado.Clear();
        }
        void cargarMatrixCargo(DataTable dt)
        {
            try
            {
                if (dtc.Rows.Count > 0)
                    dtc.Rows.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtc.Rows.Add();
                    #region DATA_TABLE

                    dtc.SetValue("#", i, i);
                    dtc.SetValue(_Planilla, i, dt.Rows[i][_Planilla].ToString());
                    dtc.SetValue(_Nombre, i, dt.Rows[i][_Nombre].ToString());
                    dtc.SetValue(_Estado, i, dt.Rows[i][_Estado].ToString());
                    dtc.SetValue(_Usuario, i, dt.Rows[i][_Usuario].ToString());
                    dtc.SetValue(_ArchivoIN, i, dt.Rows[i][_ArchivoIN].ToString());
                    dtc.SetValue(_Generado, i, dt.Rows[i][_Generado].ToString());
                    dtc.SetValue(_Marcar, i, dt.Rows[i][_Marcar].ToString());

                    #endregion
                }

                mtxResultado.LoadFromDataSourceEx();
                Menu.SBO_Application.StatusBar.SetText("PROCESO COMPLETADO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private void txtGrabar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            // throw new System.NotImplementedException();

            List<Entities.PlanillaHostBE> planillas = new List<Entities.PlanillaHostBE>();
            for (int i = 1; i <= mtxResultado.RowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)mtxResultado.Columns.Item("Col_6").Cells.Item(i).Specific).Checked == true)
                {
                    Entities.PlanillaHostBE planilla = new Entities.PlanillaHostBE();

                    planilla.HTH_PL = ((SAPbouiCOM.EditText)mtxResultado.Columns.Item("Col_0").Cells.Item(i).Specific).Value.ToString();
                    planilla.HTH_PR = ((SAPbouiCOM.EditText)mtxResultado.Columns.Item("Col_2").Cells.Item(i).Specific).Value.ToString();
                    planilla.HTH_PLG = ((SAPbouiCOM.EditText)mtxResultado.Columns.Item("Col_5").Cells.Item(i).Specific).Value.ToString();
                    planilla.HTH_CA = cboProceso.Value;

                    if (planilla.HTH_PR.Substring(0, 1).Equals("E") || planilla.HTH_PLG.Substring(0, 1).Equals("E"))
                    {
                       planillas.Add(planilla);

                        //Dictionary<string, string> parametros = new Dictionary<string, string>();
                        //parametros.Add("@vFI", Common.formatearFecha(txtFechaInicio.Value));
                        //parametros.Add("@vFF", Common.formatearFecha(txtFechaFin.Value));
                        //parametros.Add("@vProceso", cboProceso.Value);

                        //DataTable dt = Common.getData("SP_EXXIS_PLANILLA_BCP_L_HOST_BY_FECHA_PROCESO", parametros);

                        //cargarMatrixCargo(dt);
                    }
                }
            }

            if (planillas.Count > 0) { 
                Common.RemoveHostPlanillaV2(planillas,Menu.SBO_Company);
                listarPlanillasByFechaInicioAndDechaFin();

                Menu.SBO_Application.StatusBar.SetText("PROCESO COMPLETADO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            else { 
                Menu.SBO_Application.StatusBar.SetText("No hay datos para procesar", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
        }

        private SAPbouiCOM.StaticText StaticText3;
    }
}
