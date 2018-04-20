using System;
using System.Collections.Generic;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Cuerpo de la solicitud correspondiente a un paquete carretero
  /// </summary>
  public class BodyTransportation
  {
    /// <summary>
    /// Origen del viaje
    /// </summary>
    public string Origin { get; set; }

    /// <summary>
    /// Destino final del viaje
    /// </summary>
    public string Destination { get; set; }

    /// <summary>
    /// Indica la fecha de salida
    /// </summary>
    public string DepartureDate { get; set; }

    /// <summary>
    /// Indica la fecha de regreso en los viajes redondos
    /// </summary>
    public string ReturnDate { get; set; }

    /// <summary>
    /// Indica si el viaje es redondo
    /// </summary>
    public bool IsRoundTrip { get; set; }

    /// <summary>
    /// Lista de transportes incluídos en la solicitud
    /// </summary>
    public IEnumerable<ItineraryEntry> Itinerary { get; set; }  

    /// <summary>
    /// Viajeros que se incluyen en la solicitud
    /// </summary>
    public IEnumerable<Person> Travelers { get; set; }

    /// <summary>
    /// Total de las tarifas de transporte
    /// </summary>
    public decimal BasisRateMXN { get; set; }

    /// <summary>
    /// Total de las tarifas de transporte
    /// </summary>
    public decimal TaxesMXN { get; set; }

    /// <summary>
    /// Total de las tarifas de transporte
    /// </summary>
    public decimal DiscountMXN { get; set; }

    /// <summary>
    /// Total de las tarifas de transporte
    /// </summary>
    public decimal TotalMXN { get; set; }
  }
}
