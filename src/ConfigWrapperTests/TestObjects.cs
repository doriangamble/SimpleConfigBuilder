namespace ConfigWrapperTests
{

    using Configuration.SimpleConfiguration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConfigurationTests
    {
        public class ServerSettingsBase
        {
            public string HostName { get; set; }
            public int Port { get; set; }
        }

        [ConfigWrapperClass(KeyPrefix = "ServerSettings")]
        public class WebAndSQLServerSettings : ServerSettingsBase
        {
            [ConfigWrapperProperty(IsNested = true)]
            public ServerSettingsBase SQL { get; set; }
        }


        public class TestConfig02
        {
            public string TableName { get; set; }
            public decimal DecimalNumber1 { get; set; }
        }

        public class TestConfig03
        {
            [ConfigWrapperProperty(DefaultValue = 6)]
            public int Number1 { get; set; }
            [ConfigWrapperProperty(DefaultValue = 6)]
            public int IntegerNumber { get; set; }
            [ConfigWrapperProperty(DefaultValue = "unset")]
            public string ServerURL { get; set; }

        }



        public class TestConfig06
        {
            public decimal DecimalNumber1 { get; set; }

        }

        public class TestConfig07 : TestConfig02
        {
            public bool BooleanValue1 { get; set; }

            [ConfigWrapperProperty(KeyPrefix = "ServerSettings")]
            public string Hostname { get; set; }

        }

        public class TestConfig08
        {
            public bool UnmappedProperty { get; set; }
        }

        public class TestConfig09
        {
            [ConfigWrapperProperty(Optional = true)]
            public bool? UnmappedProperty { get; set; }
        }


        public class TestConfig10
        {
            public DateTime DateTimeValue { get; set; }
        }

        public class TestConfig11
        {
            public string Vegetable { get; set; }
        }



        public class Node1
        {
            [ConfigWrapperProperty(IsNested = true)]
            public Node2 Node2 { get; set; }
        }

        public class Node2
        {
            [ConfigWrapperProperty(IsNested = true)]
            public Node3 Node3 { get; set; }
        }

        public class Node3
        {
            public string Node4 { get; set; }
        }
        

    }

}
