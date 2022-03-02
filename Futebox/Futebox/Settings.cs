using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox
{
    public static class Settings
    {
        public static string BackendRoot;
        public static string ApplicationsRoot;
        public static string ApplicationHttpBaseUrl;
        public static string RobotEndpointBaseUrl;
        public static string TelegramBotToken;
        public static string TelegramNotifyUserId;
        public static bool DEBUGMODE;
        public static string ChromeDefaultDownloadFolder = "C:/Users/Michel/Downloads/";
    }
}
