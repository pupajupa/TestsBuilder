﻿using CommunityToolkit.Mvvm.Input;
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
    public partial class MaterialsPageViewModel:BaseViewModel
    {
        private readonly IDbService _dbService;
        public ObservableCollection<Material> Materials { get; set; } = new ObservableCollection<Material>();

        public MaterialsPageViewModel(IDbService dbService)
        {
            Title = "";
            _dbService = dbService;
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            var materials = _dbService.GetAllMaterials();
            if (materials != null)
            {
                foreach (var material in materials)
                {
                    Materials.Add(material);
                }
            }
        }

        [RelayCommand]
        private async Task UploadMaterialAsync()
        {
            // Код для выбора файла и его загрузки
            var file = await PickFileAsync();
            if (file != null)
            {
                var material = new Material
                {
                    Name = file.FileName,
                    FilePath = file.FullPath,
                };
                _dbService.AddMaterial(material);
                Materials.Add(material);
            }
        }

        private async Task<FileResult> PickFileAsync()
        {
            // Код для выбора файла (может отличаться в зависимости от платформы)
            var result = await FilePicker.PickAsync();
            return result;
        }

        [RelayCommand]
        private void OpenMaterial(Material material)
        {
            if (File.Exists(material.FilePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = material.FilePath,
                    UseShellExecute = true
                });
            }
        }

        [RelayCommand]
        async Task GoToMaterialsPage() => await MaterialsView();

        public async Task MaterialsView()
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
    }
}
