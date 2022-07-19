using System;
using System.Collections.Generic;

namespace dbup_test
{
    public class CommandParameters
    {
        public IEnumerable<UpgradeType> UpgradeTypes { get; set; }

        public static CommandParameters FromArguments(string[] args)
        {
            var upgradeTypes = new List<UpgradeType>();
            
            foreach (var arg in args)
            {
                var refinedArg = arg.Replace("/", "");
                upgradeTypes.Add((UpgradeType) Enum.Parse(typeof(UpgradeType), refinedArg, true));
            }

            return new CommandParameters { UpgradeTypes = upgradeTypes };
        }
    }
}
