using AramisLauncher.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.Logger
{
    class LoggerManager
    {
        private static string logFilePath = CommonData.aramisFolder + "launcher_log.txt";
        public static void log(string toLog)
        {
            toLog += "\n";
            if(!Directory.Exists(CommonData.aramisFolder))
            {
                Directory.CreateDirectory(CommonData.aramisFolder);
            }
            System.IO.File.AppendAllText(logFilePath, toLog);
        }
    }
}
