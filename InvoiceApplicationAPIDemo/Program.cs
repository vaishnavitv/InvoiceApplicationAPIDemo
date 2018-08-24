#region MS Directives
using System;
using Microsoft.Extensions.Configuration;
#endregion

#region Custom Directives
using Serilog;
using Xero.Api;
using Xero.Api.Core;
using Xero.Api.Infrastructure.OAuth;
#endregion

#region Application Directives
using InvoiceApplicationAPIDemo.Authenticators;
using InvoiceApplicationAPIDemo.TokenStore;
#endregion

namespace InvoiceApplicationAPIDemo
{
    /// <summary>
    /// Invoice Application to extract both the list of Accounts, and the list of
    /// Vendors, out of Xero.API system and store them on disk in the path where the application runs from.
    /// The file names can be customized in App.config
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// Initialize Application Configuration using App.config and appsettings.json.
            /// </summary>
            var appConfiguration = new ConfigurationBuilder().AddJsonFile(ApplicationConstants.AppSettings).Build();

            /// <summary>
            /// Using SeriLog for Logging exception and information into a file in a .Net Core application.
            /// New logs are appended to the existing application
            /// </summary>
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(appConfiguration).CreateLogger();

            var api = InitializeAPI();

            try
            {
                Log.Information("Querying and Saving an Invoice through Xero.API");
                new InvoiceExporter(api, ApplicationConstants.Accounts, ApplicationConstants.Vendors).StartExport();
            }
            catch (Exception exception)
            {
                Log.Error("Error in Application: {0}", exception.Message);
            }

            System.Console.WriteLine("Completed. Press any key to continue ...");
            System.Console.ReadLine();

        }

        /// <summary>
        /// Read API information from appSettings.json file.
        /// </summary>
        /// <returns>IXeroCoreApi instance</returns>
        private static IXeroCoreApi InitializeAPI()
        {
            var settings = new XeroApiSettings();

            //AppType is mandatory. Hence the validation. This is the main entry point
            bool isAppTypeAvailable = !string.IsNullOrEmpty(settings.AppType) ? true : false;

            if (!isAppTypeAvailable)
            {
                Console.WriteLine("Please provide the mandatory AppType information in appSettings.json and restart the application");

                Log.Information("Please provide the mandatory AppType information in appSettings.json and restart the application ");

                System.Environment.Exit(0);

            }

            //Convert string to Enum and ignore the case sensitivity
            Enum.TryParse(settings.AppType, true, out AppTypes appType);

            switch (appType)
            {
                case AppTypes.@public:
                    return PublicApp();
                case AppTypes.@private:
                    throw new NotImplementedException("AppType: private is not implemented.");
                case AppTypes.partner:
                    throw new NotImplementedException("AppType: partner is not implemented.");
                default:
                    throw new ApplicationException("AppType did not match one of: private, public, partner");
            }

        }

        /// <summary>
        /// Creates IXeroCoreApi for Public App. 
        /// </summary>
        /// <returns>IXeroCoreApi instance</returns>
        private static IXeroCoreApi PublicApp()
        {
            var tokenStore = new MemoryTokenStore();
            var user = new ApiUser { Identifier = Environment.MachineName };
            var publicAuth = new PublicAuthenticator(tokenStore);

            return new Xero.Api.Infrastructure.Applications.Public.Core(publicAuth, user)
            {
                UserAgent = "Xero API - Invoice Application Demo"
            };
        }

    }
}
