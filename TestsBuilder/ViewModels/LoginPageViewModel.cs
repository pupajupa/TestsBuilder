using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Views;
using Windows.Networking;

namespace TestsBuilder.ViewModels
{
    public partial class LoginPageViewModel:BaseViewModel
    {
        private readonly IDbService _dbService;

        [ObservableProperty]
        string login;

        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task GoLogin() => await LoginAsync();

        public LoginPageViewModel(IDbService dbService)
        {
            Title = "Registration";
            _dbService = dbService;
            dbService.Init();
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "OK");
                return;
            }

            var user = _dbService.GetStudentByLogin(Login);
            var user1 = _dbService.GetTeacherByLogin(Login);
            if (user is null && user1 is null)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Такого пользователя не существует", "OK");
                return;
            }

            else if (user != null)
            {
                if (user.Password != password) {
                    await Shell.Current.DisplayAlert("Ошибка", "Неверный пароль", "OK");
                }
                else
                {
                    _dbService.SetCurrentStudent(user.Login);
                    await GoToUserTestsPage();
                }
            }
            else if (user1!= null)
            {
                if (user1.Password != password)
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Неверный пароль", "OK");
                }
                else
                {
                    _dbService.SetCurrentTeacher(user1.Login);
                    await GoToTestsPage();
                }
            }
        }
        private async Task GoToUserTestsPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(UserTestsPage)}");
        }
        private async Task GoToTestsPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(TestsPage)}");
        }

        [RelayCommand]
        private async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }
    }
}
