using TestsBuilder.ViewModels;

namespace TestsBuilder.Views
{
    public partial class VariantsPage : ContentPage
    {
        public VariantsPage(VariantsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }
    }
}
