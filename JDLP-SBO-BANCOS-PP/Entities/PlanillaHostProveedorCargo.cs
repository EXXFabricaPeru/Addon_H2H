using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLP_SBO_BANCOS_PP.Entities
{
   public class PlanillaHostProveedorCargo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string HTH_PL { get; set; }
        public string HTP_PC { get; set; }
        public string tipoRegistro { get; set; }
        public string cantidadAbonosPlanilla { get; set; }
        public string fechaProceso { get; set; }
        public string tipoCuentaCargo { get; set; }
        public string monedaCuentaCargo { get; set; }
        public string numeroCuentaCargo { get; set; }
        public string montoTotalPlanilla { get; set; }
        public string referenciaPlanilla { get; set; }
        public string flagExoneracionITF { get; set; }
        public string totalControl { get; set; }
        public string filler { get; set; }
        public string numeroPlanilla { get; set; }
        public string numeroRegistros { get; set; }
        public string numeroRegistrosRechazados { get; set; }
        public string estado { get; set; }
        public string cargoChkSum { get; set; }



    }
}
