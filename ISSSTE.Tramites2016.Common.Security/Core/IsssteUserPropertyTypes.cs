using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Security.Core
{
    /// <summary>
    /// Contiene los diferentes tipos de propiedades que un usuario puede tener
    /// </summary>
    public static class IsssteUserPropertyTypes
    {
        /// <summary>
        /// Valor para la el tipo de propiedad "Delegación"
        /// </summary>
        public const string Delegation = "Delegación";

        /// <summary>
        /// Valor para la el tipo de propiedad "Agencia funeraria"
        /// </summary>
        public const string MortuaryAgency = "Agencia Funeraria";

        /// <summary>
        /// Valor para la el tipo de propiedad "Estancia infantil"
        /// </summary>
        public const string ChildCareCenter = "Estancia Infantil";

        /// <summary>
        /// Valor para la el tipo de propiedad "Agencia turistica"
        /// </summary>
        public const string TravelAgency = "Agencia Turística";
    }
}
