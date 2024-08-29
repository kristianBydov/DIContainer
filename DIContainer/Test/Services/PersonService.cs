using Test.Interfaces;

namespace Test.Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _people;

        public PersonService()
        {
            _people = PeopleGenerator.Generate();
        }

        public IEnumerable<Person> GetAllPeople()
        {
            return _people;
        }
    }
}
