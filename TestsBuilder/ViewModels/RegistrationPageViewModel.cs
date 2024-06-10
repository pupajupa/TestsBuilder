using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Resources;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Requests;
using TestsBuilder.Responses;
using TestsBuilder.Views;
using static System.Net.Mime.MediaTypeNames;

namespace TestsBuilder.ViewModels
{
    public partial class RegistrationPageViewModel : BaseViewModel
    {
        //private readonly IApiService _apiService;

        private readonly IDbService _dbService;

        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string login;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string secondPassword;

        [ObservableProperty]
        Student user;

        [RelayCommand]
        async Task Register() => await RegisterAsync();

        public RegistrationPageViewModel(IDbService dbService)
        {
            Title = "Registration";
            _dbService = dbService;
            dbService.Init();
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(SecondPassword))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "OK");
                return;
            }

            if (Password != SecondPassword)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пароли не совпадают.", "OK");
                return;
            }

            if (Password.Length < 5)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пароль не может содержать меньше, чем 5 символов", "OK");
                return;
            }

            if (Login.Length < 5)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Логин не может содержать меньше, чем 5 символов", "OK");
                return;
            }
            try
            {
                // Вызовите метод для регистрации пользователя
                //AuthResponse result = await _apiService.Register(FirstName,LastName,Login,Password);

                //_apiService.AuthorizationHeader = result.Token;
                // Получите текущее сборочное пространство имен
                var assembly = typeof(RegistrationPageViewModel).Assembly;

                // Убедитесь, что путь соответствует структуре ваших папок и пространств имен в проекте
                using Stream imageStream = assembly.GetManifestResourceStream("TestsBuilder.Resources.Images.default_profile_image.png");

                if (imageStream == null)
                {
                    throw new FileNotFoundException("Не удалось найти стандартное фото в ресурсах.");
                }

                using MemoryStream memoryStream = new();
                imageStream.CopyTo(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                User = new Student
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Login = Login,
                    Password = Password,
                    Image = imageBytes,
                };

                _dbService.AddStudent(User);
                _dbService.SetCurrentStudent(User.Login);

                await GoToUserTestsPage();

                // В этой точке вы можете выполнить дополнительные действия, например, перейти на другую страницу или отобразить сообщение об успешной регистрации
            }
            catch (HttpRequestException httpEx)
            {
                // Обработка ошибки HTTP-запроса
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка при выполнении запроса: {httpEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                // Обработка других ошибок
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Ошибка", $"Произошла ошибка при регистрации: {ex.Message}", "OK");
            }
        }
        private async Task GoToUserTestsPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(UserTestsPage)}");
        }
    }
}
