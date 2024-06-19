using TestsBuilder.Views;

namespace TestsBuilder
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CalculatorPage), typeof(CalculatorPage));
            Routing.RegisterRoute(nameof(MaterialsPage), typeof(MaterialsPage));
            Routing.RegisterRoute(nameof(ProfilePage),typeof(ProfilePage));
            Routing.RegisterRoute(nameof(SolutionTestPage),typeof(SolutionTestPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(TestsPage), typeof(TestsPage));
            Routing.RegisterRoute(nameof(ExpressionsPage), typeof(ExpressionsPage));
            Routing.RegisterRoute(nameof(UserTestsPage), typeof(UserTestsPage));
            Routing.RegisterRoute(nameof(VariantsPage), typeof(VariantsPage));
            Routing.RegisterRoute(nameof(VariantDetailPage), typeof(VariantDetailPage));
            Routing.RegisterRoute(nameof(TestResultsPage), typeof(TestResultsPage));
        }
    }
}
