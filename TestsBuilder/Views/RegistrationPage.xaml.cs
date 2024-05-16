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
                await DisplayAlert("������", "��������� ��� ����", "OK");
            }
            else if (repassword != password)
            {
                await DisplayAlert("������", "������ �� ����������", "OK");
            }
            else if (login.Length < 5)
            {
                await DisplayAlert("������", "������������ ����� ������ (>4)", "OK");
            }
            else if (password.Length < 5)
            {
                await DisplayAlert("������", "������� �������� ������ (>4)", "OK");
            }
            else
            {
                // ������� ������ ������������ � ��������� ��� � ���� ������
                var newUser = new User
                {
                    Name = firstName,
                    LastName = lastName,
                    Username = login,
                    Password = password
                };
                _dbService.AddUser(newUser);
                // ������� ��������� �� �������� �����������
                await DisplayAlert("�����", "������������ ������� ���������������", "OK");

                // ��������� �� �������� �����
                await Navigation.PushAsync(new TestsPage(newUser, _dbService)); // ��� ����������� Navigation.PushAsync() ��� �������� �� ������ ��������
            }
        }
        else
        {
            await DisplayAlert("������", "������������ � ����� ������� ��� ����������", "OK");
        }
    }
}