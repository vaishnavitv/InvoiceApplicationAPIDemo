#region Custom Directives
using Xero.Api;
using Xero.Api.Infrastructure.Authenticators;
using Xero.Api.Infrastructure.Interfaces;
#endregion

#region Application Directives
using InvoiceApplicationAPIDemo.Helpers;
#endregion

namespace InvoiceApplicationAPIDemo.Authenticators
{
    /// <summary>
    /// Xero API Public Authentication.
    /// </summary>
    class PublicAuthenticator : PublicAuthenticatorBase
    {
        public PublicAuthenticator(ITokenStoreAsync store)
           : this(store, new XeroApiSettings())
        {
        }

        public PublicAuthenticator(ITokenStoreAsync store, IXeroApiSettings xeroApiSettings)
            : base(store, xeroApiSettings)
        {
        }

        protected override string AuthorizeUser(IToken token, string scope = null, bool redirectOnError = false)
        {
            var authorizeUrl = GetAuthorizeUrl(token, scope, redirectOnError);
            ProcessHelper.OpenBrowser(authorizeUrl);

            System.Console.WriteLine("Enter the PIN given on the web page:");
            string pin = System.Console.ReadLine();
            return pin.Trim();
        }
    }
}
