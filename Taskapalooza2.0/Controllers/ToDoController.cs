using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;

using TaskapaloozaBeta.Models;

namespace Taskapalooza2._0.Controllers
{
    public class ToDoController : Controller
    {
        List<ToDo> listOfToDos = ToDo.CreateListOfToDos();

        public IActionResult Index()
        {
            return View();
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
                ToDo newToDo = new ToDo
                {
                    NAME = collection["NAME"],
                    NOTES = collection["NOTES"],
                    CREATED = DateTime.Now
                };

                listOfToDos.Add(newToDo);                

                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {

                    using (SqlCommand command = new SqlCommand("CreateToDo", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@NAME", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@NOTES", SqlDbType.NVarChar, -1);
                        command.Parameters.Add("@CREATED", SqlDbType.DateTime);
                        command.Parameters["@NAME"].Value = newToDo.NAME.ToString();
                        command.Parameters["@NOTES"].Value = newToDo.NOTES.ToString();
                        command.Parameters["@CREATED"].Value = newToDo.CREATED;
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


        public ActionResult Edit(int id)
        {
            ToDo currentToDo = listOfToDos.Single(t => t.ID == id);

            return View(currentToDo);
        }


        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                ToDo CurrentToDo = listOfToDos.Single(t => t.ID == id);
                CurrentToDo.NAME = collection["NAME"];
                CurrentToDo.NOTES = collection["NOTES"];

                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {

                    using (SqlCommand command = new SqlCommand("EditToDo", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID", SqlDbType.Int);
                        command.Parameters.Add("@NAME", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@NOTES", SqlDbType.NVarChar, -1);
                        command.Parameters["@ID"].Value = CurrentToDo.ID;
                        command.Parameters["@NAME"].Value = CurrentToDo.NAME.ToString();
                        command.Parameters["@NOTES"].Value = CurrentToDo.NOTES.ToString();
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


        public ActionResult Delete(int id)
        {
            ToDo currentToDo = listOfToDos.Single(t => t.ID == id);

            return View(currentToDo);
        }

        // POST: Task/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ToDo currentToDo = listOfToDos.Single(t => t.ID == id);

                using (SqlConnection sqlConnection = new SqlConnection("Data Source=5SSDHH2;Initial Catalog=JMProjectDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {

                    using (SqlCommand command = new SqlCommand("DeleteToDo", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID", SqlDbType.Int);
                        command.Parameters["@ID"].Value = currentToDo.ID;
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                listOfToDos.Remove(currentToDo);

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
                return View(listOfToDos);
            }

            catch
            {
                return View();
            }
        }
    }
}
