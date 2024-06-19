using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class TestResultsPage : ContentPage
{
	public TestResultsPage(TestResultsPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}