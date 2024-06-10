using SQLite;

namespace TestsBuilder.Models
{
    [SQLite.Table("Tests")]
    public class Test
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Indexed]
        public int TestResultsId { get; set; }
        //public TestResult Results { get; set; } = new TestResult();
    }
}
