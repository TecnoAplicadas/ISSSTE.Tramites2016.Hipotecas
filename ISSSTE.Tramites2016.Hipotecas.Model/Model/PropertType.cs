using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class PropertType
    {

        public int IdPropertyType { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }


        public int DelegationId { get; set; }
        public string Name { get; set; }
        
        
    }
}
