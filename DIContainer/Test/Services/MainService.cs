using Test.Interfaces;

namespace Test.Services
{
    public class MainService : IMainService
    {
        private readonly IPersonService _personService;

        public MainService(IPersonService personService)
        {
            _personService = personService;
        }

        public Person GetPerson(int id)
        {
            var pepole = _personService.GetAllPeople();

            return pepole.Where(p => p.Id == id).FirstOrDefault()!;
        }
    }
}
