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
        // ���������� ������� ��� ������ "Settings"
        // ����� �� ������ ��������� ��������� �� �������� �������� ��� ������� ���������� ���� � �����������
        await Navigation.PushAsync(new TestsPage(_user, _dbService));
    }

    private async void Materials_Clicked(object sender, EventArgs e)
    {
        // ���������� ������� ��� ������ "Logout"
        // ����� �� ������ ��������� ����� ������������ �� ������� ������ � ������� �� �������� �����
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
        // ���������� ����-���� ��� ����� �������� �����
        string expressionName = await DisplayPromptAsync("������� �������� �������", "");

        // ���� testName �� ����� null � �� ������, ������� ����� ����
        if (!string.IsNullOrEmpty(expressionName))
        {
            // ������� ����� ���� � ��������� ���������
            Exp newExpression = new() { Title = expressionName, TestId = _test.Id };

            // ��������� ����� ���� � ���� ������ ��� ��������� ������ ����������� ��������
            // ������� ����� �������� ��� ����������� ������ ��������� � �������� ������ ���������, ������ � ������������ � �������� ����������

            Expressions.Add(newExpression);

            ExpressionsListView.ItemsSource = Expressions;

            _dbService.AddExpressionToTest(_test.Name, newExpression);

            var createExpressionsPage = new CreateExpressionsPage(newExpression, _dbService, _user);

            // ��������� �� �������� � ����������
            await Navigation.PushAsync(createExpressionsPage);
        }
    }

    private async void ExpressionsItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Exp tappedExp)
        {
            // ������� ����� �������� ��� ����������� ������ ��������� � �������� ��������� ���� � �������� ���������
            var variantsPage = new VariantsPage(tappedExp, _dbService, _user);

            // ��������� �� �������� � ����������
            await Navigation.PushAsync(variantsPage);
        }

    // ���������� ��������� �������� ������
    ((ListView)sender).SelectedItem = null;
    }
}