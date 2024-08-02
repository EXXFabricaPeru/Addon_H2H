using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using JDLP_SBO_BANCOS_PP.Entities;

/// <summary>
/// TRANSFERENCIAS SON LOS PAGOS INDIVUALES VIA PAGO EFECTUADO SAP
/// </summary>
namespace JDLP_SBO_BANCOS_PP
{
    [FormAttribute("JDLP_SBO_BANCOS_PP.PaymentsOVPM", "PaymentsTransfer.b1f")]
    class PaymentsOVPM : UserFormBase
    {
        #region Variables Globales
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SqlConnection cnx;
        SqlDataAdapter da;
        SAPbouiCOM.DataTable dt;

        string fechaInicio;
        string fechaFin;
        #endregion
        #region Matrix Columns

        private const string _Marcar = "Marcar";
        private const string _Entry = "Entry";
        private const string _Numero = "Numero";
        private const string _Codigo = "Codigo";
        private const string _Proveedor = "Proveedor";
        private const string _Fecha = "Fecha";
        private const string _Movimiento = "Movimiento";
        private const string _Valor = "Valor";
        private const string _Comentario = "Comentario";
        private const string _Comentario2 = "Comentario2";
        private const string _Cuenta = "Cuenta";
        private const string _Moneda = "Moneda";

        #endregion

