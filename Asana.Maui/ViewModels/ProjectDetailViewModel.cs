using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class ProjectDetailViewModel
    {
        public ProjectDetailViewModel()
        {
            Model = new Project();
        }

        public ProjectDetailViewModel(int id)
        {
            Model = ProjectServiceProxy.Current.GetById(id) ?? new Project();
        }

        public ProjectDetailViewModel(Project? model)
        {
            Model = model ?? new Project();
        }

        public Project? Model { get; set; }

        public void AddOrUpdateProject()
        {
            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }
    }
}
