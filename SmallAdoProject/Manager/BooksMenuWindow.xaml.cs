using Microsoft.Data.SqlClient;
using SmallAdoProject.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public BooksMenuWindow()
        {
            InitializeComponent();
            Books= new ();
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
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Button_book_add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }


    }
}
