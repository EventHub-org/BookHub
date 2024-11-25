using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.WPF.State.Accounts;
using BookHub.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for CollectionsView.xaml
    /// </summary>
    public partial class CollectionsView : Window
    {
        private readonly CollectionsViewModel _viewModel;
        private readonly IBookService _bookService;
        private ICollectionService _collectionService;
        private readonly IAccountStore _accountStore;

        public CollectionsView(CollectionsViewModel viewModel, IBookService bookService, ICollectionService collectionService, IAccountStore accountStore)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel; 
            _viewModel = viewModel; 
            _bookService = bookService;
            _collectionService = collectionService;
            _accountStore = accountStore;
        }


        private async void SaveNewCollection_Click(object sender, RoutedEventArgs e)
        {
            var collectionName = CollectionNameTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(collectionName))
            {

                // Додаємо колекцію в базу даних за допомогою асинхронного методу
                await _viewModel.CreateCollectionAsync(collectionName);

                // Оновлюємо список на інтерфейсі
                await _viewModel.RefreshCollectionsAsync();

                // Сховати панель і очистити текстове поле
                CreateCollectionPanel.Visibility = Visibility.Collapsed;
                CollectionNameTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a collection name.");
            }
        }
        private void CreateCollectionPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateCollectionPanel.Visibility = Visibility.Collapsed;

        }
        private void CreateNewCollection_Click(object sender, RoutedEventArgs e)
        {
            CreateCollectionPanel.Visibility = Visibility.Visible; 
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо збережений інстанс BooksView з App
            var app = (App)Application.Current; // Отримуємо екземпляр App
            var booksView = app.BooksView;

            booksView.Show(); // Показуємо BooksView
            this.Close(); // Закриваємо поточне вікно UserProfileView
        }

        private void CollectionItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is CollectionDto selectedCollection)
            {
                var booksViewModel = new BooksFromCollectionViewModel(
                    _bookService,
                    selectedCollection.Id                                 
                );

                var booksView = new BooksFromCollectionView(booksViewModel, _bookService, _collectionService, _accountStore);
                booksView.Show();

                this.Close();
            }
        }

    }

}
