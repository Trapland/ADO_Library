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
using System.Net.Mail;

namespace SmallAdoProject.ManagerInteraction
{
    /// <summary>
    /// Логика взаимодействия для AddManager.xaml
    /// </summary>
    public partial class AddManager : Window
    {
        public ObservableCollection<Entity.Manager> OwnerManagers { get; set; }
        public Entity.Manager manager { get; set; }
        public AddManager()
        {
            manager = new Entity.Manager();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is AdminOptions owner)
            {
                DataContext = Owner;
                OwnerManagers = owner.Managers;
            }
            else
            {
                MessageBox.Show("Owner is not AdminOptions");
                Close();
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            manager.Name = Box_Name.Text;
            manager.Surname = Box_Surname.Text;
            manager.Phone = Box_Phone.Text;
            manager.Email = Box_Email.Text;
            try
            {
                MailAddress m = new MailAddress(manager.Email);
            }
            catch (FormatException)
            {
                MessageBox.Show("Not validate email");
                return;
            }
            manager.Address = Box_Address.Text;
            manager.Login = Box_Login.Text;
            manager.Password = Box_Password.Text;
            foreach (var item in OwnerManagers)
            {
                if(item.Email == manager.Email)
                {
                    MessageBox.Show("This email is already used");
                    return;
                }
                if(item.Login == manager.Login)
                {
                    MessageBox.Show("This login is already used");
                    return;
                }
                if (manager.Password.Length < 8)
                {
                    MessageBox.Show("This password is weak");
                    return;
                }
            }
            this.DialogResult = true;
        }

    }
}
