#region

using System.Collections.Generic;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto que contiene las citas de un derechohabiente
    /// </summary>
    public class DatesApi
    {
        /// <summary>
        ///     Lista de citas actuales
        /// </summary>
        public List<Appoinment> CurrentdAppoinments { get; set; }

        /// <summary>
        ///     Lista de citas canceladas
        /// </summary>
        public List<Appoinment> CanceledAppoinments { get; set; }

        /// <summary>
        ///     NUmero de Citas
        /// </summary>
        public int NumberAppointments { get; set; }
    }
}