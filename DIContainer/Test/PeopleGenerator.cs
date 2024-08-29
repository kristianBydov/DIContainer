namespace Test
{
    public static class PeopleGenerator
    {
        public static List<Person> Generate()
        {
            return
            [
                new() {Id = 1, Name = "Kristian"},
                new() {Id = 2, Name = "Petar"},
                new() {Id = 3, Name = "Georgi"},
                new() {Id = 4, Name = "Ivan"}
            ];
        }
    }
}
