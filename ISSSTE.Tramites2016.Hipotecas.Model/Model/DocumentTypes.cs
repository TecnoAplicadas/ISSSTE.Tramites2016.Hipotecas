using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Modelo
{
    public class DocumentTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentTypeId { get; set; }
        public String Name { get; set; }
        public String Description { get; set;  }
        public bool Required { get; set; }

    }
}
