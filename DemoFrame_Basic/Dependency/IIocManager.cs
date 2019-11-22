using System;
using Autofac;
using Autofac.Core;

namespace DemoFrame_Basic.Dependency
{
    /// <summary>
    /// IOC容器接口
    /// </summary>
    public interface IIocManager
    {
        IContainer Container { get; }

        bool IsRegistered(Type serviceType, ILifetimeScope scope = null);
        object Resolve(Type type, ILifetimeScope scope = null);
        T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class;
        T Resolve<T>(params Parameter[] parameters) where T : class;
        T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null);
        object ResolveOptional(Type serviceType, ILifetimeScope scope = null);
        object ResolveUnregistered(Type type, ILifetimeScope scope = null);
        T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class;
        ILifetimeScope Scope();
        bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance);
    }
}
