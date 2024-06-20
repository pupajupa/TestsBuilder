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
    public partial class ExpressionsPageViewModel : BaseViewModel
    {
        private readonly IDbService _dbService;
        public ObservableCollection<Example> Examples { get; set; } = new ObservableCollection<Example>();
        public ExpressionsPageViewModel(IDbService dbService) 
        {
            Title = "Examples";
            _dbService = dbService;
            GetExamplesAsync();
        }

        [ObservableProperty]
        Test test;

        void GetExamplesAsync()
        {
            Test = _dbService.GetCurrentTest();
            if (IsBusy || Test == null)
                return;

            var examples = _dbService.GetAllExpressionsByTestId(Test.Id);

            try
            {
                IsBusy = true;                // Здесь вызываете метод API, чтобы получить задания для выбранного теста
                if (Examples.Count != 0)
                {
                    Examples.Clear();
                }

                foreach (var example in examples)
                {
                    Examples.Add(example);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToVariantsPage(Example example)
        {
            _dbService.SetCurrentExample(example.Id);
            await Shell.Current.GoToAsync(nameof(VariantsPage));
        }

        [RelayCommand]
        async Task Enter() => await AddExample();
        async Task AddExample()
        {
            // Запросить у пользователя ввод имени и других необходимых данных для примера
            string exampleName = await Shell.Current.DisplayPromptAsync("Новый пример", "Введите название примера:");

            if (string.IsNullOrWhiteSpace(exampleName))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Название примера не может быть пустым.", "OK");
                return;
            }

            // Создать новый пример
            var newExample = new Example
            {
                Name = exampleName,
                Text = "",
                BaseAnswers = new List<BaseAnswer>(),
                TestId = Test.Id,
            };

            // Добавить новый пример в коллекцию
            Examples.Add(newExample);
            _dbService.AddExample(newExample);
            _dbService.SetCurrentExample(newExample.Id);
            // Вернуться на предыдущую страницу или обновить текущую страницу
            await GoToCalculatorPageAsync();

        }

        [RelayCommand]
        async Task GoToCalculatorPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(CalculatorPage));
        }

        [RelayCommand]
        async Task GoToProfilePage() => await Profile();
        public async Task Profile()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(ProfilePage));
        }

        [RelayCommand]
        async Task DeleteTest() => await Delete();

        public async Task Delete()
        {
            _dbService.DeleteTest(Test.Id);
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(TestsPage));
        }

        [RelayCommand]
        async Task GoToResultsPage() => await Results();
        public async Task Results()
        {
            await Shell.Current.GoToAsync(nameof(TestResultsPage));
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

        public async Task GoTests()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(TestsPage));
        }
    }
}