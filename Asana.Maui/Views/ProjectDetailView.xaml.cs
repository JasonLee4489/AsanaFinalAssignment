using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ProjectDetailView : ContentPage
{
    public ProjectDetailView()
    {
        InitializeComponent();
    }

    public int ProjectId { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ProjectPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectDetailViewModel)?.AddOrUpdateProject();
        Shell.Current.GoToAsync("//ProjectPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ProjectDetailViewModel(ProjectId);
    }
}
