using System;
using log4net;



namespace POCLogger
{
    public static class Logger
    {
        private static ILog Log { get; set; }

        static Logger()
        {
            Log = LogManager.GetLogger("IFCIntranet");
        }
        public static void write(object message, Exception ex = null, string level = null)
        {

            switch (level)
            {
                case "ERROR":

                    if (ex != null)
                        Log.Error(message, ex);
                    else
                        Log.Error(message);
                    break;
                case "WARN":
                    if (ex != null)
                        Log.Warn(message, ex);
                    else
                        Log.Warn(message);
                    break;

                default:
                    if (ex != null)
                        Log.Fatal(message, ex);
                    else
                        Log.Info(message);
                    break;
            }





        }

        public static void Debug(object message, Exception ex = null)
        {
            if (ex != null)
                Log.Debug(message, ex);
            else
                Log.Debug(message);
        }



        public static void Info(object message, string loggerName = null)
        {
            if (loggerName != null)
            {
                ILog logger = LogManager.GetLogger(loggerName);
                logger.Info(message);
            }
            else
                Log.Info(message);
        }

        public static void Error(object message, Exception ex = null)
        {
            if (ex != null)
                Log.Error(message, ex);
            else
                Log.Error(message);

        }

        public static void Fatal(object message, Exception ex = null)
        {
            if (ex != null)
                Log.Fatal(message, ex);
            else
                Log.Fatal(message);

        }
    }
}