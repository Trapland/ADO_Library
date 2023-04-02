using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallAdoProject.Entity
{
    public class Manager
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public String Email { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public DateTime? DeleteDt { get; set; }
        public Manager() 
        { 
            Id = Guid.NewGuid();
            Name = null!;
            Surname = null!;
            Phone = null!;
            Address = null!;
            Email = null!;
        }
        public Manager(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Surname = reader.GetString("Surname");
            Name = reader.GetString("Name");
            Phone = reader.GetString("Phone");
            Address = reader.GetString("Address");
            Email = reader.GetString("Email");
            Login = reader.GetString("Login");
            Password = reader.GetString("Password");
            DeleteDt = reader.IsDBNull("DeleteDt") ? null : reader.GetDateTime("DeleteDt");
        }
        public override string ToString()
        {
            return Surname + " " + Name + " " + Email;
        }
    }
}
