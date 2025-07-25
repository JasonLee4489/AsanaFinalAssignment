using Api.ToDoApplication.Persistence;
using Asana.Library.Models;

namespace Asana.API.Enterprise
{
    public class ProjectEC
    {
        public IEnumerable<Project>? Get(bool Expand = false)
        {
            return Filebase.Current.GetProjects(Expand)?.Take(100);
        }

        public Project? GetById(int id)
        {
            return Filebase.Current.GetProjects(true)?.FirstOrDefault(p => p.Id == id);
        }

        public Project? AddOrUpdate(Project? project)
        {
            if(project == null)
            {
                return project;
            }

            Filebase.Current.AddOrUpdateProject(project);
            return project;
        }

        public Project? Delete(int id)
        {
            var projectToDelete = GetById(id);
            if (projectToDelete != null)
            {
                Filebase.Current.DeleteProject(projectToDelete);
            }
            return projectToDelete;
        }
    }
}
