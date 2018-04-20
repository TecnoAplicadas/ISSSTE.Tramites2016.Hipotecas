namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    /// Modelo que representa la información de un producto de una agencia funeraria
    /// </summary>
    public class MortuaryProductsInformation
    {
        /// <summary>
        ///     Id de la categoria del producto / servicio
        /// </summary>
        public int IdProductType { get; set; }

        /// <summary>
        ///     Id del producto / servicio
        /// </summary>
        public int IdProductServcie { get; set; }

        /// <summary>
        ///     Nombre del producto / servicio
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Descripción del producto / servicio
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Precio para derechohabientes
        /// </summary>
        public decimal EntitlePrice { get; set; }

        /// <summary>
        ///     Precio para no derechohabientes
        /// </summary>
        public decimal NoEntitlePrice { get; set; }

        /// <summary>
        ///     Id del velatorio
        /// </summary>
        public int IdMortuary { get; set; }

        /// <summary>
        ///     Tipo de ISSSTE
        /// </summary>
        public bool IsssteType { get; set; }
    }
}