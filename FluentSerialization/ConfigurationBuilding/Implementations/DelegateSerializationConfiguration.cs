namespace FluentSerialization.Implementations;

public class DelegateSerializationConfiguration : ISerializationConfiguration
{
    private readonly Action<ISerializationConfigurationBuilder> _action;

    public DelegateSerializationConfiguration(Action<ISerializationConfigurationBuilder> action)
    {
        _action = action;
    }

    public void Configure(ISerializationConfigurationBuilder configurationBuilder)
        => _action.Invoke(configurationBuilder);
}