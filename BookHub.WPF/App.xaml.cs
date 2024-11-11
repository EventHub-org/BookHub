using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Mappers;
using BookHub.WPF.ViewModels;
using Serilog;
using System.ComponentModel;
using System.Windows;
using BookHub.WPF.Views;
using Autofac;
using BookHub.DAL.Repositories.Implementations;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using BookHub.DAL.Entities;
using DotNetEnv;
using System.IO;
using BookHub.DAL.DTO;


namespace BookHub.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IMapper _mapper;
        private Autofac.IContainer _container;

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

                //cfg.AddProfile<BookProfile>();

                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();

            ConfigureAutoMapper();


            // Реєстрація IMapper в контейнері
            builder.RegisterInstance(_mapper).As<IMapper>().SingleInstance();



            string envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\.env");

            DotNetEnv.Env.Load(envPath);
            Env.Load();
            Log.Information(envPath);

            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

            builder.Register(db =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer(connectionString); // або UseSqlite, UseNpgsql і т.д. залежно від вашої бази даних
                return new AppDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            // Реєстрація репозиторіїв та сервісів
            builder.RegisterType<BookRepository>().As<IBookRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            builder.RegisterType<BookService>().As<IBookService>();
            builder.RegisterType<UserService>().As<IUserService>();


            builder.RegisterType<BookRepository>().As<IRepository<BookEntity>>();

            builder.RegisterType<CollectionRepository>().As<IRepository<CollectionEntity>>();

            builder.RegisterType<CollectionService>().As<ICollectionService>();

            // Реєстрація ViewModels та Views
            builder.RegisterType<BooksViewModel>();
            builder.RegisterType<UserProfileViewModel>();

            builder.RegisterType<BooksView>();
            builder.RegisterType<UserProfileView>();

            _container = builder.Build();

            // Resolve the main window and show it
            var mainView = _container.Resolve<BooksView>();
            mainView.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }

}
