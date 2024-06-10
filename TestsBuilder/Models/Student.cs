using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Models
{
    [Table("Students")]
    public class Student
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
