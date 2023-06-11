using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Pfm.Core.Helpers;
using Pfm.Core.Models;

namespace Pfm.Api.Helpers
{
    public static class ApplicationSetting
    {
        public static void AddSettings(this AppSettingModel appSettings)
        {
            AppSetting.KeySecret = appSettings.KeySecret;
            AppSetting.ConnectionString = appSettings.ConnectionString;
            AppSetting.LatitudeOffice = appSettings.LatitudeOffice;
            AppSetting.LongitudeOffice = appSettings.LongitudeOffice;

            AppSetting.BasePathFile = appSettings.BasePathFile;
            AppSetting.BaseUrl = appSettings.BaseUrl;
            AppSetting.BaseUrlProxy = appSettings.BaseUrlProxy;
            AppSetting.BaseUrlFile = appSettings.BaseUrlFile;

            AppSetting.PathFileUser = $"{appSettings.BasePathFile}/{appSettings.PathFileUser}";
            AppSetting.UrlFileUser = $"/{appSettings.BaseUrlFile}/{appSettings.PathFileUser}";
            FileHelper.CreateFolder($"{appSettings.BasePathFile}/{appSettings.PathFileUser}");
            
            AppSetting.PathFilePresence = $"{appSettings.BasePathFile}/{appSettings.PathFilePresence}";
            AppSetting.UrlFilePresence = $"/{appSettings.BaseUrlFile}/{appSettings.PathFilePresence}";
            FileHelper.CreateFolder($"{appSettings.BasePathFile}/{appSettings.PathFilePresence}");

            AppSetting.OrgNama = appSettings.OrgNama;
            AppSetting.OrgEmail = appSettings.OrgEmail;
            AppSetting.OrgEmailPassword = appSettings.OrgEmailPassword;
            AppSetting.OrgEmailSmtpServer = appSettings.OrgEmailSmtpServer;
            AppSetting.OrgEmailSmtpPort = appSettings.OrgEmailSmtpPort;
        }

        public static void PathSetting(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(AppSetting.PathFileUser),
                RequestPath = new PathString(AppSetting.UrlFileUser)
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(AppSetting.PathFilePresence),
                RequestPath = new PathString(AppSetting.UrlFilePresence)
            });
        }
    }
}

