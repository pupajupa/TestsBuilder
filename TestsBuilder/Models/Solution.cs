using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Models
{
    public class Solution
    {
        public HtmlWebViewSource Formula { get; set; }
        public List<SolutionAnswer> Answers { get; set; } = new();

        public string CorrectAnswer { get; set; }
    }
}
