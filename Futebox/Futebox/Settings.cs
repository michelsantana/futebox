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

        public static bool ScheduleInitialized { get; set; }
        public static bool IS_IBM { get; set; }
        public static bool IS_GOOGLE { get; set; }
    }
}
