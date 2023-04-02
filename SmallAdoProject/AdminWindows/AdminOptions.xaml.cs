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

namespace SmallAdoProject
{
    /// <summary>
    /// Логика взаимодействия для EditManagerWindow.xaml
    /// </summary>
    public partial class AdminOptions : Window
    {
        public ObservableCollection<Entity.Manager> Managers { get; set; }
        public ManagerInteraction.AddManager dialogAdd;
        public ManagerInteraction.EditManager dialogEdit;
        public ManagerInteraction.DeleteManager dialogDel;
        private SqlConnection _connection;
        public AdminOptions()
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
                using (var reader = cmd.ExecuteReader()) {
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

        private void Add_manager_Click(object sender, RoutedEventArgs e)
        {
            dialogAdd = new ManagerInteraction.AddManager() { Owner = this };
            if(dialogAdd.ShowDialog() == true)
            {
                String sql = "INSERT INTO Managers(Id,Name,Surname,Phone,Address,Email,Login,Password) VALUES (@id, @name, @surname, @phone, @address, @email, @login, @password)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@id", dialogAdd.manager.Id);
                cmd.Parameters.AddWithValue("@name", dialogAdd.manager.Name);
                cmd.Parameters.AddWithValue("@surname", dialogAdd.manager.Surname);
                cmd.Parameters.AddWithValue("@address", dialogAdd.manager.Address);
                cmd.Parameters.AddWithValue("@phone", dialogAdd.manager.Phone);
                cmd.Parameters.AddWithValue("@email", dialogAdd.manager.Email);
                cmd.Parameters.AddWithValue("@login", dialogAdd.manager.Login);
                cmd.Parameters.AddWithValue("@password", dialogAdd.manager.Password);
                try
                {
                    cmd.ExecuteNonQuery();
                    Managers.Add(dialogAdd.manager);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        private void Edit_manager_Click(object sender, RoutedEventArgs e)
        {
            dialogEdit = new ManagerInteraction.EditManager() { Owner = this };
            if (dialogEdit.ShowDialog() == true)
            {
                SqlCommand cmd = new() { Connection = _connection };
                try
                {
                    cmd.CommandText = $"Update Managers SET Name = N'{dialogEdit.manager.Name}', Surname = N'{dialogEdit.manager.Surname}', Phone = N'{dialogEdit.manager.Phone}', Address = '{dialogEdit.manager.Address}', Email = '{dialogEdit.manager.Email}', Login = '{dialogEdit.manager.Login}', Password = '{dialogEdit.manager.Password}' WHERE Id = '{dialogEdit.manager.Id}'";
                    cmd.ExecuteNonQuery();
                    foreach(var item in Managers)
                    {
                        if(item.Id == dialogEdit.manager.Id)
                        {
                            Managers.Remove(item);
                            break;
                        }
                    }
                    Managers.Add(dialogEdit.manager);
                    cmd.Dispose();
                    MessageBox.Show(dialogEdit.manager.ToString());
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        private void Delete_manager_Click(object sender, RoutedEventArgs e)
        {
            if(Managers.Count== 0)
            {
                MessageBox.Show("Managers list is empty");
                return;
            }    
            dialogDel = new ManagerInteraction.DeleteManager() { Owner = this};
            if (dialogDel.ShowDialog() == true)
            {
                SqlCommand cmd = new() { Connection = _connection };
                try
                {

                    cmd.CommandText = $"Update Managers SET DeleteDt = SYSDATETIME() WHERE Id = '{dialogDel.manager.Id}'";
                    Managers.Remove(dialogDel.manager);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Delete error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }


    }
}
