namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    ///     Estados de la república, desde el servicio de Informix
    /// </summary>
    public class StateInformation
    {
        /// <summary>
        ///     Identificador del Estado de la República
        /// </summary>
        public int IdState { get; set; }

        /// <summary>
        ///     Nombre del Estado de la República
        /// </summary>
        public string Name { get; set; }
    }
}