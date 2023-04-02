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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace SmallAdoProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminPanelLogin : Window
    {
        public AdminPanelLogin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (password_input.Password == "admin" && login_input.Text == "admin")
            {
                this.Hide();
                new AdminOptions().ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Access denied");
            }
        }
    }
}
