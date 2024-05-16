using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestsBuilder.Context;
using TestsBuilder.Services;
using TestsBuilder.Views;

namespace TestsBuilder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
