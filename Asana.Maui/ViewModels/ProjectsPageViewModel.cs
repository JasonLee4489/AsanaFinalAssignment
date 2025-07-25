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

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectViewModel> Projects
        {
            get
            {
                return new ObservableCollection<ProjectViewModel>(
                    ProjectServiceProxy.Current.Projects.Select(p => new ProjectViewModel(p)));
            }
        }

        private ProjectViewModel? selectedProject;
        public ProjectViewModel? SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void DeleteProject()
        {
            if (SelectedProject == null) return;

            ProjectServiceProxy.Current.DeleteProject(SelectedProject.Model?.Id ?? 0);
            NotifyPropertyChanged(nameof(Projects));
        }

        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
