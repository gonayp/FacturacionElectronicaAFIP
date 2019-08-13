using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPPWindowsFormsAFIP
{

    public enum LogTarget
    {
        File, Database, EventLog
    }

    public static class LogHelper
    {
        private static LogBase logger = null;
        public static void Log(LogTarget target, string message)
        {
            switch (target)
            {
                case LogTarget.File:
                    logger = new FileLogger();
                    logger.Log(message);
                    break;
                case LogTarget.Database:
                    logger = new DBLogger();
                    logger.Log(message);
                    break;
                case LogTarget.EventLog:
                    logger = new EventLogger();
                    logger.Log(message);
                    break;
                default:
                    return;
            }
        }
    }

    public abstract class LogBase
    {
        protected readonly object lockObj = new object();
        public abstract void Log(string message);
    }

    public class FileLogger : LogBase
    {
        public string filePath = Main.LogUrl;//@"C:\soah\afip\GPP_AFIP_Log.txt";
        public override void Log(string message)
        {
            lock (lockObj)
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath,true))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }
    }

    public class EventLogger : LogBase
    {
        public override void Log(string message)
        {
            lock (lockObj)
            {
                EventLog m_EventLog = new EventLog("");
                m_EventLog.Source = "IDGEventLog";
                m_EventLog.WriteEntry(message);
            }
        }
    }

    public class DBLogger : LogBase
    {
        string connectionString = string.Empty;
        public override void Log(string message)
        {
            lock (lockObj)
            {
                //Code to log data to the database
            }
        }
    }


   
}
