using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class StartPageViewModel:BaseViewModel
    {
        public StartPageViewModel() { }

        [RelayCommand]
        async Task GoToLoginPage() => await Login();

        public async Task Login()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        [RelayCommand]
        async Task GoToRegistrationPage() => await Registration();

        public async Task Registration()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(RegistrationPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
