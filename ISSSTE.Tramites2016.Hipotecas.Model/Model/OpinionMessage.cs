#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    [Table("OpinionMessage")]
    public class OpinionMessage
    {
        public int OpinionMessageId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}