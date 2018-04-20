using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    public class DocumentData
    {
        public Guid RequestId { get; set; }
        public Guid DocumentId { get; set; }
        public String Observations { get; set; }
        public bool IsValid { get; set; }
    }
}