        public PaymentsOVPM()
        {
            this.UIAPIRawForm.Left = (Menu.SBO_Application.Desktop.Width / 2) - (UIAPIRawForm.Width / 2);
            this.UIAPIRawForm.Top = (Menu.SBO_Application.Desktop.Height / 2) - ((UIAPIRawForm.Height / 2) + 60);

            #region MATRIX
            dt = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            SAPbouiCOM.Column oColumna;

            dt.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Marcar, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Entry, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Numero, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Codigo, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Proveedor, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Fecha, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Movimiento, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Valor, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Comentario, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Comentario2, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Cuenta, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add(_Moneda, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            oColumna = Matrix.Columns.Item("#");
            oColumna.DataBind.Bind("DT_0", "#");

            oColumna = Matrix.Columns.Item("Col_0");
            oColumna.DataBind.Bind("DT_0", _Marcar);


            oColumna = Matrix.Columns.Item("Col_1");
            oColumna.DataBind.Bind("DT_0", _Entry);

            oColumna = Matrix.Columns.Item("Col_2");
            oColumna.DataBind.Bind("DT_0", _Numero);

            oColumna = Matrix.Columns.Item("Col_3");
            oColumna.DataBind.Bind("DT_0", _Codigo);

            oColumna = Matrix.Columns.Item("Col_4");
            oColumna.DataBind.Bind("DT_0", _Proveedor);

            oColumna = Matrix.Columns.Item("Col_5");
            oColumna.DataBind.Bind("DT_0", _Fecha);

            oColumna = Matrix.Columns.Item("Col_6");
            oColumna.DataBind.Bind("DT_0", _Movimiento);

            oColumna = Matrix.Columns.Item("Col_7");
            oColumna.DataBind.Bind("DT_0", _Valor);

            oColumna = Matrix.Columns.Item("Col_8");
            oColumna.DataBind.Bind("DT_0", _Comentario);

            oColumna = Matrix.Columns.Item("Col_9");
            oColumna.DataBind.Bind("DT_0", _Comentario2);

            oColumna = Matrix.Columns.Item("Col_10");
            oColumna.DataBind.Bind("DT_0", _Cuenta);

            oColumna = Matrix.Columns.Item("Col_11");
            oColumna.DataBind.Bind("DT_0", _Moneda);

            #endregion
        }

        private void cargarMatrix(DataTable table)
        {
            #region Cargar Matriz
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
                    dt.SetValue(_Entry, i, table.Rows[i][_Entry].ToString());
                    dt.SetValue(_Numero, i, table.Rows[i][_Numero].ToString());
                    dt.SetValue(_Codigo, i, table.Rows[i][_Codigo].ToString());
                    dt.SetValue(_Proveedor, i, table.Rows[i][_Proveedor].ToString());
                    dt.SetValue(_Fecha, i, table.Rows[i][_Fecha].ToString());
                    dt.SetValue(_Movimiento, i, table.Rows[i][_Movimiento].ToString());
                    dt.SetValue(_Valor, i, table.Rows[i][_Valor].ToString());
                    dt.SetValue(_Comentario2, i, table.Rows[i][_Comentario2].ToString());
                    dt.SetValue(_Cuenta, i, table.Rows[i][_Cuenta].ToString());
                    dt.SetValue(_Moneda, i, table.Rows[i][_Moneda].ToString());

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
        public override void OnInitializeFormEvents()
        {

        }
        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.EditText txtPlanilla;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText txtCodigoProveedor;
        private SAPbouiCOM.EditText txtNombreProveedor;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText txtFechaInicio;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText txtFechaFin;
        private SAPbouiCOM.Button btnBuscar;
        private SAPbouiCOM.Matrix Matrix;
        private SAPbouiCOM.Button btnGrabarPlantilla;
        private SAPbouiCOM.Button btnGenerarPlantilla;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText txtComentario;

        public override void OnInitializeComponent()
        {
            this.txtPlanilla = ((SAPbouiCOM.EditText)(this.GetItem("txtPL").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.txtCodigoProveedor = ((SAPbouiCOM.EditText)(this.GetItem("txtCC").Specific));
            this.txtCodigoProveedor.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.txtCodigoProveedor_ChooseFromListAfter);
            this.txtNombreProveedor = ((SAPbouiCOM.EditText)(this.GetItem("txtCN").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.txtFechaInicio = ((SAPbouiCOM.EditText)(this.GetItem("txtFI").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.txtFechaFin = ((SAPbouiCOM.EditText)(this.GetItem("txtFF").Specific));
            this.btnBuscar = ((SAPbouiCOM.Button)(this.GetItem("btnBU").Specific));
            this.btnBuscar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnBuscar_ClickBefore);
            this.Matrix = ((SAPbouiCOM.Matrix)(this.GetItem("MTX").Specific));
            this.btnGrabarPlantilla = ((SAPbouiCOM.Button)(this.GetItem("btnSP").Specific));
            this.btnGrabarPlantilla.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnGrabarPlantilla_ClickBefore);
            this.btnGenerarPlantilla = ((SAPbouiCOM.Button)(this.GetItem("btnGP").Specific));
            this.btnGenerarPlantilla.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnGenerarPlantilla_ClickBefore);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.txtComentario = ((SAPbouiCOM.EditText)(this.GetItem("txtCO").Specific));
            this.OnCustomInitialize();

        }

        private void btnBuscar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region Variables
            BubbleEvent = true;
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            Matrix.Clear();
            txtPlanilla.Value = string.Empty;
            txtComentario.Value = string.Empty; 
            DataTable dt;
            //fechaInicio = txtFechaInicio.Value;
            //fechaFin = txtFechaFin.Value;

           
            #endregion
            #region Validacion
            if (string.IsNullOrEmpty(txtFechaInicio.Value)) { Menu.SBO_Application.StatusBar.SetText("Debe ingresar una fecha de inicio", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return; }
            if (string.IsNullOrEmpty(txtFechaFin.Value)) { Menu.SBO_Application.StatusBar.SetText("Debe ingresar una fecha fin", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return; }
            #endregion

            try
            {
                fechaInicio = Common.formatearFecha(txtFechaInicio.Value); //DateTime.ParseExact(fechaInicio, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                fechaFin = Common.formatearFecha(txtFechaFin.Value);  //DateTime.ParseExact(fechaFin, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                parametros.Add("@FI", fechaInicio);
                parametros.Add("@FF", fechaFin);

                dt = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA_WIZARD_BY_FECHA", parametros,Menu.SBO_Company);

                if (dt.Rows.Count == 0)
                {
                    Menu.SBO_Application.StatusBar.SetText("No hay resultados para el criterio ingresado", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    return;
                }
                cargarMatrix(dt);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

        }

        private void btnGenerarPlantilla_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
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

        }

        private void btnGrabarPlantilla_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            int cantidadSeleccionados = 0;
            Dictionary<string, string> parametros;

            #region Validaciones
            
            if (string.IsNullOrEmpty(txtPlanilla.Value)) {Menu.SBO_Application.StatusBar.SetText("Primero de generar un codigo de planilla", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);return;}
            if (string.IsNullOrEmpty(txtComentario.Value)) { Menu.SBO_Application.StatusBar.SetText("Debe ingresar un comentario a la planilla", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return; }


            for (int i = 1; i <= Matrix.RowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix.Columns.Item("Col_0").Cells.Item(i).Specific).Checked == true)
                {
                    cantidadSeleccionados += 1;
                }
            }
            #endregion

            if (cantidadSeleccionados > 0)
            {
                try
                {

                    #region Planilla
                    List<PlanillaHostBE> planillas = new List<Entities.PlanillaHostBE>();
                    PlanillaHostBE planilla = new PlanillaHostBE();

                    #region Planilla bean 
                    planilla.Code = Guid.NewGuid().ToString().ToUpper();
                    planilla.Name = $"{Common.ACCION.T.ToString()}-{planilla.Code}";
                    planilla.HTH_PL = txtPlanilla.Value.ToUpper();
                    planilla.HTH_CO = txtComentario.Value.ToUpper();
                    planilla.HTH_PR = Common.ESTADO_PLANILLA.W.ToString();
                    planilla.HTH_FC = DateTime.Now.ToString();
                    planilla.HTH_FP = string.Empty;
                    planilla.HTH_US = Menu.SBO_Company.UserName;
                    planilla.HTH_CA = Common.ACCION.T.ToString();
                    planilla.HTH_NAE = string.Empty;
                    planilla.HTH_NAR1 = string.Empty;
                    planilla.HTH_NAR2 = string.Empty;
                    planilla.HTH_PLH = string.Empty;
                    planilla.HTH_WIZ = txtComentario.Value.ToUpper();
                    planilla.HTH_WIZFI = fechaInicio;
                    planilla.HTH_WIZFF = fechaFin;
                    #endregion

                    planillas.Add(planilla);
                    
                    if (Common.addHostPlanillaV2(planillas,Menu.SBO_Company))
                    {
                        parametros = new Dictionary<string, string>();
                        parametros.Add("@vPlanilla", planilla.HTH_PL);
                        DataTable dtPlanilla = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA_PLANILLA", parametros,Menu.SBO_Company);
                    }
                    else
                    {
                        throw new Exception("No se puedo registrar la planilla");
                    }
                    #endregion

                    #region Pagos OVPM
                    List<VendorPayments> pagos = new List<VendorPayments>();

                    for (int i = 1; i <= Matrix.RowCount; i++)
                    {
                        if (((SAPbouiCOM.CheckBox)Matrix.Columns.Item("Col_0").Cells.Item(i).Specific).Checked == true)
                        {
                            #region bean pagos
                            VendorPayments pago = new VendorPayments();
                            pago.HTH_PL = planilla.HTH_PL;
                            pago.Code = Guid.NewGuid().ToString().ToUpper();
                            pago.Name = $"{Common.ACCION.T.ToString()}-{pago.Code}";
                            pago.Entry = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_1").Cells.Item(i).Specific).Value.ToString();
                            pago.Numero = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_2").Cells.Item(i).Specific).Value.ToString();
                            pago.Codigo = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_3").Cells.Item(i).Specific).Value.ToString();
                            pago.Proveedor = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_4").Cells.Item(i).Specific).Value.ToString();
                            pago.Fecha = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_5").Cells.Item(i).Specific).Value.ToString();
                            pago.Movimiento = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_6").Cells.Item(i).Specific).Value.ToString();
                            pago.Valor = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_7").Cells.Item(i).Specific).Value.ToString();
                            pago.Comentario = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_8").Cells.Item(i).Specific).Value.ToString();
                            pago.Comentari2 = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_9").Cells.Item(i).Specific).Value.ToString();
                            pago.Cuenta = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_10").Cells.Item(i).Specific).Value.ToString();
                            pago.Moneda = ((SAPbouiCOM.EditText)Matrix.Columns.Item("Col_11").Cells.Item(i).Specific).Value.ToString();

                            #endregion

                            pagos.Add(pago);
                            Menu.SBO_Application.StatusBar.SetText("PROCESANDO...PAGOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                        }
                    }

                    
                    if (Common.AddTransferenciaProveedorV2(pagos,Menu.SBO_Company))
                    {
                        parametros = new Dictionary<string, string>();
                        parametros.Add("@vPlanilla", planilla.HTH_PL);

                        DataTable dtTransferencia = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_B_TRANSFERENCIA_BY_PLANILLA", parametros,Menu.SBO_Company);
                        if (dtTransferencia.Rows.Count != 0)
                        {
                            List<PlanillaHostTransferencia> transferencias = new List<PlanillaHostTransferencia>();
                            PlanillaHostTransferencia transferencia = new PlanillaHostTransferencia();

                            for (int i = 0; i < dtTransferencia.Rows.Count; i++)
                            {
                                #region Transferencia

                                transferencia.Code = Guid.NewGuid().ToString().ToUpper();
                                transferencia.Name = $"{Common.ACCION.T.ToString()}-{transferencia.Code}";
                                transferencia.HTT_TF = transferencia.Code.ToUpper();
                                transferencia.HTH_PL = planilla.HTH_PL;
                                transferencia.tipoRegistro = dtTransferencia.Rows[i]["tipoRegistro"].ToString();
                                transferencia.secuencialFila = dtTransferencia.Rows[i]["secuenciaFila"].ToString();
                                transferencia.cantidadCuentasArchivo = dtTransferencia.Rows[i]["cantidadCuentasArchivo"].ToString();
                                transferencia.identificadorCabecera = dtTransferencia.Rows[i]["identificadorCabecera"].ToString();
                                transferencia.filler = dtTransferencia.Rows[i]["filler"].ToString();

                                transferencias.Add(transferencia);
                                Menu.SBO_Application.StatusBar.SetText("PROCESANDO...TRANSFERENCIAS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                #endregion
                            }


                            if (Common.AddTransferenciaV2(transferencias,Menu.SBO_Company))
                            {
                                parametros = new Dictionary<string, string>();
                                parametros.Add("@vPlanilla", planilla.HTH_PL);
                                DataTable dtCargo = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_B_TRANSFERENCIA_CARGO_BY_PLANILLA", parametros, Menu.SBO_Company);

                                List<PlanillaHostTransferenciaCargo> cargos = new List<PlanillaHostTransferenciaCargo>();
                                for (int i = 0; i < dtCargo.Rows.Count; i++)
                                {
                                    #region Transferencia Cargo

                                    PlanillaHostTransferenciaCargo cargo = new PlanillaHostTransferenciaCargo();
                                    cargo.Code = Guid.NewGuid().ToString().ToUpper();
                                    cargo.Name = $"{Common.ACCION.T.ToString()}-{cargo.Code}";
                                    cargo.HTT_CA = cargo.Code.ToUpper();
                                    cargo.HTH_PL = planilla.HTH_PL;
                                    cargo.HTT_TF = transferencia.HTT_TF;
                                    cargo.moneda = dtCargo.Rows[i]["moneda"].ToString();
                                    cargo.tipoRegistro = dtCargo.Rows[i]["tipoRegistro"].ToString();
                                    cargo.secuencialFila = dtCargo.Rows[i]["secuencialFila"].ToString(); //posicion en el txt
                                    cargo.cantidadAbonosPlanilla = dtCargo.Rows[i]["cantidadAbonosPlanilla"].ToString();
                                    cargo.tipoCuentaCargo = dtCargo.Rows[i]["tipoCuentaCargo"].ToString();
                                    cargo.monedaCuentaCargo = dtCargo.Rows[i]["monedaCuentaCargo"].ToString();
                                    cargo.numeroCuentaCargo = dtCargo.Rows[i]["numeroCuentaCargo"].ToString();
                                    cargo.montoTotalSoles = dtCargo.Rows[i]["montoTotalSoles"].ToString();
                                    cargo.montoTotalDolares = dtCargo.Rows[i]["montoTotalDolares"].ToString();
                                    cargo.totalControl = dtCargo.Rows[i]["totalControl"].ToString();
                                    cargo.identificadorCargo = dtCargo.Rows[i]["identificadorCargo"].ToString();
                                    cargo.cargoChkSum = dtCargo.Rows[i]["cargoChkSum"].ToString();
                                    cargo.filler = "";

                                    cargos.Add(cargo);
                                    Menu.SBO_Application.StatusBar.SetText("PROCESANDO...CARGOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                    #endregion
                                }
                                
                                if (Common.AddTransferenciaCargoV2(cargos,Menu.SBO_Company))
                                {
                                    List<PlanillaHostTransferenciaAbono> abonos;
                                    for (int i = 0; i < cargos.Count; i++)
                                    {

                                        parametros = new Dictionary<string, string>();
                                        parametros.Add("@vPlanilla", planilla.HTH_PL);
                                        parametros.Add("@vNumeroCuenta",cargos[i].numeroCuentaCargo);
                                        parametros.Add("@vMoneda", cargos[i].moneda);

                                        DataTable dtAbono = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_B_TRANSFERENCIA_ABONO_BY_PLANILLA_CUENTA_MONEDA", parametros, Menu.SBO_Company);

                                        abonos = new List<PlanillaHostTransferenciaAbono>();

                                        for (int x= 0;x < dtAbono.Rows.Count; x++)
                                        {
                                            #region Transferencia Abono

                                            PlanillaHostTransferenciaAbono abono = new PlanillaHostTransferenciaAbono();
                                            abono.Code = Guid.NewGuid().ToString().ToUpper();
                                            abono.Name = $"{Common.ACCION.T.ToString()}-{abono.Code}";
                                            abono.HTT_AB = abono.Code.ToUpper();
                                            abono.HTT_CA = cargos[i].HTT_CA;
                                            abono.HTH_PL = planilla.HTH_PL;
                                            abono.HTT_TF = transferencia.HTT_TF;

                                            abono.tipoRegistro = dtAbono.Rows[x]["tipoRegistro"].ToString();
                                            abono.secuencialFila = dtAbono.Rows[x]["secuencialFila"].ToString();
                                            abono.tipoCuenta = dtAbono.Rows[x]["tipoCuenta"].ToString();
                                            abono.cuentaPropia = dtAbono.Rows[x]["cuentaPropia"].ToString();
                                            abono.numeroCuentaAbono = dtAbono.Rows[x]["numeroCuentaAbono"].ToString();
                                            abono.tipoDocumentoBeneficiario = dtAbono.Rows[x]["tipoDocumentoBeneficiario"].ToString();
                                            abono.numeroDocumentoBeneficiario = dtAbono.Rows[x]["numeroDocumentoBeneficiario"].ToString();
                                            abono.correlativoDocumento = dtAbono.Rows[x]["correlativoDocumento"].ToString();
                                            abono.nombreBeneficiario = dtAbono.Rows[x]["nombreBeneficiario"].ToString();
                                            abono.monedaMontoTransferir = dtAbono.Rows[x]["monedaMontoTransferir"].ToString();
                                            abono.montoOperacion = dtAbono.Rows[x]["montoOperacion"].ToString();
                                            abono.referencia = dtAbono.Rows[x]["referencia"].ToString();
                                            abono.identificadorAbono = dtAbono.Rows[x]["identificadorAbono"].ToString();
                                            abono.titularCuenta = dtAbono.Rows[x]["titularCuenta"].ToString();
                                            abono.emailClienteBCP = dtAbono.Rows[x]["emailClienteBCP"].ToString();
                                            abono.filler = dtAbono.Rows[x]["filler"].ToString();
                                            abono.abonoChkSum = dtAbono.Rows[x]["abonoChkSum"].ToString();

                                            abonos.Add(abono);
                                            Menu.SBO_Application.StatusBar.SetText("PROCESANDO...ABONOS", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                            #endregion
                                        }

                                        if (!Common.AddTransferenciaAbonoV2(abonos,Menu.SBO_Company))
                                        {
                                            throw new Exception("No se puedo registrar los abonos");
                                        }
                                    }

                                    txtPlanilla.Value = string.Empty;
                                    txtComentario.Value = string.Empty;
                                    txtFechaInicio.Value = string.Empty;
                                    txtFechaFin.Value = string.Empty;
                                    Matrix.Clear();

                                    Menu.SBO_Application.StatusBar.SetText("PLANILLA DE TRANSFERENCIA FUE PROCESADO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                                }
                                else
                                {
                                    throw new Exception("No se puedo registrar datos del cargo");
                                }
                            }
                            else
                            {
                                throw new Exception("No se puedo registrar las transferencias");
                            }
                        }
                        else
                        {
                            throw new Exception("No se puedo recuperar los pagos realizados a proveedores");
                        }
                       
                    }
                    else
                    {
                        throw new Exception("No se puedo registrar los pagos a proveedores");
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Common.DeleteHostPlanillaTransferenciaByPLV2(txtPlanilla.Value,Menu.SBO_Company);

                    txtPlanilla.Value = string.Empty;
                    txtComentario.Value = string.Empty;
                    Menu.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
            else
            {
                Menu.SBO_Application.StatusBar.SetText("Debe seleccionar al menos una fila", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
        }

        private void txtCodigoProveedor_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {

            }
            catch (Exception)
            {

            }

        }
    }
}
