using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskapaloozaBeta.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskapaloozaBeta.ViewModels
{
    public class AddToDosToListViewModel
    {

        private List<ToDo> ListOfToDos;

        public ToDoList CurrentList { get; set; }

        public int CurrentListID { get; set; }

        [Required]
        public string selectedToDoID { get; set; }

        public List<SelectListItem> AvailableToDos { get; set; }
    

        public AddToDosToListViewModel() { }
        

        public AddToDosToListViewModel(ToDoList list, List<ToDo> listOfToDos)
        {
            CurrentList = list;

            ListOfToDos = listOfToDos;

            AvailableToDos = new List<SelectListItem>();

            foreach (ToDo item in listOfToDos)
            {
                AvailableToDos.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.NAME.ToString()
                });

            }
        }
    }
}
