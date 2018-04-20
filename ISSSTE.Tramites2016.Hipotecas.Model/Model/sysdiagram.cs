#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class sysdiagram
    {
        [Required]
        [StringLength(128)]
        public string name { get; set; }

        public int principal_id { get; set; }

        [Key]
        public int diagram_id { get; set; }

        public int? version { get; set; }
        public byte[] definition { get; set; }
    }
}