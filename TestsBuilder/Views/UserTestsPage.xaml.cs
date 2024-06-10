using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class UserTestsPage : ContentPage
{
	public UserTestsPage(UserTestsPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}