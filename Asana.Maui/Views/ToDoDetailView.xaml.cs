using Asana.Library.Models;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ToDoId), "toDoId")]
[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ToDoDetailView : ContentPage
{
	public ToDoDetailView()
	{
		InitializeComponent();
        
	}
    public int ToDoId { get; set; }
    public int ProjectId { get; set; }
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ToDoDetailViewModel)?.AddOrUpdateToDo();
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        ToDoDetailViewModel vm;

        if (ToDoId > 0)
        {
            vm = new ToDoDetailViewModel(ToDoId);
        }
        else
        {
            vm = new ToDoDetailViewModel();
            if (ProjectId != 0)
            {
                vm.Model.ProjectId = ProjectId;
            }
        }

        BindingContext = vm;
    }
}