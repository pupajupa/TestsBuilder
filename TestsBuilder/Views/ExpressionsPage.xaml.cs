using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using TestsBuilder.Models;
using TestsBuilder.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TestsBuilder.Views;

public partial class ExpressionsPage : ContentPage
{
    private readonly IDbService _dbService;
    private TestsBuilder.Models.User _user;
    private ObservableCollection<Exp> _expressions;
    private Test _test;


    public ExpressionsPage(Test test, IDbService dbService, TestsBuilder.Models.User user)
    {
        InitializeComponent();
        BindingContext = this;
        _dbService = dbService;
        _test = test;
        _user = user;
        titleLabel.Text = test.Name;
        LoadExpressionsFromTest();
    }

    public ObservableCollection<Exp> Expressions
    {
        get => _expressions;
        set
        {
            _expressions = value;
            OnPropertyChanged(nameof(Expressions));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

    private void LoadExpressionsFromTest()
    {
        IEnumerable<Exp> expressions = new List<Exp>();
        if (_dbService.GetTestExpressions(_test.Id).Count() != 0)
        {
            expressions = _dbService.GetTestExpressions(_test.Id);
        }

        Expressions = new ObservableCollection<Exp>(expressions);
        ExpressionsListView.ItemsSource = Expressions;
    }

    private async void CreateExpressions_Clicked(object sender, EventArgs e)
    {
        // Показываем мини-окно для ввода названия теста
        string expressionName = await DisplayPromptAsync("Введите название задания", "");

        // Если testName не равен null и не пустой, создаем новый тест
        if (!string.IsNullOrEmpty(expressionName))
        {
            // Создаем новый тест с введенным названием
            Exp newExpression = new() { Title = expressionName, TestId = _test.Id };

            // Добавляем новый тест в базу данных или выполняем другие необходимые действия
            // Создаем новую страницу для отображения списка вариантов и передаем список выражений, сервис и пользователя в качестве параметров

            Expressions.Add(newExpression);

            ExpressionsListView.ItemsSource = Expressions;

            _dbService.AddExpressionToTest(_test.Name, newExpression);

            var createExpressionsPage = new CreateExpressionsPage(newExpression, _dbService, _user);

            // Переходим на страницу с вариантами
            await Navigation.PushAsync(createExpressionsPage);
        }
    }

    private async void ExpressionsItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Exp tappedExp)
        {
            // Создаем новую страницу для отображения списка вариантов и передаем выбранный тест в качестве параметра
            var variantsPage = new VariantsPage(tappedExp, _dbService, _user);

            // Переходим на страницу с вариантами
            await Navigation.PushAsync(variantsPage);
        }

    // Сбрасываем выделение элемента списка
    ((ListView)sender).SelectedItem = null;
    }
}