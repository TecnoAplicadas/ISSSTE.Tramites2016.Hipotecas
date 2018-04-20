using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ISSSTE.Tramites2016.Hipotecas.DataAccess;

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Resources
{
    /// <summary>
    /// Clase utilizada para proporcionar funcionalidades comunes al proyecto.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Método para establecer en un objeto a actualizar mediante un contexto
        /// que las propiedades a las cuales no se les asignó valor, no participen
        /// de la actualización en BD, únicamente aquellas que tienen valor.
        /// </summary>
        /// <param name="entry"></param>
        public static void SetEntityWithNullsFalse(DbEntityEntry entry)
        {
            foreach (var name in entry.CurrentValues.PropertyNames)
            {
                if (entry.Property(name).CurrentValue == null)
                    entry.Property(name).IsModified = false;
            }
        }
        
    }
}
