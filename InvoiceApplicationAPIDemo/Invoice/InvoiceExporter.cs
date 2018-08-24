#region MS Directives
using System;
using System.Collections.Generic;
#endregion

#region Custom Directives
using Serilog;
using Newtonsoft.Json;
using Xero.Api.Core;
using Xero.Api.Core.Model;
#endregion

namespace InvoiceApplicationAPIDemo
{
    internal class InvoiceExporter
    {
        private readonly IXeroCoreApi api;

        InvoiceInfo invoiceInfo;
        string accountsFileName = string.Empty;
        string vendorsFileName = string.Empty;

        /// <summary>
        /// Export Invoice to a physical location by specifying file names in app.config
        /// </summary>
        /// <param name="api"></param>
        /// <param name="accountsFileName"></param>
        /// <param name="vendorsFileName"></param>
        public InvoiceExporter(IXeroCoreApi api, string accountsFileName, string vendorsFileName)
        {
            this.api = api;
            this.accountsFileName = accountsFileName;
            this.vendorsFileName = vendorsFileName;
            invoiceInfo = new InvoiceInfo();
        }

        public void StartExport()
        {
            Dictionary<Guid, Contact> contacts = new Dictionary<Guid, Contact>();
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();

            Log.Information($"Reading Invoices");
            foreach (var invoice in api.Invoices.FindAsync().Result)
            {
                //Entry condition is only for AP (AccountsPayable)
                if (invoice.Type == Xero.Api.Core.Model.Types.InvoiceType.AccountsPayable)
                {
                    if (!contacts.ContainsKey(invoice.Contact.Id))
                        contacts.Add(invoice.Contact.Id, invoice.Contact);

                    foreach (var item in invoice.LineItems)
                    {
                        if (!accounts.ContainsKey(item.AccountCode))
                            accounts.Add(item.AccountCode, GetAccount(item.AccountCode));
                    }
                }
            }

            Log.Information($"Writing Vendors to {vendorsFileName}");
            invoiceInfo.SaveInvoiceInfo(JsonConvert.SerializeObject(contacts, Formatting.Indented), vendorsFileName);
            Log.Information($"Writing Accounts to {accountsFileName}");
            invoiceInfo.SaveInvoiceInfo(JsonConvert.SerializeObject(accounts, Formatting.Indented), accountsFileName);
        }

        private Account GetAccount(string accountCode)
        {
            Log.Debug($"Reading Account Information for: {accountCode}");
            var accountInfo = api.Accounts.FindAsync(accountCode).Result;
            return accountInfo;
        }
    }
}
