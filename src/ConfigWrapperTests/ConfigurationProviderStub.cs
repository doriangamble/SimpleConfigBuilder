using Configuration.SimpleConfiguration;
using System.Collections.Specialized;


namespace ConfigWrapperTests
{
    public class ConfigurationProviderStub : IConfigurationProvider
    {
        private readonly NameValueCollection _valuesCollection = null;

        public ConfigurationProviderStub(NameValueCollection collection)
        {
            _valuesCollection = collection;
        }

        public string GetValue(string key) => _valuesCollection.Get(key);
    }
}
