using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class UserMaterialsPage : ContentPage
{
	public UserMaterialsPage(UserMaterialsPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}