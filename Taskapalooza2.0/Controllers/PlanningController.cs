using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskapaloozaBeta.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using TaskapaloozaBeta.ViewModels;

namespace TaskapaloozaBeta.Controllers
{
    public class PlanningController : Controller
    {

        List<ToDo> listOfToDos = ToDo.CreateListOfToDos();
        List<ToDoList> listOfToDoLists = ToDoList.CreateListOfToDoLists();

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult List()
        {
            return View(listOfToDoLists);
        }



        [HttpGet]
        public IActionResult Create(int id)
        {
            ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);

            using (IDbConnection connection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
            {
                using (connection)
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@ToDoListID", currentToDoList.ID);
                    var listOfAvailableToDos= (List<ToDo>)connection.Query<TaskapaloozaBeta.Models.ToDo>("EXEC GetListOfAvailableToDos @ToDoListID", new { ToDoListID = currentToDoList.ID });

                    CreatePlanViewModel model = new CreatePlanViewModel(currentToDoList, listOfAvailableToDos);

                    return View(model);
                }
            }
        }

        [HttpPost]
        public IActionResult Create(CreatePlanViewModel model)
        {
            try { 
                foreach (ToDo item in model.AssignedToDos)
            {

                ToDo currentToDo = listOfToDos.Single(t => t.ID == item.ID);
                currentToDo.STATUS = item.STATUS;

                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {

                    using (SqlCommand command = new SqlCommand("CreatePlan", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ToDoID", SqlDbType.Int);
                        command.Parameters.Add("@ToDoStatus", SqlDbType.NVarChar,12);
                        command.Parameters.Add("@ToDoClosed", SqlDbType.DateTime);
                        command.Parameters["@ToDoID"].Value = currentToDo.ID;
                        command.Parameters["@ToDoStatus"].Value = currentToDo.STATUS;
                        command.Parameters["@ToDoClosed"].Value = currentToDo.CLOSED;
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            return RedirectToAction("Index");
        }

            catch (SqlException ex)
            {
                string msg = "Error:";
                msg += ex.Message;
                throw new Exception(msg);

            }

        }
    }
}