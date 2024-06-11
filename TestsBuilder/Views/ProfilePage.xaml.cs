    using TestsBuilder.Models;
using TestsBuilder.Services;
using Microsoft.Maui.Media;
using System.IO;
using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    //private async void Change_Photo_ClickedAsync(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Запрос на выбор фото из галереи
    //        var photo = await MediaPicker.PickPhotoAsync();

    //        if (photo != null)
    //        {
    //            // Получение потока данных из выбранного фото
    //            using (var stream = await photo.OpenReadAsync())
    //            {
    //                // Преобразование потока в массив байтов
    //                byte[] imageData;
    //                using (var memoryStream = new MemoryStream())
    //                {
    //                    await stream.CopyToAsync(memoryStream);
    //                    imageData = memoryStream.ToArray();
    //                }
    //                _image = imageData;
    //                // Создание изображения из массива байтов и его отображение на странице
    //                ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Ошибка", $"Не удалось загрузить фото: {ex.Message}", "OK");
    //    }
    //}

    //private async void Update_Info_Clicked(object sender, EventArgs e)
    //{
    //    string name = FirstName.Text;
    //    string lastName = LastName.Text;
    //    string username = Login.Text;
    //    int userId = _user.UserId;
    //    if (lastName == "" || username == "" || name == "")
    //    {
    //        await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
    //        return;
    //    }
    //    string password = _user.Password;
    //    var updaterUser = new User
    //    {
    //        UserId = userId,
    //        Name = name,
    //        LastName = lastName,
    //        Username = username,
    //        Image = _image,
    //        Password = password
    //    };
    //    _dbService.UpdateUser(updaterUser);

    //    // Обновление данных пользователя на текущей странице
    //    _user = updaterUser;
    //    FirstName.Text = _user.Name;
    //    LastName.Text = _user.LastName;
    //    Login.Text = _user.Username;
    //    if (_user.Image != null)
    //    {
    //        ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(_user.Image));
    //    }

    //    await DisplayAlert("Успех", "Информация успешно обновлена", "ОК");
    //}

}