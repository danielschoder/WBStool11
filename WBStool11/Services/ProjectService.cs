using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using WBStool11.Models;

namespace WBStool11.Services;

public interface IProjectService
{
    Task CreateNewProjectAsync();
    Task OpenProjectAsync();
    Task SaveProjectAsync();
    Task SaveAsProjectAsync();
    void AddSubElement(Element element);
    void AddNextElement(Element element);
    Project CurrentProject { get; set; }
    ObservableCollection<Project> CurrentProjectAsCollection { get; }
}

public class ProjectService(IFileService fileService) : IProjectService
{
    private readonly IFileService _fileService = fileService;
    private string _currentProjectFileName;
    private bool HasFileName => _currentProjectFileName is not null;
    private bool HasCurrentProject => CurrentProjectAsCollection.Count > 0;

    public ObservableCollection<Project> CurrentProjectAsCollection { get; } = [null];

    public Project CurrentProject
    {
        get => CurrentProjectAsCollection.Count > 0 ? CurrentProjectAsCollection[0] : null;
        set => CurrentProjectAsCollection[0] = value;
    }

    public async Task CreateNewProjectAsync()
    {
        if (await CancelUnsavedChanges()) { return; }
        CurrentProject = Project.Create();
        CurrentProject.AreChangesPending = false;
    }

    public void AddSubElement(Element element)
    {
        element.AddNewLastChild(Element.Create("New"));
        CurrentProject.AreChangesPending = true;
    }

    public void AddNextElement(Element element)
    {
        element.AddNewLastSibling(Element.Create("New"));
        CurrentProject.AreChangesPending = true;
    }

    public async Task OpenProjectAsync()
    {
        if (await CancelUnsavedChanges()) { return; }
        var openFileDialog = new OpenFileDialog
        {
            Title = "Open WBStool Project File",
            Filter = "WBStool Files (*.wbs)|*.wbs"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            _currentProjectFileName = openFileDialog.FileName;
            if (!string.IsNullOrEmpty(_currentProjectFileName))
            {
                CurrentProject = await _fileService.ReadFromFileAsync<Project>(_currentProjectFileName);
                CurrentProject?.AssignParentReferences();
            }
        }
    }

    public async Task SaveProjectAsync()
    {
        if (!HasCurrentProject) { return; }
        if (HasFileName)
        {
            await _fileService.SaveToFileAsync(_currentProjectFileName, CurrentProject);
            CurrentProject.AreChangesPending = false;
        }
        else
        {
            await SaveAsProjectAsync();
        }
    }

    public async Task SaveAsProjectAsync()
    {
        if (!HasCurrentProject) { return; }
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save WBStool Project As",
            Filter = "WBStool Files (*.wbs)|*.wbs",
            DefaultExt = ".wbs",
            FileName = HasFileName
                ? _currentProjectFileName
                : $"{Environment.CurrentDirectory}/{CurrentProject.Name}"
        };

        var result = saveFileDialog.ShowDialog();
        if (result == true)
        {
            _currentProjectFileName = saveFileDialog.FileName;
            if (!string.IsNullOrEmpty(_currentProjectFileName))
            {
                await _fileService.SaveToFileAsync(_currentProjectFileName, CurrentProject);
                CurrentProject.AreChangesPending = false;
            }
        }
    }

    private async Task<bool> CancelUnsavedChanges()
    {
        if (CurrentProject is null || !CurrentProject.AreChangesPending) { return false; }
        var confirmation = MessageBox.Show(
            "You have unsaved changes. Do you want to save before continuing?",
            "Unsaved Changes",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Warning);
        if (confirmation == MessageBoxResult.Yes)
        {
            await SaveProjectAsync();
        }
        return confirmation == MessageBoxResult.Cancel;
    }
}
