using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TestsBuilder.Context;
using TestsBuilder.Services;
using TestsBuilder.Views;

namespace TestsBuilder
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("AkayaKanadaka-Regular.ttf", "AkayaKanadakaRegular");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("texgyretermes-bold.otf", "TexBold");
                    fonts.AddFont("texgyretermes-regular.otf", "TexRegular");
                });
            //builder.Services.AddDbContext<AppDbContext>(opt => 
            //    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
            builder.Services.AddTransient<IDbService, AuthService>();
            builder.Services.AddTransient<LoginPage>(); 
            builder.Services.AddTransient<RegistrationPage>(); 
            builder.Services.AddTransient<StartPage>();
            builder.Services.AddTransient<TestsPage>();
            builder.Services.AddTransient<ExpressionsPage>();
            builder.Services.AddTransient<CreateExpressionsPage>();
            builder.Services.AddTransient<VariantsPage>();

            //string settingsStream = "TestsBuilder.appsettings.json";
            //var a = Assembly.GetExecutingAssembly();
            //using var stream = a.GetManifestResourceStream(settingsStream);
            //builder.Configuration.AddJsonStream(stream);

            //var connStr = builder.Configuration
            //    .GetConnectionString("SqliteConnection");
            //string dataDirectory = FileSystem.Current.AppDataDirectory + "/";
            //connStr = String.Format(connStr, dataDirectory);
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseSqlite(connStr)
            //    .Options;

            //builder.Services
            // .AddPersistence(options);


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
