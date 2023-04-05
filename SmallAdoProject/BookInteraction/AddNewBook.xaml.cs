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
    /// Логика взаимодействия для AddNewBook.xaml
    /// </summary>
    public partial class AddNewBook : Window
    {
        public ObservableCollection<Entity.Book> OwnerBooks { get; set; }
        public Entity.Book book { get; set; }
        public AddNewBook()
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
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            book.Title = Box_Title.Text;
            book.Author = Box_Author.Text;
            book.Genre = Box_Genre.Text;
            book.SubGenre = Box_SubGenre.Text;
            book.Height = Convert.ToInt32(Box_Height.Text);
            book.Publisher = Box_Publisher.Text;
            book.Total_Count = Convert.ToInt32(Box_Total_Count.Text);
            if(book.Total_Count < 0)
                book.Total_Count = 1;
            book.Cuurent_Count = book.Total_Count;
            foreach (var item in OwnerBooks)
            {
                if (item.Title == book.Title)
                {
                    MessageBox.Show("This Title is already used");
                    return;
                }
            }
            this.DialogResult = true;
        }
    }
}
