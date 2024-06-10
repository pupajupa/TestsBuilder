using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Models
{
    public class Constraints
    {
        public char Letter { get; set; }
        public int MinValue { get; set; } = -10000;
        public int MaxValue { get; set; } = -10000;
        public int Equals { get; set; } = -10000;

        public int NoEquals { get; set; } = -10000;
    }
}
