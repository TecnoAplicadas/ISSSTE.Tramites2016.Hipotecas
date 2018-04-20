#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required]
        [StringLength(20)]
        public string Key { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }
    }
}