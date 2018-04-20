using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSSTE.Tramites2016.Common.Web.Helpers
{
    /// <summary>
    /// Contains methods that help identify the type of a object
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Checks if the type is any kind of character based type
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsString(Type type)
        {
            return type == typeof(String) || type == typeof(Char) || type == typeof(Char?);
        }

        /// <summary>
        /// Checks if the type is any kind of integer number
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsInteger(Type type)
        {
            return type == typeof(Int16) || type == typeof(Int16?) || type == typeof(Int32) || type == typeof(Int32?) || type == typeof(Int64) || type == typeof(Int64?) ||
                type == typeof(UInt16) || type == typeof(UInt16?) || type == typeof(UInt32) || type == typeof(UInt32?) || type == typeof(UInt64) || type == typeof(UInt64?);
        }

        /// <summary>
        /// Checks if the type is any float point number
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsFloat(Type type)
        {
            return type == typeof(Single) || type == typeof(Single?) || type == typeof(Double) || type == typeof(Double?);
        }

        /// <summary>
        /// Checks if the type is a decimal number
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsDecimal(Type type)
        {
            return type == typeof(Decimal) || type == typeof(Decimal?);
        }

        /// <summary>
        /// Checks if the type is any kind of number
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsNumber(Type type)
        {
            return IsInteger(type) || IsFloat(type) || IsDecimal(type);
        }

        /// <summary>
        /// Checks if the type is a boolean
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsBoolean(Type type)
        {
            return type == typeof(Boolean) || type == typeof(Boolean?);
        }

        /// <summary>
        /// Checks if the type is a Date
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsDate(Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }

    }
}