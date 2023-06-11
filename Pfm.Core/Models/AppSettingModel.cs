using System;
namespace Pfm.Core.Models
{
    public class AppSettingModel
    {
        public string ConnectionString { get; set; }
        public string KeySecret { get; set; }
        public double LatitudeOffice { get; set; }
        public double LongitudeOffice { get; set; }

        public string BaseUrl { get; set; }
        public string BaseUrlProxy { get; set; }
        public string BaseUrlFile { get; set; }
        public string BasePathFile { get; set; }
        public string PathFileUser { get; set; }
        public string PathFilePresence { get; set; }

        public string OrgNama { get; set; }
        public string OrgEmail { get; set; }
        public string OrgEmailPassword { get; set; }
        public string OrgEmailSmtpServer { get; set; }
        public double OrgEmailSmtpPort { get; set; }

    }
}

