using System.Windows.Input;
using WBStool11.Helpers;
using WBStool11.Services;

namespace WBStool11.ViewModels;

public class ProjectViewModel
{
    public IProjectService ProjectService { get; }

    public ICommand NewProjectCommand { get; }

    public ICommand OpenProjectCommand { get; }

    public ICommand SaveProjectCommand { get; }

    public ICommand SaveAsProjectCommand { get; }

    public ProjectViewModel(IProjectService projectFileService)
    {
        ProjectService = projectFileService;

        NewProjectCommand = new AsyncRelayCommand(ProjectService.CreateNewProjectAsync);
        OpenProjectCommand = new AsyncRelayCommand(ProjectService.OpenProjectAsync);
        SaveProjectCommand = new AsyncRelayCommand(ProjectService.SaveProjectAsync);
        SaveAsProjectCommand = new AsyncRelayCommand(ProjectService.SaveAsProjectAsync);

        // await CreateNewProject();
    }
}
