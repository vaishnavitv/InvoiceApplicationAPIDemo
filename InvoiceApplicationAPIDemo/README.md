InvoiceApplication through Xero.API Demo
========================================

This is a .Net Core library to make the application cross platform
Run the application by using the command 

```dotnet InvoiceApplicationAPIDemo.dll```

 from the location where the library is deployed

This project contains an exporter for the Invoices in Xero. This will launch a browser on the
users machine and prompt for the PIN. The token returned will be stored.

Configuration
-------------

You will need to enter appropriate values in the ```appsettings.json``` and ```app.config```
 file as all fields are mandatory.

Prerequisites
-------------

- Newtonsoft.Json -> *Serializing JSON*
- Serilog.* -> *Logging for the Application*
- ConfigurationManager -> *This is not available in .Net Core Application by default.*
- Xero.Api -> *API interactions with Xero*

Contact
-------

Vaishnavi T V (mailto:vaishnavi.subramanian@gmail.com)
