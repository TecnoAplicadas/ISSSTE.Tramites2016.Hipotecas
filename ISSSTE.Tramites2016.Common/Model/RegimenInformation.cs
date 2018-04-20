#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de un régimen laboral
    /// </summary>
    public class RegimenInformation
    {
        /// <summary>
        ///     CURP
        /// </summary>
        public String Curp { get; set; }

        /// <summary>
        ///     Numero de ISSTE
        /// </summary>
        public String IsssteNumber { get; set; }

        /// <summary>
        ///     Clave del regimen
        /// </summary>
        public String RegimenKey { get; set; }

        /// <summary>
        ///     Descripcion del tipo de regimen
        /// </summary>
        public String RegimenDescription { get; set; }
        
    }
}