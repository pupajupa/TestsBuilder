using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;

namespace TestsBuilder.Services
{
    public class ExpressionGenerator:IExpressionGenerator
    {
        public List<ExampleVariant> GenerateTasksVariant(string expression, List<BaseAnswer> answers, List<Constraints> constraints, int numberOfTasks, int correctAnswer,int exampleId)
        {
            Random random = new Random();
            List<ExampleVariant> generatedTasks = new List<ExampleVariant>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                ExampleVariant task = new ExampleVariant();
                Dictionary<char, int> generatedValues = new Dictionary<char, int>();
                task.ExampleId = exampleId;
                foreach (var constraint in constraints)
                {
                    int value;
                    if (constraint.Equals != -10000)
                    {
                        value = constraint.Equals;
                    }
                    else if (constraint.NoEquals != -10000 && constraint.MaxValue != -10000 && constraint.MinValue != -10000)
                    {
                        int value1 = random.Next(constraint.MinValue, constraint.NoEquals);
                        int value2 = random.Next(constraint.NoEquals, constraint.MaxValue);
                        value = random.Next(value1, value2);
                    }
                    else if (constraint.NoEquals != -10000 && constraint.MaxValue != -10000)
                    {
                        int value1 = random.Next(constraint.NoEquals - 30, constraint.NoEquals);
                        int value2 = random.Next(constraint.NoEquals, constraint.MaxValue);
                        value = random.Next(value1, value2);
                    }
                    else if (constraint.NoEquals != -10000 && constraint.MinValue != -10000)
                    {
                        int value1 = random.Next(constraint.MinValue, constraint.NoEquals);
                        int value2 = random.Next(constraint.NoEquals, constraint.NoEquals + 30);
                        value = random.Next(value1, value2);
                    }
                    else if (constraint.MinValue != -10000 && constraint.MaxValue != -10000)
                    {
                        value = random.Next(constraint.MinValue, constraint.MaxValue);
                    }
                    else if (constraint.MinValue != -10000)
                    {
                        value = random.Next(constraint.MinValue, constraint.MinValue + 50);
                    }
                    else
                    {
                        value = random.Next(constraint.MaxValue - 50, constraint.MaxValue);
                    }
                    generatedValues[constraint.Letter] = value;
                }
                string modifiedExpression = expression;
                foreach (var kvp in generatedValues)
                {
                    modifiedExpression = modifiedExpression.Replace(kvp.Key.ToString(), kvp.Value.ToString());
                }
                task.Expression = modifiedExpression;

                List<Answer> modifiedAnswers = new List<Answer>();
                foreach (var answer in answers)
                {
                    string modifiedAnswer = answer.Text;
                    foreach (var kvp in generatedValues)
                    {
                        modifiedAnswer = modifiedAnswer.Replace(kvp.Key.ToString(), kvp.Value.ToString());
                    }
                    modifiedAnswers.Add(new Answer { Text = modifiedAnswer, ExampleVariantId = task.Id});
                }
                task.Answers = modifiedAnswers;
                task.CorrectAnswer = task.Answers[correctAnswer - 1].Text;
                task.Number = (i + 1).ToString();
                generatedTasks.Add(task);
            }

            return generatedTasks;
        }
    }
}
