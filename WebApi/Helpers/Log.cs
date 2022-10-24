using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using static Library.CommomExtensions;

namespace WebApi.Helpers
{
    public class Log
    {
        private static readonly Log _instance = new Log();
        protected ILog publicLogger = LogManager.GetLogger("PublicApiLogger");
        protected ILog seednetLogger = LogManager.GetLogger("SeednetApiLogger");

        #region Public Log

        public static void PublicInfo(string message)
        {
            _instance.publicLogger.Info(message);
        }

        public static void PublicError(Exception ex)
        {
            var exString = "";

            try
            {
                if (ex is DbEntityValidationException)
                    exString = (ex as DbEntityValidationException).DbEntityValidationExceptionToString();
                else
                    exString = ex.ToString();

                _instance.publicLogger.Error(exString);
            }
            catch (Exception iex)
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.WriteEntry(iex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        #endregion

        #region Seednet Log

        public static void SeednetInfo(string message)
        {
            _instance.publicLogger.Info(message);
        }

        public static void SeednetError(Exception ex)
        {
            var exString = "";

            try
            {
                if (ex is DbEntityValidationException)
                    exString = (ex as DbEntityValidationException).DbEntityValidationExceptionToString();
                else
                    exString = ex.ToString();

                _instance.seednetLogger.Error(exString);
            }
            catch (Exception iex)
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.WriteEntry(iex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        #endregion
    }
}