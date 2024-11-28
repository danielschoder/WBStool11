using Microsoft.Win32;
using System.ComponentModel;
using WBStool11.Helpers;
using WBStool11.Models;

namespace WBStool11.Services;

public interface IProjectService
{
    Task CreateNewProjectAsync();
    Task OpenProjectAsync();
    Task SaveProjectAsync();
    Task SaveAsProjectAsync();
}

public class ProjectService(IFileService fileService)
    : ObservableObject, IProjectService, INotifyPropertyChanged
{
    private readonly IFileService _fileService = fileService;

    private Project _currentProject;
    private string _currentProjectFileName;
    private bool _hasPendingChanges;
    private bool HasCurrentProject => _currentProject is not null;
    private bool HasFileName => _currentProjectFileName is not null;

    public Project CurrentProject
    {
        get => _currentProject;
        private set
        {
            if (_currentProject != value)
            {
                if (_currentProject is not null)
                {
                    _currentProject.PropertyChanged -= OnCurrentProjectPropertyChanged;
                }
                _currentProject = value;
                if (_currentProject is not null)
                {
                    _currentProject.PropertyChanged += OnCurrentProjectPropertyChanged;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentProjectName));
            }
        }
    }

    public string CurrentProjectName => $"{CurrentProject?.Name ?? string.Empty}{(_hasPendingChanges ? "*" : string.Empty)}";
    private void OnCurrentProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        _hasPendingChanges = true;
        if (e.PropertyName == nameof(Project.Name))
        {
            OnPropertyChanged(nameof(CurrentProjectName));
        }
    }

    public async Task CreateNewProjectAsync()
    {
        if (_hasPendingChanges)
        {
            var cancel = true;
            if (cancel) { return; }
            var saveChanges = true;
            if (saveChanges)
            {
                await SaveProjectAsync();
            }
        }
        CurrentProject = new Project { Name = "New Project" };
        _hasPendingChanges = false;
    }

    public async Task OpenProjectAsync()
    {
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
            }
        }
    }

    public async Task SaveProjectAsync()
    {
        if (!HasCurrentProject) { return; }
        if (HasFileName)
        {
            await _fileService.SaveToFileAsync(_currentProjectFileName, CurrentProject);
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
                _hasPendingChanges = false;
            }
        }
    }
}
