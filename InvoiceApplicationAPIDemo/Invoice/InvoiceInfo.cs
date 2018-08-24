#region MS Directives
using System;
using System.IO;
#endregion

namespace InvoiceApplicationAPIDemo
{
    /// <summary>
    /// Save the json format string in a file. Specify the desired file name in App.config
    /// </summary>
    public class InvoiceInfo
    {
        private readonly string path;

        public InvoiceInfo()
        {
            //Get the application's current working directory
            path = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public void SaveInvoiceInfo(string input, string fileName)
        {
            string fullPath = Path.Combine(path, fileName);
          
            try
            {
                //Create a new file on every run
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    file.WriteLine(input);
                    file.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

