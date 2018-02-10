using System;
using System.Collections.Generic;
using System.Configuration;
using Configuration.SimpleConfiguration;
using ConfigWrapperTests.ConfigurationTests;
using NUnit.Framework;
using System.Linq;

namespace ConfigWrapperTests
{
    public class AppConfigBasedTests
    {
        Dictionary<string, string> _config;
        IConfigurationProvider _configProvider;


        [OneTimeSetUp]
        public void Setup()
        {
            var collection = ConfigurationManager.AppSettings;

            _configProvider = new ConfigurationProviderStub(collection);

            _config = collection.AllKeys.ToDictionary(k=>k, k=>collection[k], StringComparer.InvariantCultureIgnoreCase);
        }



        [Test]
        public void ConfigWrapper_Test_Config_Object_IsPopulated_Normally_WhenNoAttributeOn_AppConfig()
        {
            var sut = new ConfigWrapperImpl<TestConfig02>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(_config["TableName"], sut.Value.TableName);
            Assert.AreEqual(Convert.ToDecimal(_config["DecimalNumber1"]), sut.Value.DecimalNumber1);
        }

        [Test]
        public void ConfigWrapper_Test_Config_Object_IsntPopulated_WhenNoAttributeOn_AppConfig2()
        {
            var sut = new ConfigWrapperImpl<TestConfig03>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(6, sut.Value.Number1);
            Assert.AreEqual("unset", sut.Value.ServerURL);
        }

        [Test]
        public void ConfigWrapper_Test_Config_Object_IsntPopulated_WhenDefaultAttributeIsOn_AndAppConfigContainsValue()
        {
            var sut = new ConfigWrapperImpl<TestConfig03>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(5, sut.Value.IntegerNumber);
        }



        [Test]
        public void ConfigWrapper_Test_Config_Object_IsPopulated_WhenWithComplexTypes_AreUsed()
        {
            var sut = new ConfigWrapperImpl<TestConfig06>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(Convert.ToDecimal(_config["DecimalNumber1"]), sut.Value.DecimalNumber1);
        }

        [Test]
        public void ConfigWrapper_Test_InheritedConfig_Object_IsPopulated_WithAllComplexTypesAreUsed()
        {
            var sut = new ConfigWrapperImpl<TestConfig07>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(Convert.ToDecimal(_config["DecimalNumber1"]), sut.Value.DecimalNumber1);
            Assert.AreEqual(_config["TableName"], sut.Value.TableName);
            Assert.AreEqual(Convert.ToBoolean(_config["BooleanValue1"]), sut.Value.BooleanValue1);
            Assert.AreEqual(_config["ServerSettings.Hostname"], sut.Value.Hostname);
        }

        [Test]
        public void ConfigWrapper_Test_NonMappedProperties()
        {
            Assert.Throws<Exception>(() => new ConfigWrapperImpl<TestConfig08>(new ConfigWrapperBuilder(_configProvider)));
        }

        [Test]
        public void ConfigWrapper_Test_OptionalNonMappedProperty()
        {
            var sut = new ConfigWrapperImpl<TestConfig09>(new ConfigWrapperBuilder(_configProvider));

            Assert.IsNull(sut.Value.UnmappedProperty);
        }


        [Test]
        public void ConfigWrapper_Test_ObtainSupplyLicenceSettings()
        {
            var sut = new ConfigWrapperImpl<WebAndSQLServerSettings>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(_config["ServerSettings.HostName"], sut.Value.HostName);
            Assert.AreEqual(int.Parse(_config["ServerSettings.Port"]), sut.Value.Port);

            Assert.AreEqual(_config["ServerSettings.SQL.HostName"], sut.Value.SQL.HostName);
            Assert.AreEqual(int.Parse(_config["ServerSettings.SQL.Port"]), sut.Value.SQL.Port);

        }

        [Test]
        public void ConfigWrapper_Test_DateTimeProperty()
        {
            var sut = new ConfigWrapperImpl<TestConfig10>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(DateTime.Parse( _config["DateTimeValue"]), sut.Value.DateTimeValue);
            Assert.AreEqual(_config["DateTimeValue"], sut.Value.DateTimeValue.ToString($"yyyy-MM-dd"));


        }


        [Test]
        public void ConfigWrapper_Test_CaseInsensitivity()
        {
            var sut = new ConfigWrapperImpl<TestConfig11>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(_config["Vegetable"], sut.Value.Vegetable);


        }

        [Test]
        public void ConfigWrapper_Test_DeepNesting()
        {
            var sut = new ConfigWrapperImpl<Node1>(new ConfigWrapperBuilder(_configProvider));

            Assert.AreEqual(_config["Node1.Node2.Node3.Node4"], sut.Value.Node2.Node3.Node4);


        }
        

    }
}
