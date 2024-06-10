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
 //       BindingContext = this; // ��������� ��������� �������� ������ �� ������� ��������� ��������
 //       _user = user;
 //       _dbService = dbService;
 //       LoadTests();
 //   }

 //   private async void Tests_Clicked(object sender, EventArgs e)
 //   {
 //       // ���������� ������� ��� ������ "Settings"
 //       // ����� �� ������ ��������� ��������� �� �������� �������� ��� ������� ���������� ���� � �����������
 //       await Navigation.PushAsync(new TestsPage(_user,_dbService));
 //   }

 //   private async void Materials_Clicked(object sender, EventArgs e)
 //   {
 //       // ���������� ������� ��� ������ "Logout"
 //       // ����� �� ������ ��������� ����� ������������ �� ������� ������ � ������� �� �������� �����
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
 //           // ������� ����� �������� ��� ����������� ������ ��������� � �������� ��������� ���� � �������� ���������
 //           var expressionsPage = new ExpressionsPage(tappedTest,_dbService,_user);

 //           // ��������� �� �������� � ����������
 //           await Navigation.PushAsync(expressionsPage);
 //       }

 //   // ���������� ��������� �������� ������
 //       ((ListView)sender).SelectedItem = null;
 //   }

 //   private async void Expressions_Clicked(object sender, EventArgs e)
 //   {
 //       // ���������� ����-���� ��� ����� �������� �����
 //       string testName = await DisplayPromptAsync("������� �������� �����", "");

 //       // ���� testName �� ����� null � �� ������, ������� ����� ����
 //       if (!string.IsNullOrEmpty(testName))
 //       {
 //           // ������� ����� ���� � ��������� ���������
 //           Test newTest = new Test { Name = testName};

 //           // ��������� ����� ���� � ���� ������ ��� ��������� ������ ����������� ��������
 //           _dbService.AddTest(newTest);

 //           Tests.Add(newTest);
 //           // ������� ����� �������� ��� ����������� ������ ��������� � �������� ������ ���������, ������ � ������������ � �������� ����������
 //           var expressionsPage = new ExpressionsPage(newTest, _dbService, _user);

 //           // ��������� �� �������� � ����������
 //           await Navigation.PushAsync(expressionsPage);
 //       }
 //   }

    //        
//}