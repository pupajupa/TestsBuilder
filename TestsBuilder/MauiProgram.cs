using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestsBuilder.ApplicationDbContext;
using TestsBuilder.Interfaces;
using TestsBuilder.Services;

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

            builder.Services
                .RegisterPages()
                .RegisterViewModels();
            builder.Services.AddTransient<IExpressionGenerator, ExpressionGenerator>();
            //builder.Services.AddHttpClient<IApiService, ApiService>();
            builder.Services.AddSingleton<IDbService, DbService>();

            // Регистрация сервисов для работы с базой данных
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
