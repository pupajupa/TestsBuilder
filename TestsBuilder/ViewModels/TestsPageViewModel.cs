using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;
using CommunityToolkit.Mvvm;

using CommunityToolkit.Mvvm.Input;
using TestsBuilder.Responses;
using TestsBuilder.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using TestsBuilder.Interfaces;

namespace TestsBuilder.ViewModels
{
    public partial class TestsPageViewModel:BaseViewModel
    {
        //Зарегать сервис
        private readonly IDbService _dbService;
        public ObservableCollection<Test> Tests { get; set; } = new();

        [ObservableProperty]
        User user;

        public TestsPageViewModel(IDbService dbService) 
        {
            Title = "Tests";
            //service
            _dbService = dbService;
            _dbService.Init();
            GetTestsAsync();
        }


        [RelayCommand]
        async Task GoToExpressionsPageAsync(Test test)
        {
            _dbService.SetCurrentTest(test.Id);
            await Shell.Current.GoToAsync(nameof(ExpressionsPage));
        }



        [RelayCommand]
        async Task GoToMaterialsPage() => await Materials();

        public async Task Materials()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync($"{nameof(MaterialsPage)}");
        }

        [RelayCommand]
        async Task GoToProfilePage() => await Profile();
        public async Task Profile()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync($"{nameof(ProfilePage)}");
        }

        [RelayCommand]
        async Task GoToTestsPage() => await GoTests();

        async Task GoTests()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync($"{nameof(TestsPage)}");
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
                    if (Tests.Count != 0)
                    {
                        Tests.Clear();
                    }
                    foreach (var test in tests)
                    {
                        Tests.Add(test);
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
        async Task Enter() => await AddTest();
        async Task AddTest()
        {
            // Запросить у пользователя ввод имени и других необходимых данных для примера
            string testName = await Shell.Current.DisplayPromptAsync("Новый тест", "Введите название теста:");

            if (string.IsNullOrWhiteSpace(testName))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Название теста не может быть пустым.", "OK");
                return;
            }

            var newTest = new Test
            {
                Name = testName,
                Description = "Нет информации о тесте",
            };
            // Добавить новый пример в коллекцию
            Tests.Add(newTest);

            _dbService.AddTest(newTest);
            await GoToExpressionsPageAsync(newTest);
        }
    }   
}
