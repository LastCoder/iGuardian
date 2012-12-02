using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Typhon.Common;

namespace iGuardian.Methods
{
    public static class Logger
    {
        public static void WriteLog(LogLevel logLevel, string message)
        {
            Logging.Write(logLevel, "[{0} {1}] {2}", iGuardian.Instance.Name, iGuardian.Instance.Version, message);
        }

        public static void WriteLog(LogLevel logLevel, string format, params object[] args)
        {
            WriteLog(logLevel, string.Format(format, args));
        }

        public static void Write(string message)
        {
            WriteLog(LogLevel.Normal, message);
        }

        public static void Write(string message, params object[] args)
        {
            WriteLog(LogLevel.Normal, message, args);
        }

        public static void WriteVerbose(string message)
        {
            WriteLog(LogLevel.Verbose, message);
        }

        public static void WriteVerbose(string message, params object[] args)
        {
            WriteLog(LogLevel.Verbose, message, args);
        }
    }
}
