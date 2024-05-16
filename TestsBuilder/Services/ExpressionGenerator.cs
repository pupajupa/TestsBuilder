using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;

namespace TestsBuilder.Services
{
    public class ExpressionGenerator
    {
        private readonly Random _random;

        public ExpressionGenerator()
        {
            _random = new Random();
        }

        // Метод для генерации 10 вариантов задания
        public List<Variant> GenerateExpressions(string expression,int amin, int amax, int bmin, int bmax)
        {
            bool containsA = expression.Contains('a');
            bool containsB = expression.Contains('b');
            string newExpression = expression;
            List<Variant> result = new List<Variant>();
            for (int i = 0; i < 10; i++)
            {
                // Генерируем случайные значения
                // для a и b
                if (containsA && containsB)
                {
                    int a = _random.Next(amin, amax); // Примерный диапазон для a
                    int b = _random.Next(bmin, bmax); // Примерный диапазон для b
                    newExpression = expression.Replace("a", a.ToString());
                    newExpression = newExpression.Replace("b", b.ToString());
                }

                else if (containsB)
                {
                    int b = _random.Next(bmin, bmax); // Примерный диапазон для b
                    newExpression = newExpression.Replace("b", b.ToString());
                }
                // Формируем выражение с новыми значениями a и b
                else if (containsA)
                {
                    int a = _random.Next(amin, amax); // Примерный диапазон для a
                    newExpression = expression.Replace("a", a.ToString());
                }

                result.Add(new Variant
                {
                    VariantExpression = newExpression,
                    VariantNumber = i + 1,
                    VariantAnswer = ""
                }) ;
            }

            return result;
        }
    }
}
