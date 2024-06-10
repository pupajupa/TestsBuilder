using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Responses;

namespace TestsBuilder.Models
{
    [Table("ExampleVariants")]
    public class ExampleVariant
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public string Number { get; set; }
        public string Expression { get; set; }
        public string CorrectAnswer { get; set; }

        [Indexed]
        public int ExampleId { get; set; }

        // Ссылка на таблицу вариантов ответов
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

    // Отдельный класс для хранения вариантов ответов
    [Table("Answers")]
    public class Answer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }

        [ForeignKey(typeof(ExampleVariant))]
        public int ExampleVariantId { get; set; }
    }
}
