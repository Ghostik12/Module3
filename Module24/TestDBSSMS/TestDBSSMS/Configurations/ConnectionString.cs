namespace TestDBSSMS.Configurations
{
    public static class ConnectionString
    {
        public static string MsSqlConnection => @"Server=.\SQLEXPRESS01;Database=test;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}