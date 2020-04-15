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

        [TestMethod]
        public void TestMethod2()
        {
            var configFile = new ConfigFile("Test.ini");

            var abc = configFile.ToObject<Model>();

            Assert.IsTrue(abc != null);
        }
        
        public class Model
        {
            public VideoModel Video { get; set; }
            
            public LevelModel Level { get; set; }

            public class VideoModel
            {
                public bool? Fullscreen { get; set; }

                public int? Width { get; set; }

                public int? Height { get; set; }
                
                public int?[] Ints { get; set; }

                public double?[] Doubles { get; set; }

                public string[] BankPaymentlimits { get; set; }
            }

            public class LevelModel
            {
                public int? Foo { get; set; }

                public int? Bar { get; set; }

                public string[] Names { get; set; }

                public bool[] Booleans { get; set; }
            }
        }
    }
}
