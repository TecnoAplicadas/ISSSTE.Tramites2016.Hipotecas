using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Escrituracion.Models.Model
{
    public class Seguimiento
    {
        [Required]
        public string Folio { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid RequestId { get; set; }

        [StringLength(30)]
        public DateTime Date { get; set; }

        //[StringLength(256)]
        //public string Observations { get; set; }

    }
}
