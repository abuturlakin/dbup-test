using System.Configuration;

namespace Project.Configuration
{
    public class ConfigurationSettings
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
    }
}
