using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using ISSSTE.Tramites2016.Hipotecas.Model.Modelo;
namespace ISSSTE.Tramites2016.Hipotecas.Model.Modelo
{
    public class RequestDocuments
    {
        public long DocumentId { get; set; }
        public int isRejection { get; set; }
        public int Required { get; set; }
        public String RequestId { get; set; }
        public long DocumentTypeId { get; set; }
        public int IsValid { get; set; }
        public String Observations { get; set; }
        public String FileExtension { get; set; }
        public String DocumentName { get; set; }
        public Byte[] Data { get; set; }
    }
}
