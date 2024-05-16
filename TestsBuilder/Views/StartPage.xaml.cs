using TestsBuilder.Services;

namespace TestsBuilder.Views;

public partial class StartPage : ContentPage
{
	private readonly IDbService _dbService;

	public StartPage(IDbService dbService)
	{
		InitializeComponent();
		_dbService = dbService;
		_dbService.Init();
	}

    private async void Login_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new LoginPage(_dbService));
    }

    private async void Registration_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new RegistrationPage(_dbService));
    }
}