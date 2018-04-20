#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class StatusNextStatu
    {
        [Key]
        public int StatusNextStatusId { get; set; }

        public int StatusId { get; set; }
        public int NextStatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        [ForeignKey("NextStatusId")]
        public virtual Status NextStatus { get; set; }
    }
}