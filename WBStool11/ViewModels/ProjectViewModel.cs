using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WBStool11.Helpers;
using WBStool11.Models;
using WBStool11.Services;

namespace WBStool11.ViewModels;

public class ProjectViewModel : ObservableObject, INotifyPropertyChanged
{
    public IProjectService ProjectService { get; }

    public ICommand NewProjectCommand { get; }

    public ICommand OpenProjectCommand { get; }

    public ICommand SaveProjectCommand { get; }

    public ICommand SaveAsProjectCommand { get; }

    public ICommand AddSubCommand { get; }

    public ICommand AddNextCommand { get; }

    public ObservableCollection<Project> CurrentProject => ProjectService.CurrentProjectAsCollection;

    public string CurrentProjectName
        => $"{ProjectService.CurrentProject?.Name ?? string.Empty}" +
            $"{(ProjectService.CurrentProject is not null
                && ProjectService.CurrentProject.AreChangesPending ? "*" : string.Empty)}";

    private Element _selectedElement;
    public Element SelectedElement
    {
        get => _selectedElement;
        set
        {
            if (_selectedElement != value)
            {
                if (_selectedElement is not null)
                {
                    _selectedElement.PropertyChanged -= OnProjectPropertyChanged;
                }
                _selectedElement = value;
                if (_selectedElement is not null)
                {
                    _selectedElement.PropertyChanged += OnProjectPropertyChanged;
                }
                OnPropertyChanged();
            }
        }
    }

    public ProjectViewModel(IProjectService projectFileService)
    {
        ProjectService = projectFileService;

        NewProjectCommand = new AsyncRelayCommand(CreateNewProjectAsync);
        OpenProjectCommand = new AsyncRelayCommand(OpenProjectAsync);
        SaveProjectCommand = new AsyncRelayCommand(SaveProjectAsync);
        SaveAsProjectCommand = new AsyncRelayCommand(SaveAsProjectAsync);
        AddSubCommand = new RelayCommand(AddSubElement);
        AddNextCommand = new RelayCommand(AddNextElement);
    }

    private async Task CreateNewProjectAsync()
    {
        if (ProjectService.CurrentProject is not null)
        {
            ProjectService.CurrentProject.PropertyChanged -= OnProjectPropertyChanged;
        }
        await ProjectService.CreateNewProjectAsync();
        OnPropertyChanged(nameof(CurrentProjectName));
        ProjectService.CurrentProject.PropertyChanged += OnProjectPropertyChanged;
    }

    private async Task OpenProjectAsync()
    {
        if (ProjectService.CurrentProject is not null)
        {
            ProjectService.CurrentProject.PropertyChanged -= OnProjectPropertyChanged;
        }
        await ProjectService.OpenProjectAsync();
        OnPropertyChanged(nameof(CurrentProjectName));
        ProjectService.CurrentProject.PropertyChanged += OnProjectPropertyChanged;
    }

    private async Task SaveProjectAsync()
    {
        await ProjectService.SaveProjectAsync();
        OnPropertyChanged(nameof(CurrentProjectName));
    }

    private async Task SaveAsProjectAsync()
    {
        await ProjectService.SaveAsProjectAsync();
        OnPropertyChanged(nameof(CurrentProjectName));
    }

    private void AddSubElement()
    {
        ProjectService.AddSubElement(_selectedElement);
    }

    private void AddNextElement()
    {
        ProjectService.AddNextElement(_selectedElement);
    }

    private void OnProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        => OnPropertyChanged(nameof(CurrentProjectName));
}
