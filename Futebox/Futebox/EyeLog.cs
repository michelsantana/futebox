using System;
using System.IO;

namespace Futebox
{
    public class EyeLog
    {
        public static string Log(string message, int tryTimes = 0)
        {
            try
            {
                string folder = $"{Settings.ApplicationsRoot}/log";
                string file = $"{folder}/{DateTime.Now.ToString("yyyyMMdd")}.txt";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (!File.Exists(file)) File.WriteAllText(file, "");
                var newMessage = $"{DateTime.Now.ToString("HH:mm:ss")} - {message}";
                File.AppendAllText(file, $"\n{newMessage}");
                Console.WriteLine(newMessage);
                return newMessage;
            }
            catch (Exception ex)
            {
                if (tryTimes < 5) return Log(message, tryTimes++);
                else throw ex;
            }
        }

        public static string Log(Exception ex, int tryTimes = 0)
        {
            try
            {
                var message = "";
                do
                {
                    message += $"\n{ex.Message}";
                    ex = ex?.InnerException;
                } while (ex?.InnerException != null);

                string folder = $"{Settings.ApplicationsRoot}/log";
                string file = $"{folder}/{DateTime.Now.ToString("yyyyMMdd")}.txt";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (!File.Exists(file)) File.WriteAllText(file, "");
                var newMessage = $"{DateTime.Now.ToString("HH:mm:ss")} - {message}";
                File.AppendAllText(file, $"\n{newMessage}");
                Console.WriteLine(newMessage);
                return newMessage;
            }
            catch (Exception exx)
            {
                if (tryTimes < 5) return Log(ex, tryTimes++);
                else throw exx;
            }
        }

        public static string LogObject(object message, int tryTimes = 0)
        {
            try
            {
                string folder = $"{Settings.ApplicationsRoot}/log";
                string file = $"{folder}/{DateTime.Now.ToString("yyyyMMdd")}.txt";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (!File.Exists(file)) File.WriteAllText(file, "");
                var newMessage = $"{DateTime.Now.ToString("HH:mm:ss")} - {message}";
                File.AppendAllText(file, $"\n{newMessage}");
                Console.WriteLine(message);
                return newMessage;
            }
            catch (Exception ex)
            {
                if (tryTimes < 5) return LogObject(message, tryTimes++);
                else throw ex;
            }
        }
    }
}
