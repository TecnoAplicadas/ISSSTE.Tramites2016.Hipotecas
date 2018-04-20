using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Cuerpo de una solicitud para paquetes turísticos
  /// </summary>
  public class BodyPackage
  {
    /// <summary>
    /// Nombre del operador
    /// </summary>
    public string OperatorName { get; set; }

    /// <summary>
    /// Nombre del paquete
    /// </summary>
    public string PackageName { get; set; }

    /// <summary>
    /// Nombre del hotel
    /// </summary>
    public string HotelName { get; set; }

    /// <summary>
    /// Categoría del hotel
    /// </summary>
    public string HotelCategory { get; set; }

    /// <summary>
    /// Fecha en que inicia el paquete solicitado
    /// </summary>
    public string InitialDate { get; set; }

    /// <summary>
    /// Fecha en que finaliza la solicitud
    /// </summary>
    public string EndDate { get; set; }

    /// <summary>
    /// Numero de habitaciones sencillas
    /// </summary>
    public int SingleRoom { get; set; }

    /// <summary>
    /// Número de habitaciones dobles
    /// </summary>
    public int DoubleRoom { get; set; }

    /// <summary>
    /// Número de habitaciones triples
    /// </summary>
    public int TripleRoom { get; set; }

    /// <summary>
    /// Número de habitaciones cuadrúples
    /// </summary>
    public int CuadrupleRoom { get; set; }

    /// <summary>
    /// Listado de personas incluidas en la solicitud
    /// </summary>
    public IEnumerable<Person> PackagePersons { get; set; }

    /// <summary>
    /// Comentarios adicionales
    /// </summary>
    public string AdditionalComments { get; set; }

    /// <summary>
    /// Tarifa de transporte terrestre
    /// </summary>
    public decimal LandingFareUSD { get; set;}

    /// <summary>
    /// Tarifa de transporte aéreo
    /// </summary>
    public decimal AirFareUSD { get; set; }

    /// <summary>
    /// Total de las tarifas de transporte
    /// </summary>
    public decimal TotalUSD { get; set; }
    
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

    /// <summary>
    /// Tipo de cambio
    /// </summary>
    public decimal ExchangeRate { get; set; }
  }
}
