#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Configuration
    {
        [Key]
        public int ConfiguratonId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}