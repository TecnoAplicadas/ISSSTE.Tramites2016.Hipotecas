#region

using System;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Clase que sirve para tranferencia de parametros en apy
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        ///     Llave o nombre del valor
        /// </summary>
        public String Key { get; set; }

        /// <summary>
        ///     Valor del parametros
        /// </summary>
        public String Value { get; set; }
    }
}