using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class SolutionTestPage : ContentPage
{
	public SolutionTestPage(SolutionTestPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}