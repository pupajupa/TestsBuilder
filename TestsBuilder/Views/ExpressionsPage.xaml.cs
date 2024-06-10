using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using TestsBuilder.Models;
using TestsBuilder.Services;
using TestsBuilder.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace TestsBuilder.Views;

public partial class ExpressionsPage : ContentPage
{ 
    public ExpressionsPage(ExpressionsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}