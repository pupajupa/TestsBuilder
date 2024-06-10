using TestsBuilder.ViewModels;

namespace TestsBuilder.Views;

public partial class StartPage : ContentPage
{
	public StartPage()
	{
		InitializeComponent();
        BindingContext = new StartPageViewModel();
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}