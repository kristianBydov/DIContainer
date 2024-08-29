using DIContainer;
using Microsoft.AspNetCore.Mvc;
using Test.Tests;

namespace Test.Controllers
{
    public class OperationController : Controller
    {
        private readonly OperationService _operationService;
        private readonly IOperationTransient _transientOperation;
        private readonly IOperationScoped _scopedOperation;
        private readonly IOperationSingleton _singletonOperation;

        public OperationController()
        {
            _operationService = Container.Resolve<OperationService>();
            _transientOperation = Container.Resolve<IOperationTransient>();
            _scopedOperation = Container.Resolve<IOperationScoped>();
            _singletonOperation = Container.Resolve<IOperationSingleton>();
        }

        [HttpGet, Route("operation")]
        public ActionResult Index()
        {
            // ViewBag contains controller-requested services
            ViewBag.Transient = _transientOperation;
            Console.WriteLine($"Transient:{_transientOperation.OperationId}");
            ViewBag.Scoped = _scopedOperation;
            Console.WriteLine($"Scoped:{_scopedOperation.OperationId}");
            ViewBag.Singleton = _singletonOperation;
            Console.WriteLine($"Singleton:{_singletonOperation.OperationId}");

            // Operation service has its own requested services
            ViewBag.Service = _operationService;
            Console.WriteLine($"Transient:{_operationService.OperationTransient.OperationId}");
            Console.WriteLine($"Scoped:{_operationService.OperationScoped.OperationId}");
            Console.WriteLine($"Singleton:{_operationService.OperationSingleton.OperationId}");

            Console.WriteLine();
            return Ok();
        }
    }
}

