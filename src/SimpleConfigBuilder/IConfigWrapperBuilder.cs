
namespace Configuration.SimpleConfiguration
{
    public interface IConfigWrapperBuilder
    {
        T PopulateConfigObject<T>() where T : new();
    }
}
