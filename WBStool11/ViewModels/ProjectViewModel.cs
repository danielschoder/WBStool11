using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WBStool11.Helpers;
using WBStool11.Models;
using WBStool11.Services;

namespace WBStool11.ViewModels;

public class ProjectViewModel : INotifyPropertyChanged
{
    private readonly IFileService _fileService;

    private Project _currentProject;

    public Project CurrentProject
    {
        get => _currentProject;
        private set
        {
            if (_currentProject != value)
            {
                _currentProject = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand NewProjectCommand { get; }
    public ICommand OpenProjectCommand { get; }
    public ICommand SaveProjectCommand { get; }

    public ProjectViewModel(IFileService fileService)
    {
        _fileService = fileService;

        NewProjectCommand = new RelayCommand(CreateNewProject);
        OpenProjectCommand = new RelayCommand(OpenProject);
        SaveProjectCommand = new RelayCommand(SaveProject);

        CreateNewProject();
    }

    private void CreateNewProject()
    {
        CurrentProject = new Project { Name = "New Project" };
    }

    private void SaveProject()
    {
        _fileService.SaveToFile("project.json", CurrentProject);
    }

    private void OpenProject()
    {
        CurrentProject = _fileService.ReadFromFile<Project>("project.json");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
