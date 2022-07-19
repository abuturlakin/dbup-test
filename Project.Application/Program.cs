using System;

using Project.Configuration;

namespace Project.Application
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(ConfigurationSettings.ConnectionString);
            Console.ReadLine();
        }
    }
}