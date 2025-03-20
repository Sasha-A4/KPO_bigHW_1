using System;
using System.Collections.Generic;

namespace BankApplication.DI
{
    public class DI
    {
        private readonly Dictionary<Type, object> singletons;
        private readonly Dictionary<Type, Type> transientTypes;

        public DI()
        {
            singletons = new Dictionary<Type, object>();
            transientTypes = new Dictionary<Type, Type>();
        }

        public void RegisterSingleton<T>(T instance)
        {
            singletons[typeof(T)] = instance;
        }

        public void RegisterSingleton(Type type, object instance)
        {
            singletons[type] = instance;
        }

        public void RegisterTransient<TInterface, TImplementation>()
        {
            transientTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (singletons.ContainsKey(type))
            {
                return (T)singletons[type];
            }

            if (transientTypes.ContainsKey(type))
            {
                var implementationType = transientTypes[type];
                return (T)Activator.CreateInstance(implementationType);
            }

            throw new InvalidOperationException($"Type {type.Name} is not registered in the DI container.");
        }
    }
} 