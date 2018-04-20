namespace ISSSTE.Tramites2016.Hipotecas.Model.Pocos
{
    /// <summary>
    ///     Objeto que representa los parametros que se recibiran el post
    /// </summary>
    public class PostData
    {
        /// <summary>
        ///     NoIssste del drechohabiente
        /// </summary>
        public string NoIssste { get; set; }

        /// <summary>
        ///     Tipo de Pension
        /// </summary>
        public int PensionId { get; set; }


        /// <summary>
        ///     CURP derechohabiente
        /// </summary>
        public string CURP { get; set; }
    }
}