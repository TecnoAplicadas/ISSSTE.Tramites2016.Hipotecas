using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSSTE.Tramites2016.Common.Reports.Model.Turissste
{
  /// <summary>
  /// Datos de la persona involucrada
  /// </summary>
  public class Person
  {
    /// <summary>
    /// Nombre de la persona
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Edad de la persona
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Tipo de persona (Adulto, Junior, etc...)
    /// </summary>
    public string GuestType { get; set; }

    /// <summary>
    /// Género de la persona
    /// </summary>
    public string Gender { get; set; }
  }
}
