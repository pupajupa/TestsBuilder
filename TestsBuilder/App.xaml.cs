using Microsoft.EntityFrameworkCore;
using TestsBuilder.ApplicationDbContext;
using TestsBuilder.Services;
using TestsBuilder.ViewModels;
using TestsBuilder.Views;

namespace TestsBuilder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            //MainPage = new AppShell();
        }
    }
}
