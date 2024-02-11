using System;

namespace Astral.Canvas
{
    public enum LoggingMode
    {
        None,
        Console,
        VSDebug
    }
    public static class Debugger
    {
        /// <summary>
        /// Logs a message to the output as specified in Application.Config
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dateTime"></param>
        public static void Log(object message, bool dateTime = true)
        {
            if (Application.loggingMode == LoggingMode.None) return;

            string messageWithDatetime = dateTime ? '[' + DateTime.Now.ToString() + "] " + message.ToString() : message.ToString();
            if (Application.loggingMode == LoggingMode.Console)
            {
                Console.WriteLine(messageWithDatetime);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(messageWithDatetime);
            }
        }
    }
}
