#region MS Directives
using System.Diagnostics;
using System.Runtime.InteropServices;
#endregion

namespace InvoiceApplicationAPIDemo.Helpers
{
    public class ProcessHelper
    {
        /// <summary>
        /// .net Core Helper to Open Browser.
        /// It has issues on Windows starting a process to open a browser.
        /// Credit: https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
        /// </summary>

        public static void OpenBrowser(string url)
        {

            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
