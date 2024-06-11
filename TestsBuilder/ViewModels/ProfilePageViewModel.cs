using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private byte[] _image;
        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string login;

        [ObservableProperty]
        ImageSource profileImage;
        public ProfilePageViewModel(IDbService dbService) 
        {
            Title = "Profile";
            _dbService = dbService;
            var user1 = _dbService.GetCurrentStudent();
            var user2 = _dbService.GetCurrentTeacher();
            if (user1 != null)
            {
                FirstName = user1.FirstName;
                LastName = user1.LastName;
                Login = user1.Login;
                if (user1 != null && user1.Image != null)
                {
                    try
                    {
                        // Создание MemoryStream из массива байтов
                        using MemoryStream ms = new MemoryStream(user1.Image);
                        // Убедитесь, что MemoryStream содержит данные
                        if (ms.Length > 0)
                        {
                            // Установка источника изображения
                            ProfileImage = ImageSource.FromStream(() => ms);
                            OnPropertyChanged(nameof(ProfileImage));
                        }
                        else
                        {
                            // Использование изображения по умолчанию, если MemoryStream пуст
                            ProfileImage = ImageSource.FromFile("Resources/Images/default_profile_image.png");
                            OnPropertyChanged(nameof(ProfileImage));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключений, если возникла ошибка при конвертации
                        Debug.WriteLine($"Error setting image source: {ex.Message}");
                        ProfileImage = ImageSource.FromFile("Resources/Images/default_profile_image.png");
                        OnPropertyChanged(nameof(ProfileImage));
                    }
                }
            }
            else if(user2 != null)
            {
                firstName = user2.FirstName;
                lastName = user2.LastName;
                login = user2.Login;
                if (user2 != null && user2.Image != null)
                {
                    try
                    {
                        // Создание MemoryStream из массива байтов
                        using MemoryStream ms = new MemoryStream(user2.Image);
                        // Убедитесь, что MemoryStream содержит данные
                        if (ms.Length > 0)
                        {
                            // Установка источника изображения
                            ProfileImage = ImageSource.FromStream(() => ms);
                            OnPropertyChanged(nameof(ProfileImage));
                        }
                        else
                        {
                            // Использование изображения по умолчанию, если MemoryStream пуст
                            ProfileImage = ImageSource.FromFile("Resources/Images/default_profile_image.png");
                            OnPropertyChanged(nameof(ProfileImage));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключений, если возникла ошибка при конвертации
                        Debug.WriteLine($"Error setting image source: {ex.Message}");
                        ProfileImage = ImageSource.FromFile("Resources/Images/default_profile_image.png");
                    }
                }
            }
        }

        [RelayCommand]
        private async Task ChangePhoto()
        {
            try
            {
                // Запрос на выбор фото из галереи
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    // Получение потока данных из выбранного фото
                    using var stream = await photo.OpenReadAsync();
                    // Преобразование потока в массив байтов
                    byte[] imageData;
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                    _image = imageData;
                    // Создание изображения из массива байтов и его отображение на странице
                    ProfileImage = ImageSource.FromStream(() => new MemoryStream(imageData));
                    OnPropertyChanged(nameof(ProfileImage));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка!", $"Не удалось загрузить фото: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task UpdateInfo()
        {
            if (LastName == "" || FirstName == "" || Login == "")
            {
                await Shell.Current.DisplayAlert("Ошибка!", "Заполните все поля!", "OK");
                return;
            }
            if (Login.Length < 5)
            {
                await Shell.Current.DisplayAlert("Ошибка!", "Слишком короткий логин", "OK");
                return;
            }
            if(_dbService.GetCurrentStudent()!=null)
            {
                var student = _dbService.GetCurrentStudent();
                student.FirstName = FirstName;
                student.LastName = LastName;
                student.Login = Login;
                student.Image = _image;
                _dbService.UpdateStudent(student);
                _dbService.SetCurrentStudent(student.Login);
            }
            else if (_dbService.GetCurrentTeacher() != null)
            {
                var teacher = _dbService.GetCurrentTeacher();
                teacher.FirstName = FirstName;
                teacher.LastName = LastName;
                teacher.Login = Login;
                teacher.Image = _image;
                _dbService.UpdateTeacher(teacher);  
                _dbService.SetCurrentTeacher(teacher.Login);
            }
            await Shell.Current.DisplayAlert("Успех", "Информация успешно обновлена", "ОК");
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
            if (_dbService.GetCurrentTeacher() != null)
            {
                await Shell.Current.GoToAsync($"{nameof(TestsPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(UserTestsPage)}");
            }
        }
    }
}
