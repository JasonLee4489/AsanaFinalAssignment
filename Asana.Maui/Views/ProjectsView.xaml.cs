using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

public partial class ProjectsView : ContentPage
{
	public ProjectsView()
	{
		InitializeComponent();
        BindingContext = new ProjectsPageViewModel();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ProjectsPageViewModel)?.RefreshPage();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ProjectDetails");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var projId = (BindingContext as ProjectsPageViewModel)?.SelectedProject?.Model?.Id ?? 0;
        Shell.Current.GoToAsync($"//ProjectDetails?projectId={projId}");
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectsPageViewModel)?.DeleteProject();
    }

    private void GoToToDoListClicked(object sender, EventArgs e)
    {
        var projId = (BindingContext as ProjectsPageViewModel)?.SelectedProject?.Model?.Id ?? 0;
        if (projId == 0) return;

        Shell.Current.GoToAsync($"//ProjectToDos?projectId={projId}");
    }
}