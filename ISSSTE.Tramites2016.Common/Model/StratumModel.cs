namespace ISSSTE.Tramites2016.Common.Model
{
    /// <summary>
    ///     Modelo que toma la información del estrato de base de datos
    /// </summary>
    public class StratumModel
    {
        public int StratumId { get; set; }

        /// <summary>
        ///     Años iniciales del estrato
        /// </summary>
        public int StartAge { get; set; }

        /// <summary>
        ///     Años finales del estrato
        /// </summary>
        public int EndAge { get; set; }

        /// <summary>
        ///     Meses iniciales del estrato
        /// </summary>
        public int StartMonths { get; set; }

        /// <summary>
        ///     Meses finales del estrato
        /// </summary>
        public int EndMonths { get; set; }

        /// <summary>
        ///     Días iniciales del estrato
        /// </summary>
        public int StartDays { get; set; }

        /// <summary>
        ///     Días finales del estrato
        /// </summary>
        public int EndDays { get; set; }

        public override string ToString()
        {
            return string.Format("De {0} años {1} meses {2} días, a {3} años {4} meses {5} días",
                StartAge, StartMonths, StartDays, EndAge, EndMonths, EndDays);
        }
    }
}