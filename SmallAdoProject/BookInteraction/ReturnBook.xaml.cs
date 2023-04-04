using SmallAdoProject.Entity;
using SmallAdoProject.Manager;
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
    /// Логика взаимодействия для ReturnBook.xaml
    /// </summary>
    public partial class ReturnBook : Window
    {
        public ObservableCollection<Entity.User> OwnerUsers { get; set; }
        public Entity.User user { get; set; }
        public ReturnBook()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is BooksMenuWindow owner)
            {
                DataContext = Owner;
                OwnerUsers = owner.Users;
                if (owner.lv.SelectedItem is Entity.Book book)
                {
                    var Users_ = OwnerUsers.Where(u => u.Book1 == book.Title);
                    var Users2_ = OwnerUsers.Where(u => u.Book2 == book.Title);
                    var Users3_ = OwnerUsers.Where(u => u.Book3 == book.Title);
                    var UsersTotal = Users3_.Concat(Users_.Concat(Users2_));
                    Combo_Users.ItemsSource = UsersTotal;
                }
            }
            else
            {
                MessageBox.Show("Owner is not BooksMenuWindow");
                Close();
            }
        }

        private void Button_Return_Click(object sender, RoutedEventArgs e)
        {
            if (Owner is BooksMenuWindow owner)
            {
                if (owner.lv.SelectedItem is Entity.Book book)
                {
                    if (user.Book1 == book.Title)
                    {
                        user.Book1 = "";
                        book.Cuurent_Count++;
                    }
                    else if (user.Book2 == book.Title)
                    {
                        user.Book2 = "";
                        book.Cuurent_Count++;
                    }
                    else if (user.Book3 == book.Title)
                    {
                        user.Book3 = "";
                        book.Cuurent_Count++;
                    }
                }
            }
            this.DialogResult = true;
        }

        private void Combo_Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo_Users.SelectedItem is Entity.User us)
            {
                user = us;
            }
            else
            {
                return;
            }
        }


        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


    }
}
