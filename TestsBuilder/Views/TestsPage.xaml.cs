using System.Collections.ObjectModel;
using System.ComponentModel;
using TestsBuilder.Models;
using TestsBuilder.Services;
using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class TestsPage : ContentPage
{
	public TestsPage(TestsPageViewModel viewModel) 
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }	
}