using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;

namespace TestsBuilder.Interfaces
{
    public interface IExpressionGenerator
    {
        List<ExampleVariant> GenerateTasksVariant(string expression, List<BaseAnswer> answers, List<Constraints> constraints, int numberOfTasks, int correctAnswer, int exampleId);
    }
}
