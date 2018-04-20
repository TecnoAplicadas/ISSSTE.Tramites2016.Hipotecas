using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class RequestAdministrator
    {
        public Guid RequestId { get; set; }

        [Required]
        [StringLength(30)]
        public string Folio { get; set; }
        [Required]
        [StringLength(20)]
        public string CURP { get; set; }

        [Required]
        [StringLength(15)]
        public string NoISSSTE { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Estatus { get; set; }

        public int StatusId { get; set; }


    }
}
