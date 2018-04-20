#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DocumentId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public Guid RequestId { get; set; }

        public bool? IsValid { get; set; }

        [StringLength(250)]
        public string Observations { get; set; }
        
        public Byte[] Data { get; set; }

        [StringLength(50)]
        public string FileExtension { get; set; }

    }
}