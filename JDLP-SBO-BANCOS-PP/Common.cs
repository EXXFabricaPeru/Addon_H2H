using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JDLP_SBO_BANCOS_PP.Entities;
using System.Globalization;
using System.IO;
using Sap.Data.Hana;
using System.Configuration;

namespace JDLP_SBO_BANCOS_PP
{
    public static class Common
    {
        #region enumeracion
        public enum ACCION
        {
            T, //TRANSFERENCIA
            P // PROVEEDOR
        }
        public enum ESTADO_PLANILLA
        {
            W, //PENDIENTE
            C, //COMPLETO
            P, // PARCIAL
            E // ERROR
        }

        #endregion
        #region  variables sftp
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string pathTransferenciaIN = @"C:\LOCAL_SFTP\TRANSFERENCIA\IN";
        public static string pathTransferenciaOU = @"C:\LOCAL_SFTP\TRANSFERENCIA\OU";

        public static string pathProveedorIN = @"C:\LOCAL_SFTP\PROVEEDOR\IN";
        public static string pathProveedorOU = @"C:\LOCAL_SFTP\PROVEEDOR\OU";
        public static string SBO_LOCATE;

        //OLD
        //public static string pathSFTPTransferenciaIN = "/sftp/FTTH2H58/TLC_Transferencias/IN/";
        //public static string pathSFTPTransferenciaOU = "/FTTH2H58/TLC_Transferencias/OU/";

        //public static string pathSFTPProveedorIN = "/sftp/FTTH2H58/TLC_PagosMasivos/IN/";
        //public static string pathSFTPProveedorOU = "/FTTH2H58/TLC_PagosMasivos/OU/";

        //NEW /sftp/FTTH2H58/TLC_PagosMasivos/IN
        public static string pathSFTPTransferenciaIN = "/sftp/FTTH2H58/TLC_Transferencias/IN/";
        public static string pathSFTPTransferenciaOU = "/sftp/FTTH2H58/TLC_Transferencias/OUT/";

        public static string pathSFTPProveedorIN = "/sftp/FTTH2H58/TLC_PagosMasivos/IN/";
        public static string pathSFTPProveedorOU = "/sftp/FTTH2H58/TLC_PagosMasivos/OUT/";

