using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using JDLP_SBO_BANCOS_PP.Entities;

namespace JDLP_SBO_BANCOS_PP
{
    [FormAttribute("JDLP_SBO_BANCOS_PP.Payments", "PaymentsWizard.b1f")]
    class Payments : UserFormBase
    {
        #region variables
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SAPbouiCOM.DataTable dt;

        string fechaInicio;
        string fechaFin;

        #endregion
        #region Matrix Columns
        private const string _Marcar = "Marcar";
        private const string _Wizard = "Wizard";
        private const string _Estado = "Estado";
        private const string _Banco = "Banco";
        private const string _Nombre = "Nombre";
        private const string _Fecha = "Fecha";
        private const string _Monto = "Monto";
        private const string _Codigo = "Codigo";
        private const string _Cuenta = "Cuenta";

        #endregion

        public Payments()
        {
            #region tamanio ventana
            this.UIAPIRawForm.Left = (Menu.SBO_Application.Desktop.Width / 2) - (UIAPIRawForm.Width / 2);
            this.UIAPIRawForm.Top = (Menu.SBO_Application.Desktop.Height / 2) - ((UIAPIRawForm.Height / 2) + 60);
            #endregion
            #region matrix
            dt = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            SAPbouiCOM.Column oColumna;

            dt.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Marcar, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Wizard, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Estado, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Banco, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Nombre, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Fecha, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Monto, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Codigo, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Cuenta, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            oColumna = Matrix.Columns.Item("#");
            oColumna.DataBind.Bind("DT_0", "#");

            oColumna = Matrix.Columns.Item("Col_0");
            oColumna.DataBind.Bind("DT_0", _Marcar);


            oColumna = Matrix.Columns.Item("Col_1");
            oColumna.DataBind.Bind("DT_0", _Wizard);

            oColumna = Matrix.Columns.Item("Col_2");
            oColumna.DataBind.Bind("DT_0", _Estado);

            oColumna = Matrix.Columns.Item("Col_3");
            oColumna.DataBind.Bind("DT_0", _Banco);

            oColumna = Matrix.Columns.Item("Col_4");
            oColumna.DataBind.Bind("DT_0", _Nombre);

            oColumna = Matrix.Columns.Item("Col_5");
            oColumna.DataBind.Bind("DT_0", _Fecha);

            oColumna = Matrix.Columns.Item("Col_6");
            oColumna.DataBind.Bind("DT_0", _Monto);

            oColumna = Matrix.Columns.Item("Col_7");
            oColumna.DataBind.Bind("DT_0", _Codigo);

            oColumna = Matrix.Columns.Item("Col_8");
            oColumna.DataBind.Bind("DT_0", _Cuenta);
            #endregion
        }

