using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallAdoProject.Entity
{
    public class Book
    {
        public Guid ID { get; set; }
        public String? Title { get; set; }
        public String? Author { get; set; }
        public String? Genre { get; set; }
        public String? SubGenre { get; set; }
        public Int32 Height { get; set; }
        public String? Publisher { get; set; }
        public Int32 Total_Count { get; set; }
        public Int32 Cuurent_Count { get; set; }
        public Book()
        {
            ID = Guid.NewGuid();
            Title = "";
            Author = "";
            Genre = "";
            SubGenre = "";
            Height = 0;
            Publisher = "";
            Total_Count = 0;
            Cuurent_Count = 0;
        }
        public Book(SqlDataReader reader)
        {
            ID = reader.GetGuid("ID");
            Title = reader.IsDBNull("Title") ? "" : reader.GetString("Title");
            Author = reader.IsDBNull("Author") ? "" : reader.GetString("Author");
            Genre = reader.IsDBNull("Genre") ? "" : reader.GetString("Genre");
            SubGenre = reader.IsDBNull("SubGenre") ? "" : reader.GetString("SubGenre");
            Height = reader.GetInt32("Height"); ;
            Publisher = reader.IsDBNull("Publisher") ? "" : reader.GetString("Publisher");
            Total_Count = reader.GetInt32("Total_Count"); ;
            Cuurent_Count = reader.GetInt32("Cuurent_Count"); ;
        }
        public override string ToString()
        {
            return Title + " " + Author + " " + Genre;
        }
    }
}
