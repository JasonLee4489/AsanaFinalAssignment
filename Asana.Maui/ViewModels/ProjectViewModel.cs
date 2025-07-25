using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectViewModel
    {
        public Project? Model { get; set; }

        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                if (Model == null || Model.ToDoList == null)
                {
                    return new ObservableCollection<ToDoDetailViewModel>();
                }

                return new ObservableCollection<ToDoDetailViewModel>(
                    Model.ToDoList.Select(t => new ToDoDetailViewModel(t)));
            }
        }

        public ProjectViewModel()
        {
            Model = new Project();
        }
        public ProjectViewModel(Project? model)
        {
            Model = model;
        }
    }
}
