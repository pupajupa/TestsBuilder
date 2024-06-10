using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;
using TestsBuilder.Responses;

namespace TestsBuilder.Models
{
    [SQLite.Table("Examples")]
    public class Example
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        // Foreign Key
        [Indexed]
        public int TestId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<BaseAnswer> BaseAnswers { get; set; } = new List<BaseAnswer>();
    }

    // Отдельный класс для хранения вариантов ответов
    [SQLite.Table("BaseAnswers")]
    public class BaseAnswer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }

        [SQLiteNetExtensions.Attributes.ForeignKey(typeof(Example))]
        public int ExampleId { get; set; }
    }
}
