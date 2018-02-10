using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Configuration.SimpleConfiguration
{
    /// <summary>
    /// Builds The Config Object From A Configuration Source (Usually App Settings)
    /// </summary>
    public class ConfigWrapperBuilder : IConfigWrapperBuilder
    {
        private readonly IConfigurationProvider _configurationProvider;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="configProvider">The source of the configuration</param>
        public ConfigWrapperBuilder(IConfigurationProvider configProvider)
        {
            this._configurationProvider = configProvider;
        }


        public virtual T PopulateConfigObject<T>() where T : new()
        {
            var wrappedType = typeof(T);

            // Create An Instance Of The Type
            T instance = new T();

            // Then Attempt To Populate It
            PopulateConfigObject(instance, wrappedType, wrappedType.Name);

            return instance;
        }



        protected void PopulateConfigObject(object instance, Type wrappedType, string basePropertyName)
        {
            var errors = new List<string>();

            var instanceAttribute = Attribute.GetCustomAttribute(wrappedType, typeof(ConfigWrapperClassAttribute), true) as ConfigWrapperClassAttribute;

            foreach (var subPropertyInfo in wrappedType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!subPropertyInfo.CanWrite) break; // No Point Wasting Our Time Here If The Property Is Not Writeable

                var propertyAttribute = Attribute.GetCustomAttribute(subPropertyInfo, typeof(ConfigWrapperPropertyAttribute), true) as ConfigWrapperPropertyAttribute;

                if (propertyAttribute?.IsNested ?? false) // If The Property Is Nested We Recurse Into It
                {
                    PopulateNestedProperty(instance, subPropertyInfo, propertyAttribute?.KeyPrefix ?? instanceAttribute?.KeyPrefix ?? basePropertyName);

                    continue; // We're Done Here - Move To The Next Property
                }

                var result = GetConfigurationValue(propertyAttribute?.KeyPrefix ?? instanceAttribute?.KeyPrefix, subPropertyInfo.Name, basePropertyName);

                // If We Can't Use The Value, Can We Fallback To The Default (If Specified On The Attribute)
                var toConvert = result ?? (propertyAttribute != null ? propertyAttribute.DefaultValue : null);

                if (null == toConvert)
                {
                    if (propertyAttribute?.Optional ?? false) break;

                    errors.Add($"{subPropertyInfo.Name}");
                    break;
                }

                subPropertyInfo.SetValue(instance, Convert.ChangeType(toConvert, subPropertyInfo.PropertyType));
            }

            if (errors.Any())
            {
                throw new Exception($"{nameof(ConfigWrapperBuilder)} - Could not find the following configuration properties: [{string.Join(", ", errors)}] when building config object: [{wrappedType.Name}]");
            }

        }


        private string GetConfigurationValue(string prefix, string propertyName, string basePropertyName)
        {
            // Get The Config Value Using The Prefix And Property Name
            // If That Was Null - Try Again, But This Time With The Base Property Name As The Prefix
            return
                   GetConfigurationValue(BuildConfigKey(prefix, propertyName))
                ?? GetConfigurationValue(BuildConfigKey(basePropertyName, propertyName));
        }


        // This Could Be Overridden If An Alternative Configuration Source Is Used
        protected virtual string GetConfigurationValue(string key)
        {
            return _configurationProvider.GetValue(key);
        }


        /// <summary>
        /// 
        /// </summary>
        private void PopulateNestedProperty(object instance, PropertyInfo propertyInfo, string basePropertyName)
        {
            var nestedProperty = propertyInfo.GetValue(instance);

            if (null == nestedProperty)
            {
                nestedProperty = BuildObject(propertyInfo.PropertyType);
            }

            PopulateConfigObject(nestedProperty, propertyInfo.PropertyType, BuildConfigKey(basePropertyName, propertyInfo.Name));

            propertyInfo.SetValue(instance, nestedProperty);
        }


        //private T BuildConfigurationObject<T>() where T : new() => new T();

        private object BuildObject(Type type) => Activator.CreateInstance(type);

        private string BuildConfigKey(params string[] keyparts) => string.Join(".", keyparts.Where(x => !string.IsNullOrWhiteSpace(x)));

    }

}
