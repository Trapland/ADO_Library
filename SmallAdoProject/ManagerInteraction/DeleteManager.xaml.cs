using SmallAdoProject.Entity;
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

namespace SmallAdoProject.ManagerInteraction
{
    /// <summary>
    /// Логика взаимодействия для DeleteManager.xaml
    /// </summary>
    
    public partial class DeleteManager : Window
    {
        public ObservableCollection<Entity.Manager> OwnerManagers { get; set; }
        public Entity.Manager manager { get; set; }
        public DeleteManager()
        {
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

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Combo_Managers.SelectedItem is Entity.Manager man)
            {
                manager = man;
            }
            this.DialogResult = true;
        }
    }
}
