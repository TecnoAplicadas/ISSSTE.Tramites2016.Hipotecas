#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto de tranfenrecia para el api
    /// </summary>
    public class IntDateApi
    {
        /// <summary>
        ///     Entero que representa un Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Fecha para tranferencia
        /// </summary>
        public DateTime Date { get; set; }
    }
}