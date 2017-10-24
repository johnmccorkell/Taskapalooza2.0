using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TaskapaloozaBeta.Models
{
    public class ToDoList
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public DateTime CREATED { get; set; }
        public DateTime CLOSED { get; set; }


        public static List<ToDoList> CreateListOfToDoLists()
        {

            using (IDbConnection connection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
            {
                using (connection)
                {
                    string sql = "EXEC GetToDoLists";

                    var listOfToDoLists = (List<ToDoList>)connection.Query<ToDoList>(sql);

                    return listOfToDoLists;
                }
            }

        }
    }
}
