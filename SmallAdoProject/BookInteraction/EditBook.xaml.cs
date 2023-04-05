﻿using SmallAdoProject.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SmallAdoProject.BookInteraction
{
    /// <summary>
    /// Логика взаимодействия для EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        public ObservableCollection<Entity.Book> OwnerBooks { get; set; }
        public Entity.Book book { get; set; }
        public EditBook()
        {
            book = new Entity.Book();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is BooksMenuWindow owner)
            {
                DataContext = Owner;
                OwnerBooks = owner.Books;
            }
            else
            {
                MessageBox.Show("Owner is not BooksMenuWindow");
                Close();
            }
            Box_Title.Text = book.Title;
            Box_Author.Text = book.Author;
            Box_Genre.Text = book.Genre;
            Box_SubGenre.Text = book.SubGenre;
            Box_Height.Text = book.Height.ToString();
            Box_Publisher.Text = book.Publisher;
            Box_Total_Count.Text = book.Total_Count.ToString();
            Box_Current_Count.Text = book.Cuurent_Count.ToString();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void Button_Change_Click(object sender, RoutedEventArgs e)
        {
            book.Title = Box_Title.Text;
            book.Author = Box_Author.Text;
            book.Genre = Box_Genre.Text;
            book.SubGenre = Box_SubGenre.Text;
            book.Height = Convert.ToInt32(Box_Height.Text);
            book.Publisher = Box_Publisher.Text;
            if (Convert.ToInt32(Box_Total_Count.Text) > book.Total_Count)
            {
                book.Cuurent_Count += Convert.ToInt32(Box_Total_Count.Text) - book.Total_Count;
            }
            book.Total_Count = Convert.ToInt32(Box_Total_Count.Text);
            if (book.Total_Count < 0)
                book.Total_Count = 1;
            foreach (var item in OwnerBooks)
            {
                if (item.Title == book.Title && item.ID != book.ID)
                {
                    MessageBox.Show("This Title is already used");
                    return;
                }
            }
            this.DialogResult = true;
        }
    }
}

