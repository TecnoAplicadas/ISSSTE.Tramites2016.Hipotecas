#region

using System;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Hipotecas.Model.Enums;
using ISSSTE.Tramites2016.Hipotecas.Model.Model;
using System.Globalization;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Model
{
    /// <summary>
    ///     Clase uxiliar para conversion entre tipos
    /// </summary>
    public static class ConverterData
    {
        /// <summary>
        ///     Convierte de un tipo de SipeAv a Entitle
        /// </summary>
        /// <param name="entitleSipe"></param>
        /// <returns></returns>
        public static Entitle EntitleConverter(EntitleSipeInformation entitleSipe)
        {
            var entitle = new Entitle
            {
                Birthdate = (DateTime)entitleSipe.BirthDate,
                Birthplace = entitleSipe.EntityBirth,
                CURP = entitleSipe.Curp,
                City = entitleSipe.Population,
                Colony = entitleSipe.Colony,
                EntitleId =entitleSipe.Curp,
                Gender = entitleSipe.Genger,
                MaritalStatus = entitleSipe.MaritalStatus,
                MaternalLastName = entitleSipe.SecondSurname,
                Name = entitleSipe.Name,
                NoISSSTE = entitleSipe.NumIssste,
                NumExt = entitleSipe.ExteriorNumber,
                NumInt = entitleSipe.InteriorNumber,
                PaternalLastName = entitleSipe.FirstSurname,
                RFC = entitleSipe.Rfc,
                Street = entitleSipe.Street,
                ZipCode = entitleSipe.PostalCode,
                Age = entitleSipe.Age,
                State = entitleSipe.State,
                MobilePhone = entitleSipe.MobilePhone
            };
            return entitle;
        }

      

        /// <summary>
        ///     Convierte un Relative SipeAv a un Debtor
        /// </summary>
        /// <param name="relativesSipe"></param>
        /// <returns></returns>
        public static Debtor RelativeConverter(RelativesSipeInformation relativesSipe)
        {
            var debtor = new Debtor();
            debtor.Birthplace = relativesSipe.BirthEntity;
            debtor.CURP = relativesSipe.Curp;
            debtor.MaternalLastName = relativesSipe.SecondSurname;
            debtor.Name = relativesSipe.Name;
            debtor.PaternalLastName = relativesSipe.FirstSurname;
            debtor.Relationship = relativesSipe.RelationshipDescription;
            debtor.Age = relativesSipe.Age;

            if (relativesSipe.Relationship != "")
            {
                relativesSipe.Relationship = relativesSipe.Relationship.Substring(0, 2);
                debtor.KeyRelationship = Int32.Parse(relativesSipe.Relationship);
            }

            return debtor;
        }

      

        /// <summary>
        ///     Regresa el equivalente de WeekDayEnum apartir de un DayofWeek
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static WeekDayEnum GetWeekEnumDay(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday)
                return WeekDayEnum.Domingo;
            if (dayOfWeek == DayOfWeek.Friday)
                return WeekDayEnum.Viernes;
            if (dayOfWeek == DayOfWeek.Monday)
                return WeekDayEnum.Lunes;
            if (dayOfWeek == DayOfWeek.Tuesday)
                return WeekDayEnum.Martes;
            if (dayOfWeek == DayOfWeek.Wednesday)
                return WeekDayEnum.Miercoles;
            if (dayOfWeek == DayOfWeek.Thursday)
                return WeekDayEnum.Jueves;
            if (dayOfWeek == DayOfWeek.Friday)
                return WeekDayEnum.Viernes;
            if (dayOfWeek == DayOfWeek.Saturday)
                return WeekDayEnum.Sabado;
            return WeekDayEnum.Lunes;
        }
    }
}