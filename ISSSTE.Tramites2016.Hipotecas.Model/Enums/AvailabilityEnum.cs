#region

using System.ComponentModel;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model.Enums
{
    /// <summary>
    ///     Enum el cual indicada la disponiblidad de citas en los dias de un calendario
    /// </summary>
    public enum AvailabilityEnum
    {
        [Description("Disponible")] Avaliable = 1,
        [Description("Baja Disponibilidad")] LowAvailability = 2,
        [Description("No Disponible")] Unavailable = 3,
        [Description("Sin Servicio")] NoService = 4,
        [Description("Fecha Pasada")] PastDate = 5
    }
}