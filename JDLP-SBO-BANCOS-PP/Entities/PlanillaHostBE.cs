using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
    public class PlanillaHostBE
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Planilla
        /// </summary>
        public string HTH_PL { get; set; }
        /// <summary>
        /// Comentario
        /// </summary>
        public string HTH_CO { get; set; }
        /// <summary>
        /// Procesado {W:Pendiente} {C:Completado} {P:Parcial} {E:Error}
        /// </summary>
        public string HTH_PR { get; set; }
        /// <summary>
        /// Fecha de creacion
        /// </summary>
        public string HTH_FC { get; set; }
        /// <summary>
        /// Fecha Procesado
        /// </summary>
        public string HTH_FP { get; set; }
        /// <summary>
        /// Usuario
        /// </summary>
        public string HTH_US { get; set; }
        /// <summary>
        /// Codigo Accion {P:Proveedor} {T:Transferencia}
        /// </summary>
        public string HTH_CA { get; set; }
        /// <summary>
        /// Nombre de archivo enviado
        /// </summary>
        public string HTH_NAE { get; set; }
        /// <summary>
        /// Nombre de archivo respuesta 1
        /// </summary>
        public string HTH_NAR1 { get; set; }
        /// <summary>
        /// Nombre de archivo respuesta 2
        /// </summary>
        public string HTH_NAR2 { get; set; }
        /// <summary>
        /// Path local Host
        /// </summary>
        public string HTH_PLH { get; set; }
        /// <summary>
        /// Planilla SAP
        /// </summary>
        public string HTH_WIZ { get; set; }
        /// <summary>
        /// Planilla SAP FI
        /// </summary>
        public string HTH_WIZFI { get; set; }
       /// <summary>
       /// Planilla SAP FF
       /// </summary>
        public string HTH_WIZFF { get; set; }
        public string HTH_SFTPP { get; set; }
        public string HTH_SFTPT { get; set; }
        /// <summary>
        /// Planilla generada
        /// </summary>
        public string HTH_PLG { get; set; }

    }
}
