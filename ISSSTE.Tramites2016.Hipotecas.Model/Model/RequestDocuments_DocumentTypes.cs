using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Modelo
{
    public class RequestDocuments_DocumentTypes
    {

        public long DocumentTypeId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String DocumentId { get; set; }
        public int isRejection { get; set; }
        public int Required { get; set; }
        public String RequestId { get; set; }
        public int IsValid { get; set; }
        public String Observations { get; set; }
        public String FileExtension { get; set; }
        public String DocumentName { get; set; }
        public Byte[] Data { get; set; }

    }
}
