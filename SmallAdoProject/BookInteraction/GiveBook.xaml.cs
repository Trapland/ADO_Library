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
    /// Логика взаимодействия для GiveBook.xaml
    /// </summary>
    public partial class GiveBook : Window
    {
        public ObservableCollection<Entity.User> OwnerUsers { get; set; }
        public ObservableCollection<Entity.Book> Books { get; set; }
        public Entity.User user { get; set; }
        public GiveBook()
        {
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

        private void Button_Give_Click(object sender, RoutedEventArgs e)
        {
            if (user.Book1 == "")
            {
                if (Owner is BooksMenuWindow owner)
                {
                    if (owner.lv.SelectedItem is Entity.Book book)
                    {
                        user.Book1 = book.Title;
                        book.Cuurent_Count--;
                    }
                }
            }
            else if (user.Book2 == "")
            {
                if (Owner is BooksMenuWindow owner)
                {
                    if (owner.lv.SelectedItem is Entity.Book book)
                    {
                        user.Book2 = book.Title;
                        book.Cuurent_Count--;
                    }
                }
            }
            else if (user.Book3 == "")
            {
                if (Owner is BooksMenuWindow owner)
                {
                    if (owner.lv.SelectedItem is Entity.Book book)
                    {
                        user.Book3 = book.Title;
                        book.Cuurent_Count--;
                    }
                }
            }
            else
            {
                MessageBox.Show("This user already has 3 books");
                return;
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
    }
}
