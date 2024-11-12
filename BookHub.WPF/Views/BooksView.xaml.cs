
﻿using BookHub.DAL.Entities;

﻿using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.ViewModels;
using BookHub.WPF.Views;

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
using AutoMapper;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for BooksView.xaml
    /// </summary>
    public partial class BooksView : Window
    {

        private readonly ICollectionService _collectionService;
        private readonly IReadingProgressService _readingProgressService;
        private readonly IBookService _bookService;

        public BooksView(BooksViewModel viewModel, ICollectionService collectionService, IReadingProgressService readingProgressService, IBookService bookService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _collectionService = collectionService;
            _readingProgressService = readingProgressService;
            _bookService = bookService;
        }
        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            var collectionsViewModel = new CollectionsViewModel(_collectionService);
            var collectionsView = new CollectionsView(collectionsViewModel);
            collectionsView.Show(); 
            this.Close(); 

        }

        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; // Отримайте реальний ID користувача, як вам потрібно
            var user = await ((BooksViewModel)DataContext).GetUserByIdAsync(userId);

            if (user != null)
            {
                // Тільки якщо user не null, створюємо ViewModel
                var userProfileViewModel = new UserProfileViewModel(user);
                var userProfileView = new UserProfileView
                {
                    DataContext = userProfileViewModel
                };
                userProfileView.ShowDialog();
            }
            else
            {
                MessageBox.Show("User not found.");
            }
        }

        private async void Journal_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; // Replace with actual logic to get the user ID
            var user = await ((BooksViewModel)DataContext).GetUserByIdAsync(userId);

            if (user != null)
            {
                // Initialize the JournalViewModel directly with dependencies
                var journalViewModel = new JournalViewModel(
                    user,
                    _readingProgressService,
                    _bookService
                );

                // Create and show the JournalView window with the view model
                var journalView = new JournalView(journalViewModel);
                journalView.ShowDialog();
            }
            else
            {
                MessageBox.Show("User not found.");
            }
        }

    }

}
