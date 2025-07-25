using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double CompletePercent { get; set; }

        public Project()
        {
            ToDoList = new List<ToDo>();
            CompletePercent = 0;
        }

        public List<ToDo> ToDoList { get; set; }
    }
}
