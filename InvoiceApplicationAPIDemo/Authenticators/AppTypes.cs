namespace InvoiceApplicationAPIDemo
{
    /// <summary>
    /// Mapping appSettings.json Application Type through a readable Enum for ease of validation
    /// </summary>
    internal enum AppTypes
    {
        //As public and private are keywords, prepend the enum with @
        @private,
        @public,
        partner
    }

}
