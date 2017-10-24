using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskapaloozaBeta.Models;

namespace TaskapaloozaBeta.ViewModels
{
    public class CreatePlanViewModel
    {
        public List<ToDo> AssignedToDos { get; set; }
        public ToDoList CurrentList { get; set; }
        public int CurrentListID { get; set; }
        public List<SelectListItem> StatusEnum { get; set; }

        public CreatePlanViewModel() {  }      

        public CreatePlanViewModel(ToDoList currentList, List<ToDo> assignedToDos)
        {

            this.AssignedToDos = assignedToDos;

            this.CurrentList = currentList;

            StatusEnum = new List<SelectListItem>();

            StatusEnum.Add(new SelectListItem()
            {
                Value=Status.Do.ToString(),
                Text=Status.Do.ToString()
            });
            StatusEnum.Add(new SelectListItem()
            {
                Value = Status.Delegate.ToString(),
                Text = Status.Delegate.ToString()
            });
            StatusEnum.Add(new SelectListItem()
            {
                Value = Status.Delay.ToString(),
                Text = Status.Delay.ToString()
            });
            StatusEnum.Add(new SelectListItem()
            {
                Value = Status.Delete.ToString(),
                Text = Status.Delete.ToString()
            });

        }
    }
}
