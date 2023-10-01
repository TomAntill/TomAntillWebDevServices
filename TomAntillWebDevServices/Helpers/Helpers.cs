using System;
using TomAntillWebDevServices.Data.Enums;
using Microsoft.Extensions.Configuration;

namespace TomAntillWebDevServices.Helpers
{
    public static class Helpers
    {
        private static string blobBasePath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetValue<string>("BlobStorageBasePath");
        public static string TrimFileName(WebsiteName appName, string fileName) => fileName.Replace($"{blobBasePath}{appName}/".ToLower(), String.Empty);


    }
}
