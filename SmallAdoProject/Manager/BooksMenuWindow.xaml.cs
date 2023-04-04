using Microsoft.Data.SqlClient;
using SmallAdoProject.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
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

namespace SmallAdoProject.Manager
{
    /// <summary>
    /// Логика взаимодействия для BooksMenuWindow.xaml
    /// </summary>
    public partial class BooksMenuWindow : Window
    {
        public ObservableCollection<Entity.Book> Books { get; set; }
        private SqlConnection _connection;
        private List<String> Category;
        private int SelectedCategory;
        public BookInteraction.AddNewBook dialogAdd;
        public BookInteraction.GiveBook dialogGive;
        public BookInteraction.ReturnBook dialogReturn;
        public BooksMenuWindow()
        {
            InitializeComponent();
            Books = new ();
            DataContext = this;
            _connection = new(App.Connection);
            Category = new List<String>
            {
                "Title",
                "Author",
                "Genre",
                "SubGenre",
                "Publisher"
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT B.* FROM Books B";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Books.Add(new Entity.Book(reader));
                    }
                }
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Button_book_add_Click(object sender, RoutedEventArgs e)
        {
            dialogAdd = new BookInteraction.AddNewBook() { Owner = this };
            if (dialogAdd.ShowDialog() == true)
            {
                String sql = "INSERT INTO Books(ID,Title,Author,Genre,SubGenre,Height,Publisher,Total_Count,Cuurent_Count) VALUES (@id, @title, @author, @genre, @subgenre, @height, @publisher, @total_count, @cuurent_count)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@id", dialogAdd.book.ID);
                cmd.Parameters.AddWithValue("@title", dialogAdd.book.Title);
                cmd.Parameters.AddWithValue("@author", dialogAdd.book.Author);
                cmd.Parameters.AddWithValue("@genre", dialogAdd.book.Genre);
                cmd.Parameters.AddWithValue("@subgenre", dialogAdd.book.SubGenre);
                cmd.Parameters.AddWithValue("@height", dialogAdd.book.Height);
                cmd.Parameters.AddWithValue("@publisher", dialogAdd.book.Publisher);
                cmd.Parameters.AddWithValue("@total_count", dialogAdd.book.Total_Count);
                cmd.Parameters.AddWithValue("@cuurent_count", dialogAdd.book.Cuurent_Count);
                try
                {
                    cmd.ExecuteNonQuery();
                    Books.Add(dialogAdd.book);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_book_return_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_book_give_Click(object sender, RoutedEventArgs e)
        {

        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender,RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lv.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
                Search();
        }

        private void ComboBox_SearchCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCategory = ComboBox_SearchCategory.SelectedIndex;
            if(this.IsLoaded)
                Search();
        }

        private async void Search()
        {
            if (SelectedCategory == 0)
            {
                var Books2 = Books.Where(b => b.Title.Contains(TextBox_Search.Text));
                lv.ItemsSource = Books2;
            }
            else if (SelectedCategory == 1)
            {
                var Books2 = Books.Where(b => b.Author.Contains(TextBox_Search.Text));
                lv.ItemsSource = Books2;
            }
            else if (SelectedCategory == 2)
            {
                var Books2 = Books.Where(b => b.Genre.Contains(TextBox_Search.Text));
                lv.ItemsSource = Books2;
            }
            else if (SelectedCategory == 3)
            {
                var Books2 = Books.Where(b => b.SubGenre.Contains(TextBox_Search.Text));
                lv.ItemsSource = Books2;
            }
            else if (SelectedCategory == 4)
            {
                var Books2 = Books.Where(b => b.Publisher.Contains(TextBox_Search.Text));
                lv.ItemsSource = Books2;
            }
        }


    }
}
