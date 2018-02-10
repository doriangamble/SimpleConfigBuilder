namespace Configuration.SimpleConfiguration
{
    public interface IConfigurationProvider
    {
        string GetValue(string key);
    }
}