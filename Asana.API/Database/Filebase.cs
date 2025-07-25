using Asana.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Api.ToDoApplication.Persistence
{
    public class Filebase
    {
        private static Filebase? _instance;
        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }
                return _instance;
            }
        }

        private string _root;
        private string _toDoRoot;
        private string _projectRoot;        
        
        private Filebase()
        {
            _root = @"C:\temp";
            _toDoRoot = $"{_root}\\ToDos";
            _projectRoot = $"{_root}\\Projects";

            Directory.CreateDirectory(_root);
            Directory.CreateDirectory(_toDoRoot);
            Directory.CreateDirectory(_projectRoot);

            AddOrUpdateProject(new Project { Id = 1, Name = "Project 1", Description = "My Project 1" });
            AddOrUpdateProject(new Project { Id = 2, Name = "Project 2", Description = "My Project 2" });
            AddOrUpdateProject(new Project { Id = 3, Name = "Project 3", Description = "My Project 3" });
            AddOrUpdateProject(new Project { Id = 4, Name = "Project 4", Description = "My Project 4" });
            AddOrUpdateProject(new Project { Id = 5, Name = "Project 5", Description = "My Project 5" });
            AddOrUpdateToDo(new ToDo { Id = 1, Name = "Task 1", Description = "My Task 1", IsCompleted = true, ProjectId = 1, Priority = 0 });
            AddOrUpdateToDo(new ToDo { Id = 2, Name = "Task 2", Description = "My Task 2", IsCompleted = false, ProjectId = 1, Priority = 0 });
            AddOrUpdateToDo(new ToDo { Id = 3, Name = "Task 3", Description = "My Task 3", IsCompleted = true, ProjectId = 1, Priority = 0 });
            AddOrUpdateToDo(new ToDo { Id = 4, Name = "Task 4", Description = "My Task 4", IsCompleted = false, ProjectId = 2, Priority = 0 });
            AddOrUpdateToDo(new ToDo { Id = 5, Name = "Task 5", Description = "My Task 5", IsCompleted = true, ProjectId = 3, Priority = 0 });
        }

        public int LastToDoKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public int LastProjectKey
        {
            get
            {
                if (Projects.Any())
                {
                    return Projects.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        private void UpdateCompletePercent(Project proj)
        {
            var todos = ToDos.Where(t => t.ProjectId == proj.Id).ToList();

            if (todos.Count == 0)
            {
                proj.CompletePercent = 0;
            }
            else
            {
                proj.CompletePercent = (double)todos.Count(t => t.IsCompleted ?? false) / todos.Count * 100;
            }
        }

        public List<ToDo> ToDos
        {
            get
            {
                var root = new DirectoryInfo(_toDoRoot);
                var _toDos = new List<ToDo>();

                foreach (var toDoFile in root.GetFiles())
                {
                    var toDo = JsonConvert.DeserializeObject<ToDo>(File.ReadAllText(toDoFile.FullName));

                    if (toDo != null)
                    {
                        _toDos.Add(toDo);
                    }
                }

                return _toDos;
            }
        }

        public ToDo? AddOrUpdateToDo(ToDo? toDoToAdd)
        {
            if (toDoToAdd == null)
            {
                return toDoToAdd;
            }

            if (toDoToAdd.Id <= 0)
            {
                toDoToAdd.Id = LastToDoKey + 1;
            }

            string path = $"{_toDoRoot}\\{toDoToAdd.Id}.json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(toDoToAdd));

            if (toDoToAdd.ProjectId > 0)
            {
                var project = Projects.FirstOrDefault(p => p.Id == toDoToAdd.ProjectId);

                if (project != null)
                {
                    UpdateCompletePercent(project);

                    string projPath = $"{_projectRoot}\\{project.Id}.json";

                    if (File.Exists(projPath))
                    {
                        File.Delete(projPath);
                    }

                    File.WriteAllText(projPath, JsonConvert.SerializeObject(project));
                }
            }

            return toDoToAdd;
        }

        public ToDo? DeleteToDo(ToDo? toDoToDelete)
        {
            if (toDoToDelete == null)
            {
                return null;
            }

            string path = $"{_toDoRoot}\\{toDoToDelete.Id}.json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (toDoToDelete.ProjectId > 0)
            {
                var project = Projects.FirstOrDefault(p => p.Id == toDoToDelete.ProjectId);

                if (project != null)
                {
                    UpdateCompletePercent(project);

                    string projPath = $"{_projectRoot}\\{project.Id}.json";

                    if (File.Exists(projPath))
                    {
                        File.Delete(projPath);
                    }

                    File.WriteAllText(projPath, JsonConvert.SerializeObject(project));
                }
            }

            return toDoToDelete;
        }

        public List<Project> Projects
        {
            get
            {
                var root = new DirectoryInfo(_projectRoot);
                var _projects = new List<Project>();

                foreach (var projectFile in root.GetFiles())
                {
                    var proj = JsonConvert.DeserializeObject<Project>(File.ReadAllText(projectFile.FullName));

                    if (proj != null)
                    {
                        _projects.Add(proj);
                    }
                }

                return _projects;
            }
        }

        public List<Project>? GetProjects(bool Expand = false)
        {
            if (Expand)
            {
                var projectList = new List<Project>();

                foreach (var project in Projects)
                {
                    var proj = project;
                    proj.ToDoList = ToDos.Where(t => t.ProjectId == proj.Id).ToList();
                    UpdateCompletePercent(proj);
                    projectList.Add(proj);
                }

                return projectList;
            }

            return Projects;
        }

        public Project? AddOrUpdateProject(Project? projectToAdd)
        {
            if (projectToAdd == null)
            {
                return projectToAdd;
            }

            if (projectToAdd.Id <= 0)
            {
                projectToAdd.Id = LastProjectKey + 1;
            }

            UpdateCompletePercent(projectToAdd);

            string path = $"{_projectRoot}\\{projectToAdd.Id}.json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(projectToAdd));

            return projectToAdd;
        }

        public Project? DeleteProject(Project? projectToDelete)
        {
            if (projectToDelete == null)
            {
                return null;
            }

            string path = $"{_projectRoot}\\{projectToDelete.Id}.json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            foreach (var todo in ToDos.Where(t => t.ProjectId == projectToDelete.Id).ToList())
            {
                DeleteToDo(todo);
            }

            return projectToDelete;
        }
    }
}
