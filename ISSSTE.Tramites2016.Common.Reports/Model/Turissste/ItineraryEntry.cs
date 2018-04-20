using System;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Entrada al itinerario del viaje
  /// </summary>
  public class ItineraryEntry
  {
    /// <summary>
    /// Tipo de transporte tomado (Aéreo, Terrestre)
    /// </summary>
    public string TransportType { get; set; }

    /// <summary>
    /// Línea con quien se reservó el viaje
    /// </summary>
    public string Line { get; set; }

    /// <summary>
    /// Lugar de origen donde surge la entrada
    /// </summary>
    public string Origin { get; set; }

    /// <summary>
    /// Destino marcado para la entrada
    /// </summary>
    public string Destination { get; set; }

    /// <summary>
    /// Fecha de salida
    /// </summary>
    public string DepartureDate { get; set; }

    /// <summary>
    /// Fecha de arribo desde el viaje
    /// </summary>
    public string ArrivalDate { get; set; }

    /// <summary>
    /// Número de vuelo o corrida de la entrada
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Clave de la entrada
    /// </summary>
    public string Key { get; set; }
  }
}
