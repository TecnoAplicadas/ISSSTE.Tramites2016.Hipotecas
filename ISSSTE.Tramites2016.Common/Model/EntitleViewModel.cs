namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de contacto de un derechohabiente
    /// </summary>
    public class EntitleViewModel
    {
        /// <summary>
        ///     Numero telefonico del derechohabiente
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        ///     Correo electronico del derechohabiente
        /// </summary>
        public string Email { get; set; }
    }
}