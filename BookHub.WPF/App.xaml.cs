using AutoMapper;
using BookHub.DAL.Mappers;
using Serilog;
using System.Windows;

namespace BookHub.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IMapper _mapper;

        public App()
        {
            ConfigureLogging();
        }

        private void ConfigureLogging()
        {
            var URL = "http://localhost:5341";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // Мінімальний рівень логів
                .Enrich.FromLogContext() // Контекст до логів
                .WriteTo.Console()
                .WriteTo.Seq(URL) // Виводимо логи в Seq, вказавши його адресу
                .CreateLogger();

            Log.Information("Serilog налаштовано для WPF!");
        }

        private void ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookProfile>();
            });

            _mapper = config.CreateMapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Відловлюємо виключення
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                Log.Fatal(args.ExceptionObject as Exception, "Невловима помилка");

            Log.Information("Додаток запущено");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Додаток закрито");
            Log.CloseAndFlush(); // Закриваємо логування перед завершенням додатка
            base.OnExit(e);
        }
    }

}
