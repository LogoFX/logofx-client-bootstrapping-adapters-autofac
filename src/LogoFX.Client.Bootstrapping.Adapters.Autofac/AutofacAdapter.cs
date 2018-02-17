using System;
using System.Collections.Generic;
using Autofac;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Adapters.Autofac
{
    /// <summary>
    /// Represents implementation of ioc container and bootstrapper adapter using Autofac.
    /// </summary>
    public class AutofacAdapter : IIocContainer, 
        IIocContainerAdapter<IContainer>, 
        IIocContainerAdapter<ILifetimeScope>,
        IBootstrapperAdapter
    {
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
        private ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>        
        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="dependencies">The dependencies.</param>        
        public void RegisterCollection(Type dependencyType, IEnumerable<object> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                _containerBuilder.RegisterInstance(dependency).AsSelf().As(dependencyType);
            }
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="dependencyTypes">The dependency types.</param>        
        public void RegisterCollection(Type dependencyType, IEnumerable<Type> dependencyTypes)
        {
            foreach (var type in dependencyTypes)
            {
                _containerBuilder.RegisterType(type).AsSelf().As(dependencyType);
            }
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="dependencies">The dependencies.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void RegisterCollection<TService>(IEnumerable<TService> dependencies) where TService : class
        {
            foreach (var service in dependencies)
            {
                _containerBuilder.RegisterInstance(service).AsSelf().As<TService>();
            }
        }

        /// <summary>
        /// Registers the collection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="dependencyTypes">The dependency types.</param>
        public void RegisterCollection<TService>(IEnumerable<Type> dependencyTypes) where TService : class
        {
            foreach (var type in dependencyTypes)
            {
                _containerBuilder.RegisterType(type).AsSelf().As<TService>();
            }
        }

        /// <summary>
        /// Registers the handler.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="handler">The handler.</param>        
        public void RegisterHandler(Type dependencyType, Func<object> handler)
        {
            _containerBuilder.Register(a => handler).As(dependencyType);
        }

        /// <summary>
        /// Registers the handler.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="handler">The handler.</param>        
        public void RegisterHandler<TService>(Func<TService> handler) where TService : class
        {
            _containerBuilder.Register(a => handler).As<TService>();
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="instance">The instance.</param>        
        public void RegisterInstance(Type dependencyType, object instance)
        {
            _containerBuilder.RegisterInstance(instance).As(dependencyType).InstancePerLifetimeScope();
        }

        public void RegisterSingleton(Type serviceType, Type implementationType, Func<object> dependencyCreator)
        {
            _containerBuilder.Register(c => dependencyCreator()).As(serviceType).InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>        
        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _containerBuilder.RegisterInstance(instance).AsSelf().InstancePerLifetimeScope();
        }

        public void RegisterSingleton<TDependency, TImplementation>(Func<TImplementation> dependencyCreator) 
            where TImplementation : class, TDependency
        {
            _containerBuilder.Register(c => dependencyCreator()).As<TDependency>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>        
        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            _containerBuilder.RegisterType(implementationType).As(serviceType).InstancePerLifetimeScope();
        }

        public void RegisterSingleton<TDependency>(Func<TDependency> dependencyCreator) where TDependency : class
        {
            _containerBuilder.Register(c => dependencyCreator()).As<TDependency>().InstancePerLifetimeScope();            
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>        
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _containerBuilder.RegisterType<TImplementation>().As<TService>().InstancePerLifetimeScope();
        }

        public void RegisterTransient<TDependency>(Func<TDependency> dependencyCreator) where TDependency : class
        {
            _containerBuilder.Register(c => dependencyCreator()).As<TDependency>().InstancePerDependency();
        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>        
        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _containerBuilder.RegisterType(implementationType).As(serviceType).InstancePerDependency();
        }

        public void RegisterTransient(Type serviceType, Type implementationType, Func<object> dependencyCreator)
        {
            _containerBuilder.Register(c => dependencyCreator()).As(serviceType).InstancePerDependency();
        }

        public void RegisterSingleton<TDependency>() where TDependency : class
        {
            _containerBuilder.RegisterType<TDependency>().AsSelf().InstancePerLifetimeScope();
        }

        public void RegisterTransient<TDependency, TImplementation>(Func<TImplementation> dependencyCreator) where TImplementation : class, TDependency
        {
            _containerBuilder.Register(c => dependencyCreator()).As<TDependency>().InstancePerDependency();
        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>        
        public void RegisterTransient<TService>() where TService : class
        {
            _containerBuilder.RegisterType<TService>().AsSelf().InstancePerDependency();
        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>        
        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            _containerBuilder.RegisterType<TImplementation>().As<TService>().InstancePerDependency();
        }

        /// <summary>
        /// Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>        
        public object Resolve(Type serviceType)
        {
            return _lifetimeScope.Resolve(serviceType);
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>        
        public TService Resolve<TService>() where TService : class
        {
            return _lifetimeScope.Resolve<TService>();
        }

        /// <summary>
        /// Resolves an instance of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param><param name="key">Optional service key</param>
        /// <returns>
        /// Instance of service
        /// </returns>
        public object GetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (!_lifetimeScope.IsRegistered(serviceType))
                {
                    var builder = new ContainerBuilder();
                    builder.RegisterType(serviceType).As(serviceType);
                    if (_lifetimeScope is IContainer container)
                    {
                        //TODO: Replace this implementation in the final version
                        builder.Update(container);
                    }
                    
                }
                return _lifetimeScope.Resolve(serviceType);
            }
            if (_lifetimeScope.IsRegisteredWithName(key, serviceType))
            {
                return _lifetimeScope.ResolveNamed(key, serviceType);
            }
            throw new Exception($"Could not locate any instances of contract {key}.");
        }

        /// <summary>
        /// Resolves all instances of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>
        /// All instances of requested service
        /// </returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _lifetimeScope.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        /// <summary>
        /// Resolves instance's dependencies and injects them into the instance
        /// </summary>
        /// <param name="instance">Instance to get injected with dependencies</param>
        public void BuildUp(object instance)
        {
            _lifetimeScope.InjectProperties(instance);
        }

        /// <summary>
        /// Builds the container.
        /// </summary>
        public void Build()
        {
            _lifetimeScope = _containerBuilder.Build();
        }

        /// <summary>
        /// Builds the container as lifetime scope.
        /// </summary>
        public void BuildAsLifetimeScope()
        {
            _lifetimeScope = _containerBuilder.Build().BeginLifetimeScope();
        }
    }
}
