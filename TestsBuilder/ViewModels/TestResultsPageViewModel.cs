using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Interfaces;
using TestsBuilder.Models;
using TestsBuilder.Views;

namespace TestsBuilder.ViewModels
{
    public partial class TestResultsPageViewModel:BaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<Student> students = new();

        [ObservableProperty]
        public ObservableCollection<TestResult> results = new();

        private List<TestResult> resultsList = new();

        private Test test = new();

        private readonly IDbService _dbService;
        public TestResultsPageViewModel(IDbService dbService)
        {
            _dbService = dbService;
            LoadData();
        }

        public void LoadData()
        {
            test = _dbService.GetCurrentTest();
            resultsList = _dbService.GetAllResultsByTestId(test.Id);
            if(resultsList.Count != 0)
            {
                foreach(var result in resultsList)
                {
                    Results.Add(result);
                    Students.Add(_dbService.GetStudentById(result.StudentId));
                }
            }
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

        public async Task GoTests()
        {
            _dbService.ClearCurrentTest();
            await Shell.Current.GoToAsync(nameof(TestsPage));
        }
    }
}
