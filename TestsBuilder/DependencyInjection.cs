using TestsBuilder.ViewModels;
using TestsBuilder.Views;

namespace TestsBuilder
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterPages(this IServiceCollection services)
        {
            services
                .AddTransient<LoginPage>()
                .AddTransient<UserMaterialsPage>()
                .AddTransient<RegistrationPage>()
                .AddTransient<StartPage>()
                .AddTransient<TestsPage>()
                .AddTransient<ExpressionsPage>()
                .AddTransient<VariantsPage>()
                .AddTransient<CalculatorPage>()
                .AddTransient<VariantDetailPage>()
                .AddTransient<MaterialsPage>()
                .AddTransient<ProfilePage>()
                .AddTransient<SolutionTestPage>()
                .AddTransient<UserTestsPage>()
                .AddTransient<TestResultsPage>();

            return services;
        }
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services
                .AddTransient<TestsPageViewModel>()
                .AddTransient<UserMaterialsPageViewModel>()
                .AddTransient<CalculatorPageViewModel>()
                .AddTransient<ExpressionsPageViewModel>()
                .AddTransient<BaseViewModel>()
                .AddTransient<VariantsPageViewModel>()
                .AddTransient<VariantDetailsPageViewModel>()
                .AddTransient<LoginPageViewModel>()
                .AddTransient<RegistrationPageViewModel>()
                .AddTransient<ProfilePageViewModel>()
                .AddTransient<StartPageViewModel>()
                .AddTransient<MaterialsPageViewModel>()
                .AddTransient<SolutionTestPageViewModel>()
                .AddTransient<UserTestsPageViewModel>()
                .AddTransient<TestResultsPageViewModel>();
            return services;
        }
    }
}
