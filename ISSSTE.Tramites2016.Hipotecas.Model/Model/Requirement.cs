#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Requirement
    {
        public int RequirementId { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public int PensionId { get; set; }
    }
}