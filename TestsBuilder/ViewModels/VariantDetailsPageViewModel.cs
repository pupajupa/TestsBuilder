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
