using Asana.Library.Models;
using Asana.Maui.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ProjectServiceProxy
    {
        private List<Project> projects;
        public List<Project> Projects
        {
            get
            {
                return projects;
            }
        }
        private ProjectServiceProxy() 
        {
            var projectData = new WebRequestHandler().Get("/Project/Expand").Result;
            projects = JsonConvert.DeserializeObject<List<Project>>(projectData) ?? new List<Project>();
        }
        private static object _lock = new object();
        private static ProjectServiceProxy? instance;
        public static ProjectServiceProxy Current
        {
            get
            {
                lock (_lock) {
                    if (instance == null)
                    {
                        instance = new ProjectServiceProxy();
                    }
                }

                return instance;
            }
        }

        public Project? AddOrUpdate(Project? project)
        {
            if (project == null)
            {
                return project;
            }

            var isNewProject = project.Id == 0;
            var projData = new WebRequestHandler().Post("/Project", project).Result;

            var newProj = JsonConvert.DeserializeObject<Project>(projData);

            if (newProj != null)
            {
                if (!isNewProject)
                {
                    var existing = projects.FirstOrDefault(p => p.Id == newProj.Id);
                    if (existing != null)
                    {
                        var index = projects.IndexOf(existing);
                        projects.RemoveAt(index);
                        projects.Insert(index, newProj);
                    }
                }
                else
                {
                    projects.Add(newProj);
                }
            }

            return newProj;
        }

        public Project? GetById(int id)
        {
            return Projects.FirstOrDefault(p => p.Id == id);
        }


        public void DeleteProject(int id)
        {
            if (id == 0)
            {
                return;
            }

            var projectData = new WebRequestHandler().Delete($"/Project/{id}").Result;

            var projectToDelete = JsonConvert.DeserializeObject<Project>(projectData);

            if (projectToDelete != null)
            {
                var localProject = projects.FirstOrDefault(p => p.Id == id);

                if (localProject != null)
                {
                    projects.Remove(localProject);
                }

                var orphanTodos = ToDoServiceProxy.Current.ToDos.Where(t => t.ProjectId == id).ToList();

                foreach (var todo in orphanTodos)
                {
                    ToDoServiceProxy.Current.ToDos.Remove(todo);
                }
            }
        }

        public void RecalculatePercent(int projectId)
        {
            if (projectId == 0)
            {
                return;
            }

            var proj = projects.FirstOrDefault(p => p.Id == projectId);

            if (proj == null)
            {
                return;
            }

            var todos = ToDoServiceProxy.Current.ToDos.Where(t => t.ProjectId == projectId).ToList();

            if (todos.Count == 0)
            {
                proj.CompletePercent = 0;
            }
            else
            {
                proj.CompletePercent = (double)todos.Count(t => t.IsCompleted ?? false) / todos.Count * 100;
            }
        }
    }
}
