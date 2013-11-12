using System;
using System.Collections.Generic;
using System.Text;

using EasyConfig;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigFile configFile = new ConfigFile("Test.ini");

            foreach (KeyValuePair<string, SettingsGroup> group in configFile.SettingGroups)
            {
                Console.WriteLine("****************************");
                Console.WriteLine(group.Key + ":");
                Console.WriteLine();

                foreach (KeyValuePair<string, Setting> value in group.Value.Settings)
                    Console.WriteLine("{0} = {1} (Is Array? {2})", value.Key, value.Value.RawValue, value.Value.IsArray);

                Console.WriteLine();
            }

            Console.ReadKey(true);

            configFile.Save("TestConfig2.ini");
        }
    }
}
