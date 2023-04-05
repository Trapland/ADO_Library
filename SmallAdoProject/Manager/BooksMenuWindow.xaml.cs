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
        public ObservableCollection<Entity.User> Users { get; set; }
        private SqlConnection _connection;
        private int SelectedCategory;
        public BookInteraction.AddNewBook dialogAdd;
        public BookInteraction.GiveBook dialogGive;
        public BookInteraction.ReturnBook dialogReturn;
        public BookInteraction.AddUser dialogAddUser;
        public BookInteraction.EditBook dialogEditBook;
        public BooksMenuWindow()
        {
            InitializeComponent();
            Books = new();
            Users = new();
            DataContext = this;
            _connection = new(App.Connection);
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
                cmd.CommandText = "SELECT U.* FROM Users U";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Users.Add(new Entity.User(reader));
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
                    MessageBox.Show(ex.Message, "Create Book error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dialogEditBook = new BookInteraction.EditBook() { Owner = this };
            dialogEditBook.book = lv.SelectedItem as Entity.Book;
            if (dialogEditBook.ShowDialog() == true)
            {
                String sql = $"Update Books SET ID=@id, Title=@title, Author=@author, Genre=@genre, SubGenre=@subgenre, Height=@height, Publisher=@publisher, Total_Count=@total_count, Cuurent_Count=@cuurent_count WHERE ID = '{dialogEditBook.book.ID}'";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@id", dialogEditBook.book.ID);
                cmd.Parameters.AddWithValue("@title", dialogEditBook.book.Title);
                cmd.Parameters.AddWithValue("@author", dialogEditBook.book.Author);
                cmd.Parameters.AddWithValue("@genre", dialogEditBook.book.Genre);
                cmd.Parameters.AddWithValue("@subgenre", dialogEditBook.book.SubGenre);
                cmd.Parameters.AddWithValue("@height", dialogEditBook.book.Height);
                cmd.Parameters.AddWithValue("@publisher", dialogEditBook.book.Publisher);
                cmd.Parameters.AddWithValue("@total_count", dialogEditBook.book.Total_Count);
                cmd.Parameters.AddWithValue("@cuurent_count", dialogEditBook.book.Cuurent_Count);
                try
                {
                    cmd.ExecuteNonQuery();
                    foreach (var item in Books)
                    {
                        if (item.ID == dialogEditBook.book.ID)
                        {
                            Books.Remove(item);
                            break;
                        }
                    }
                    Books.Add(dialogEditBook.book);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Create Book error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        private void Button_book_return_Click(object sender, RoutedEventArgs e)
        {
            if (lv.SelectedItem != null && lv.SelectedItem is Entity.Book book && book.Cuurent_Count < book.Total_Count)
            {
                dialogReturn = new BookInteraction.ReturnBook() { Owner = this };
                if (dialogReturn.ShowDialog() == true)
                {

                    String sql = $"Update Users SET Id=@id, Name=@name, Surname=@surname, Email=@email, Book1=@book1, Book2=@book2, Book3=@book3 WHERE Id = '{dialogReturn.user.Id}'";
                    try
                    {
                        using SqlCommand cmd = new(sql, _connection);
                        cmd.Parameters.AddWithValue("@id", dialogReturn.user.Id);
                        cmd.Parameters.AddWithValue("@name", dialogReturn.user.Name);
                        cmd.Parameters.AddWithValue("@surname", dialogReturn.user.Surname);
                        cmd.Parameters.AddWithValue("@email", dialogReturn.user.Email);
                        cmd.Parameters.AddWithValue("@book1", dialogReturn.user.Book1);
                        cmd.Parameters.AddWithValue("@book2", dialogReturn.user.Book2);
                        cmd.Parameters.AddWithValue("@book3", dialogReturn.user.Book3);
                        cmd.ExecuteNonQuery();
                        foreach (var item in Users)
                        {
                            if (item.Id == dialogReturn.user.Id)
                            {
                                Users.Remove(item);
                                break;
                            }
                        }
                        Users.Add(dialogReturn.user);
                        cmd.Dispose();

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Update User error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    String sqlBook = $"Update Books SET Cuurent_Count ={book.Cuurent_Count} WHERE ID='{book.ID}'";
                    using SqlCommand cmdBook = new(sqlBook, _connection);
                    cmdBook.ExecuteNonQuery();
                    foreach (var item in Books)
                    {
                        if (item.ID == book.ID)
                        {
                            Books.Remove(item);
                            break;
                        }
                    }
                    Books.Add(book);
                    cmdBook.Dispose();

                }
                else
                {
                    MessageBox.Show("Operation Cancelled");
                }
            }
        }

        private void Button_book_give_Click(object sender, RoutedEventArgs e)
        {
            if (lv.SelectedItem != null && lv.SelectedItem is Entity.Book book && book.Cuurent_Count > 0)
            {
                dialogGive = new BookInteraction.GiveBook() { Owner = this };
                if (dialogGive.ShowDialog() == true)
                {

                    String sql = $"Update Users SET Book1=@book1, Book2=@book2, Book3=@book3 WHERE Id = '{dialogGive.user.Id}'";
                    try
                    {
                        using SqlCommand cmd = new(sql, _connection);
                        cmd.Parameters.AddWithValue("@book1", dialogGive.user.Book1);
                        cmd.Parameters.AddWithValue("@book2", dialogGive.user.Book2);
                        cmd.Parameters.AddWithValue("@book3", dialogGive.user.Book3);
                        cmd.ExecuteNonQuery();
                        foreach (var item in Users)
                        {
                            if (item.Id == dialogGive.user.Id)
                            {
                                Users.Remove(item);
                                break;
                            }
                        }
                        Users.Add(dialogGive.user);
                        cmd.Dispose();

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Update User error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    String sqlBook = $"Update Books SET Cuurent_Count ={book.Cuurent_Count} WHERE ID='{book.ID}'";
                    using SqlCommand cmdBook = new(sqlBook, _connection);
                    cmdBook.ExecuteNonQuery();
                    foreach (var item in Books)
                    {
                        if (item.ID == book.ID)
                        {
                            Books.Remove(item);
                            break;
                        }
                    }
                    Books.Add(book);
                    cmdBook.Dispose();

                }
                else
                {
                    MessageBox.Show("Operation Cancelled");
                }
            }
        }

        private void Button_user_create_Click(object sender, RoutedEventArgs e)
        {
            dialogAddUser = new BookInteraction.AddUser() { Owner = this };
            if (dialogAddUser.ShowDialog() == true)
            {
                String sql = "INSERT INTO Users(Id,Name,Surname,Email,Book1,Book2,Book3) VALUES (@id, @name, @surname, @email, @book1, @book2, @book3)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@id", dialogAddUser.user.Id);
                cmd.Parameters.AddWithValue("@name", dialogAddUser.user.Name);
                cmd.Parameters.AddWithValue("@surname", dialogAddUser.user.Surname);
                cmd.Parameters.AddWithValue("@email", dialogAddUser.user.Email);
                cmd.Parameters.AddWithValue("@book1", dialogAddUser.user.Book1);
                cmd.Parameters.AddWithValue("@book2", dialogAddUser.user.Book2);
                cmd.Parameters.AddWithValue("@book3", dialogAddUser.user.Book3);
                try
                {
                    cmd.ExecuteNonQuery();
                    Users.Add(dialogAddUser.user);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Create User error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
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
            if (this.IsLoaded)
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
