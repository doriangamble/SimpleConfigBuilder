using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.SimpleConfiguration
{

    /// <summary>
    /// Attribute That Can Be Applied To An Entire Class That
    /// </summary>    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public abstract class BaseConfigWrapperClassAttribute : Attribute
    {
        /// <summary>
        /// The Name Of The Prefix Used In The Configuration Store
        /// For Example An AppConfig Key Of 'queuesettings.name' Would Map To A 
        /// Property Called 'Name' In A Class Called 'QueueSettings'
        /// </summary>
        public string KeyPrefix { get; set; }
    }

    /// <summary>
    /// Attribute That Can Be Applied To An Entire Class That
    /// </summary>    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ConfigWrapperClassAttribute : BaseConfigWrapperClassAttribute { }

    /// <summary>
    /// Attribute That Can Be Applied To A Property
    /// </summary>    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ConfigWrapperPropertyAttribute : BaseConfigWrapperClassAttribute
    {
        /// <summary>
        /// Specify A Default (const) Value
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Property Is A Class And Should Be Traversed
        /// </summary>
        public bool IsNested { get; set; }

        /// <summary>
        /// This Setting Is Optional And Therefore May Not Be Set
        /// </summary>
        public bool Optional { get; set; }
    }
}
