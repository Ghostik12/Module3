namespace TestDBSSMS.Configurations
{
    public static class ConnectionString
    {
        public static string MsSqlConnection => @"Server=.\SQLEXPRESS;Database=test;Trusted_Connection=True;";
    }
}