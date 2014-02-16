using System.Configuration;

namespace DHallPicker.Data
{
    public static class ConfigurationSettings
    {
        private static string connectionString;

        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }

            return connectionString;
        }
    }
}
