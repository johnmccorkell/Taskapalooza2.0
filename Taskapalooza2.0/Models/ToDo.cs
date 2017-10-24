using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;


namespace TaskapaloozaBeta.Models
{
    public class ToDo
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string NOTES { get; set; }
        public string STATUS { get; set; }
        public DateTime CREATED { get; set; }
        public DateTime CLOSED { get; set; }

        public static List<ToDo> CreateListOfToDos()
        {

            using (IDbConnection connection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
            {
                using (connection)
                {
                    string sql = "EXEC GetToDos";

                    var listOfTodDos = (List<ToDo>)connection.Query<ToDo>(sql);

                    return listOfTodDos;
                }
            }

        }
    }
}
