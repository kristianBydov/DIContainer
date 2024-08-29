using DIContainer;
using Microsoft.AspNetCore.Mvc;
using Test.Interfaces;

namespace Test.Controllers
{
    public class PersonController : ControllerBase
    {
        private readonly IMainService _mainService;

        public PersonController()
        {
            _mainService = Container.Resolve<IMainService>();
        }

        [HttpGet, Route("person/{id}")]
        public ActionResult<Person> GetSinglePerson([FromRoute] int id)
        {
            return Ok(_mainService.GetPerson(id));
        }
    }
}
