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
                .AddTransient<UserTestsPage>();

            return services;
        }
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services
                .AddTransient<TestsPageViewModel>()
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
                .AddTransient<UserTestsPageViewModel>();

            return services;
        }
    }
}
