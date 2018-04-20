namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de un velatorio
    /// </summary>
    public class MortuaryInformation
    {
        /// <summary>
        ///     Id del velatorio
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Nombre del velatorio
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Dirección del velatorio
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Código postal del velatorio
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        ///     Estatus: Activo o no activo
        /// </summary>
        public string Status { get; set; }
    }
}