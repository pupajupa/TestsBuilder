using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Requests
{
    public class TestRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ExampleRequest>? Examples { get; set; }
    }

    public class ExampleRequest
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public List<string> BaseAnswers { get; set; }
        public List<VariantRequest>? Variants { get; set; }
    }

    public class VariantRequest
    {
        public string Number { get; set; }
        public string Expression { get; set; }
        public List<string> Answers { get; set; }
    }
}