        #endregion
        private static string cnxSCO_INTCO()
        {
            const string SERVER = "svr-sap";
            //const string SBO_LOCATE = "DB_FUNERARIA_JARDINES";
            const string DBUSER = "jardines";
            const string DBPSW = "C3ntr4l.P4ssw0rd";

            return $"Data Source={SERVER};Initial Catalog={SBO_LOCATE};User Id={DBUSER};Password={DBPSW};";
        }
        public static DataTable getData(string procedure, Dictionary<string, string> parametros)
        {
            #region Variables
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            #endregion
            #region ExecuteNonQuery
            using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
            {
                try
                {
                    cmd = new SqlCommand(procedure, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, string> param in parametros)
                    {
                        cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    logger.Error($"{procedure} : {ex.Message}");
                    logger.Error(ex.StackTrace);
                }
            }

            return dt;

            #endregion
        }
       
        public static HanaConnectionStringBuilder ConnectionStringHANA()
        {
            string SERVER = ConfigurationManager.AppSettings["SERVER"].ToString();
            string DBUSER = ConfigurationManager.AppSettings["USER"].ToString();
            string DBPSW = ConfigurationManager.AppSettings["PASS"].ToString();
            HanaConnectionStringBuilder builder = new HanaConnectionStringBuilder
            {
                ConnectionTimeout = 0,
                Server = SERVER,
                Pooling = true,
                UserName = DBUSER,
                Password = DBPSW
            };
            return builder;
        }

        public static DataTable getDataHana(string procedure, Dictionary<string, string> parametros)
        {
            #region Variables
            HanaCommand cmd = new HanaCommand();
            HanaDataAdapter da = new HanaDataAdapter();
            DataTable dt = new DataTable();

            #endregion
            #region ExecuteNonQuery
            using (HanaConnection connection = new HanaConnection(ConnectionStringHANA().ConnectionString))
            {
                try
                {
                    cmd = new HanaCommand("\""+ "sBO_Company.CompanyDB"+"\"."+procedure, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, string> param in parametros)
                    {
                        cmd.Parameters.Add(new HanaParameter(param.Key, param.Value));
                    }

                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    logger.Error($"{procedure} : {ex.Message}");
                    logger.Error(ex.StackTrace);
                }
            }
            return dt;

            #endregion
        }

        public static DataTable getDataHanaV2(string procedure, Dictionary<string, string> parametros, SAPbobsCOM.Company sBO_Company)
        {
            DataTable dt = new DataTable();

            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                // Crear el comando para el procedimiento almacenado
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append($"CALL \""+ sBO_Company.CompanyDB+"\".\"{procedure}\" (");
                //sbQuery.Append($"CALL \""+ sBO_Company.CompanyDB+"\".\"{procedure}\" (");

                bool first = true;
                foreach (var param in parametros)
                {
                    if (!first)
                    {
                        sbQuery.Append(", ");
                    }
                    sbQuery.Append($"'{param.Value}'");
                    first = false;
                }
                sbQuery.Append(")");
                logger.Error(sbQuery.ToString());
                oRecordset.DoQuery(sbQuery.ToString());
             
                // Crear las columnas del DataTable dinámicamente
                for (int i = 0; i < oRecordset.Fields.Count; i++)
                {
                    dt.Columns.Add(oRecordset.Fields.Item(i).Name, typeof(object));
                }
                while (!oRecordset.EoF)
                {
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < oRecordset.Fields.Count; i++)
                    {
                        row[i] = oRecordset.Fields.Item(i).Value;
                    }
                    dt.Rows.Add(row);
                    oRecordset.MoveNext();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{procedure} : {ex.Message}");
                logger.Error(ex.StackTrace);
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return dt;

        }
        public static bool addHostPlanilla(List<Entities.PlanillaHostBE> planillas)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            int cantidad = planillas.Count;

            #region Interacion de datatable
            foreach (var bean in planillas)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_WIZARD", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vHTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vHTH_CO", bean.HTH_CO);
                        cmd.Parameters.AddWithValue("@vHTH_PR", bean.HTH_PR);
                        cmd.Parameters.AddWithValue("@vHTH_FC", bean.HTH_FC);
                        cmd.Parameters.AddWithValue("@vHTH_FP", bean.HTH_FP);
                        cmd.Parameters.AddWithValue("@vHTH_US", bean.HTH_US);
                        cmd.Parameters.AddWithValue("@vHTH_CA", bean.HTH_CA);
                        cmd.Parameters.AddWithValue("@vHTH_NAE", bean.HTH_NAE);
                        cmd.Parameters.AddWithValue("@vHTH_NAR1", bean.HTH_NAR1);
                        cmd.Parameters.AddWithValue("@vHTH_NAR2", bean.HTH_NAR2);
                        cmd.Parameters.AddWithValue("@vHTH_PLH", bean.HTH_PLH);
                        cmd.Parameters.AddWithValue("@vHTH_WIZ", bean.HTH_WIZ);
                        cmd.Parameters.AddWithValue("@vHTH_WIZFI", bean.HTH_WIZFI);
                        cmd.Parameters.AddWithValue("@vHTH_WIZFF", bean.HTH_WIZFF);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_WIZARD: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }
            #endregion

