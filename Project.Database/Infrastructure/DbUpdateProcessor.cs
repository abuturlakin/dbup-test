using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using DbUp;
using DbUp.Engine;
using DbUp.Oracle;

using Oracle.ManagedDataAccess.Client;

namespace dbup_test
{
    public class DbUpdateProcessor
    {
        private readonly string _connectionString;
        private readonly TimeSpan? _timeOut;
        private readonly CommandParameters _commandParameters;
        
        public DbUpdateProcessor(
            CommandParameters commandParameters,
            string connectionString,
            TimeSpan? timeOut = null
        )
        {
            _connectionString = connectionString;
            _timeOut = _timeOut ?? TimeSpan.FromMinutes(1);
            _commandParameters = commandParameters;
        }

        public void Process()
        {
            ProcessCleanup();
            ProcessUpgrades();
        }

        private void ProcessCleanup()
        {
            var upgradeType = UpgradeType.Cleanup;
            
            if (!HasUpgradeType(upgradeType)) return;
            
            using (var connection = new OracleConnection(_connectionString))
            {
                try
                {
#warning TODO: Investigate to how use syntax similar to IF EXIST THEN ... to execute commands below only if tables are presented...
                    connection.Open();
                    ExecuteCommand(connection, "drop table SYSTEM.SCHEMAVERSIONS");
                    ExecuteCommand(connection, "drop sequence SYSTEM.SCHEMAVERSIONS_SEQUENCE");
                    Console.WriteLine("Database cleanup has been performed successfully");
                }
                catch (OracleException e)
                {
                    if (e.Number != 942)
                        throw;
                    Console.WriteLine("No \"schemaversion\" table found...");
                }
            }
        }
        
        private static void ExecuteCommand(OracleConnection connection, string cmdText)
        {
            var command = connection.CreateCommand();
            command.CommandText = cmdText;
            command.ExecuteNonQuery();
        }

        private DatabaseUpgradeResult ProcessUpgrades()
        {
            var processor = DeployChanges.To
                .OracleDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(
                    Assembly.GetExecutingAssembly(), path => FilterUpgradeScripts(path)
                 )
                .WithTransaction()
                .WithExecutionTimeout(_timeOut)
                .Build();

            return processor.PerformUpgrade(); 
        }

        private bool HasUpgradeType(UpgradeType upgradeType)
        {
            return _commandParameters.UpgradeTypes.Any(t => t == upgradeType);
        }
        
        private bool FilterUpgradeScripts(string path)
        {
            return _commandParameters.UpgradeTypes.Any(t => path.Contains(t.ToString()));
        }
    }
}
