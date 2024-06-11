using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Requests;
using TestsBuilder.Services;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class CalculatorPageViewModel:BaseViewModel
    {
        [ObservableProperty]
        Example example;

        private readonly IDbService _dbService;
        public ObservableCollection<Constraints> ConstraintsList { get; set; } = new ObservableCollection<Constraints>();
        public ObservableCollection<BaseAnswer> BaseAnswers { get; set; } = new ObservableCollection<BaseAnswer>();
        public ObservableCollection<ExampleVariant> ExampleVariantsList { get; set; } = new ObservableCollection<ExampleVariant>();

        [ObservableProperty]
        private string inputText = "";

        [ObservableProperty]
        private string calculatedResult = "0";

        private bool isSciOpWaiting = false;

        [ObservableProperty]
        private string newConstraint = "";

        [ObservableProperty]
        private string newAnswer = "";

        [ObservableProperty]
        private HtmlWebViewSource formula;


        public CalculatorPageViewModel(IDbService dbService)
        {
            Title = "Calculate";

            _dbService = dbService;
            _dbService.Init();
            Example = _dbService.GetCurrentExample();
        }

        public string formulaStr { get; set; }

        [RelayCommand]
        private void SaveFormula()
        {
            SaveFormulaAsync();
        }

        private async Task SaveFormulaAsync()
        {
            formulaStr = InputText;
            InputText = NormalizeInputString();
            UpdateFormula(InputText);
            InputText = string.Empty;
        }

        private async Task AddAnswerAsync()
        {
            if (NewAnswer != "")
            {
                NewAnswer += "\n";
            }
            InputText = NormalizeInputString();
            NewAnswer += InputText;
            string answer = InputText;
            BaseAnswers.Add(new BaseAnswer { Text = answer, ExampleId = Example.Id });
            InputText = string.Empty;
        }

        private static readonly char[] AllowedLetters = { 'a', 'b', 'c', 'k', 'm' };
        public char? FindFirstAllowedLetter(string input)
        {
            foreach (char c in input)
            {
                if (AllowedLetters.Contains(c))
                {
                    return c;
                }
            }
            return null;
        }
        public int CountComparisonOperators(string input)
        {
            // Используем регулярное выражение для поиска знаков сравнения и подсчета их количества
            string pattern = @"(>=|<=|!=|>|<|=)";
            MatchCollection matches = Regex.Matches(input, pattern);
            return matches.Count;
        }

        public (int Count, List<string> Operators) CountAndReturnComparisonOperators(string input)
        {
            // Используем регулярное выражение для поиска знаков сравнения
            string pattern = @"(>=|<=|!=|>|<|=)";
            MatchCollection matches = Regex.Matches(input, pattern);
            List<string> foundOperators = matches.Cast<Match>().Select(m => m.Value).ToList();
            return (matches.Count, foundOperators);
        }

        public int? FindNumber(string input)
        {
            // Регулярное выражение для поиска числа
            string pattern = @"-?\d+\.?\d*";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string numberStr = match.Value;
                if (int.TryParse(numberStr, out int number))
                {
                    return number;
                }
            }

            return null;
        }

        public List<int> FindNumbers(string input)
        {
            List<int> numbers = new List<int>();

            // Регулярное выражение для поиска чисел
            string pattern = @"-?\d+\.?\d*";
            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                string numberStr = match.Value;
                if (int.TryParse(numberStr, out int number))
                {
                    numbers.Add(number);
                }
            }

            return numbers;
        }

        [RelayCommand]
        private void AddConstraint()
        {
            if (NewConstraint != "")
            {
                NewConstraint += "\n";
            }
            NewConstraint += InputText;
            Constraints con = new Constraints();
            char? letter = FindFirstAllowedLetter(InputText);
            if (letter != null)
            {
                con.Letter = letter.Value;
                int? value1;
                List<int> value2 = new();
                var (count, operators) = CountAndReturnComparisonOperators(InputText);
                if (count > 0)
                {
                    if (count == 1)
                    {
                        if (operators[0] == ">" || operators[0] == ">=")
                        {
                            value1 = FindNumber(InputText);
                            if (value1 == null)
                            {
                                Shell.Current.DisplayAlert("Ошибка", "Введите верное ограничение", "OK");
                                InputText = string.Empty;
                                return;
                            }
                            else
                            {
                                if (operators[0] == ">")
                                {
                                    con.MinValue = value1.Value + 1;
                                }
                                else
                                {
                                    con.MinValue = value1.Value;
                                }
                            }
                        }
                        else if (operators[0] == "<" || operators[0] == "<=")
                        {
                            value1 = FindNumber(InputText);
                            if (value1 == null)
                            {
                                Shell.Current.DisplayAlert("Ошибка", "Введите верное ограничение", "OK");
                                InputText = string.Empty;
                                return;
                            }
                            else
                            {
                                if (operators[0] == "<")
                                {
                                    con.MaxValue = value1.Value - 1;
                                }
                                else
                                {
                                    con.MaxValue = value1.Value;
                                }
                            }
                        }
                        else if (operators[0] == "=" || operators[0] == "!=")
                        {
                            value1 = FindNumber(InputText);
                            if (value1 == null)
                            {
                                Shell.Current.DisplayAlert("Ошибка", "Введите верное ограничение", "OK");
                                InputText = string.Empty;
                                return;
                            }
                            else
                            {
                                if (operators[0] == "=")
                                {
                                    con.Equals = value1.Value;
                                }
                                else
                                {
                                    con.NoEquals = value1.Value;
                                }
                            }
                        }
                    }
                    else if(count == 2)
                    {
                        value2 = FindNumbers(InputText);
                        if(value2.Count != 2)
                        {
                            Shell.Current.DisplayAlert("Ошибка", "Введите верное ограничение", "OK");
                            InputText = string.Empty;
                            return;
                        }
                        else
                        {
                            if (operators[0] == "<" && operators[1]=="<")
                            {
                                con.MinValue = value2[0]+1;
                                con.MaxValue = value2[1]-1;
                            }
                            else if (operators[0]=="<=" && operators[1] == "<")
                            {
                                con.MinValue = value2[0];
                                con.MaxValue = value2[1]-1;
                            }
                            else if (operators[0]=="<" && operators[1] == "<=")
                            {
                                con.MinValue = value2[0]+1;
                                con.MaxValue = value2[1];
                            }
                            else if (operators[0] == ">" && operators[1] == ">")
                            {
                                con.MinValue = value2[1] + 1;
                                con.MaxValue = value2[0] - 1;
                            }
                            else if (operators[0] == ">=" && operators[1] == ">")
                            {
                                con.MinValue = value2[1]+1;
                                con.MaxValue = value2[0];
                            }
                            else if (operators[0] == ">" && operators[1] == ">=")
                            {
                                con.MinValue = value2[1];
                                con.MaxValue = value2[0] - 1;
                            }
                            else
                            {
                                Shell.Current.DisplayAlert("Ошибка", "Введите верное ограничение", "OK");
                                InputText = string.Empty;
                                return;
                            }
                        }
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Ошибка", "Максимальное количество знаков в ограничении == 2", "OK");
                        InputText = string.Empty;
                        return;
                    }
                }
                else
                {
                    Shell.Current.DisplayAlert("Ошибка", "Не найдены операторы сравнения", "OK");
                    InputText = string.Empty;
                    return;
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Ошибка", "Введите верное ограничение", "OK");
                InputText = string.Empty;
                return;
            }
            ConstraintsList.Add(con);
            InputText = string.Empty;
        }

        [RelayCommand]
        private void AddAnswer()
        {
            AddAnswerAsync();
        }

        [RelayCommand]
        async Task Continue() => await GenerateVariants();
        async Task GenerateVariants()
        {
            if(BaseAnswers.Count == 0 || ConstraintsList.Count == 0 || string.IsNullOrEmpty(Formula.Html))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Введите все данные", "OK");
                return;
            }
                if (ExampleVariantsList.Count > 0)
            {
                ExampleVariantsList.Clear();
            }
            ExpressionGenerator generator = new ExpressionGenerator();
            string varNumber = await Shell.Current.DisplayPromptAsync("Верный ответ", "Введите номер верного ответа:");
            if (string.IsNullOrWhiteSpace(varNumber))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Название примера не может быть пустым.", "OK");
                return;
            }
            varNumber= varNumber.Trim();

            if (!int.TryParse(varNumber, out int exampleNumber))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Номер верного ответа должен быть числом.", "OK");
                return;
            }
            List<ExampleVariant> variants = generator.GenerateTasksVariant(formulaStr, BaseAnswers.ToList(), ConstraintsList.ToList(), 20, exampleNumber,Example.Id);
            Example.Text = formulaStr;

            foreach (var baseAnswers in BaseAnswers.ToList())
            {
                _dbService.AddBaseAnswerToExample(Example.Id, baseAnswers.Text);
            }

            foreach (var variant in variants)
            {
                var i = 0;
                _dbService.AddExampleVariant(variant);
                ExampleVariantsList.Add(variant);
                foreach (var ans in variant.Answers)
                {
                    i++;
                    _dbService.AddAnswerToVariant(variant.Id, ans.Text);
                    if (i == exampleNumber)
                    {
                        variant.CorrectAnswer = ans.Text;
                    }
                }
            }
            BaseAnswers.Clear();
            ConstraintsList.Clear();
            Formula = new HtmlWebViewSource { Html = "" };
            await GoExpressions(Example);
        }

        [RelayCommand]
        private void Reset()
        {
            CalculatedResult = "0";
            InputText = "";
            isSciOpWaiting = false;
        }

        [RelayCommand]
        private void Calculate()
        {
            if (InputText.Length == 0)
                return;

            if (isSciOpWaiting)
            {
                InputText += "";
                isSciOpWaiting = false;
            }

            try
            {
                var inputString = NormalizeInputString();
                var expression = new NCalc.Expression(inputString);
                var result = expression.Evaluate();

                CalculatedResult = result.ToString();
            }
            catch
            {
                CalculatedResult = "NaN";
            }
        }

        private string NormalizeInputString()
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

            var retString = InputText;

            foreach (var key in _opMapper.Keys)
            {
                retString = retString.Replace(key, _opMapper[key]);
            }

            return retString;
        }

        [RelayCommand]
        private void Backspace()
        {
            if (InputText.Length > 0)
                InputText = InputText.Substring(0, InputText.Length - 1);
        }

        [RelayCommand]
        private void NumberInput(string key)
        {
            InputText += key;
        }

        [RelayCommand]
        private void MathOperator(string op)
        {
            if (isSciOpWaiting)
            {
                isSciOpWaiting = false;
            }

            InputText += $"{op}";
        }

        [RelayCommand]
        private void RegionOperator(string op)
        {
            if (op == ")")
                isSciOpWaiting = false;

            InputText += op;
        }

        [RelayCommand]
        private void ScientificOperator(string op)
        {
            InputText += $"{op}(";
            isSciOpWaiting = true;
        }

        [RelayCommand]
        async Task GoToProfilePage() => await Profile();

        public async Task Profile()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(ProfilePage));
        }

        [RelayCommand]
        async Task GoToMaterialsPage() => await Materials();

        public async Task Materials()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(MaterialsPage));
        }

        [RelayCommand]
        async Task GoToTestsPage() => await GoTests();


        public async Task GoExpressions(Example example)
        {
            _dbService.SetCurrentExample(example.Id);
            await Shell.Current.GoToAsync(nameof(VariantsPage));
        }
        public async Task GoTests()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(TestsPage));
        }

        public string ConvertToMathJax(string input)
        {
            string intPattern = @"integral\(([^,]+)\)";
            input = Regex.Replace(input, intPattern, match =>
                {
                    string expression = match.Groups[1].Value.Trim();
                    return $@"\int{{{expression}}} \, dx";
                });     
            string integralPattern = @"integral([(]{1}([^\s]+), ?([^\s]+), ?([^\s)]+))";
            input = Regex.Replace(input, integralPattern, match =>
            {
                string expression = match.Groups[2].Value.Trim();
                string lowerLimit = match.Groups[3].Value.Trim();
                string upperLimit = match.Groups[4].Value.Trim();
                return $@"\int_{{{lowerLimit}}}^{{{upperLimit}}} {{{expression}}} \, dx";
            });

            // Обработка sqrt(F(x))
            string sqrtPattern = @"sqrt\(([^()]+|(?<Level>\()|(?<-Level>\)))+(?(Level)(?!))\)";
            input = Regex.Replace(input, sqrtPattern, match =>
            {
                string expressionInside = match.Value.Substring(5, match.Value.Length - 6);
                return $@"\sqrt{{{expressionInside}}}";
            });

            // Обработка возведения в степень power(base, exponent)
            string powerPattern = @"power\(([^,]+),\s*([^()]+)\)";
            input = Regex.Replace(input, powerPattern, match =>
            {
                string baseExpression = match.Groups[1].Value.Trim();
                string exponent = match.Groups[2].Value.Trim();

                // Проверяем, содержит ли основание деление
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

                return $@"{{{baseExpression}}}^{{{exponent}}}";
            });
            // Обработка дробей g(x)/f(x)
            string fractionPattern = @"\(([^()]+)\)\s*\/\s*\(([^()]+)\)";
            input = Regex.Replace(input, fractionPattern, match =>
            {
                string numerator = match.Groups[1].Value.Trim();
                string denominator = match.Groups[2].Value.Trim();
                return $@"\frac{{{numerator}}}{{{denominator}}}";
            });
            return input;
        }


        public string GenerateHtml(string mathJaxExpression)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
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
        }

        public void UpdateFormula(string expression)
        {
            var htmlContent = GenerateHtml(ConvertToMathJax(expression));
            Formula = new HtmlWebViewSource { Html = htmlContent };
        }
    }
}
