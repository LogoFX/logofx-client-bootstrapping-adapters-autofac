using System;
using Autofac;
using LogoFX.Bootstrapping;
using LogoFX.Core;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping.Adapters.Autofac
{
    /// <summary>
    /// Builds and registers autofac ioc container as a lifetime scope.
    /// </summary>    
    public class RegisterAutofacContainerMiddleware : IMiddleware<IBootstrapperWithContainer<AutofacAdapter, ContainerBuilder>>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns/>
        public IBootstrapperWithContainer<AutofacAdapter, ContainerBuilder> Apply(IBootstrapperWithContainer<AutofacAdapter, ContainerBuilder> @object)
        {
            EventHandler strongHandler = ObjectOnInitializationCompleted;
            @object.InitializationCompleted += WeakDelegate.From(strongHandler);
            return @object;
        }

        private void ObjectOnInitializationCompleted(object sender, EventArgs eventArgs)
        {
            var bootstrapper = sender as IBootstrapperWithContainer<AutofacAdapter, ContainerBuilder>;
            if (bootstrapper != null)
            {
                bootstrapper.Container.RegisterInstance(bootstrapper.Container);
                var container = bootstrapper.Container.Build();
                bootstrapper.Container.RegisterInstance(container);
                var lifetimeScope = container.BeginLifetimeScope();
                bootstrapper.Container.RegisterInstance(lifetimeScope);                
            }            
        }
    }
}
