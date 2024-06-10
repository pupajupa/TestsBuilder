using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class MaterialsPage : ContentPage
{
	public MaterialsPage(MaterialsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}