namespace Logikfabrik.Umbraco.Jet
{
    public interface IContainer
    {
        void Register<TContract, TImplementation>()
            where TImplementation : TContract;

        void Register<TContract>(TContract instance);

        T Resolve<T>(
);
    }
}