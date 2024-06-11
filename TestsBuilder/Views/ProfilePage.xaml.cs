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
    //        // ������ �� ����� ���� �� �������
    //        var photo = await MediaPicker.PickPhotoAsync();

    //        if (photo != null)
    //        {
    //            // ��������� ������ ������ �� ���������� ����
    //            using (var stream = await photo.OpenReadAsync())
    //            {
    //                // �������������� ������ � ������ ������
    //                byte[] imageData;
    //                using (var memoryStream = new MemoryStream())
    //                {
    //                    await stream.CopyToAsync(memoryStream);
    //                    imageData = memoryStream.ToArray();
    //                }
    //                _image = imageData;
    //                // �������� ����������� �� ������� ������ � ��� ����������� �� ��������
    //                ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("������", $"�� ������� ��������� ����: {ex.Message}", "OK");
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
    //        await DisplayAlert("������", "��������� ��� ����", "��");
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

    //    // ���������� ������ ������������ �� ������� ��������
    //    _user = updaterUser;
    //    FirstName.Text = _user.Name;
    //    LastName.Text = _user.LastName;
    //    Login.Text = _user.Username;
    //    if (_user.Image != null)
    //    {
    //        ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(_user.Image));
    //    }

    //    await DisplayAlert("�����", "���������� ������� ���������", "��");
    //}

}