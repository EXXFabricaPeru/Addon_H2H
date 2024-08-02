using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
    public class PlanillaHostTransferenciaCargo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string HTT_CA { get; set; }
        public string HTH_PL { get; set; }
        public string HTT_TF { get; set; }
        /// <summary>
        ///2.01-Tipo de registro -Longitud: 1
        /// </summary>
        public string tipoRegistro { get; set; }
        /// <summary>
        /// 2.02-Secuencial de fila -Longitud: 6
        /// </summary>
        public string secuencialFila { get; set; }
        /// <summary>
        /// 2.03-Cantidad de abonos de la planilla -Longitud: 6
        /// </summary>
        public string cantidadAbonosPlanilla { get; set; }
        /// <summary>
        /// 2.04-Tipo de la cuenta de cargo -Longitud: 1
        /// </summary>
        public string tipoCuentaCargo { get; set; }
        /// <summary>
        /// 2.05-Moneda de la cuenta de cargo -Longitud: 4
        /// </summary>
        public string monedaCuentaCargo { get; set; }
        /// <summary>
        /// 2.06-Número de cuenta de cargo -Longitud: 20
        /// </summary>
        public string numeroCuentaCargo { get; set; }
        /// <summary>
        /// 2.07-Monto total Soles -Longitud: 17
        /// </summary>
        public string montoTotalSoles { get; set; }
        /// <summary>
        /// 2.08-Monto total Dolares -Longitud: 17
        /// </summary>
        public string montoTotalDolares { get; set; }
        /// <summary>
        /// 2.09-Total de control (checksum) -Longitud: 15
        /// </summary>
        public string totalControl { get; set; }
        /// <summary>
        /// 2.10-Identificador cargo -Longitud: 32
        /// </summary>
        public string identificadorCargo { get; set; }
        /// <summary>
        /// 2.11-Filler -Longitud: 231
        /// </summary>
        public string filler { get; set; }
        public string moneda { get; set; }
        public string cargoChkSum { get; set; }


    }
}
