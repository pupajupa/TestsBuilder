using TestsBuilder.Models;
using TestsBuilder.Services;
using Microsoft.Maui.Media;
using System.IO;

namespace TestsBuilder.Views;

public partial class ProfilePage : ContentPage
{
    private readonly IDbService _dbService;
    private byte[] _image;
	private User _user;
	public ProfilePage(User user,IDbService dbService)
	{
		InitializeComponent();
        _dbService = dbService;
        _user = user;
        FirstName.Text = _user.Name;
        LastName.Text = _user.LastName;
        Login.Text = _user.Username;
        if (_user.Image != null)
        {
            ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(_user.Image));
        }
    }

    private async void Change_Photo_ClickedAsync(object sender, EventArgs e)
    {
        try
        {
            // Запрос на выбор фото из галереи
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo != null)
            {
                // Получение потока данных из выбранного фото
                using (var stream = await photo.OpenReadAsync())
                {
                    // Преобразование потока в массив байтов
                    byte[] imageData;
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                    _image = imageData;
                    // Создание изображения из массива байтов и его отображение на странице
                    ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить фото: {ex.Message}", "OK");
        }
    }

    private async void Tests_Clicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Settings"
        // Здесь вы можете выполнить навигацию на страницу настроек или открыть диалоговое окно с настройками
        await Navigation.PushAsync(new TestsPage(_user, _dbService));
    }

    private async void Materials_Clicked(object sender, EventArgs e)
    {
        // Обработчик события для кнопки "Logout"
        // Здесь вы можете выполнить выход пользователя из учетной записи и перейти на страницу входа
    }
    private async void Profile_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage(_user, _dbService));
    }

    private async void Update_Info_Clicked(object sender, EventArgs e)
    {
        string name = FirstName.Text;
        string lastName = LastName.Text;
        string username = Login.Text;
        int userId = _user.UserId;
        if (lastName == "" || username == "" || name == "")
        {
            await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
            return;
        }
        string password = _user.Password;
        var updaterUser = new User
        {
            UserId = userId,
            Name = name,
            LastName = lastName,
            Username = username,
            Image = _image,
            Password = password
        };
        _dbService.UpdateUser(updaterUser);

        // Обновление данных пользователя на текущей странице
        _user = updaterUser;
        FirstName.Text = _user.Name;
        LastName.Text = _user.LastName;
        Login.Text = _user.Username;
        if (_user.Image != null)
        {
            ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(_user.Image));
        }

        await DisplayAlert("Успех", "Информация успешно обновлена", "ОК");
    }

}