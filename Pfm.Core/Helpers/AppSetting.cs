using System;
namespace Pfm.Core.Helpers
{
    public static class AppSetting
    {
        public static string ConnectionString { get; set; }
        public static string KeySecret { get; set; }

        public static string BaseUrl { get; set; }
        public static string BaseUrlProxy { get; set; }
        public static string BaseUrlFile { get; set; }
        public static string BasePathFile { get; set; }

        public static string PathFileUser { get; set; }
        public static string PathFilePresence { get; set; }

        public static string UrlFileUser { get; set; }
        public static string UrlFilePresence { get; set; }

        public static string OrgNama { get; set; }
        public static string OrgEmail { get; set; }
        public static string OrgEmailPassword { get; set; }
        public static string OrgEmailSmtpServer { get; set; }
        public static double OrgEmailSmtpPort { get; set; }

    }
}

