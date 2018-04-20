using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Modelo
{
    public class UrbanCenter
    {
        public int IdUrbanCenter { get; set; }
        public string Description { get; set; }
        public string Ubication { get; set; }
        public string NoEdif { get; set; }
        public string NoDeptos { get; set; }
        public string ComercialLocals { get; set; }
        public bool Active { get; set; }

        public string Locality { get; set; }
    }
}
