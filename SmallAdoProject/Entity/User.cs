using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SmallAdoProject.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Book1 { get; set; }
        public string Book2 { get; set; }
        public string Book3 { get; set; }
        public User()
        {
            Id = Guid.NewGuid();
            Name = "";
            Surname = "";
            Email = "";
            Book1 = "";
            Book2 = "";
            Book3 = "";
        }
        public User(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.IsDBNull("Name") ? "" : reader.GetString("Name");
            Surname = reader.IsDBNull("Surname") ? "" : reader.GetString("Surname");
            Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email");
            Book1 = reader.IsDBNull("Book1") ? "" : reader.GetString("Book1");
            Book2 = reader.IsDBNull("Book2") ? "" : reader.GetString("Book2");
            Book3 = reader.IsDBNull("Book3") ? "" : reader.GetString("Book3");
        }
    }
}
