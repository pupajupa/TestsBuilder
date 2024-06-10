using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBuilder.Responses
{
    public class TestResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ExampleResponse> Examples { get; set; }

        public string HostId { get; set; }
    }

    public class ExampleResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public List<string> BaseAnswers { get; set; }
        public List<VariantResponse> Variants { get; set; }
    }

    public class VariantResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Expression { get; set; }
        public List<string> Answers { get; set; }

        public string CorrectAnswer { get; set; }
    }


}
