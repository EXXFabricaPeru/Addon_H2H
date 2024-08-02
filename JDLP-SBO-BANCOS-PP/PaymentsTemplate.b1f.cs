using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Data;
using System.Data.SqlClient;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;
using System.Text.RegularExpressions;

namespace JDLP_SBO_BANCOS_PP
{
    [FormAttribute("JDLP_SBO_BANCOS_PP.PaymentsTemplate", "PaymentsTemplate.b1f")]
    class PaymentsTemplate : UserFormBase
    {
        #region variables
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        SAPbouiCOM.DataTable dtc;
        #endregion
        #region Columna caplanilla

        private const string _Marcar = "Marcar";
        private const string _HTH_PL = "U_HTH_PL";
        private const string _HTH_CO = "U_HTH_CO";
        private const string _HTH_PR = "U_HTH_PR";
        private const string _HTH_FC = "U_HTH_FC";
        private const string _HTH_US = "U_HTH_US";

        #endregion



        public PaymentsTemplate()
        {
            this.UIAPIRawForm.Left = (Menu.SBO_Application.Desktop.Width / 2) - (UIAPIRawForm.Width / 2);
            this.UIAPIRawForm.Top = (Menu.SBO_Application.Desktop.Height / 2) - ((UIAPIRawForm.Height / 2) + 60);

            #region MTX CARGO
            dtc = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            SAPbouiCOM.Column oColumnaCargo;

            dtc.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_Marcar, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_HTH_PL, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_HTH_PR, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_HTH_US, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_HTH_FC, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtc.Columns.Add(_HTH_CO, SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            oColumnaCargo = mtxPlanilla.Columns.Item("#");
            oColumnaCargo.DataBind.Bind("DT_0", "#");

            oColumnaCargo = mtxPlanilla.Columns.Item("Col_0");
            oColumnaCargo.DataBind.Bind("DT_0", _Marcar);

            oColumnaCargo = mtxPlanilla.Columns.Item("Col_1");
            oColumnaCargo.DataBind.Bind("DT_0", _HTH_PL);

            oColumnaCargo = mtxPlanilla.Columns.Item("Col_2");
            oColumnaCargo.DataBind.Bind("DT_0", _HTH_PR);

            oColumnaCargo = mtxPlanilla.Columns.Item("Col_3");
            oColumnaCargo.DataBind.Bind("DT_0", _HTH_US);

            oColumnaCargo = mtxPlanilla.Columns.Item("Col_4");
            oColumnaCargo.DataBind.Bind("DT_0", _HTH_FC);

            oColumnaCargo = mtxPlanilla.Columns.Item("Col_5");
            oColumnaCargo.DataBind.Bind("DT_0", _HTH_CO);


            #endregion



        }

        #region diseño
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {

        }
        private SAPbouiCOM.Button btnBuscar;
        private SAPbouiCOM.Matrix mtxPlanilla;
        private SAPbouiCOM.Button btnDescargar;
        private SAPbouiCOM.Button btnEnviar;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText txtFechaInicio;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText txtFechaFin;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.ComboBox cboProceso;
        public override void OnInitializeComponent()
        {
            this.btnBuscar = ((SAPbouiCOM.Button)(this.GetItem("btnB").Specific));
            this.btnBuscar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnBuscar_ClickBefore);
            this.mtxPlanilla = ((SAPbouiCOM.Matrix)(this.GetItem("MTXC").Specific));
            this.mtxPlanilla.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxPlanilla_ClickBefore);
            //  this.mtxAbono.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxAbono_ClickBefore);
            this.btnDescargar = ((SAPbouiCOM.Button)(this.GetItem("btnD").Specific));
            this.btnDescargar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnDescargar_ClickBefore);
            this.btnEnviar = ((SAPbouiCOM.Button)(this.GetItem("btnE").Specific));
            this.btnEnviar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnEnviar_ClickBefore);
            //  this.mtxDocumento.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxDocumento_ClickBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.txtFechaInicio = ((SAPbouiCOM.EditText)(this.GetItem("txtFI").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.txtFechaFin = ((SAPbouiCOM.EditText)(this.GetItem("txtFF").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.cboProceso = ((SAPbouiCOM.ComboBox)(this.GetItem("cboPR").Specific));
            this.OnCustomInitialize();

        }

        #endregion
        private void btnBuscar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region variables 
            BubbleEvent = true;
            try
            {
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                #endregion

                #region Validacion
                if (string.IsNullOrEmpty(txtFechaInicio.Value.Trim())) { Menu.SBO_Application.StatusBar.SetText("Debe ingresar una fecha de inicio", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return; }

                if (string.IsNullOrEmpty(txtFechaFin.Value.Trim())) { Menu.SBO_Application.StatusBar.SetText("Debe ingresar una fecha de fin", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); return; }

                #endregion

                #region buscar
                parametros.Add("@vHTH_PR", "W");
                switch (cboProceso.Value)
                {
                    case "0":
                        break;
                    case "P":
                        parametros.Add("@vHTH_CA", "P");
                        break;
                    case "T":
                        parametros.Add("@vHTH_CA", "T");
                        break;
                }
                parametros.Add("@vFechaI", Common.formatearFecha(txtFechaInicio.Value));
                parametros.Add("@vFechaF", Common.formatearFecha(txtFechaFin.Value));


                //DataTable dtPlanilla = Common.getData("SP_EXXIS_PLANILLA_BCP_L_HOST_BY_ACCION", parametros);
                DataTable dtPlanilla = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_HOST_BY_ACCION", parametros,Menu.SBO_Company);

                if (dtPlanilla != null && dtPlanilla.Rows.Count > 0)
                    cargarMatrixCargo(dtPlanilla);
                else
                    Menu.SBO_Application.StatusBar.SetText("No hay resultados para mostrar", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                #endregion
            }
            catch (Exception e)
            {
                Menu.SBO_Application.StatusBar.SetText(e.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);

            }
            //string planillaId = txtPlanilla.Value.Trim();



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
                    dtc.SetValue(_Marcar, i, dt.Rows[i][_Marcar].ToString());
                    dtc.SetValue(_HTH_PL, i, dt.Rows[i][_HTH_PL].ToString());
                    dtc.SetValue(_HTH_PR, i, dt.Rows[i][_HTH_PR].ToString());
                    dtc.SetValue(_HTH_US, i, dt.Rows[i][_HTH_US].ToString());
                    dtc.SetValue(_HTH_FC, i, dt.Rows[i][_HTH_FC].ToString());
                    dtc.SetValue(_HTH_CO, i, dt.Rows[i][_HTH_CO].ToString());


                    #endregion
                }

                mtxPlanilla.LoadFromDataSourceEx();
                Menu.SBO_Application.StatusBar.SetText("PROCESO COMPLETADO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }
        private void mtxPlanilla_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string idMovimiento = (mtxPlanilla.Columns.Item("Col_1").Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();
            Console.WriteLine(idMovimiento);
        }
        private void mtxAbono_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        private void mtxDocumento_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        private void btnEnviar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            #region variables
            BubbleEvent = true;
            string proceso = cboProceso.Value;

            #endregion
            #region proceso
            if (proceso.Equals("P"))
            {
                #region provedores
                List<Entities.PlanillaHostBE> planillas = new List<Entities.PlanillaHostBE>();
                for (int i = 1; i <= mtxPlanilla.RowCount; i++)
                {
                    Entities.PlanillaHostBE planilla = new Entities.PlanillaHostBE();

                    if (((SAPbouiCOM.CheckBox)mtxPlanilla.Columns.Item("Col_0").Cells.Item(i).Specific).Checked == true)
                    {
                        planilla.HTH_PL = ((SAPbouiCOM.EditText)mtxPlanilla.Columns.Item("Col_1").Cells.Item(i).Specific).Value;

                        if (crearArchivoProveedores(planilla.HTH_PL))
                            logger.Info("");
                        else
                            logger.Info("");

                        planillas.Add(planilla);
                    }  
                }

                mtxPlanilla.Clear();

                #endregion
            }
            else if (proceso.Equals("T"))
            {
                #region transferencia
                List<Entities.PlanillaHostBE> planillas = new List<Entities.PlanillaHostBE>();
                for (int i = 1; i <= mtxPlanilla.RowCount; i++)
                {
                    Entities.PlanillaHostBE planilla = new Entities.PlanillaHostBE();
                    if (((SAPbouiCOM.CheckBox)mtxPlanilla.Columns.Item("Col_0").Cells.Item(i).Specific).Checked == true)
                    {
                        planilla.HTH_PL = ((SAPbouiCOM.EditText)mtxPlanilla.Columns.Item("Col_1").Cells.Item(i).Specific).Value;
                        if (crearArchivoTransferencia(planilla.HTH_PL))
                            logger.Info("se proceoso archivo");
                        else
                            logger.Info("No se envio archivo");

                        planillas.Add(planilla);
                    }
                }

                mtxPlanilla.Clear();

                #endregion
            }

            #endregion
        }

        private void btnDescargar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private bool crearArchivoProveedores(string planillaId)
        {
            #region variables
            bool respuesta = false;
            Dictionary<string, string> parametros;
            #endregion
            #region procesar

            parametros = new Dictionary<string, string>();
            parametros.Add("@vPlanilla", planillaId);

            DataTable dtc = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_CARGO_BY_PLANILLA", parametros,Menu.SBO_Company);

            foreach (DataRow rowc in dtc.Rows)
            {
                StringBuilder mensaje = new StringBuilder();
                #region bean planilla
                Entities.PlanillaHostBE planilla = new Entities.PlanillaHostBE();
                planilla.HTH_PL = rowc["U_HTH_PL"].ToString();
                planilla.HTH_NAE = Common.nombrarFileProveedorSFTP(DateTime.Now);
                planilla.HTH_PLH = $"{Common.pathProveedorIN}\\{planilla.HTH_NAE}.txt";
                planilla.HTH_SFTPP = Common.pathSFTPProveedorIN;
                #endregion
                #region bean cargos
                #region cargo
                Entities.PlanillaHostProveedorCargo cargo = new Entities.PlanillaHostProveedorCargo();
                cargo.Code = rowc["Code"].ToString();
                cargo.HTH_PL = rowc["U_HTH_PL"].ToString();
                cargo.HTP_PC = rowc["U_HTP_PC"].ToString();
                cargo.tipoRegistro = rowc["U_101_TR"].ToString();
                cargo.cantidadAbonosPlanilla = rowc["U_102_CAP"].ToString();
                cargo.fechaProceso = rowc["U_103_FP"].ToString();
                cargo.tipoCuentaCargo = rowc["U_104_TCC"].ToString();
                cargo.monedaCuentaCargo = rowc["U_105_MCC"].ToString();
                cargo.numeroCuentaCargo = rowc["U_106_NCC"].ToString();
                cargo.montoTotalPlanilla = rowc["U_107_MTP"].ToString();
                cargo.referenciaPlanilla = rowc["U_108_RP"].ToString();
                cargo.flagExoneracionITF = rowc["U_109_FEITF"].ToString();
                //cargo.totalControl = rowc["U_110_TC"].ToString();
                cargo.totalControl = rowc["TotalControCHK"].ToString();
                cargo.filler = rowc["U_111_FI"].ToString().PadRight(100);
                #endregion
                #region Linea Cargo
                mensaje.Append(cargo.tipoRegistro);
                mensaje.Append(cargo.cantidadAbonosPlanilla);
                mensaje.Append(cargo.fechaProceso);
                mensaje.Append(cargo.tipoCuentaCargo);
                mensaje.Append(cargo.monedaCuentaCargo);
                mensaje.Append(cargo.numeroCuentaCargo);
                mensaje.Append(cargo.montoTotalPlanilla);
                mensaje.Append(cargo.referenciaPlanilla);
                mensaje.Append(cargo.flagExoneracionITF);
                mensaje.Append(cargo.totalControl);
                //mensaje.Append(cargo.filler);
                mensaje.AppendLine();
                #endregion
                #endregion

                parametros = new Dictionary<string, string>();
                parametros.Add("@vPlanilla", planillaId);
                DataTable dta = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_ABONO_BY_PLANILLA", parametros, Menu.SBO_Company);

                foreach (DataRow rowa in dta.Rows)
                {
                    #region abonos
                    #region bean abono
                    Entities.PlanillaHostProveedorAbono abono = new Entities.PlanillaHostProveedorAbono();
                    abono.Code = rowa["Code"].ToString();
                    abono.HTH_PL = rowa["U_HTH_PL"].ToString();
                    abono.HTP_PC = rowa["U_HTP_PC"].ToString();
                    abono.HTA_PA = rowa["U_HTA_PA"].ToString();
                    abono.tipoRegistro = rowa["U_201_TR"].ToString();
                    abono.tipoCuentaAbono = rowa["U_202_TCA"].ToString();
                    abono.numeroCuentaAbono = rowa["U_203_NCA"].ToString();
                    abono.modalidadPago = rowa["U_204_MP"].ToString();
                    abono.tipoDocumentoProveedor = rowa["U_205_TDP"].ToString();
                    abono.numeroDocumentoProveedor = rowa["U_206_NDP"].ToString();
                    abono.correlativoDocumentoProveedor = rowa["U_207_CDP"].ToString();
                    abono.nombreProveedor = rowa["U_208_NP"].ToString();
                    abono.referenciaBeneficiario = rowa["U_209_RB"].ToString();
                    abono.referenciaEmpresa = rowa["U_210_RE"].ToString();
                    abono.monedaImporteAbonar = rowa["U_211_MIA"].ToString();
                    abono.importeAbonar = rowa["U_212_IA"].ToString();
                    abono.flagValidarIDC = rowa["U_213_FVIDC"].ToString();
                    abono.Filler = rowa["U_214_FI"].ToString().PadRight(100);



                    #endregion
                    #region linea abono
                    mensaje.Append(abono.tipoRegistro);
                    mensaje.Append(abono.tipoCuentaAbono);
                    mensaje.Append(abono.numeroCuentaAbono);
                    mensaje.Append(abono.modalidadPago);
                    mensaje.Append(abono.tipoDocumentoProveedor);
                    mensaje.Append(abono.numeroDocumentoProveedor);
                    mensaje.Append(abono.correlativoDocumentoProveedor);
                    mensaje.Append(abono.nombreProveedor); //(Regex.Replace(abono.nombreProveedor.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", ""));
                    mensaje.Append(abono.referenciaBeneficiario);
                    mensaje.Append(abono.referenciaEmpresa);
                    mensaje.Append(abono.monedaImporteAbonar);
                    mensaje.Append(abono.importeAbonar);
                    mensaje.Append(abono.flagValidarIDC);
                    //mensaje.Append(abono.Filler);
                    mensaje.AppendLine();
                    #endregion
                    #endregion

                    parametros = new Dictionary<string, string>();
                    parametros.Add("@vPlanilla", abono.HTA_PA); //PLANILLA DEL ABONO
                    DataTable dtd = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_PROVEEDOR_DOCUMENTO_BY_PLANILLA", parametros, Menu.SBO_Company);

                    foreach (DataRow rowd in dtd.Rows)
                    {
                        #region documentos
                        #region documento
                        Entities.PlanillaHostProveedorDocumento documento = new Entities.PlanillaHostProveedorDocumento();
                        documento.Code = rowd["Code"].ToString();
                        documento.HTH_PL = rowd["U_HTH_PL"].ToString();
                        documento.HTP_PC = rowd["U_HTP_PC"].ToString();
                        documento.HTA_PA = rowd["U_HTA_PA"].ToString();
                        documento.HTD_PD = rowd["U_HTD_PD"].ToString();
                        documento.tipoRegistro = rowd["U_301_TR"].ToString();
                        documento.tipoDocumnetoPagar = rowd["U_302_TDP"].ToString();
                        documento.numeroDocumentoPagar = rowd["U_303_NDP"].ToString();
                        documento.importeDocumnetoPagar = rowd["U_304_IDP"].ToString();

                        #endregion
                        #region linea documento 
                        mensaje.Append(documento.tipoRegistro);
                        mensaje.Append(documento.tipoDocumnetoPagar);
                        mensaje.Append(documento.numeroDocumentoPagar);
                        mensaje.Append(documento.importeDocumnetoPagar);
                        mensaje.AppendLine();
                        #endregion
                        #endregion
                    }
                }

                #region Estructura Archivo

                using (FileStream fs = File.Create(planilla.HTH_PLH))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(mensaje.ToString());
                    fs.Write(info, 0, info.Length);
                    fs.Close();

                    Menu.SBO_Application.StatusBar.SetText("PLANILLA DE PROVEEDORES GENERADO CON EXITO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    if (subirArchivoSftp(planilla))
                    {
                        planilla.HTH_PLG = "G";
                        respuesta = true;
                    }
                    else
                    {
                        //planilla.HTH_PLG = "E";
                        respuesta = false;
                    }

                    Common.UpdateHostPlanillaV2(planilla,Menu.SBO_Company);
                }
                #endregion
            }

            #endregion
            return respuesta;

        }

        private bool crearArchivoTransferencia(string planillaId)
        {
            #region variables
            bool respuesta = false;
            Dictionary<string, string> parametros;
            List<Entities.PlanillaHostBE> planillas = new List<Entities.PlanillaHostBE>();
            #endregion
            #region procesar

            parametros = new Dictionary<string, string>();
            parametros.Add("@vPlanilla", planillaId);

            DataTable dtt = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA", parametros, Menu.SBO_Company);

            foreach (DataRow rowt in dtt.Rows)
            {
                StringBuilder mensaje = new StringBuilder();
                #region bean planilla
                Entities.PlanillaHostBE planilla = new Entities.PlanillaHostBE();
                planilla.HTH_PL = rowt["U_HTH_PL"].ToString();
                planilla.HTH_NAE = Common.nombrarFileTransferenciaSFTP(DateTime.Now);
                planilla.HTH_PLH = $"{Common.pathTransferenciaIN}\\{planilla.HTH_NAE}.txt";
                planilla.HTH_SFTPT = Common.pathSFTPTransferenciaIN;
                #endregion
                #region bean transferencias
                #region transferencia
                Entities.PlanillaHostTransferencia transferencia = new Entities.PlanillaHostTransferencia();
                transferencia.Code = rowt["Code"].ToString();
                transferencia.HTT_TF = rowt["U_HTT_TF"].ToString();
                transferencia.HTH_PL = rowt["U_HTH_PL"].ToString();
                transferencia.tipoRegistro = rowt["U_101_TR"].ToString();
                transferencia.secuencialFila = rowt["U_102_SF"].ToString();
                //transferencia.cantidadCuentasArchivo = rowt["U_103_CCA"].ToString();
                transferencia.cantidadCuentasArchivo = rowt["103_CCA"].ToString();
                transferencia.identificadorCabecera = rowt["U_104_IC"].ToString();
                transferencia.filler = rowt["U_105_FI"].ToString();
                #endregion
                #region Linea transferencia
                mensaje.Append(transferencia.tipoRegistro);
                mensaje.Append(transferencia.secuencialFila);
                mensaje.Append(transferencia.cantidadCuentasArchivo);
                mensaje.Append(transferencia.identificadorCabecera);
                mensaje.Append(transferencia.filler.PadRight(305));
                mensaje.AppendLine();
                #endregion
                #endregion

                parametros = new Dictionary<string, string>();
                parametros.Add("@vPlanilla", planillaId);
                DataTable dtc = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA_CARGO", parametros, Menu.SBO_Company);

                foreach (DataRow rowc in dtc.Rows)
                {
                    #region cargos
                    #region bean cargo
                    Entities.PlanillaHostTransferenciaCargo cargo = new Entities.PlanillaHostTransferenciaCargo();
                    cargo.Code = rowc["Code"].ToString();
                    cargo.HTT_CA = rowc["U_HTT_CA"].ToString();
                    cargo.HTH_PL = rowc["U_HTH_PL"].ToString();
                    cargo.HTT_TF = rowc["U_HTT_TF"].ToString();
                    cargo.tipoRegistro = rowc["U_201_TR"].ToString();
                    cargo.secuencialFila = rowc["U_202_SF"].ToString();
                    cargo.cantidadAbonosPlanilla = rowc["U_203_CAP"].ToString();
                    cargo.tipoCuentaCargo = rowc["U_204_TCC"].ToString();
                    cargo.monedaCuentaCargo = rowc["U_205_MCC"].ToString();
                    cargo.numeroCuentaCargo = rowc["U_206_NCC"].ToString();
                    cargo.montoTotalSoles = rowc["U_207_MTS"].ToString();
                    cargo.montoTotalDolares = rowc["U_208_MTD"].ToString();
                    //cargo.totalControl = rowc["U_209_TC"].ToString();
                    cargo.totalControl = rowc["TotalControCHK"].ToString();
                    cargo.identificadorCargo = rowc["U_210_IC"].ToString();
                    cargo.filler = rowc["U_211_FI"].ToString();
                    #endregion
                    #region linea abono
                    mensaje.Append(cargo.tipoRegistro);
                    mensaje.Append(cargo.secuencialFila);
                    mensaje.Append(cargo.cantidadAbonosPlanilla);
                    mensaje.Append(cargo.tipoCuentaCargo);
                    mensaje.Append(cargo.monedaCuentaCargo);
                    mensaje.Append(cargo.numeroCuentaCargo);
                    mensaje.Append(cargo.montoTotalSoles);
                    mensaje.Append(cargo.montoTotalDolares);
                    mensaje.Append(cargo.totalControl);
                    mensaje.Append(cargo.identificadorCargo);
                    mensaje.Append(cargo.filler.PadRight(231));

                    mensaje.AppendLine();
                    #endregion
                    #endregion

                    parametros = new Dictionary<string, string>();
                    parametros.Add("@vPlanilla", cargo.HTT_CA); //PLANILLA DEL ABONO
                    DataTable dta = Common.getDataHanaV2("SP_EXXIS_PLANILLA_BCP_L_TRANSFERENCIA_ABONO", parametros, Menu.SBO_Company);

                    foreach (DataRow rowa in dta.Rows)
                    {
                        #region abonos
                        #region abono
                        Entities.PlanillaHostTransferenciaAbono abono = new Entities.PlanillaHostTransferenciaAbono();
                        abono.Code = rowa["Code"].ToString();
                        abono.HTT_AB = rowa["U_HTT_AB"].ToString();
                        abono.HTT_CA = rowa["U_HTT_CA"].ToString();
                        abono.HTH_PL = rowa["U_HTH_PL"].ToString();
                        abono.HTT_TF = rowa["U_HTT_TF"].ToString();
                        abono.tipoRegistro = rowa["U_301_TR"].ToString();
                        abono.secuencialFila = rowa["U_302_SF"].ToString();
                        abono.tipoCuenta = rowa["U_303_TC"].ToString();
                        abono.cuentaPropia = rowa["U_304_CP"].ToString();
                        abono.numeroCuentaAbono = rowa["U_305_NCA"].ToString();
                        abono.tipoDocumentoBeneficiario = rowa["U_306_TDB"].ToString();
                        abono.numeroDocumentoBeneficiario = rowa["U_307_NDB"].ToString();
                        abono.correlativoDocumento = rowa["U_308_CD"].ToString();
                        abono.nombreBeneficiario = rowa["U_309_NB"].ToString();
                        abono.monedaMontoTransferir = rowa["U_310_MMT"].ToString();
                        abono.montoOperacion = rowa["U_311_MO"].ToString();
                        abono.referencia = rowa["U_312_RE"].ToString();
                        abono.identificadorAbono = rowa["U_313_IA"].ToString();
                        abono.titularCuenta = rowa["U_314_TC"].ToString();
                        abono.emailClienteBCP = rowa["U_315_ECBCP"].ToString();
                        abono.filler = rowa["U_316_FI"].ToString();
                        #endregion
                        #region linea documento 
                        mensaje.Append(abono.tipoRegistro);
                        mensaje.Append(abono.secuencialFila);
                        mensaje.Append(abono.tipoCuenta);
                        mensaje.Append(abono.cuentaPropia);
                        mensaje.Append(abono.numeroCuentaAbono);
                        mensaje.Append(abono.tipoDocumentoBeneficiario);
                        mensaje.Append(abono.numeroDocumentoBeneficiario);
                        mensaje.Append(abono.correlativoDocumento);
                        mensaje.Append(abono.nombreBeneficiario);
                        mensaje.Append(abono.monedaMontoTransferir);
                        mensaje.Append(abono.montoOperacion);
                        mensaje.Append(abono.referencia);
                        mensaje.Append(abono.identificadorAbono);
                        mensaje.Append(abono.titularCuenta);
                        mensaje.Append(abono.emailClienteBCP);
                        mensaje.Append(abono.filler.PadRight(34));


                        mensaje.AppendLine();
                        #endregion
                        #endregion
                    }
                }

                #region Estructura Archivo

                using (FileStream fs = File.Create(planilla.HTH_PLH))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(mensaje.ToString());
                    fs.Write(info, 0, info.Length);
                    fs.Close();

                    Menu.SBO_Application.StatusBar.SetText("PLANILLA DE TRANSFERENCIA GENERADO CON EXITO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    if (subirArchivoSftp(planilla))
                    {
                        planilla.HTH_PLG = "G";
                        respuesta = true;
                    }
                    else
                    {
                        planilla.HTH_PLG = "E";
                        respuesta = false;
                    }

                    Common.UpdateHostPlanillaV2(planilla,Menu.SBO_Company);
                }
                #endregion
            }

            #endregion
            return respuesta;
        }

        private bool subirArchivoSftp(Entities.PlanillaHostBE planilla)
        {
            bool respuesta = false;
            //NEW
            //string servidor = "216.244.162.230";
            //int puerto = 7022;
            //string usuario = "FTBCPA02";
            //string clave = "TRECA987uy";

            //OLD
            const string servidor = "200.37.27.177";
            const int puerto = 1722;
            const string usuario = "FTTH2H58";
            const string clave = "WQZQbzC23S";
            //200.37.27.177

            //PROD
            //const string servidor = "200.37.27.177";
            //const int puerto = 7022;
            //const string usuario = "FTTH2H58";
            //const string clave = "WQZQbzC23S";

            try
            {
                //using (SftpClient sftp = new SftpClient(servidor, puerto, usuario, clave))
                //{

                int intentosMaximos = 5;
                int tiempoEsperaEntreIntentosEnMilisegundos = 5000; // 5 segundos
                for (int intento = 1; intento <= intentosMaximos; intento++)
                {
                    try
                    {
                        var sftp = new SftpClient(servidor, puerto, usuario, clave);
                        sftp.Connect();
                        Menu.SBO_Application.StatusBar.SetText("CONECTANDO AL SERVIDOR SFTP", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                        Console.WriteLine($"Conexion exitosa");
                        //PROCESO
                        if (sftp.IsConnected)
                        {
                            using (var fs = new FileStream(planilla.HTH_PLH, FileMode.Open))
                            {
                                if (Path.GetFileName(planilla.HTH_PLH).Substring(0, 1).Equals("T"))
                                {
                                    sftp.UploadFile(fs, string.Concat(Common.pathSFTPTransferenciaIN, Path.GetFileName(planilla.HTH_PLH)));
                                    respuesta = true;
                                }
                                else
                                {
                                    sftp.UploadFile(fs, string.Concat(Common.pathSFTPProveedorIN, Path.GetFileName(planilla.HTH_PLH)));
                                    respuesta = true;
                                }

                                Menu.SBO_Application.StatusBar.SetText("LA PLANILLA FUE ENVIADA AL BANCO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                            }
                        }
                        else
                        {

                            logger.Error($"La conexion SFTP esta cerrada: servidor: {servidor} puerto: {puerto.ToString()} usuario: {usuario}");
                        }
                        // Si la conexión se establece correctamente, establecemos el marcador de conexión exitosa y salimos del bucle
                        //FIN PROCESO
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Si ocurre algún error durante la conexión, mostramos un mensaje y esperamos un tiempo antes de intentarlo de nuevo
                        logger.Error($"Intento {intento} fallido: {ex.Message}");
                        Menu.SBO_Application.StatusBar.SetText($"Intento {intento} fallido: {ex.Message}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                        System.Threading.Thread.Sleep(tiempoEsperaEntreIntentosEnMilisegundos);
                    }
                }
                //var sftp = new SftpClient(servidor, puerto, usuario, clave);
                //sftp.Connect();

                //if (sftp.IsConnected)
                //{
                //    using (var fs = new FileStream(planilla.HTH_PLH, FileMode.Open))
                //    {
                //        if (Path.GetFileName(planilla.HTH_PLH).Substring(0, 1).Equals("T"))
                //        {
                //            sftp.UploadFile(fs, string.Concat(Common.pathSFTPTransferenciaIN, Path.GetFileName(planilla.HTH_PLH)));
                //            respuesta = true;
                //        }
                //        else
                //        {
                //            sftp.UploadFile(fs, string.Concat(Common.pathSFTPProveedorIN, Path.GetFileName(planilla.HTH_PLH)));
                //            respuesta = true;
                //        }

                //        Menu.SBO_Application.StatusBar.SetText("LA PLANILLA FUE ENVIADA AL BANCO", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                //    }
                //}
                //else
                //{
                //    logger.Error($"La conexion SFTP esta cerrada: servidor: {servidor} puerto: {puerto.ToString()} usuario: {usuario}");
                //}
                //}
            }
            catch (Exception ex)
            {
                respuesta = false;
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return respuesta;
        }
    }
}
