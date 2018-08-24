#region MS Directives
using System.Configuration;
using System.Linq;
#endregion

namespace InvoiceApplicationAPIDemo
{
    /// <summary>
    /// App.settings constants reside here
    /// </summary>
    public static class ApplicationConstants
    {
        public static string AppSettings => ConfigurationManager.AppSettings["AppSettings"] ?? "appsettings.json";
        public static string Vendors => ConfigurationManager.AppSettings.Get("Vendors") ?? "Vendors.txt";
        public static string Accounts => ConfigurationManager.AppSettings["Accounts"] ?? "Accounts.txt";
    }
}
