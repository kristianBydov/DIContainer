namespace Test.Tests
{
    public class OperationService
    {
        public IOperationScoped? OperationScoped { get; }
        public IOperationTransient? OperationTransient { get; }
        public IOperationSingleton? OperationSingleton { get; }

        public OperationService(IOperationScoped operationScoped,
            IOperationSingleton operationSingleton,
            IOperationTransient operationTransient)
        {
            OperationScoped = operationScoped;
            OperationTransient = operationTransient;
            OperationSingleton = operationSingleton;
        }
    }
}
