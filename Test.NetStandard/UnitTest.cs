namespace Test.NetStandard
{
    using System.Collections.Generic;
    using System.IO;
    using EasyConfig.NetStandard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var list = new List<string>();

            var configFile = new ConfigFile("Test.ini");

            foreach (var group in configFile.SettingGroups)
            {
                list.Add("****************************");

                list.Add($"{group.Key}:");

                foreach (KeyValuePair<string, Setting> value in group.Value.Settings)
                {
                    list.Add($"{value.Key} = {value.Value.RawValue} (Is Array? {value.Value.IsArray})");
                }

                list.Add("****************************");
            }

            configFile.Save("TestConfig2.ini");

            Assert.IsTrue(list.Count > 0 && File.Exists("TestConfig2.ini"));
        }
    }
}
