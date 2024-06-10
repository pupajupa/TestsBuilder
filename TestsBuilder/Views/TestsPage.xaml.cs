using System.Collections.ObjectModel;
using System.ComponentModel;
using TestsBuilder.Models;
using TestsBuilder.Services;
using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class TestsPage : ContentPage
{
	public TestsPage(TestsPageViewModel viewModel) 
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }	
}
 //   private ObservableCollection<Test> _tests;
 //   private User _user;
 //   private readonly IDbService _dbService;

	//public TestsPage(User user,IDbService dbService)
	//{
	//	InitializeComponent();
 //       BindingContext = this; // Установка контекста привязки данных на текущий экземпляр страницы
 //       _user = user;
 //       _dbService = dbService;
 //       LoadTests();
 //   }

 //   private async void Tests_Clicked(object sender, EventArgs e)
 //   {
 //       // Обработчик события для кнопки "Settings"
 //       // Здесь вы можете выполнить навигацию на страницу настроек или открыть диалоговое окно с настройками
 //       await Navigation.PushAsync(new TestsPage(_user,_dbService));
 //   }

 //   private async void Materials_Clicked(object sender, EventArgs e)
 //   {
 //       // Обработчик события для кнопки "Logout"
 //       // Здесь вы можете выполнить выход пользователя из учетной записи и перейти на страницу входа
 //   }

 //   private async void Profile_Clicked(object sender, EventArgs e)
 //   {
 //       await Navigation.PushAsync(new ProfilePage(_user,_dbService));
 //   }

 //   public ObservableCollection<Test> Tests
 //   {
 //       get => _tests;
 //       set
 //       {
 //           _tests = value;
 //           OnPropertyChanged(nameof(Tests));
 //       }
 //   }

 //   private void LoadTests()
 //   {
 //       IEnumerable<Test> tests = new List<Test>();
 //       if (_dbService.GetAllTests().Count() != 0)
 //       {
 //               tests = _dbService.GetAllTests();
 //       }

 //       Tests = new ObservableCollection<Test>(tests);
 //   }

 //   public event PropertyChangedEventHandler PropertyChanged;

 //   protected virtual void OnPropertyChanged(string propertyName)
 //   {
 //       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
 //   }

 //   private async void TestsItemTapped(object sender, ItemTappedEventArgs e)
 //   {
 //       if(e.Item is Test tappedTest)
 //       {
 //           // Создаем новую страницу для отображения списка вариантов и передаем выбранный тест в качестве параметра
 //           var expressionsPage = new ExpressionsPage(tappedTest,_dbService,_user);

 //           // Переходим на страницу с вариантами
 //           await Navigation.PushAsync(expressionsPage);
 //       }

 //   // Сбрасываем выделение элемента списка
 //       ((ListView)sender).SelectedItem = null;
 //   }

 //   private async void Expressions_Clicked(object sender, EventArgs e)
 //   {
 //       // Показываем мини-окно для ввода названия теста
 //       string testName = await DisplayPromptAsync("Введите название теста", "");

 //       // Если testName не равен null и не пустой, создаем новый тест
 //       if (!string.IsNullOrEmpty(testName))
 //       {
 //           // Создаем новый тест с введенным названием
 //           Test newTest = new Test { Name = testName};

 //           // Добавляем новый тест в базу данных или выполняем другие необходимые действия
 //           _dbService.AddTest(newTest);

 //           Tests.Add(newTest);
 //           // Создаем новую страницу для отображения списка вариантов и передаем список выражений, сервис и пользователя в качестве параметров
 //           var expressionsPage = new ExpressionsPage(newTest, _dbService, _user);

 //           // Переходим на страницу с вариантами
 //           await Navigation.PushAsync(expressionsPage);
 //       }
 //   }

    //        
//}