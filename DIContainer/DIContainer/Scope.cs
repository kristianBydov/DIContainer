namespace DIContainer
{
    public class Scope : IDisposable
    {
        private readonly Dictionary<Type, object> _scopedInstances = [];
        private readonly bool _autoDispose;

        public Scope(bool autoDispose = false)
        {
            _autoDispose = autoDispose;
        }

        // Resolves or creates a scoped instance
        public object ResolveScoped(Type interfaceType, Func<object> createInstance)
        {
            if (_scopedInstances.TryGetValue(interfaceType, out var instance))
            {
                return instance;
            }

            instance = createInstance();
            _scopedInstances[interfaceType] = instance;
            return instance;
        }

        // Disposes the scope and any disposable scoped instances
        public void Dispose()
        {
            foreach (var instance in _scopedInstances.Values.OfType<IDisposable>())
            {
                instance.Dispose();
            }

            _scopedInstances.Clear();

            if (_autoDispose)
            {
                Container._currentScope = null; // Clear the current scope if it was auto-created
            }
        }
    }
}
