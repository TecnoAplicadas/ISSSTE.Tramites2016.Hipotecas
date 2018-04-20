using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ISSSTE.Tramites2016.Hipotecas.Domian.Resources
{
    public class Resources
    {

        #region Constants

        public const string PrefixFolio = "VYC";
        public const string FormatDateForFolio = "yyyyMMddHHmmss";
        public const int NumberPositionsRFC = 10;


        private const string UnauthorizedUser = "Usuario no autorizado";

        #endregion Constants

        #region AppSettings Properties
        public static string EntitledNotExist
        {
            get
            {
                return ConfigurationManager.AppSettings["EntitledNotExist"];
            }
        }

        public static string GeneralPublicNotExist
        {
            get
            {
                return ConfigurationManager.AppSettings["GeneralPublicNotExist"];
            }
        }

        public static string NullParametersEntitle
        {
            get
            {
                return ConfigurationManager.AppSettings["NullParametersEntitle"];
            }
        }

        public static int QuoteIncompleteStatus
        {
            get
            {
                int status;
                if (!int.TryParse(ConfigurationManager.AppSettings["QuoteIncompleteStatus"], out status))
                    throw new Exception(Resources.QuoteKeyStatusNotAvailable);

                return status;
            }
        }

        public static int QuoteCompleteStatus
        {
            get
            {
                int status;
                if (!int.TryParse(ConfigurationManager.AppSettings["QuoteCompleteStatus"], out status))
                    throw new Exception(Resources.QuoteKeyStatusNotAvailable);

                return status;
            }
        }

        public static string QuoteKeyStatusNotAvailable
        {
            get
            {
                return ConfigurationManager.AppSettings["QuoteKeyStatusNotAvailable"];
            }
        }

        public static int EffectiveQuote
        {
            get
            {
                int effectiveQuote;
                if (!int.TryParse(ConfigurationManager.AppSettings["EffectiveQuote"], out effectiveQuote))
                    throw new Exception(Resources.QuoteKeyStatusNotAvailable);

                return effectiveQuote;
            }
        }

        public static string StatesNotExists
        {
            get
            {
                return ConfigurationManager.AppSettings["StatesNotExists"];
            }
        }

        public static string PackagesListNotExist
        {
            get
            {
                return ConfigurationManager.AppSettings["PackagesListNotExist"];
            }
        }

        public static string MortuariesListNotExist
        {
            get
            {
                return ConfigurationManager.AppSettings["MortuariesListNotExist"];
            }
        }
        public static string UserNotValid
        {
            get
            {
                return UnauthorizedUser;
            }
        }

        public static string ProductFromSirvelNotExists
        {
            get
            {
                return ConfigurationManager.AppSettings["ProductFromSirvelNotExists"];
            }
        }

        public static string ProductTypeExistingInQuotation
        {
            get
            {
                return ConfigurationManager.AppSettings["ProductTypeExistInQuotation"];
            }
        }

        public static string QuotationNotExisting
        {
            get
            {
                return ConfigurationManager.AppSettings["QuotationNotExisting"];
            }
        }

        public static string InvalidInput
        {
            get
            {
                return ConfigurationManager.AppSettings["InvalidInput"];
            }
        }

        public static int DaysDueQuotesToQuery
        {
            get
            {
                int days;
                if (!int.TryParse(ConfigurationManager.AppSettings["DaysDueQuotesToQuery"], out days))
                    throw new Exception(Resources.QuoteKeyStatusNotAvailable);

                return days;
            }
        }

        public static string TypesProductNotAvailable
        {
            get
            {
                return ConfigurationManager.AppSettings["TypesProductNotAvailable"];
            }
        }

        public static string QuotesNotExist
        {
            get
            {
                return ConfigurationManager.AppSettings["QuotesNotExist"];
            }
        }

        public static string CurpAndRfcMissing
        {
            get
            {
                return ConfigurationManager.AppSettings["CurpAndRfcMissing"];

            }
        }

        public static string SubjectCustomerSurvey
        {
            get
            {
                return ConfigurationManager.AppSettings["SubjectCustomerSurvey"];
            }
        }

        public static string BodyCustomerSurvey
        {
            get
            {
                return ConfigurationManager.AppSettings["BodyCustomerSurvey"];
            }
        }

        public static string EmailSendError
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailSendError"];
            }
        }

        public static string ErrorToDownLoadQuotation
        {
            get
            {
                return ConfigurationManager.AppSettings["ErrorToDownLoadQuotation"];

            }
        }
        public static string FileNotFound
        {
            get
            {
                return ConfigurationManager.AppSettings["FileNotFound"];

            }
        }
        #endregion AppSettings Properties
    }
}
