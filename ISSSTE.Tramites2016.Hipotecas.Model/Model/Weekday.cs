#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Weekday
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WeekdayId { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }
    }
}