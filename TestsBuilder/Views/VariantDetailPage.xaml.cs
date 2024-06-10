using TestsBuilder.ViewModels;

namespace TestsBuilder.Views
{
    public partial class VariantDetailPage : ContentPage
    {
        public VariantDetailPage(VariantDetailsPageViewModel viewModel)
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
