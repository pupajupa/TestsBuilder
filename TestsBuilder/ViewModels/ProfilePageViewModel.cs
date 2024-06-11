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
using static System.Net.Mime.MediaTypeNames;

namespace TestsBuilder.ViewModels
{
    public partial class ProfilePageViewModel:BaseViewModel
    {
        private readonly IDbService _dbService;
        [ObservableProperty]
        User user;

        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string login;

        [ObservableProperty]
        Image profileImage;
        public ProfilePageViewModel(IDbService dbService) 
        {
            Title = "Profile";
            _dbService = dbService;
            var user1 = _dbService.GetCurrentStudent();
            var user2 = _dbService.GetCurrentTeacher();
        }
        [RelayCommand]
        async Task GoToMaterialsPage() => await Materials();

        public async Task Materials()
        {
            await Shell.Current.GoToAsync($"{nameof(MaterialsPage)}");
        }

        [RelayCommand]
        async Task GoToProfilePage() => await Profile();
        public async Task Profile()
        {
            await Shell.Current.GoToAsync($"{nameof(ProfilePage)}");
        }

        [RelayCommand]
        async Task GoToTestsPage() => await GoTests();

        async Task GoTests()
        {
            await Shell.Current.GoToAsync($"{nameof(TestsPage)}");
        }
    }
}
