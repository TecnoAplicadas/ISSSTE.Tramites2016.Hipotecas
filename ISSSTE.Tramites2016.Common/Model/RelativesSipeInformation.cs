#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de un deudo de un derechohabiente
    /// </summary>
    public class RelativesSipeInformation
    {
        /// <summary>
        ///     Edad del deudo
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        ///     Lugar de nacimiento del deudo
        /// </summary>
        public String BirthEntity { get; set; }

        /// <summary>
        ///     Fecha de nacimiento del deudo
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        ///     CURP del deudo
        /// </summary>
        public String Curp { get; set; }

        /// <summary>
        ///     Parentesco con el derechohabiente del deudo
        /// </summary>
        public String Relationship { get; set; }

        /// <summary>
        ///     Descripción del parentesco con el derechohabiente del deudo
        /// </summary>
        public String RelationshipDescription { get; set; }

        /// <summary>
        ///     Apellido paterno del deudo
        /// </summary>
        public String FirstSurname { get; set; }

        /// <summary>
        ///     Apellido materno del deudo
        /// </summary>
        public String SecondSurname { get; set; }

        /// <summary>
        ///     Nombre del deudo
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