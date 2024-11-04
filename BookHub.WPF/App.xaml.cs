using System;
using System.Windows;
using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Services;
using BookHub.DAL.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using BookHub.BLL.Services.Implementations;
using BookHub.WPF.Views;
using BookHub.DAL.Repositories.Implementations;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.WPF.ViewModels;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookHub.WPF
{
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }
        private IMapper _mapper;

        public App()
        {
            ConfigureLogging();
        }

        private void ConfigureLogging()
        {
            var URL = "http://localhost:5341";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(URL)
                .CreateLogger();

            Log.Information("Serilog налаштовано для WPF!");
        }

        private void ConfigureAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookProfile>();
            });

            _mapper = config.CreateMapper();
            services.AddSingleton(_mapper);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ConfigureAutoMapper(serviceCollection);

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                Log.Fatal(args.ExceptionObject as Exception, "Невловима помилка");

            Log.Information("Додаток запущено");

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Create MainWindow through DI
            var mainWindow = new MainWindow(ServiceProvider.GetService<IBookService>());
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Server=STANISLAV;Database=BookHub;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

            services.AddTransient<IBookRepository, BookRepository>(); // Register repository
            services.AddSingleton<IBookService, BookService>(); // Register service
            services.AddTransient<MainViewModel>(); // Register ViewModel

            // Register MainWindow for DI
            services.AddTransient<MainWindow>(); // Register MainWindow
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Додаток закрито");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
