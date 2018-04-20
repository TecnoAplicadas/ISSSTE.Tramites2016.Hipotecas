using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Documents
    {
        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        public int? DocumentTypeId { get; set; }

        [Required]
        public bool? IsValid { get; set; }

        [Required]
        public Guid RequestId { get; set; }

        [StringLength(256)]
        public string Observations { get; set; }

        [Required]
        [StringLength(50)]
        public string FileExtension { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
