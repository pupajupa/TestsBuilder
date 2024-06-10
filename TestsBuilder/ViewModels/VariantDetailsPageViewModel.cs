using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class VariantDetailsPageViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        [ObservableProperty]
        ExampleVariant variant;
        public VariantDetailsPageViewModel(IDbService dbService) 
        {
            Title = "VariantDetailsPage";
            _dbService = dbService;
            _dbService.Init();
            Variant = _dbService.GetCurrentExampleVariant();
        }

        [RelayCommand]
        async Task GoToProfilePage() => await Profile();
        public async Task Profile()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(ProfilePage));
        }

        [RelayCommand]
        async Task GoToMaterialsPage() => await Materials();

        public async Task Materials()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(MaterialsPage));
        }

        [RelayCommand]
        async Task GoToTestsPage() => await GoTests();

        async Task GoTests()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(TestsPage));
        }
    }
}
