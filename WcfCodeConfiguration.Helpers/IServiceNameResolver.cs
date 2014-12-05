namespace WcfCodeConfiguration.Helpers
{
    public interface IServiceNameResolver
    {
        string Resolve<T>();
    }
}