using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
  public  class PlanillaHostTransferenciaAbono
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string HTT_AB { get; set; }
        public string HTT_CA { get; set; }
        public string HTH_PL { get; set; }
        public string HTT_TF { get; set; }
        /// <summary>
        /// 3.01-Tipo de registro	-Longitud:1
        /// </summary>
        public string tipoRegistro { get; set; }
        /// <summary>
        /// 3.02-Secuencial de fila	-Longitud:6
        /// </summary>
        public string secuencialFila { get; set; }
        /// <summary>
        /// 3.03-Tipo de Cuenta	-Longitud:1
        /// </summary>
        public string tipoCuenta { get; set; }
        /// <summary>
        /// 3.04-Cuenta Propia	-Longitud:1
        /// </summary>
        public string cuentaPropia { get; set; }
        /// <summary>
        /// 3.05-Número de cuenta de abono	-Longitud:20
        /// </summary>
        public string numeroCuentaAbono { get; set; }
        /// <summary>
        /// 3.06-Tipo de documento del beneficiario	-Longitud:3
        /// </summary>
        public string tipoDocumentoBeneficiario { get; set; }
        /// <summary>
        /// 3.07-Número de documento del beneficiario	-Longitud:12
        /// </summary>
        public string numeroDocumentoBeneficiario { get; set; }
        /// <summary>
        /// 3.08-Correlativo de documento	-Longitud:3
        /// </summary>
        public string correlativoDocumento { get; set; }
        /// <summary>
        /// 3.09-Nombre del beneficiario	-Longitud:75
        /// </summary>
        public string nombreBeneficiario { get; set; }
        /// <summary>
        /// 3.10-Moneda del monto a transferir	-Longitud:4
        /// </summary>
        public string monedaMontoTransferir { get; set; }
        /// <summary>
        /// 3.11-Monto de la operación	-Longitud:17
        /// </summary>
        public string montoOperacion { get; set; }
        /// <summary>
        /// 3.12-Referencia	-Longitud:40
        /// </summary>
        public string referencia { get; set; }
        /// <summary>
        /// 3.13-Identificador Abono	-Longitud:32
        /// </summary>
        public string identificadorAbono { get; set; }
        /// <summary>
        /// 3.14-Titular de la cuenta	-Longitud:1
        /// </summary>
        public string titularCuenta { get; set; }
        /// <summary>
        /// 3.15-Email del cliente BCP	-Longitud:100
        /// </summary>
        public string emailClienteBCP { get; set; }
        /// <summary>
        /// 3.16-Filler	-Longitud:34
        /// </summary>
        public string filler { get; set; }
        public string abonoChkSum { get; set; }
    }
}
