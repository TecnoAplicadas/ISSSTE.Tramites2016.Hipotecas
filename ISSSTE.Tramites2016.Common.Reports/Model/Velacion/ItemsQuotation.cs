using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Velacion
{
    public class ItemsQuotation
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public decimal EntitlePrice { get; set; }

        public decimal PublicPrice { get; set; }
    }
}
