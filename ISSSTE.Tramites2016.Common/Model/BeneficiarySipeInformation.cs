#region

using System;
using ISSSTE.Tramites2016.Common.Util;

#endregion

namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de un beneficiario de un derechohabiente
    /// </summary>
    public class BeneficiarySipeInformation
    {
        /// <summary>
        ///     Edad del beneficiario en Años, meses, dias, minutos, segundos
        /// </summary>
        public DateTimeSpan Age { get; set; }

        /// <summary>
        ///     Edad del beneficiario en años
        /// </summary>
        public int AgeYears { get; set; }

        /// <summary>
        ///     Lugar de nacimiento del beneficiario
        /// </summary>
        public String BirthEntity { get; set; }

        /// <summary>
        ///     Fecha de nacimiento del beneficiario
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        ///     CURP del beneficiario
        /// </summary>
        public String Curp { get; set; }

        /// <summary>
        ///     Parentesco con el derechohabiente del beneficiario
        /// </summary>
        public String Relationship { get; set; }

        /// <summary>
        ///     Descripción del parentesco con el derechohabiente del beneficiario
        /// </summary>
        public String RelationshipDescription { get; set; }

        /// <summary>
        ///     Apellido paterno del beneficiario
        /// </summary>
        public String FirstSurname { get; set; }

        /// <summary>
        ///     Apellido materno del beneficiario
        /// </summary>
        public String SecondSurname { get; set; }

        /// <summary>
        ///     Nombre del beneficiario
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Clave del tipo de prorroga
        /// </summary>
        public String VersionKey { get; set; }

        /// <summary>
        ///     Nombre del tipo de prorroga
        /// </summary>
        public String Version { get; set; }
    }
}