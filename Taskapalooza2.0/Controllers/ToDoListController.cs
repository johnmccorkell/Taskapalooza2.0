using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using TaskapaloozaBeta.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskapaloozaBeta.ViewModels;
using Dapper;

namespace Taskapalooza2._0.Controllers
{
    public class ToDoListController : Controller
    {
        List<ToDo> listOfToDos = ToDo.CreateListOfToDos();
        List<ToDoList> listOfToDoLists = ToDoList.CreateListOfToDoLists();


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Details(int id)
        {

            ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);

            using (IDbConnection connection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
            {
                using (connection)
                {

                    DynamicParameters p = new DynamicParameters();
                    p.Add("@ListID", currentToDoList.ID);
                    var listOfAvailableToDos = (List<ToDo>)connection.Query<ToDo>("EXEC GetListOfAvailableToDos @ListID", new { ListID = currentToDoList.ID });

                    ToDoListDetailsViewModel toDoListDetailsViewModel = new ToDoListDetailsViewModel(currentToDoList, listOfAvailableToDos);

                    return View(toDoListDetailsViewModel);
                }

            }
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                ToDoList newToDoList = new ToDoList
                {
                    NAME = collection["NAME"],
                    CREATED = DateTime.Now
                };

                listOfToDoLists.Add(newToDoList);

                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {
                    using (SqlCommand command = new SqlCommand("CreateToDoList", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@NAME", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@CREATED", SqlDbType.DateTime);
                        command.Parameters["@NAME"].Value = newToDoList.NAME.ToString();
                        command.Parameters["@CREATED"].Value = newToDoList.CREATED;
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult AddToDosToList(int id)
        {

            ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);

            AddToDosToListViewModel addTodosToListViewModel = new AddToDosToListViewModel(currentToDoList, listOfToDos);

            return View(addTodosToListViewModel);
        }


        [HttpPost]
        public ActionResult AddToDosToList(AddToDosToListViewModel model)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {
                    using (SqlCommand command = new SqlCommand("AddToDoToList", sqlConnection))
                    {
                        command.Parameters.Add("@ToDoID", SqlDbType.Int);
                        command.Parameters.Add("@ListID", SqlDbType.Int);
                        command.Parameters["@ToDoID"].Value = Int32.Parse(model.selectedToDoID);
                        command.Parameters["@ListID"].Value = model.CurrentListID;
                        sqlConnection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }

            catch (SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
                throw new Exception(msg);

            }
        
            return RedirectToAction("AddTasksToList");


    }
        public ActionResult Delete(int id)
        {

            ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);

            return View(currentToDoList);
        }

        
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);

                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {

                    using (SqlCommand command = new SqlCommand("DeleteToDoList", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID", SqlDbType.Int);
                        command.Parameters["@ID"].Value = currentToDoList.ID;
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                listOfToDoLists.Remove(currentToDoList);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult List()
        {
            try
            {
                return View(listOfToDoLists);
            }

            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);


            return View(currentToDoList); 
         }         


        
        [HttpPost]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            ToDoList currentToDoList = listOfToDoLists.Single(t => t.ID == id);
            try
            {

                currentToDoList.NAME = collection["NAME"];

                 using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {
                    using (SqlCommand command = new SqlCommand("EditToDoList", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("ID", SqlDbType.Int);
                        command.Parameters.Add("NAME", SqlDbType.NVarChar,50);
                        command.Parameters["ID"].Value = currentToDoList.ID;
                        command.Parameters["NAME"].Value = collection["NAME"].ToString();
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                        return RedirectToAction("List");

                    }
                }
                    
            }

            catch
            {
                return View(currentToDoList.ID);
            }


        }





    }
}

