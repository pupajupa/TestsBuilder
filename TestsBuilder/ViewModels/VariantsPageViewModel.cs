using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Services;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{ 
    public partial class VariantsPageViewModel:BaseViewModel
    {
        private readonly IDbService _dbService;
        public ObservableCollection<ExampleVariant> Variants { get; set; } = new ObservableCollection<ExampleVariant>();
        public VariantsPageViewModel(IDbService dbService)
        {
            Title = "Variants";
            _dbService = dbService;
            _dbService.Init();
            GetVariants();
        }

        [ObservableProperty]
        Example example;

        void GetVariants()
        {
            Example = _dbService.GetCurrentExample();
            if (IsBusy || Example==null)
                return;
            var variants = _dbService.GetAllExpressionVariantsByExampleId(Example.Id); 
            try
            {
                IsBusy = true;
                // Здесь вызываете метод API, чтобы получить задания для выбранного теста

                if (variants.Count() != 0)
                {
                    Variants.Clear();
                }

                foreach (var variant in variants)
                {
                    Variants.Add(variant);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //await Shell.Current.DisplayAlert("Ошибка!", $"Невозможно получить задания: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GoToVariantDetailPage(ExampleVariant variant)
        {
            _dbService.SetCurrentExampleVariant(variant.Id);    
            await Shell.Current.GoToAsync(nameof(VariantDetailPage));
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
