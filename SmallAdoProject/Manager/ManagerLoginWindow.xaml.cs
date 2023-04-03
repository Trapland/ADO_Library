using Microsoft.Data.SqlClient;
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

namespace SmallAdoProject.Manager
{
    /// <summary>
    /// Логика взаимодействия для ManagerLoginWindow.xaml
    /// </summary>
    public partial class ManagerLoginWindow : Window
    {
        public ObservableCollection<Entity.Manager> Managers { get; set; }
        private SqlConnection _connection;

        public ManagerLoginWindow()
        {
            InitializeComponent();
            Managers = new();
            DataContext = this;
            _connection = new(App.Connection);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT M.* FROM Managers M WHERE DeleteDt IS NULL";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Managers.Add(new Entity.Manager(reader));
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Managers)
            {
                if((item.Login == login_input.Text || item.Email == login_input.Text) && item.Password == password_input.Password)
                {
                    this.Hide();
                    new BooksMenuWindow().ShowDialog();
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("Incorrect Login/Email or Password");
        }


    }
}
