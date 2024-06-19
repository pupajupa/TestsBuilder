using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class VariantDetailsPageViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        [ObservableProperty]
        ExampleVariant variant;

        [ObservableProperty]
        private string textAnswerStr;
        [ObservableProperty]
        private HtmlWebViewSource textAnswer;

        [ObservableProperty]
        private HtmlWebViewSource formula;

        [ObservableProperty]
        private string formulaStr;
        public VariantDetailsPageViewModel(IDbService dbService)
        {
            Title = "VariantDetailsPage";
            _dbService = dbService;
            _dbService.Init();
            Variant = _dbService.GetCurrentExampleVariant();
            FormulaStr = Variant.Expression;
            FormulaStr = NormalizeInputString(FormulaStr);

            UpdateFormula(FormulaStr);
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
        public async Task GoToTestsPage()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(TestsPage));
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
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        #math-container {{
                            font-size: 28px; /* Размер текста */
                            color: #333; /* Цвет текста (если необходимо) */
                            background-color: #99B5EB; /* Цвет заднего фона */
                            border: 1px solid black; /* Граница для отладки */
                        }}
                        body {{
                            background-color: #99B5EB; /* Цвет заднего фона для body */
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
        }

        public void UpdateFormula(string expression)
        {
            var htmlContent = GenerateHtml(ConvertToMathJax(expression));
            Formula = new HtmlWebViewSource { Html = htmlContent };
        }
    }
}
