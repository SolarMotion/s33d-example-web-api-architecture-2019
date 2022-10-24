using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;

namespace Library
{
    public static class CommomExtensions
    {
        #region Entity Framework

        /// <summary>
        ///     A DbEntityValidationException extension method that formates validation errors to string.
        /// </summary>
        public static string DbEntityValidationExceptionToString(this DbEntityValidationException e)
        {
            var validationErrors = e.DbEntityValidationResultToString();
            var exceptionMessage = string.Format("{0}{1}Validation errors:{1}{2}", e, Environment.NewLine, validationErrors);
            return exceptionMessage;
        }

        /// <summary>
        ///     A DbEntityValidationException extension method that aggregate database entity validation results to string.
        /// </summary>
        public static string DbEntityValidationResultToString(this DbEntityValidationException e)
        {
            return e.EntityValidationErrors
                    .Select(dbEntityValidationResult => dbEntityValidationResult.DbValidationErrorsToString(dbEntityValidationResult.ValidationErrors))
                    .Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
        }

        /// <summary>
        ///     A DbEntityValidationResult extension method that to strings database validation errors.
        /// </summary>
        public static string DbValidationErrorsToString(this DbEntityValidationResult dbEntityValidationResult, IEnumerable<DbValidationError> dbValidationErrors)
        {
            var entityName = string.Format("[{0}]", dbEntityValidationResult.Entry.Entity.GetType().Name);
            const string indentation = "\t - ";
            var aggregatedValidationErrorMessages = dbValidationErrors.Select(error => string.Format("[{0} - {1}]", error.PropertyName, error.ErrorMessage))
                                                   .Aggregate(string.Empty, (current, validationErrorMessage) => current + (Environment.NewLine + indentation + validationErrorMessage));
            return string.Format("{0}{1}", entityName, aggregatedValidationErrorMessages);
        }

        #endregion

        #region Transaction Scope

        public static TransactionScope ReadCommittedTransactionScope()
        {
            var transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }

        public static TransactionScope ReadUncommittedTransactionScope()
        {
            var transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }

        #endregion

        #region Formatting

        public static Guid ToGuid(this string value)
        {
            return new Guid(value);
        }

        public static string GetEnumDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }

            return GenericEnum.ToString();
        }

        public static string ToJson(this object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static string GetTime()
        {
            return DateTime.Now.ToString("HHmmss");
        }

        public static string ConvertDateTimeToDetailedString(this DateTime value)
        {
            return value.ToString("yyyyMMddhhmmss");
        }

        public static string ConvertDateTimeToString(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString("d/M/yyyy h:mm:ss tt");

            return "";
        }

        public static string ConvertDateTimeToString(this DateTime value)
        {
            return value.ToString("d/M/yyyy h:mm:ss tt");
        }

        public static string ConvertDateToString(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString("d/M/yyyy");

            return "";
        }

        public static string ConvertDateToString(this DateTime value)
        {
            return value.ToString("d/M/yyyy");
        }

        public static string ConvertTimeToString(this DateTime value)
        {
            return value.ToString("HHmmss");
        }

        public static string ConvertDecimalToString(this decimal? value)
        {
            if (value.HasValue)
                return Convert.ToDecimal(Math.Round(value.Value, 2)).ToString();

            return "0.00";
        }

        public static string ConvertDecimalToString(this decimal value)
        {
            return Convert.ToDecimal(Math.Round(value, 2)).ToString();
        }

        public static decimal ConvertToTwoDecimals(this decimal value)
        {
            return Convert.ToDecimal(Math.Round(value, 2));
        }

        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt(this int? value)
        {
            return value.HasValue ? Convert.ToInt32(value) : 0;
        }

        public static decimal ToDecimal(this decimal? value)
        {
            return value.HasValue ? value.Value : 0;
        }

        public static string ToString2(this object value)
        {
            return value == null ? "" : value.ToString();
        }

        public static string FormatBooleanFlag(this bool value)
        {
            return value ? "Yes" : "No";
        }

        #endregion

        #region Checking

        public static bool IsEmpty(this object value)
        {
            return value == null;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> value)
        {
            return (value == null || value.Count() == 0);
        }

        public static bool IsEmpty(this byte[] value)
        {
            return (value == null || value.Length == 0);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsEmpty(this int? value)
        {
            return !(value.HasValue && value.Value > 0);
        }

        public static bool IsEmpty(this DateTime? value)
        {
            return !value.HasValue;
        }

        #endregion
    }
}
