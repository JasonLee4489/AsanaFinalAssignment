using Asana.Library.Models;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ProjectToDosView : ContentPage
{
    public ProjectToDosView()
    {
        InitializeComponent();
    }

    public int ProjectId { get; set; }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ProjectToDosPageViewModel(ProjectId);
        (BindingContext as ProjectToDosPageViewModel)?.RefreshPage();
    }

    private void BackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ProjectPage");
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectToDosPageViewModel)?.DeleteToDo();
    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//ToDoDetails?projectId={ProjectId}");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var toDoId = (BindingContext as ProjectToDosPageViewModel)?.SelectedToDo?.Model?.Id ?? 0;
        if (toDoId == 0) return;

        Shell.Current.GoToAsync($"//ToDoDetails?toDoId={toDoId}&projectId={ProjectId}");
    }
}