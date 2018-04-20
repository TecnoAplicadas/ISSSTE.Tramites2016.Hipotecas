#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class StatusRelatedStatu
    {
        [Key]
        public int StatusNextStatusId { get; set; }

        public int StatusId { get; set; }
        public int RelatesStatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        [ForeignKey("RelatesStatusId")]
        public virtual Status RelatesStatus { get; set; }
    }
}