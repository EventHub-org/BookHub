﻿using BookHub.DAL.Entities;
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
    /// Interaction logic for BooksView.xaml
    /// </summary>
    public partial class BooksView : Window
    {
        public BooksView(BooksViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
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
    }

}