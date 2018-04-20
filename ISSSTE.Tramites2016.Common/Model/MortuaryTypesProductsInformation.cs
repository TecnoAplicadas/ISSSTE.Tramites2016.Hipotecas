namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de una categoría de productos de una agencia funeraria
    /// </summary>
    public class MortuaryTypesProductsInformation
    {
        /// <summary>
        ///     Clave del tipo de producto
        /// </summary>
        public int IdType { get; set; }

        /// <summary>
        ///     Nombre del tipo de producto
        /// </summary>
        public string Name { get; set; }
    }
}