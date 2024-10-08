using BookHub.DAL.DataAccess;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IO;
using System.Windows;

namespace BookHub.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

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
