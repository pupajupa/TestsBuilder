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
}