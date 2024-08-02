using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
   public class PlanillaHostProveedorDocumento
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string HTH_PL { get; set; }
        public string HTP_PC { get; set; }
        public string HTA_PA { get; set; }
        public string HTD_PD { get; set; }
        public string tipoRegistro { get; set; }
        public string tipoDocumnetoPagar { get; set; }
        public string numeroDocumentoPagar { get; set; }
        public string importeDocumnetoPagar { get; set; }

    }
}
