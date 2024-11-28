using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WBStool11.Models;

public class Project : Element, INotifyPropertyChanged
{
    public Guid Id { get; set; }

    private bool _areChangesPending;
    [JsonIgnore]
    public bool AreChangesPending
    {
        get => _areChangesPending;
        set
        {
            if (_areChangesPending != value)
            {
                _areChangesPending = value;
                OnPropertyChanged();
            }
        }
    }

    public static Project Create()
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            _name = "New Project",
        };
        project
            .AddNewLastChild(Create("Phase 1"))
            .AddNewLastChild(Create("Phase 2"))
            .AddNewLastChild(Create("Phase 3"));
        return project;
    }

    protected override void SetAreChangesPending()
        => AreChangesPending = true;
}
