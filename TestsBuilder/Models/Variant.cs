using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text.Json;

namespace TestsBuilder.Models
{
    [SQLite.Table("Variant")]
    public class Variant
    {
        [PrimaryKey, AutoIncrement, Indexed]
        [SQLite.Column("Id")]
        public int VariantId { get; set; }
        public int VariantNumber { get; set; }
        public string VariantExpression { get; set; }
        public string VariantAnswer { get; set; }

        [Indexed]
        public int ExpId { get; set; } // Внешний ключ для связи с Exp
    }
}
