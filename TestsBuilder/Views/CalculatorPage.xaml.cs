using TestsBuilder.Models;
using TestsBuilder.Services;
using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class CalculatorPage : ContentPage

{
    public CalculatorPage(CalculatorPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}