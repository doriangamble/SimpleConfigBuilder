namespace Configuration.SimpleConfiguration
{

    /// <summary>
    /// Provides A Wrapper Around An Object Containing Properties That Should Match Keys From A Configuration Store Such As 'appSettings.config'.
    /// Does The Wiring Up Automatically And Can Nest Properties When Keys Are Grouped With A Dot ("QueueSettings.HostName"). 
    /// So the following class: 
    ///  public class QueueSettings
    ///  {
    ///      public string QueueName { get; set; }
    ///      public string HostName { get; set; }
    ///  }
    /// - would map from appSettings keys: "QueueSettings.QueueName" and "QueueSettings.HostName"
    /// 
    /// Use Attributes To Further Define The Behaviour
    /// </summary>
    /// <typeparam name="T">Config Object To Be Populated</typeparam>
    public interface IConfigWrapper<T> where T : new()
    {
        /// <summary>
        /// The Configuration Object
        /// </summary>
        T Value { get; }
    }


}
