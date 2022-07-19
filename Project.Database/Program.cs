using dbup_test;

using Project.Configuration;

namespace Project.Database
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var parameters = CommandParameters.FromArguments(args);
            var processor = new DbUpdateProcessor(parameters, ConfigurationSettings.ConnectionString);
            processor.Process();
        }
    }
}