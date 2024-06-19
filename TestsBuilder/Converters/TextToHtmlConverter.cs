using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestsBuilder.Converters
{
    public class TextToHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string text)
            {
                string htmlContent = GenerateHtml(ConvertToMathJax(text));
                return new HtmlWebViewSource { Html = htmlContent };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
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
        private string ConvertToMathJax(string input)
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

            string sqrtPattern = @"sqrt\(([^()]+|(?<Level>\()|(?<-Level>\)))+(?(Level)(?!))\)";
            input = Regex.Replace(input, sqrtPattern, match =>
            {
                string expressionInside = match.Value.Substring(5, match.Value.Length - 6);
                return $@"\sqrt{{{expressionInside}}}";
            });

            return input;
        }

        private string GenerateHtml(string mathJaxExpression)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            #math-container {{
                font-size: 21px; /* Размер текста */
                color: #333; /* Цвет текста (если необходимо) */
                background-color: #99B5EB; /* Прозрачный задний фон */
                border: 1px solid black; /* Граница для отладки */
            }}
            body {{
                background-color: #99B5EB; /* Прозрачный задний фон для body */
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

    }
}
