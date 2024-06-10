using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Models
{
    [Table("TestResults")]
    public class TestResult
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        [Indexed]
        public int TestId { get; set; }

        [Indexed]
        public int StudentId { get; set; }
        // Результат теста для данного студента
        public int Score { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;

    }
}
