using TestsBuilder.Views;

namespace TestsBuilder
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(TestsPage), typeof(TestsPage));
            Routing.RegisterRoute(nameof(ExpressionsPage), typeof(ExpressionsPage));
            Routing.RegisterRoute(nameof(CreateExpressionsPage), typeof(CreateExpressionsPage));
            Routing.RegisterRoute(nameof(VariantsPage), typeof(VariantsPage));
            Routing.RegisterRoute(nameof(VariantDetailPage), typeof(VariantDetailPage));
        }
    }
}