            return respuesta;
        }

        public static bool addHostPlanillaV2(List<Entities.PlanillaHostBE> planillas, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in planillas)
                {
                    try
                    {
                        // Crear el comando para el procedimiento almacenado
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_WIZARD\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTH_CO}', ");
                        sbQuery.Append($"'{bean.HTH_PR}', ");
                        sbQuery.Append($"'{bean.HTH_FC}', ");
                        sbQuery.Append($"'{bean.HTH_FP}', ");
                        sbQuery.Append($"'{bean.HTH_US}', ");
                        sbQuery.Append($"'{bean.HTH_CA}', ");
                        sbQuery.Append($"'{bean.HTH_NAE}', ");
                        sbQuery.Append($"'{bean.HTH_NAR1}', ");
                        sbQuery.Append($"'{bean.HTH_NAR2}', ");
                        sbQuery.Append($"'{bean.HTH_PLH}', ");
                        sbQuery.Append($"'{bean.HTH_WIZ}', ");
                        sbQuery.Append($"'{bean.HTH_WIZFI}', ");
                        sbQuery.Append($"'{bean.HTH_WIZFF}'");
                        sbQuery.Append(")");
                        logger.Error($"SP_EXX_BCP_A_WIZARD: {sbQuery.ToString()}");
                        oRecordset.DoQuery(sbQuery.ToString());

                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_WIZARD: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }

        public static bool removeHostPlanilla(List<Entities.PlanillaHostBE> planillas)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            int cantidad = planillas.Count;

            #region Interacion de datatable
            foreach (var bean in planillas)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXXIS_PLANILLA_BCP_D_HTH_PLANILLA_BY_PL", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vPL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vCA", bean.HTH_CA);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXXIS_PLANILLA_BCP_D_HTH_PLANILLA_BY_PL: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }
            #endregion

            return respuesta;
        }

        public static bool RemoveHostPlanillaV2(List<Entities.PlanillaHostBE> planillas, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in planillas)
                {
                    try
                    {
                        // Crear el comando para el procedimiento almacenado
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXXIS_PLANILLA_BCP_D_HTH_PLANILLA_BY_PL\" (");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTH_CA}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXXIS_PLANILLA_BCP_D_HTH_PLANILLA_BY_PL: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }


        public static bool addProveedorPago(List<Entities.PlanillaHostProveedorPago> pagos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            int cantidad = pagos.Count;

            #region Interacion de datatable
            foreach (var bean in pagos)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_RESUMEN_PAGO", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vHTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vWIZNAME", bean.WIZNAME);
                        cmd.Parameters.AddWithValue("@vWIZFI", bean.WIZFI);
                        cmd.Parameters.AddWithValue("@vWIZFF", bean.WIZFF);
                        cmd.Parameters.AddWithValue("@vFECHA", bean.Fecha);


                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_RESUMEN_PAGO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }
            #endregion

            return respuesta;
        }

        public static bool AddProveedorPagoV2(List<Entities.PlanillaHostProveedorPago> pagos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in pagos)
                {
                    try
                    {
                        // Crear el comando para el procedimiento almacenado
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_RESUMEN_PAGO\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.WIZNAME}', ");
                        sbQuery.Append($"'{bean.WIZFI:yyyy-MM-dd}', ");  // Asumiendo que bean.WIZFI es DateTime
                        sbQuery.Append($"'{bean.WIZFF:yyyy-MM-dd}', ");  // Asumiendo que bean.WIZFF es DateTime
                        sbQuery.Append($"'{bean.Fecha:yyyy-MM-dd}'");  // Asumiendo que bean.Fecha es DateTime
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_RESUMEN_PAGO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }
            }

            return respuesta;
        }


        public static bool addProveedorCargo(List<PlanillaHostProveedorCargo> cargos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in cargos)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_PROVEEDOR_CARGO", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vU_HTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vU_HTP_PC", bean.HTP_PC);
                        cmd.Parameters.AddWithValue("@vU_101_TR", bean.tipoRegistro);
                        cmd.Parameters.AddWithValue("@vU_102_CA", bean.cantidadAbonosPlanilla);
                        cmd.Parameters.AddWithValue("@vU_103_FP", bean.fechaProceso);
                        cmd.Parameters.AddWithValue("@vU_104_TC", bean.tipoCuentaCargo);
                        cmd.Parameters.AddWithValue("@vU_105_MC", bean.monedaCuentaCargo);
                        cmd.Parameters.AddWithValue("@vU_106_NC", bean.numeroCuentaCargo);
                        cmd.Parameters.AddWithValue("@vU_107_MT", bean.montoTotalPlanilla);
                        cmd.Parameters.AddWithValue("@vU_108_RP", bean.referenciaPlanilla);
                        cmd.Parameters.AddWithValue("@vU_109_FE", bean.flagExoneracionITF);
                        cmd.Parameters.AddWithValue("@vU_110_TC", bean.totalControl);
                        cmd.Parameters.AddWithValue("@vCCHK", bean.cargoChkSum);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_WIZARD: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddProveedorCargoV2(List<PlanillaHostProveedorCargo> cargos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in cargos)
                {
                    try
                    {
                        // Crear el comando para el procedimiento almacenado
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_PROVEEDOR_CARGO\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTP_PC}', ");
                        sbQuery.Append($"'{bean.tipoRegistro}', ");
                        sbQuery.Append($"'{bean.cantidadAbonosPlanilla}', ");
                        sbQuery.Append($"'{bean.fechaProceso:yyyy-MM-dd}', ");  // Asumiendo que bean.fechaProceso es DateTime
                        sbQuery.Append($"'{bean.tipoCuentaCargo}', ");
                        sbQuery.Append($"'{bean.monedaCuentaCargo}', ");
                        sbQuery.Append($"'{bean.numeroCuentaCargo}', ");
                        sbQuery.Append($"'{bean.montoTotalPlanilla}', ");
                        sbQuery.Append($"'{bean.referenciaPlanilla}', ");
                        sbQuery.Append($"'{bean.flagExoneracionITF}', ");
                        sbQuery.Append($"'{bean.totalControl}', ");
                        sbQuery.Append($"'{bean.cargoChkSum}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_PROVEEDOR_CARGO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }
            }

            return respuesta;
        }


        public static bool addProveedorAbono(List<PlanillaHostProveedorAbono> abonos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in abonos.OrderBy(t=>t.order))
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_PROVEEDOR_ABONO", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vU_HTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vU_HTP_PC", bean.HTP_PC);
                        cmd.Parameters.AddWithValue("@vU_HTA_PA", bean.HTA_PA);
                        cmd.Parameters.AddWithValue("@vU_201_TR", bean.tipoRegistro);
                        cmd.Parameters.AddWithValue("@vU_202_TCA", bean.tipoCuentaAbono);
                        cmd.Parameters.AddWithValue("@vU_203_NCA", bean.numeroCuentaAbono);
                        cmd.Parameters.AddWithValue("@vU_204_MP", bean.modalidadPago);
                        cmd.Parameters.AddWithValue("@vU_205_TDP", bean.tipoDocumentoProveedor);
                        cmd.Parameters.AddWithValue("@vU_206_NDP", bean.numeroDocumentoProveedor);
                        cmd.Parameters.AddWithValue("@vU_207_CDP", bean.correlativoDocumentoProveedor);
                        cmd.Parameters.AddWithValue("@vU_208_NP", bean.nombreProveedor);
                        cmd.Parameters.AddWithValue("@vU_209_RB", bean.referenciaBeneficiario);
                        cmd.Parameters.AddWithValue("@vU_210_RE", bean.referenciaEmpresa);
                        cmd.Parameters.AddWithValue("@vU_211_MIA", bean.monedaImporteAbonar);
                        cmd.Parameters.AddWithValue("@vU_212_IA", bean.importeAbonar);
                        cmd.Parameters.AddWithValue("@vU_213_FVIDC", bean.flagValidarIDC);
                        cmd.Parameters.AddWithValue("@vACHK", bean.abonoChkSum);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_PROVEEDOR_ABONO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddProveedorAbonoV2(List<PlanillaHostProveedorAbono> abonos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in abonos.OrderBy(t => t.order))
                {
                    try
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_PROVEEDOR_ABONO\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTP_PC}', ");
                        sbQuery.Append($"'{bean.HTA_PA}', ");
                        sbQuery.Append($"'{bean.tipoRegistro}', ");
                        sbQuery.Append($"'{bean.tipoCuentaAbono}', ");
                        sbQuery.Append($"'{bean.numeroCuentaAbono}', ");
                        sbQuery.Append($"'{bean.modalidadPago}', ");
                        sbQuery.Append($"'{bean.tipoDocumentoProveedor}', ");
                        sbQuery.Append($"'{bean.numeroDocumentoProveedor}', ");
                        sbQuery.Append($"'{bean.correlativoDocumentoProveedor}', ");
                        sbQuery.Append($"'{bean.nombreProveedor}', ");
                        sbQuery.Append($"'{bean.referenciaBeneficiario}', ");
                        sbQuery.Append($"'{bean.referenciaEmpresa}', ");
                        sbQuery.Append($"'{bean.monedaImporteAbonar}', ");
                        sbQuery.Append($"{bean.importeAbonar}, "); // No comillas para valores numéricos
                        sbQuery.Append($"'{bean.flagValidarIDC}', ");
                        sbQuery.Append($"'{bean.abonoChkSum}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_PROVEEDOR_ABONO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }
            }

            return respuesta;
        }


        public static bool addProveedorDocumento(List<PlanillaHostProveedorDocumento> documentos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in documentos)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_PROVEEDOR_DOCUMENTO", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vU_HTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vU_HTP_PC", bean.HTP_PC);
                        cmd.Parameters.AddWithValue("@vU_HTA_PA", bean.HTA_PA);
                        cmd.Parameters.AddWithValue("@vU_HTD_PD", bean.HTD_PD);
                        cmd.Parameters.AddWithValue("@vU_301_TR", bean.tipoRegistro);
                        cmd.Parameters.AddWithValue("@vU_302_TDP", bean.tipoDocumnetoPagar);
                        cmd.Parameters.AddWithValue("@vU_303_NDP", bean.numeroDocumentoPagar);
                        cmd.Parameters.AddWithValue("@vU_304_IDP", bean.importeDocumnetoPagar);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_PROVEEDOR_DOCUMENTO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddProveedorDocumentoV2(List<PlanillaHostProveedorDocumento> documentos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in documentos)
                {
                    try
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_PROVEEDOR_DOCUMENTO\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTP_PC}', ");
                        sbQuery.Append($"'{bean.HTA_PA}', ");
                        sbQuery.Append($"'{bean.HTD_PD}', ");
                        sbQuery.Append($"'{bean.tipoRegistro}', ");
                        sbQuery.Append($"'{bean.tipoDocumnetoPagar}', ");
                        sbQuery.Append($"'{bean.numeroDocumentoPagar}', ");
                        sbQuery.Append($"{bean.importeDocumnetoPagar}"); // No comillas para valores numéricos
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_PROVEEDOR_DOCUMENTO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }

        public static string formatearFecha(string fecha)
        {
            return DateTime.ParseExact(fecha, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
        }

        public static bool addTransferenciaProveedor(List<Entities.VendorPayments> pagos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in pagos)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_TRANSFERENCIA_OVPM", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vDocEntry", bean.Entry);
                        cmd.Parameters.AddWithValue("@vDocNum", bean.Numero);
                        cmd.Parameters.AddWithValue("@vDocDate", bean.Fecha);
                        cmd.Parameters.AddWithValue("@vDocCurr", bean.Moneda);
                        cmd.Parameters.AddWithValue("@vDocTotal", bean.Valor);
                        cmd.Parameters.AddWithValue("@vTrsfrAcct", bean.Cuenta);
                        cmd.Parameters.AddWithValue("@vHTH_PL", bean.HTH_PL);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_OVPM: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddTransferenciaProveedorV2(List<Entities.VendorPayments> pagos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in pagos)
                {
                    try
                    {
                        
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_TRANSFERENCIA_OVPM\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"{bean.Entry}, "); // No comillas para valores numéricos
                        sbQuery.Append($"'{bean.Numero}', ");
                        sbQuery.Append($"'{bean.Fecha:yyyy-MM-dd}', ");  // Formatear fecha
                        sbQuery.Append($"'{bean.Moneda}', ");
                        sbQuery.Append($"{bean.Valor}, "); // No comillas para valores numéricos
                        sbQuery.Append($"'{bean.Cuenta}', ");
                        sbQuery.Append($"'{bean.HTH_PL}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_OVPM: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }

        public static bool addTransferencia(List<Entities.PlanillaHostTransferencia> transferencias)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in transferencias)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_TRANSFERENCIA", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vHTT_TF", bean.HTT_TF);
                        cmd.Parameters.AddWithValue("@vHTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@v101_TR", bean.tipoRegistro);
                        cmd.Parameters.AddWithValue("@v102_SF", bean.secuencialFila);
                        cmd.Parameters.AddWithValue("@v103_CCA", bean.cantidadCuentasArchivo);
                        cmd.Parameters.AddWithValue("@v104_IC", bean.identificadorCabecera);
                        cmd.Parameters.AddWithValue("@v105_FI", bean.filler);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_TRANSFERENCIA: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddTransferenciaV2(List<Entities.PlanillaHostTransferencia> transferencias, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in transferencias)
                {
                    try
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_TRANSFERENCIA\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTT_TF}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.tipoRegistro}', ");
                        sbQuery.Append($"'{bean.secuencialFila}', ");
                        sbQuery.Append($"'{bean.cantidadCuentasArchivo}', ");
                        sbQuery.Append($"'{bean.identificadorCabecera}', ");
                        sbQuery.Append($"'{bean.filler}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_TRANSFERENCIA: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }

        public static bool addTransferenciaCargo(List<Entities.PlanillaHostTransferenciaCargo> cargos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in cargos)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_TRANSFERENCIA_CARGO", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vHTT_CA", bean.HTT_CA);
                        cmd.Parameters.AddWithValue("@vHTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vHTT_TF", bean.HTT_TF);
                        cmd.Parameters.AddWithValue("@v201_TR", bean.tipoRegistro);
                        cmd.Parameters.AddWithValue("@v202_SF", bean.secuencialFila);
                        cmd.Parameters.AddWithValue("@v203_CAP", bean.cantidadAbonosPlanilla);
                        cmd.Parameters.AddWithValue("@v204_TCC", bean.tipoCuentaCargo);
                        cmd.Parameters.AddWithValue("@v205_MCC", bean.monedaCuentaCargo);
                        cmd.Parameters.AddWithValue("@v206_NCC", bean.numeroCuentaCargo);
                        cmd.Parameters.AddWithValue("@v207_MTS", bean.montoTotalSoles);
                        cmd.Parameters.AddWithValue("@v208_MTD", bean.montoTotalDolares);
                        cmd.Parameters.AddWithValue("@v209_TC", bean.totalControl);
                        cmd.Parameters.AddWithValue("@v210_IC", bean.identificadorCargo);
                        cmd.Parameters.AddWithValue("@v211_FI", bean.filler);
                        cmd.Parameters.AddWithValue("@vCCHK", bean.cargoChkSum);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_TRANSFERENCIA_CARGO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddTransferenciaCargoV2(List<Entities.PlanillaHostTransferenciaCargo> cargos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in cargos)
                {
                    try
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_TRANSFERENCIA_CARGO\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTT_CA}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTT_TF}', ");
                        sbQuery.Append($"'{bean.tipoRegistro}', ");
                        sbQuery.Append($"'{bean.secuencialFila}', ");
                        sbQuery.Append($"'{bean.cantidadAbonosPlanilla}', ");
                        sbQuery.Append($"'{bean.tipoCuentaCargo}', ");
                        sbQuery.Append($"'{bean.monedaCuentaCargo}', ");
                        sbQuery.Append($"'{bean.numeroCuentaCargo}', ");
                        sbQuery.Append($"{bean.montoTotalSoles}, "); // No comillas para valores numéricos
                        sbQuery.Append($"{bean.montoTotalDolares}, "); // No comillas para valores numéricos
                        sbQuery.Append($"'{bean.totalControl}', ");
                        sbQuery.Append($"'{bean.identificadorCargo}', ");
                        sbQuery.Append($"'{bean.filler}', ");
                        sbQuery.Append($"'{bean.cargoChkSum}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_TRANSFERENCIA_CARGO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }
            }

            return respuesta;
        }


        public static bool addTransferenciaAbono(List<Entities.PlanillaHostTransferenciaAbono> abonos)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            foreach (var bean in abonos)
            {
                #region ExecuteNonQuery
                using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
                {
                    try
                    {
                        connection.Open();
                        cmd = new SqlCommand("SP_EXX_BCP_A_TRANSFERENCIA_ABONO", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vCode", bean.Code);
                        cmd.Parameters.AddWithValue("@vName", bean.Name);
                        cmd.Parameters.AddWithValue("@vHTT_AB", bean.HTT_AB);
                        cmd.Parameters.AddWithValue("@vHTT_CA", bean.HTT_CA);
                        cmd.Parameters.AddWithValue("@vHTH_PL", bean.HTH_PL);
                        cmd.Parameters.AddWithValue("@vHTT_TF", bean.HTT_TF);
                        cmd.Parameters.AddWithValue("@v301_TR", bean.tipoRegistro);
                        cmd.Parameters.AddWithValue("@v302_SF", bean.secuencialFila);
                        cmd.Parameters.AddWithValue("@v303_TC", bean.tipoCuenta);
                        cmd.Parameters.AddWithValue("@v304_CP", bean.cuentaPropia);
                        cmd.Parameters.AddWithValue("@v305_NCA", bean.numeroCuentaAbono);
                        cmd.Parameters.AddWithValue("@v306_TDB", bean.tipoDocumentoBeneficiario);
                        cmd.Parameters.AddWithValue("@v307_NDB", bean.numeroDocumentoBeneficiario);
                        cmd.Parameters.AddWithValue("@v308_CD", bean.correlativoDocumento);
                        cmd.Parameters.AddWithValue("@v309_NB", bean.nombreBeneficiario);
                        cmd.Parameters.AddWithValue("@v310_MMT", bean.monedaMontoTransferir);
                        cmd.Parameters.AddWithValue("@v311_MO", bean.montoOperacion);
                        cmd.Parameters.AddWithValue("@v312_RE", bean.referencia);
                        cmd.Parameters.AddWithValue("@v313_IA", bean.identificadorAbono);
                        cmd.Parameters.AddWithValue("@v314_TC", bean.titularCuenta);
                        cmd.Parameters.AddWithValue("@v315_ECBCP", bean.emailClienteBCP);
                        cmd.Parameters.AddWithValue("@v316_FI", bean.filler);
                        cmd.Parameters.AddWithValue("@vACHK", bean.abonoChkSum);

                        cmd.ExecuteNonQuery();
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_TRANSFERENCIA_ABONO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
                #endregion
            }

            #endregion

            return respuesta;
        }

        public static bool AddTransferenciaAbonoV2(List<Entities.PlanillaHostTransferenciaAbono> abonos, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (var bean in abonos)
                {
                    try
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXX_BCP_A_TRANSFERENCIA_ABONO\" (");
                        sbQuery.Append($"'{bean.Code}', ");
                        sbQuery.Append($"'{bean.Name}', ");
                        sbQuery.Append($"'{bean.HTT_AB}', ");
                        sbQuery.Append($"'{bean.HTT_CA}', ");
                        sbQuery.Append($"'{bean.HTH_PL}', ");
                        sbQuery.Append($"'{bean.HTT_TF}', ");
                        sbQuery.Append($"'{bean.tipoRegistro}', ");
                        sbQuery.Append($"'{bean.secuencialFila}', ");
                        sbQuery.Append($"'{bean.tipoCuenta}', ");
                        sbQuery.Append($"'{bean.cuentaPropia}', ");
                        sbQuery.Append($"'{bean.numeroCuentaAbono}', ");
                        sbQuery.Append($"'{bean.tipoDocumentoBeneficiario}', ");
                        sbQuery.Append($"'{bean.numeroDocumentoBeneficiario}', ");
                        sbQuery.Append($"'{bean.correlativoDocumento}', ");
                        sbQuery.Append($"'{bean.nombreBeneficiario}', ");
                        sbQuery.Append($"'{bean.monedaMontoTransferir}', ");
                        sbQuery.Append($"{bean.montoOperacion}, "); // No comillas para valores numéricos
                        sbQuery.Append($"'{bean.referencia}', ");
                        sbQuery.Append($"'{bean.identificadorAbono}', ");
                        sbQuery.Append($"'{bean.titularCuenta}', ");
                        sbQuery.Append($"'{bean.emailClienteBCP}', ");
                        sbQuery.Append($"'{bean.filler}', ");
                        sbQuery.Append($"'{bean.abonoChkSum}'");
                        sbQuery.Append(")");
                        logger.Error(sbQuery.ToString());
                        oRecordset.DoQuery(sbQuery.ToString());
                        respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                        logger.Error($"SP_EXX_BCP_A_TRANSFERENCIA_ABONO: {ex.Message}");
                        logger.Error(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error al conectar a SAP: {ex.Message}");
                logger.Error(ex.StackTrace);
                respuesta = false;
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }
            }

            return respuesta;
        }

        public static bool updateHostPlanilla(Entities.PlanillaHostBE planilla)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            //foreach (var planilla in planillas)
            //{
            #region ExecuteNonQuery
            using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand("SP_EXXIS_PLANILLA_BCP_U_TRANSFERENCIA_BY_HTH_PL", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vHTH_PL", planilla.HTH_PL);
                    cmd.Parameters.AddWithValue("@vHTH_NAE", planilla.HTH_NAE);
                    cmd.Parameters.AddWithValue("@vHTH_PLH", planilla.HTH_PLH);
                    cmd.Parameters.AddWithValue("@vHTH_PLG", planilla.HTH_PLG);
                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    logger.Error($"SP_EXXIS_PLANILLA_BCP_U_TRANSFERENCIA_BY_HTH_PL: {ex.Message}");
                    logger.Error(ex.StackTrace);
                }
            }
            #endregion
            //}

            #endregion

            return respuesta;
        }

        public static bool UpdateHostPlanillaV2(Entities.PlanillaHostBE planilla, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXXIS_PLANILLA_BCP_U_TRANSFERENCIA_BY_HTH_PL\" (");
                sbQuery.Append($"'{planilla.HTH_PL}', ");
                sbQuery.Append($"'{planilla.HTH_NAE}', ");
                sbQuery.Append($"'{planilla.HTH_PLH}', ");
                sbQuery.Append($"'{planilla.HTH_PLG}'");
                sbQuery.Append(")");
                logger.Error(sbQuery.ToString());
                oRecordset.DoQuery(sbQuery.ToString());
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                logger.Error($"SP_EXXIS_PLANILLA_BCP_U_TRANSFERENCIA_BY_HTH_PL: {ex.Message}");
                logger.Error(ex.StackTrace);
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }

        public static bool deleteHostPlanillaProveedorByPL(string planilla)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            #region ExecuteNonQuery
            using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand("SP_EXXIS_PLANILLA_BCP_R_PLANILLA_PROVEEDOR_BY_PL", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vPL", planilla);
                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    logger.Error($"SP_EXXIS_PLANILLA_BCP_R_PLANILLA_PROVEEDOR_BY_PL: {ex.Message}");
                    logger.Error(ex.StackTrace);
                }
            }
            #endregion
            #endregion

            return respuesta;
        }

        public static bool DeleteHostPlanillaProveedorByPLV2(string planilla, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXXIS_PLANILLA_BCP_R_PLANILLA_PROVEEDOR_BY_PL\" (");
                sbQuery.Append($"'{planilla}'");
                sbQuery.Append(")");
                logger.Error(sbQuery.ToString());
                oRecordset.DoQuery(sbQuery.ToString());
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                logger.Error($"SP_EXXIS_PLANILLA_BCP_R_PLANILLA_PROVEEDOR_BY_PL: {ex.Message}");
                logger.Error(ex.StackTrace);
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }
  
            }

            return respuesta;
        }

        public static bool deleteHostPlanillaTransferenciaByPL(string planilla)
        {
            #region Variables
            bool respuesta = false;
            SqlCommand cmd = new SqlCommand();
            #endregion

            #region Interacion de datatable
            #region ExecuteNonQuery
            using (SqlConnection connection = new SqlConnection(cnxSCO_INTCO()))
            {
                try
                {
                    connection.Open();
                    cmd = new SqlCommand("SP_EXXIS_PLANILLA_BCP_R_PLANILLA_TRANSFERENCIA_BY_PL", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vPL", planilla);
                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    logger.Error($"SP_EXXIS_PLANILLA_BCP_R_PLANILLA_TRANSFERENCIA_BY_PL: {ex.Message}");
                    logger.Error(ex.StackTrace);
                }
            }
            #endregion
            #endregion

            return respuesta;
        }

        public static bool DeleteHostPlanillaTransferenciaByPLV2(string planilla, SAPbobsCOM.Company sBO_Company)
        {
            bool respuesta = false;
            SAPbobsCOM.Company oCompany = null;
            SAPbobsCOM.Recordset oRecordset = null;

            try
            {
                oCompany = sBO_Company;
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("CALL \""+ sBO_Company.CompanyDB+"\".\"SP_EXXIS_PLANILLA_BCP_R_PLANILLA_TRANSFERENCIA_BY_PL\" (");
                sbQuery.Append($"'{planilla}'");
                sbQuery.Append(")");
                logger.Error(sbQuery.ToString());
                oRecordset.DoQuery(sbQuery.ToString());
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                logger.Error($"SP_EXXIS_PLANILLA_BCP_R_PLANILLA_TRANSFERENCIA_BY_PL: {ex.Message}");
                logger.Error(ex.StackTrace);
            }
            finally
            {
                if (oRecordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                    oRecordset = null;
                }

            }

            return respuesta;
        }


        public static bool crearCarpeta(string path)
        {
            bool respuesta = false;
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return respuesta;
        }
        public static string nombrarFileProveedorSFTP(DateTime date)
        {
            return $"P{date.ToString("yyyyMMdd")}{date.ToString("hhmmss")}P";
        }
        public static string nombrarFileTransferenciaSFTP(DateTime date)
        {
            return $"T{date.ToString("yyyyMMdd")}{date.ToString("hhmmss")}P";
        }
    }
}
