using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class ProfilePageViewModel:BaseViewModel
    {
        [ObservableProperty]
        User user;

        public ProfilePageViewModel() { }

        [RelayCommand]
        async Task GoToMaterialsPage() => await Materials();

        public async Task Materials()
        {
            await Shell.Current.GoToAsync(nameof(MaterialsPage));
        }

        [RelayCommand]
        async Task GoToProfilePage() => await Profile();
        public async Task Profile()
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage));
        }

        [RelayCommand]
        async Task GoToTestsPage() => await GoTests(User);

        async Task GoTests(User user)
        {
            await Shell.Current.GoToAsync(nameof(TestsPage));
        }
    }
}
