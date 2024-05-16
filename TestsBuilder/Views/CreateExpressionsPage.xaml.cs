using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using TestsBuilder.Models;
using TestsBuilder.Services;

namespace TestsBuilder.Views;

public partial class CreateExpressionsPage : ContentPage
{
    private readonly IDbService _dbService;
    private TestsBuilder.Models.User _user;
    private Exp _expression;
    private string _expName;

    public CreateExpressionsPage(Exp exp, IDbService dbService, TestsBuilder.Models.User user)
    {
        InitializeComponent();
        BindingContext = this;
        _expression = exp;
        _dbService = dbService;
        _user = user;
        _expName = exp.Title;
        //titleLabel.Text = exp.Text;
    }

    private async void Tests_Clicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Settings"
        // Здесь вы можете выполнить навигацию на страницу настроек или открыть диалоговое окно с настройками
        await Navigation.PushAsync(new TestsPage(_user, _dbService));
    }

    private async void Materials_Clicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Logout"
        // Здесь вы можете выполнить выход пользователя из учетной записи и перейти на страницу входа
    }
    private async void Profile_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage(_user, _dbService));
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        // Проверка наличия символов 'a' и 'b' в уравнении
        bool containsA = equationEntry.Text.Contains('a');
        bool containsB = equationEntry.Text.Contains('b');

        // Если нет ни 'a', ни 'b', выводим сообщение об ошибке
        if (!containsA && !containsB)
        {
            await DisplayAlert("Ошибка", "Уравнение не содержит переменных 'a' или 'b'", "OK");
            return;
        }

        // Если есть 'a' или 'b', но отсутствуют ограничения, выводим сообщение об ошибке
        if (containsA && (AMin.Text == "" || AMax.Text == ""))
        {
            await DisplayAlert("Ошибка", "Введите ограничения для 'a'", "OK");
            return;
        }

        if (containsB && (BMin.Text == "" || BMax.Text == ""))
        {
            await DisplayAlert("Ошибка", "Введите ограничения для 'b'", "OK");
            return;
        }

        // Парсинг ограничений для 'a' и 'b'
        int amin = 0, amax = 0, bmin = 0, bmax = 0;
        try
        {
            if (containsA)
            {
                if (!string.IsNullOrWhiteSpace(AMin.Text) && !string.IsNullOrWhiteSpace(AMax.Text))
                {
                    amin = int.Parse(AMin.Text);
                    amax = int.Parse(AMax.Text);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Введите корректные значения ограничений для 'a'", "OK");
                    return;
                }
            }
            if (containsB)
            {
                if (!string.IsNullOrWhiteSpace(BMin.Text) && !string.IsNullOrWhiteSpace(BMax.Text))
                {
                    bmin = int.Parse(BMin.Text);
                    bmax = int.Parse(BMax.Text);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Введите корректные значения ограничений для 'b'", "OK");
                    return;
                }
            }
        }
        catch (FormatException)
        {
            await DisplayAlert("Ошибка", "Введите корректные значения ограничений", "OK");
            return;
        }

        Exp newExp = new Exp {
            Text = equationEntry.Text,
            Title = _expName,
            AMin = amin,
            AMax = amax,
            BMax = bmax,
            BMin = bmin,
            TestId = _expression.TestId,
            ExpId = _expression.ExpId,
        };

        _dbService.UpdateExpression(newExp);
        // Генерация вариантов и добавление их в базу данных
        List<Variant> variants = new List<Variant>();
        ExpressionGenerator expressionGenerator = new ExpressionGenerator();
        variants = expressionGenerator.GenerateExpressions(equationEntry.Text, amin, amax, bmin, bmax);
        // Добавление вариантов в базу данных
        _dbService.AddVariantsToExpression(newExp.ExpId, variants);

        await DisplayAlert("Успех", "Варианты успешно сгенерированы", "OK");

        var expressionsPage = new ExpressionsPage(_dbService.GetTestById(newExp.TestId), _dbService, _user);
        // Возвращение на страницу всех заданий данного теста
        await Navigation.PushAsync(expressionsPage);
    }   

    public event PropertyChangedEventHandler PropertyChanged;
    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string ExpName
    {
        get { return _expName; }
        set
        {
            _expName = value;
            OnPropertyChanged(nameof(ExpName));
        }
    }

    private string minValue;
    public string MinValue
    {
        get { return minValue; }
        set
        {
            minValue = value;
            OnPropertyChanged(nameof(MinValue));
        }
    }

    private string maxValue;
    public string MaxValue
    {
        get { return maxValue; }
        set
        {
            maxValue = value;
            OnPropertyChanged(nameof(MaxValue));
        }
    }

    private string equation;
    public string Equation
    {
        get { return equation; }
        set
        {
            equation = value;
            OnPropertyChanged(nameof(Equation));
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string buttonText = button.Text;

        // Добавляем текст кнопки к текущему уравнению
        equationEntry.Text += buttonText;
        OnPropertyChanged(nameof(Equation));
    }

    private void IntegralButton_Clicked(object sender, EventArgs e)
    {
        equationEntry.Text += "∫";
        OnPropertyChanged(nameof(Equation));
    }

    private void RootButton_Clicked(object sender, EventArgs e)
    {
        equationEntry.Text += "√";
        OnPropertyChanged(nameof(Equation));
    }

    private void DegreeButton_Clicked(object sender, EventArgs e)
    {
        equationEntry.Text += "²";
        OnPropertyChanged(nameof(Equation));
    }
}