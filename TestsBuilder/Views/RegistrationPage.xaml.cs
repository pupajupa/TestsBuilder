using TestsBuilder.Models;
using TestsBuilder.Services;

namespace TestsBuilder.Views;

public partial class RegistrationPage : ContentPage
{
	private readonly IDbService _dbService;
	public RegistrationPage(IDbService dbService)
	{
		InitializeComponent();
		_dbService = dbService;
	}

    private async void Registration_Clicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text;
		string lastName = LastNameEntry.Text;
		string login = LoginEntry.Text;
		string password = PasswordEntry.Text;
		string repassword = ReenterPasswordEntry.Text;
        var user = new User();
        if (_dbService.GetAllUsers().Count() != 0)
        {
            user = _dbService.GetUserByUsername(login);
        }

        if (user.Username == null)
        {
            if ((login == null) || (password == null) || (lastName == null) || (firstName == null) || (repassword == null))
            {
                await DisplayAlert("Ошибка", "Заполните все поля", "OK");
            }
            else if (repassword != password)
            {
                await DisplayAlert("Ошибка", "Пароли не соывпадают", "OK");
            }
            else if (login.Length < 5)
            {
                await DisplayAlert("Ошибка", "Недопустимая длина логина (>4)", "OK");
            }
            else if (password.Length < 5)
            {
                await DisplayAlert("Ошибка", "Слишком короткий пароль (>4)", "OK");
            }
            else
            {
                // Создаем нового пользователя и добавляем его в базу данных
                var newUser = new User
                {
                    Name = firstName,
                    LastName = lastName,
                    Username = login,
                    Password = password
                };
                _dbService.AddUser(newUser);
                // Выводим сообщение об успешной регистрации
                await DisplayAlert("Успех", "Пользователь успешно зарегистрирован", "OK");

                // Переходим на страницу входа
                await Navigation.PushAsync(new TestsPage(newUser, _dbService)); // или используйте Navigation.PushAsync() для перехода на другую страницу
            }
        }
        else
        {
            await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует", "OK");
        }
    }
}