        #region disenio
        private SAPbouiCOM.EditText txtFechaInicio;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText txtFechaFin;
        private SAPbouiCOM.Button btnBuscar;
        private SAPbouiCOM.Matrix Matrix;
        private SAPbouiCOM.EditText txtPlanilla;
        private SAPbouiCOM.Button btnGenerarPlanilla;
        private SAPbouiCOM.Button btnConsolidarEjecutado;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText txtComentario;
        private SAPbouiCOM.StaticText StaticText0;

        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.txtFechaInicio = ((SAPbouiCOM.EditText)(this.GetItem("FI").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.txtFechaFin = ((SAPbouiCOM.EditText)(this.GetItem("FF").Specific));
            this.btnBuscar = ((SAPbouiCOM.Button)(this.GetItem("btnB").Specific));
            this.btnBuscar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnBuscar_ClickBefore);
            this.Matrix = ((SAPbouiCOM.Matrix)(this.GetItem("MTX").Specific));
            this.txtPlanilla = ((SAPbouiCOM.EditText)(this.GetItem("txtPL").Specific));
            this.btnGenerarPlanilla = ((SAPbouiCOM.Button)(this.GetItem("btnGP").Specific));
            this.btnGenerarPlanilla.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnGenerarPlanilla_ClickBefore);
            this.btnConsolidarEjecutado = ((SAPbouiCOM.Button)(this.GetItem("btnCE").Specific));
            this.btnConsolidarEjecutado.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnConsolidarEjecutado_ClickBefore);
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.txtComentario = ((SAPbouiCOM.EditText)(this.GetItem("txtCO").Specific));
            this.OnCustomInitialize();

        }


        public override void OnInitializeFormEvents()
        {
        }



        private void OnCustomInitialize()
        {

        }
        #endregion

        private void btnBuscar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region variables
            BubbleEvent = true;
            Matrix.Clear();

            DataTable dt;
            fechaInicio = txtFechaInicio.Value;
            fechaFin = txtFechaFin.Value;

            Dictionary<string, string> parametros = new Dictionary<string, string>();
            #endregion
            #region validar
            if (string.IsNullOrEmpty(fechaInicio))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar fecha de inicio", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return;
            }

            if (string.IsNullOrEmpty(fechaFin))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar fecha de fin", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return;
            }
            #endregion
            #region procesar
            try
            {
                fechaInicio = DateTime.ParseExact(fechaInicio, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                fechaFin = DateTime.ParseExact(fechaFin, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                parametros.Add("@FI", fechaInicio);
                parametros.Add("@FF", fechaFin);

                //dt = Common.getDataHana("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_WIZARD_BY_FECHA", parametros);
                dt = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_WIZARD_BY_FECHA", parametros, Menu.SBO_Company);
                cargarMatrix(dt);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            #endregion
        }
        private void cargarMatrix(DataTable table)
        {
            #region cargar
            try
            {
                if (dt.Rows.Count > 0)
                    dt.Rows.Clear();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dt.Rows.Add();
                    #region DATA_TABLE

                    dt.SetValue("#", i, i);
                    dt.SetValue(_Marcar, i, table.Rows[i][_Marcar].ToString());
                    dt.SetValue(_Wizard, i, table.Rows[i][_Wizard].ToString());
                    dt.SetValue(_Estado, i, table.Rows[i][_Estado].ToString());
                    dt.SetValue(_Banco, i, table.Rows[i][_Banco].ToString());
                    dt.SetValue(_Nombre, i, table.Rows[i][_Nombre].ToString());
                    dt.SetValue(_Fecha, i, table.Rows[i][_Fecha].ToString());
                    dt.SetValue(_Monto, i, table.Rows[i][_Monto].ToString());
                    dt.SetValue(_Codigo, i, table.Rows[i][_Codigo].ToString());
                    dt.SetValue(_Cuenta, i, table.Rows[i][_Cuenta].ToString());


                    #endregion
                }

                Matrix.LoadFromDataSourceEx();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            #endregion
        }
        private void btnGenerarPlanilla_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region procesar
            BubbleEvent = true;

            try
            {
                txtPlanilla.Value = Guid.NewGuid().ToString().ToUpper();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            #endregion
        }

        private void btnCopiar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region procesar
            BubbleEvent = true;

            try
            {
                //Clipboard.SetText(txtPlanilla.Value);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            #endregion
        }

        private void btnConsolidarEjecutado_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region variables
            BubbleEvent = true;
            DataTable dtCargo = null;
            DataTable dtAbono = null;
            DataTable dtDocumento = null;
            Dictionary<string, string> parametros = null;


            string idplanilla = txtPlanilla.Value.Trim();
            string comentario = txtComentario.Value.Trim();



            int cdatos = Matrix.RowCount;
            #endregion
            #region validaciones

            if (string.IsNullOrEmpty(fechaInicio))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar fecha de inicio", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return;
            }

            if (string.IsNullOrEmpty(fechaFin))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar fecha de fin", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return;
            }

            if (string.IsNullOrEmpty(idplanilla))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe generar un codigo planilla", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return;
            }

            if (string.IsNullOrEmpty(comentario))
            {
                Menu.SBO_Application.StatusBar.SetText("Debe ingresar un comentario", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return;
            }

            if (cdatos == 0)
            {
                Menu.SBO_Application.StatusBar.SetText("No hay datos para procesar", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                return;
            }

            #endregion
            #region procesar
            try
            {
                #region crear planilla

                #region bean planilla
                List<PlanillaHostBE> planillas = new List<PlanillaHostBE>();
                Entities.PlanillaHostBE planilla = new PlanillaHostBE();

                planilla.Code = Guid.NewGuid().ToString().ToUpper();
                planilla.Name = $"{Common.ACCION.P.ToString()}-{planilla.Code}";
                planilla.HTH_PL = idplanilla.ToUpper();
                planilla.HTH_CO = txtComentario.Value.ToUpper();
                planilla.HTH_PR = Common.ESTADO_PLANILLA.W.ToString();
                planilla.HTH_FC = DateTime.Now.ToString();
                planilla.HTH_FP = string.Empty;
                planilla.HTH_US = Menu.SBO_Company.UserName;
                planilla.HTH_CA = Common.ACCION.P.ToString();
                planilla.HTH_NAE = string.Empty;
                planilla.HTH_NAR1 = string.Empty;
                planilla.HTH_NAR2 = string.Empty;
                planilla.HTH_PLH = string.Empty;
                planilla.HTH_WIZ = txtComentario.Value.ToUpper();
                planilla.HTH_WIZFI = fechaInicio;
                planilla.HTH_WIZFF = fechaFin;

                planillas.Add(planilla);
                Menu.SBO_Application.StatusBar.SetText("PROCESANDO...PLANILLA", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                #endregion

                if (Common.addHostPlanillaV2(planillas, Menu.SBO_Company))
                {
                    #region wizname en una sola planilla

                    List<Entities.PlanillaHostProveedorPago> pagos = new List<PlanillaHostProveedorPago>();
                    for (int i = 1; i <= Matrix.RowCount; i++)
                    {
                        #region Wizname seleccionados
                        if (((SAPbouiCOM.CheckBox)Matrix.Columns.Item("Col_0").Cells.Item(i).Specific).Checked == true)
                        {
                            #region bean pago
                            Entities.PlanillaHostProveedorPago pago = new PlanillaHostProveedorPago();
                            pago.Code = Guid.NewGuid().ToString().ToUpper();
                            pago.Name = $"{Common.ACCION.P.ToString()}{pago.Code}";
                            pago.HTH_PL = idplanilla.ToUpper();
                            pago.WIZNAME = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_1").Cells.Item(i).Specific).Value.ToString();
                            pago.Fecha = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_5").Cells.Item(i).Specific).Value.ToString();
                            pago.WIZFI = fechaInicio;
                            pago.WIZFF = fechaFin;
                            pagos.Add(pago);
                            Menu.SBO_Application.StatusBar.SetText("PROCESANDO...PAGOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                            #endregion
                        }
                        #endregion
                    }

                    #endregion

                    if (Common.AddProveedorPagoV2(pagos,Menu.SBO_Company))
                    {
                        #region registrar cargo(s) vinculado a una planilla
                        parametros = new Dictionary<string, string>();
                        parametros.Add("@planillaSAP", planilla.HTH_PL);

                        dtCargo = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_CARGO_BY_WIZARD_AND_FECHA", parametros, Menu.SBO_Company);

                        List<Entities.PlanillaHostProveedorCargo> cargos = new List<PlanillaHostProveedorCargo>();
                        for (int c = 0; c < dtCargo.Rows.Count; c++)
                        {
                            #region bean cargo
                            PlanillaHostProveedorCargo cargo = new PlanillaHostProveedorCargo();
                            cargo.Code = Guid.NewGuid().ToString().ToUpper();
                            cargo.Name = $"{Common.ACCION.P.ToString()}-{cargo.Code}";
                            cargo.HTH_PL = planilla.HTH_PL;
                            cargo.HTP_PC = cargo.Code.ToUpper();
                            cargo.tipoRegistro = dtCargo.Rows[c]["1.01_TipoRegistro"].ToString();
                            cargo.cantidadAbonosPlanilla = dtCargo.Rows[c]["1.02_CantidadAbonosPlanilla"].ToString();
                            cargo.fechaProceso = dtCargo.Rows[c]["1.03_FechaProceso"].ToString();
                            cargo.tipoCuentaCargo = dtCargo.Rows[c]["1.04_TipoCuentaCargo"].ToString();
                            cargo.monedaCuentaCargo = dtCargo.Rows[c]["1.05_MonedaCuentaCargo"].ToString();
                            cargo.numeroCuentaCargo = dtCargo.Rows[c]["1.06_NumeroCuentaCargo"].ToString();
                            cargo.montoTotalPlanilla = dtCargo.Rows[c]["1.07_MontoTotalPlanilla"].ToString();
                            cargo.referenciaPlanilla = dtCargo.Rows[c]["1.08_ReferenciaPlanilla"].ToString();
                            cargo.flagExoneracionITF = dtCargo.Rows[c]["1.09_FlagExoneracionITF"].ToString();
                            cargo.totalControl = dtCargo.Rows[c]["1.10_TotalControl"].ToString();
                            cargo.cargoChkSum = dtCargo.Rows[c]["cargoChkSum"].ToString();
                            cargos.Add(cargo);
                            Menu.SBO_Application.StatusBar.SetText("PROCESANDO...CARGOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                            #endregion
                        }

                        if (Common.AddProveedorCargoV2(cargos,Menu.SBO_Company))
                        {
                            #region registra abono(s) vinculado(s) al cargo(s) de una planilla
                            foreach (var lcargo in cargos)
                            {
                                parametros = new Dictionary<string, string>();
                                parametros.Add("@planillaSAP", planilla.HTH_PL);
                                dtAbono = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_ABONO_BY_WIZARD_AND_FECHA", parametros, Menu.SBO_Company);

                                List<Entities.PlanillaHostProveedorAbono> abonos = new List<PlanillaHostProveedorAbono>();
                                for (int a = 0; a < dtAbono.Rows.Count; a++)
                                {
                                    #region bean abonos
                                    Entities.PlanillaHostProveedorAbono abono = new PlanillaHostProveedorAbono();
                                    abono.Code = Guid.NewGuid().ToString().ToUpper();
                                    abono.Name = $"{Common.ACCION.P.ToString()}-{abono.Code}";
                                    abono.HTH_PL = planilla.HTH_PL;
                                    abono.HTP_PC = lcargo.HTP_PC;
                                    abono.HTA_PA = abono.Code.ToUpper();
                                    abono.tipoRegistro = dtAbono.Rows[a]["2.01_TipoRegistro"].ToString();
                                    abono.tipoCuentaAbono = dtAbono.Rows[a]["2.02_TipoCuentaAbono"].ToString();
                                    abono.numeroCuentaAbono = dtAbono.Rows[a]["2.03_NumeroCuentaAbono"].ToString();
                                    abono.modalidadPago = dtAbono.Rows[a]["2.04_ModalidadPago"].ToString();
                                    abono.tipoDocumentoProveedor = dtAbono.Rows[a]["2.05_TipoDocumentoProveedor"].ToString();
                                    abono.numeroDocumentoProveedor = dtAbono.Rows[a]["2.06_NumeroDocumentoProveedor"].ToString();
                                    abono.correlativoDocumentoProveedor = dtAbono.Rows[a]["2.07_CorrelativoDocumentoProveedor"].ToString();
                                    abono.nombreProveedor = dtAbono.Rows[a]["2.08_NombreProveedor"].ToString();
                                    abono.referenciaBeneficiario = dtAbono.Rows[a]["2.09_ReferenciaBeneficiario"].ToString();
                                    abono.referenciaEmpresa = dtAbono.Rows[a]["2.10_ReferenciaEmpresa"].ToString();
                                    abono.monedaImporteAbonar = dtAbono.Rows[a]["2.11_MonedaImporteAbonar"].ToString();
                                    abono.importeAbonar = dtAbono.Rows[a]["2.12_ImporteAbonar"].ToString();
                                    abono.flagValidarIDC = dtAbono.Rows[a]["2.13_FlagValidarIDC"].ToString();
                                    abono.abonoChkSum = dtAbono.Rows[a]["abonoChkSum"].ToString();
                                    abono.order = dtAbono.Rows[a]["orderProv"].ToString();

                                    abonos.Add(abono);
                                    Menu.SBO_Application.StatusBar.SetText("PROCESANDO...ABONOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                    #endregion
                                }
                                if (Common.AddProveedorAbonoV2(abonos,Menu.SBO_Company))
                                {
                                    #region registra documentos vinculados a un abono 

                                    foreach (var labono in abonos)
                                    {
                                        parametros = new Dictionary<string, string>();
                                        parametros.Add("@planillaSAP", planilla.HTH_PL);
                                        parametros.Add("@vRutProveedor", labono.numeroDocumentoProveedor);
                                        dtDocumento = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_DOCUMENTO_BY_WIZARD_AND_FECHA", parametros, Menu.SBO_Company);

                                        List<PlanillaHostProveedorDocumento> documentos = new List<PlanillaHostProveedorDocumento>();
                                        for (int d = 0; d < dtDocumento.Rows.Count; d++)
                                        {
                                            #region bean documento 
                                            PlanillaHostProveedorDocumento documento = new PlanillaHostProveedorDocumento();
                                            documento.Code = Guid.NewGuid().ToString().ToUpper();
                                            documento.Name = $"{Common.ACCION.P.ToString()}-{documento.Code}";
                                            documento.HTH_PL = planilla.HTH_PL;
                                            documento.HTP_PC = lcargo.HTP_PC;
                                            documento.HTA_PA = labono.HTA_PA;
                                            documento.HTD_PD = documento.Code.ToUpper();
                                            documento.tipoRegistro = dtDocumento.Rows[d]["3.01_TipoRegistro"].ToString();
                                            documento.tipoDocumnetoPagar = dtDocumento.Rows[d]["3.02_TipoDocumentoPagar"].ToString();
                                            documento.numeroDocumentoPagar = dtDocumento.Rows[d]["3.03_NumeroDocumentoPagar"].ToString();
                                            documento.importeDocumnetoPagar = dtDocumento.Rows[d]["3.04_ImporteDocumentoPagar"].ToString();

                                            documentos.Add(documento);
                                            //Menu.SBO_Application.StatusBar.SetText("PROCESANDO...DOCUMENTOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                            #endregion
                                        }

                                        if (!Common.AddProveedorDocumentoV2(documentos,Menu.SBO_Company))
                                        {
                                            throw new Exception("No se pudo registrar documentos de la planilla");
                                        }
                                        else { Menu.SBO_Application.StatusBar.SetText("PROCESANDO...DOCUMENTOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success); }
                                    }
                                    #endregion
                                }
                                else { throw new Exception("No se pudo registrar abonos de la planilla"); }
                            }
                            #endregion

                            txtFechaInicio.Value = string.Empty;
                            txtFechaFin.Value = string.Empty;
                            Matrix.Clear();
                            txtPlanilla.Value = string.Empty;
                            txtComentario.Value = string.Empty;

                            Menu.SBO_Application.StatusBar.SetText("PLANILLA DE PROVEEDORES FUE PROCESADO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                        }
                        else { throw new Exception("No se pudo registrar cargos de la planilla"); }

                        #endregion
                    }
                    else { throw new Exception("No se puedo registrar los pagos(planillas) masivos"); }
                }
                else { throw new Exception("No se puedo registrar la planilla"); }

                #endregion
            }
            catch (Exception ex)
            {
                Common.DeleteHostPlanillaProveedorByPLV2(txtPlanilla.Value,Menu.SBO_Company);
                txtPlanilla.Value = string.Empty;
                txtComentario.Value = string.Empty;
                Menu.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            #endregion
        }
    }
}
