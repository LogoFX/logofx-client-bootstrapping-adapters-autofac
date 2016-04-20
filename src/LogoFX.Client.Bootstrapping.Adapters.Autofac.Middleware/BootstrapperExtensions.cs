using Autofac;
using LogoFX.Bootstrapping;

namespace LogoFX.Client.Bootstrapping.Adapters.Autofac
{
    /// <summary>
    /// The bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the autofac container registration.
        /// </summary>        
        /// <param name="bootstrapper">The bootstrapper.</param>
        public static IBootstrapperWithContainer<AutofacAdapter, ContainerBuilder> UseAutofacContainerRegistration(
            this IBootstrapperWithContainer<AutofacAdapter, ContainerBuilder> bootstrapper)
        {
            return bootstrapper.Use(new RegisterAutofacContainerMiddleware());
        }
    }
}
