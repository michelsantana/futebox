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
        public static string ApplicationRoot;
        public static string ApplicationHttpBaseUrl;
        public static string TelegramBotToken;
        public static string TelegramNotifyUserId;
        public static string ChromeDefaultDownloadFolder;
        public static bool DEBUGMODE;
    }
}
