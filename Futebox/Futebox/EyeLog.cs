using System;
using System.IO;

namespace Futebox
{
    public class EyeLog
    {
        public static void Log(string message, int tryTimes = 0)
        {
            try
            {
                string folder = $"{Settings.ApplicationsRoot}/log";
                string file = $"{folder}/{DateTime.Now.ToString("yyyyMMdd")}.txt";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (!File.Exists(file)) File.WriteAllText(file, "");

                File.AppendAllText(file, $"\n{DateTime.Now.ToString("HH:mm:ss")} - {message}");
            }
            catch (Exception ex)
            {
                if (tryTimes < 5) Log(message, tryTimes++);
                else throw ex;
            }
        }
    }
}
