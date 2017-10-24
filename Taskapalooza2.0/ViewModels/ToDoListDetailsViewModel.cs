using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskapaloozaBeta.Models;

namespace TaskapaloozaBeta.ViewModels
{
    public class ToDoListDetailsViewModel
    {

        public List<ToDo> AssignedToDos { get; set; }

        public ToDoList CurrentList { get; set; }


        public ToDoListDetailsViewModel() { }


        public ToDoListDetailsViewModel(ToDoList currrentList, List<ToDo> assignedToDos)
        {
            AssignedToDos = assignedToDos;

            CurrentList = currrentList;


        }

   



    }
}
