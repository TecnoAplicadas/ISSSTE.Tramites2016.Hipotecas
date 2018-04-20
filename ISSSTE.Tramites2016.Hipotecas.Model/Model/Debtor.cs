#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Debtor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DebtorId { get; set; }

        [StringLength(50)]
        public string Birthplace { get; set; }

        [StringLength(50)]
        public string CURP { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }

        [StringLength(50)]
        public string PaternalLastName { get; set; }

        [StringLength(50)]
        public string MaternalLastName { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public Guid? RequestId { get; set; }
        public int? Age { get; set; }
        public virtual Request Request { get; set; }

        public int KeyRelationship { get; set; }

        [StringLength(15)]
        public string NoISSSTE { get; set; }
    }
}