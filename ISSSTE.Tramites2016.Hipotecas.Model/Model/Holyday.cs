#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Holyday
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
    }
}