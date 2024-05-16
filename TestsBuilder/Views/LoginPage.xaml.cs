using TestsBuilder.Services;

namespace TestsBuilder.Views;

public partial class LoginPage : ContentPage
{
	private readonly IDbService _dbService;
	public LoginPage(IDbService dbService)
	{
		InitializeComponent();
        _dbService = dbService;
    }

    private void Login_Clicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        var user = _dbService.CheckUser(username, password);
        if (user != null)
        {
            Navigation.PushAsync(new ProfilePage(user,_dbService));
        }
        else
        {
            // Выводим сообщение об ошибке в случае неверных учетных данных
            DisplayAlert("Ошибка", "Неверное имя пользователя или пароль", "OK");
        }
    }

    private async void Registration_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegistrationPage(_dbService));
    }
}