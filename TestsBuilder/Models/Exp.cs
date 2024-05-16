using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Models
{
    [Table("Exp")]
    public class Exp
    {
        [PrimaryKey, AutoIncrement, Indexed]
        [Column("Id")]
        public int ExpId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public int AMin { get; set; }
        public int BMin { get; set; }
        public int AMax { get; set; }
        public int BMax { get; set; }

        [Indexed]
        public int TestId { get; set; } // Внешний ключ для связи с Test

    }
}
