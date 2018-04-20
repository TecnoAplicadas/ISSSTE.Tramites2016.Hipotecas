#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Model
{
    public class Entitle
    {
        [StringLength(20)]
        public string EntitleId { get; set; }

        [Required]
        [StringLength(15)]
        public string NoISSSTE { get; set; }

        [StringLength(50)]
        public string PaternalLastName { get; set; }

        [StringLength(50)]
        public string MaternalLastName { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(15)]
        public string RFC { get; set; }

        public int? Age { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthdate { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(256)]
        public string Street { get; set; }

        [StringLength(10)]
        public string NumExt { get; set; }

        [StringLength(10)]
        public string NumInt { get; set; }

        [StringLength(256)]
        public string Colony { get; set; }

        [StringLength(7)]
        public string ZipCode { get; set; }

        public int? DelegationId { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(18)]
        public string Telephone { get; set; }


        [StringLength(18)]
        public string Lada { get; set; }

        [StringLength(70)]
        public string City { get; set; }

        [StringLength(20)]
        public string CURP { get; set; }

        [StringLength(20)]
        public string MaritalStatus { get; set; }

        [StringLength(50)]
        public string Birthplace { get; set; }


        public virtual Delegation Delegation { get; set; }


        //  public string StateDescription { get; set; }

        //CAP
        [StringLength(30)]
        public string State { get; set; }

        //        public string DirectType { get; set; }

        [StringLength(256)]
        public string RegimeType { get; set; }


        [Required]
        public bool IsActive { get; set; }

        [StringLength(256)]
        public string MobilePhone { get; set; }

    }
}