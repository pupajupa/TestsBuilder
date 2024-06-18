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
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class UserTestsPageViewModel:BaseViewModel
    {
        private readonly IDbService _dbService;
        public ObservableCollection<TestWithResult> TestsWithResults { get; set; } = new ObservableCollection<TestWithResult>();
        public UserTestsPageViewModel(IDbService dbService)
        {
            _dbService = dbService;
            _dbService.Init();
            GetTestsAsync();
        }

        async Task GetTestsAsync()
        {
            if (IsBusy)
            {
                return;
            }
            var tests = _dbService.GetAllTests();
            try
            {
                IsBusy = true;
                if (tests != null)
                {
                    if (TestsWithResults.Count != 0)
                    {
                        TestsWithResults.Clear();
                    }
                    foreach (var test in tests)
                    {
                        var res = _dbService.GetTestResultByTestId(test.Id);
                        TestsWithResults.Add(new TestWithResult { Test = test, Result = res });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Ошибка!", $"Невозможно получить тесты: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToMaterialsPage() => await Materials();

        [RelayCommand] 
        async Task GoToTestsPage()
        {
            await Shell.Current.GoToAsync($"{nameof(UserTestsPage)}");
        }
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
        async Task GoToSolutionsPage(TestWithResult test)
        {
            _dbService.SetCurrentTest(test.Test.Id);
            await Shell.Current.GoToAsync($"{nameof(SolutionTestPage)}");
        }
    }
}
