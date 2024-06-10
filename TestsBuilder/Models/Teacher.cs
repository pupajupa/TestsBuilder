using SQLite;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Models
{
    [Table("Teachers")]
    public class Teacher
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; } // Primary key
        public string FirstName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public byte[] Image { get; set; }
    }
}
