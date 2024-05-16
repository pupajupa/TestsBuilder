using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TestsBuilder.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Indexed]
        [Column("Id")]
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name {  get; set; }

        public string LastName {  get; set; }

        public byte[] Image { get; set; }
        
    }
}
