namespace ISSSTE.Tramites2016.Common.Reports.Model.Dashboard
{
    /// <summary>
    /// Representa el nivel de cumplimiento de un estado o de un trámite
    /// </summary>
    public enum FulfillmentCategory
    {
        /// <summary>
        /// Representa que no aplica la evaluación de cumplimiento
        /// </summary>
        NotApply = 0,
        /// <summary>
        /// Representa que esta por debajo del tiempo establecido
        /// </summary>
        InTime,
        /// <summary>
        /// Establece que esta por debajo del tiempo establecido, pero cerca de él
        /// </summary>
        NearLimit,
        /// <summary>
        /// Representa que supero el tiempo establecido
        /// </summary>
        OffLimit
    }
}