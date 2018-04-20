#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Opinion
    {
        public int OpinionId { get; set; }
        public Guid RequestId { get; set; }

        [Column("Opinion")]
        [Required]
        public string Opinion1 { get; set; }

        public virtual Request Request { get; set; }
    }
}