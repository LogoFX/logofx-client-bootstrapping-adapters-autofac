using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace LogoFX.Client.Bootstrapping.Adapters.Autofac.Tests
{
    public class LifetimeRegistrationTests
    {
        [Fact]
        public void ServiceIsRegisteredAsTransient_TwoResolvedInstancesAreDifferent()
        {
            var adapter = new AutofacAdapter();
            adapter.RegisterTransient<ITestDependency, TestDependencyA>();
            adapter.BuildAsLifetimeScope();

            var firstDependency = adapter.Resolve<ITestDependency>();
            var secondDependency = adapter.Resolve<ITestDependency>();

            firstDependency.ShouldNotBeSameAs(secondDependency);
        }

        [Fact]
        public void ServiceIsRegisteredAsSingleton_TwoResolvedInstancesAreSame()
        {
            var adapter = new AutofacAdapter();
            adapter.RegisterSingleton<ITestDependency, TestDependencyA>();
            adapter.BuildAsLifetimeScope();

            var firstDependency = adapter.Resolve<ITestDependency>();
            var secondDependency = adapter.Resolve<ITestDependency>();

            firstDependency.ShouldBeSameAs(secondDependency);
        }
    }

    public class CollectionRegistrationTests
    {
        [Fact]
        public void MultipleImplementationAreRegisteredByType_ResolvedCollectionContainsAllImplementations()
        {
            var adapter = new AutofacAdapter();
            adapter.RegisterCollection<ITestDependency>(new[] { typeof(TestDependencyA), typeof(TestDependencyB) });
            adapter.BuildAsLifetimeScope();

            var collection = adapter.Resolve<IEnumerable<ITestDependency>>().ToArray();

            collection.ShouldContain(r => r is TestDependencyA);
            collection.ShouldContain(r => r is TestDependencyB);
        }

        [Fact]
        public void MultipleImplementationAreRegisteredByTypeAsParameter_ResolvedCollectionContainsAllImplementations()
        {
            var adapter = new AutofacAdapter();
            adapter.RegisterCollection(typeof(ITestDependency), new[] { typeof(TestDependencyA), typeof(TestDependencyB) });
            adapter.BuildAsLifetimeScope();

            var collection = adapter.Resolve<IEnumerable<ITestDependency>>().ToArray();            

            collection.ShouldContain(r => r is TestDependencyA);
            collection.ShouldContain(r => r is TestDependencyB);
        }

        [Fact]
        public void MultipleImplementationAreRegisteredByInstance_ResolvedCollectionContainsAllImplementations()
        {
            var adapter = new AutofacAdapter();
            var instanceA = new TestDependencyA();
            var instanceB = new TestDependencyB();
            adapter.RegisterCollection(new ITestDependency[] { instanceA, instanceB });
            adapter.BuildAsLifetimeScope();

            var collection = adapter.Resolve<IEnumerable<ITestDependency>>().ToArray();
           
            collection.ShouldContain(instanceA);
            collection.ShouldContain(instanceB);            
        }
    }

    interface ITestDependency
    {

    }

    class TestDependencyA : ITestDependency
    {

    }

    class TestDependencyB : ITestDependency
    {

    }
}
