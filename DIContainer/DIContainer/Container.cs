using System.Reflection;

namespace DIContainer
{
    public class Container
    {
        private static readonly Dictionary<Type, Type> _registeredTypes;
        private static readonly Dictionary<Type, object> _singeltons = [];
        private static readonly HashSet<Type> _transients = [];
        private static readonly HashSet<Type> _scoped = [];

        [ThreadStatic]
        internal static Scope? _currentScope;

        static Container()
        {
            _registeredTypes = [];
        }

        public static Scope CreateScope()
        {
            _currentScope = new Scope();
            return _currentScope;
        }

        public static void Register<TInterface, TImplementation>(ServiceLifecycle lifecycle) where TImplementation : TInterface
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);

            var registrationBulder = new RegistrationBuilder(typeof(TInterface));

            if (lifecycle == ServiceLifecycle.Transient)
                _ = registrationBulder.AsTransient();

            if (lifecycle == ServiceLifecycle.Scoped)
                _ = registrationBulder.AsScoped();

            if (lifecycle == ServiceLifecycle.Singleton)
                _ = registrationBulder.AsSingleton();
        }

        public static RegistrationBuilder Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);

            return new RegistrationBuilder(typeof(TInterface));
        }

        public static TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolve(typeof(TInterface));
        }

        private static object Resolve(Type interfaceType)
        {
            if (_registeredTypes.TryGetValue(interfaceType, out Type? implementationType))
            {
                if (_singeltons.TryGetValue(interfaceType, out var val) && val is not null)
                {
                    return _singeltons[interfaceType];
                }

                if (_scoped.Contains(interfaceType))
                {
                    EnsureScopeExists();
                    return _currentScope!.ResolveScoped(interfaceType, () => CreateInstance(implementationType));
                }

                if (_transients.Contains(interfaceType))
                {
                    return CreateInstance(implementationType);
                }

                var instance = CreateInstance(implementationType);

                TryAddSingleton(interfaceType, instance);

                return instance;
            }
            else
            {
                throw new Exception();
            }
        }

        private static void EnsureScopeExists()
            => _currentScope ??= new Scope(autoDispose: true);

        private static object CreateInstance(Type implementationType)
        {
            var implementationConstructor = implementationType.GetConstructors().First();
            var constructorParameters = implementationConstructor.GetParameters();

            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementationType)!;
            }
            else
            {
                var parameterInstances = GetConstructorParameters(constructorParameters);
                return Activator.CreateInstance(implementationType, parameterInstances.ToArray())!;
            }
        }

        private static void TryAddSingleton(Type interfaceType, object instance)
        {
            if (_singeltons.ContainsKey(interfaceType))
            {
                _singeltons[interfaceType] = instance;
            }
        }

        private static List<object> GetConstructorParameters(ParameterInfo[] constructorParameters)
        {
            var parameterInstances = new List<object>();
            foreach (var parameter in constructorParameters)
            {
                var parameterType = parameter.ParameterType;
                var parameterInstance = Resolve(parameterType);
                parameterInstances.Add(parameterInstance);
            }

            return parameterInstances;
        }

        public class RegistrationBuilder
        {
            private readonly Type _interfaceType;

            public RegistrationBuilder(Type interfaceType)
            {
                _interfaceType = interfaceType;
            }

            /// <summary>
            /// Adds a singleton service of the type specified in <paramref name="serviceType"/>.
            /// </summary>
            public RegistrationBuilder AsSingleton()
            {
                _singeltons[_interfaceType] = null!; // Initializes as null to be created on first resolve
                return this;
            }

            // Marks the registered type as scoped
            public RegistrationBuilder AsScoped()
            {
                _scoped.Add(_interfaceType);
                return this;
            }

            // Marks the registered type as transient
            public RegistrationBuilder AsTransient()
            {
                _transients.Add(_interfaceType);
                return this;
            }
        }
    }
}
