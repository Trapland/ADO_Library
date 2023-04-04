using SmallAdoProject.Entity;
using SmallAdoProject.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public ObservableCollection<Entity.User> OwnerUsers { get; set; }
        public Entity.User user { get; set; }
        public AddUser()
        {
            user = new();
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is BooksMenuWindow owner)
            {
                DataContext = Owner;
                OwnerUsers = owner.Users;
            }
            else
            {
                MessageBox.Show("Owner is not BooksMenuWindow");
                Close();
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            user.Name = Box_Name.Text;
            user.Surname = Box_Surname.Text;
            user.Email = Box_Email.Text;
            try
            {
                MailAddress m = new MailAddress(user.Email);
            }
            catch (FormatException)
            {
                MessageBox.Show("Not validate email");
                return;
            }

            foreach (var item in OwnerUsers)
            {
                if (item.Email == user.Email)
                {
                    MessageBox.Show("This Email is already used");
                    return;
                }
            }
            this.DialogResult = true;
        }
    }
}
