using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
     public  class PlanillaHostProveedorAbono
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string HTH_PL { get; set; }
        public string HTP_PC { get; set; }
        public string HTA_PA { get; set; }
        public string tipoRegistro { get; set; }
        public string tipoCuentaAbono { get; set; }
        public string numeroCuentaAbono { get; set; }
        public string modalidadPago { get; set; }
        public string tipoDocumentoProveedor { get; set; }
        public string numeroDocumentoProveedor { get; set; }
        public string correlativoDocumentoProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string referenciaBeneficiario { get; set; }
        public string referenciaEmpresa { get; set; }
        public string monedaImporteAbonar { get; set; }
        public string importeAbonar { get; set; }
        public string flagValidarIDC { get; set; }
        public string Filler { get; set; }
        public string tipoCambio { get; set; }
        public string montoCambio { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }
        public string abonoChkSum { get; set; }
        public string order { get; set; }
    }
}
