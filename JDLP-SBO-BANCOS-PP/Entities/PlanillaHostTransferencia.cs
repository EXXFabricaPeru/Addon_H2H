using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
    public class PlanillaHostTransferencia
    {
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 1.01-Tipo de registro-Longitud: 1
        /// </summary>
        public string tipoRegistro { get; set; }
        /// <summary>
        /// 1.02-Secuencial de fila-Longitud: 6
        /// </summary>
        public string secuencialFila { get; set; }
        /// <summary>
        /// 1.03-Cantidad de cuentas en el archivo-Longitud: 6
        /// </summary>
        public string cantidadCuentasArchivo { get; set; }
        /// <summary>
        /// 1.04-Identificador Cabecera-Longitud: 32
        /// </summary>
        public string identificadorCabecera { get; set; }
        /// <summary>
        /// 1.05-Filler-Longitud: 305
        /// </summary>
        public string filler { get; set; }
        /// <summary>
        /// PK Codigo unico transferencia
        /// </summary>
        public string HTT_TF { get; set; }
        /// <summary>
        /// FK Codigo de planilla
        /// </summary>
        public string HTH_PL { get; set; }

    }
}
