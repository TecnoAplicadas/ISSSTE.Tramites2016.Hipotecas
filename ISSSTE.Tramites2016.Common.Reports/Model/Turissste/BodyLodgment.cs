using System;
using System.Collections.Generic;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  public class BodyLodgment
  {
    public string HotelName { get; set; }

    public string AdditionalObservations { get; set; }

    public IEnumerable<Person> Guests { get; set; }

    public int PensionatedNumber { get; set; }

    public string ArrivalDate { get; set; }

    public string DepartureDate  { get; set; }

    public int SingleRoom { get; set; }

    public int DoubleRoom { get; set; }

    public int TripleRoom { get; set; }

    public int CuadrupleRoom { get; set; }

    public string SpecialOffer { get; set; }

    public string Plan { get; set; }

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
