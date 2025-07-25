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
    public class ProjectToDosPageViewModel : INotifyPropertyChanged
    {
        private readonly int projectId;

        public ProjectToDosPageViewModel(int id)
        {
            projectId = id;
        }

        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                return new ObservableCollection<ToDoDetailViewModel>(
                    ToDoServiceProxy.Current.ToDos.Where(t => t.ProjectId == projectId).Select(t => new ToDoDetailViewModel(t)));
            }
        }

        private ToDoDetailViewModel? selectedToDo;
        public ToDoDetailViewModel? SelectedToDo
        {
            get => selectedToDo;
            set
            {
                if (selectedToDo != value)
                {
                    selectedToDo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void DeleteToDo()
        {
            if (SelectedToDo == null) return;

            ToDoServiceProxy.Current.DeleteToDo(SelectedToDo.Model?.Id ?? 0);
            NotifyPropertyChanged(nameof(ToDos));
        }

        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(ToDos));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
