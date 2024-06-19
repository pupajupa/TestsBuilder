using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class SolutionTestPageViewModel : ObservableObject
    {
        private readonly IDbService _dbService;
        private List<Example> examplesList = new();
        public ObservableCollection<Example> Examples { get; set; } = new();

        [ObservableProperty]
        private ObservableCollection<Solution> solutions;

        [ObservableProperty]
        private Test test;

        private TestResult result;

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private string textAnswerStr;

        [ObservableProperty]
        private HtmlWebViewSource textAnswer;

        private HtmlWebViewSource formula;

        private string formulaStr;

        public SolutionTestPageViewModel(IDbService dbService)
        {
            _dbService = dbService;
            Solutions = new ObservableCollection<Solution>();
            LoadData();
        }

        private void LoadData()
        {
            Test = _dbService.GetCurrentTest();
            examplesList = (List<Example>)_dbService.GetAllExpressionsByTestId(Test.Id);
            if (examplesList != null)
            {
                int counterForGroupName = 0;
                foreach (var ex in examplesList)
                {
                    Examples.Add(ex);
                    Solution solution = new();
                    var userVariants = (List<ExampleVariant>)_dbService.GetAllExpressionVariantsByExampleId(ex.Id);
                    var userVariant = userVariants[0];
                    if (userVariant != null)
                    {
                        foreach (var ans in userVariant.Answers)
                        {

                            SolutionAnswer solutionAnswer = new SolutionAnswer
                            {
                                Text = ans.Text,
                                IsSelected = false,
                                GroupName = counterForGroupName.ToString(),
                            };
                            if (ans.Text == userVariant.CorrectAnswer)
                            {
                                solution.CorrectAnswer = ans.Text;
                            }
                            solution.Answers.Add(solutionAnswer);
                        }
                        counterForGroupName++;
                        formulaStr = userVariant.Expression;
                        formulaStr = NormalizeInputString(formulaStr);
                        formula = UpdateFormula(formulaStr);
                        solution.Formula = formula;
                        Solutions.Add(solution);
                        Debug.WriteLine($"Added solution with {solution.Answers.Count} answers");
                    }
                }
            }
        }

        [RelayCommand]
        public void RadioButtonCheckedChange(SolutionAnswer selectedAnswer)
        {
            // Найдите решение, к которому относится выбранный ответ
            var solution = Solutions.FirstOrDefault(s => s.Answers.Contains(selectedAnswer));
            if (solution != null)
            {
                // Обработайте выбранный ответ
                Debug.WriteLine($"Answer '{selectedAnswer.Text}' selected for solution with formula '{solution.Formula}'.");

                // Дополнительные действия, если необходимо
            }
            else
            {
                Debug.WriteLine($"No solution found for answer '{selectedAnswer.Text}'.");
            }
        }

        [RelayCommand]
        public async void FinishTest()
        {
            int correctCount = CalculateCorrectAnswersCount();
            SaveTestResult(correctCount);
            await Shell.Current.DisplayAlert("Результат", $"Количество правильных ответов: {correctCount} из {Solutions.Count}", "OK");
            GoToUserTestsPage();
        }
        async void GoToUserTestsPage()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync($"{nameof(UserTestsPage)}");
        }
        private int CalculateCorrectAnswersCount()
        {
            int correctCount = 0;
            foreach (var solution in Solutions)
            {
                var selectedAnswer = solution.Answers.FirstOrDefault(a => a.IsSelected);
                if (selectedAnswer != null && selectedAnswer.Text == solution.CorrectAnswer)
                {
                    correctCount++;
                }
            }
            return correctCount;
        }

        private void SaveTestResult(int correctCount)
        {
            var currentStudent = _dbService.GetCurrentStudent();
            var currentTest = _dbService.GetCurrentTest();
            result = new TestResult
            {
                StudentId = currentStudent.Id,
                TestId = currentTest.Id,
                IsCompleted = true,
                Score = correctCount,
            };
            _dbService.AddTestResult(result);
        }

        private Solution FindSolutionByAnswer(string answer)
        {
            return Solutions.FirstOrDefault(s => s.Answers.Any(a => a.Text == answer));
        }

        public string ConvertToMathJax(string input)
        {

            string powerPattern = @"power\(([^\s].*?)\,([^\s].*?\)(?=\+|\-|\*|\/|\n|$))";
            input = PowerCheck(powerPattern, input);

            string fractionPattern = @"([^\s]+)\/([^\s]+)";


            input = Regex.Replace(input, fractionPattern, match =>
            {
                string numerator = match.Groups[1].Value.Trim();
                string denominator = match.Groups[2].Value.Trim();
                return $@"\frac{{{numerator}}}{{{denominator}}}";
            });

            string integralPattern = @"integral([(]{1}(.*[^\s].*), ?([^\s\n]+), ?([^\s]+))";
            if (Regex.IsMatch(input, integralPattern))
            {
                input = Regex.Replace(input, integralPattern, match =>
                {
                    string expression = match.Groups[2].Value.Trim();
                    string lowerLimit = match.Groups[3].Value.Trim();
                    string upperLimit = match.Groups[4].Value.Trim();
                    return $@"\int_{{{lowerLimit}}}^{{{upperLimit}}} {{{expression}}} \, dx";
                });
            }
            else
            {
                string intPattern = @"integral\((.*[^\s]+.*)\)";
                input = Regex.Replace(input, intPattern, match =>
                {
                    string expression = match.Groups[1].Value.Trim();
                    return $@"\int{{{expression}}} \, dx";
                });
            }
            // Обработка sqrt(F(x))
            string sqrtPattern = @"sqrt\(([^()]+|(?<Level>\()|(?<-Level>\)))+(?(Level)(?!))\)";
            input = Regex.Replace(input, sqrtPattern, match =>
            {
                string expressionInside = match.Value.Substring(5, match.Value.Length - 6);
                return $@"\sqrt{{{expressionInside}}}";
            });

            // Обработка возведения в степень power(base, exponent)

            // Обработка дробей g(x)/f(x)

            return input;
        }


        public string GenerateHtml(string mathJaxExpression)
        {
            string htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        #math-container {{
                            font-size: 24px; /* Размер текста */
                            border: 1px solid black; /* Граница для отладки */
                        }}
                    </style>
                    <script type='text/javascript' async
                        src='https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/latest.js?config=TeX-MML-AM_CHTML'>
                    </script>
                </head>
                <body>
                    <div id='math-container'>
                        $$ {mathJaxExpression} $$
                    </div>
                </body>
                </html>";

            Debug.WriteLine($"Generated HTML: {htmlContent}");
            return htmlContent;
        }


        public HtmlWebViewSource UpdateFormula(string expression)
        {
            var htmlContent = GenerateHtml(ConvertToMathJax(expression));
            var formula = new HtmlWebViewSource { Html = htmlContent };
            return formula;
        }


        private string NormalizeInputString(string text)
        {
            Dictionary<string, string> _opMapper = new()
            {
                {"×", "*"},
                {"÷", "/"},
                {"SIN", "sin"},
                {"COS", "cos"},
                {"TAN", "tg"},
                {"ASIN", "arcsin"},
                {"ACOS", "arccos"},
                {"ATAN", "arctg"},
                {"LOG", "log"},
                {"LN", "ln"},
                {"EXP", "exp"},
                {"LOG10", "log10"},
                {"SQRT", "sqrt"},
                {"ABS", "|"},
                {"∫", "integral"},
                {"STEP", "power"},
            };

            var retString = text;

            foreach (var key in _opMapper.Keys)
            {
                retString = retString.Replace(key, _opMapper[key]);
            }

            return retString;
        }

        public string PowerCheck(string pattern, string input)
        {
            while (Regex.IsMatch(input, pattern))
            {
                input = Regex.Replace(input, pattern, match =>
                {
                    string baseExpression = match.Groups[1].Value.Trim();
                    string exponent = match.Groups[2].Value.Trim();
                    exponent = exponent.Remove(exponent.Length - 1);
                    if (baseExpression.Contains("/"))
                    {
                        baseExpression = $@"\left({baseExpression}\right)";
                    }
                    else
                    {
                        // Убираем внешние скобки, если они есть
                        if (baseExpression.StartsWith("(") && baseExpression.EndsWith(")"))
                        {
                            baseExpression = baseExpression.Substring(1, baseExpression.Length - 2).Trim();
                        }
                    }
                    exponent = PowerCheck(pattern, exponent);

                    return $@"{{{baseExpression}}}^{{{exponent}}}";
                });

            }
            return input;
        }
    }
}
