using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace WBStool11.Models;

public class Element : INotifyPropertyChanged
{
    protected string _name;
    public string Name { get => _name; set => SetProperty(ref _name, value); }

    public Element _parent;
    [JsonIgnore]
    public Element Parent { get => _parent; set => SetProperty(ref _parent, value); }

    [JsonIgnore]
    public bool IsRoot => _parent is null;

    [JsonIgnore]
    public bool HasChildren => _elements is not null;

    private ObservableCollection<Element> _elements;
    public ObservableCollection<Element> Elements { get => _elements; set => SetProperty(ref _elements, value); }

    private bool _isExpanded;
    public bool IsExpanded { get => _isExpanded; set => SetProperty(ref _isExpanded, value); }

    public event PropertyChangedEventHandler PropertyChanged;

    public static Element Create(string name = "New")
        => new() { _name = name };

    public Element AddNewLastChild(Element newElement)
    {
        IsExpanded = true;
        newElement.Parent = this;
        Elements ??= [];
        Elements.Add(newElement);
        return this;
    }

    public Element AddNewSibling(Element newElement)
    {
        Parent.IsExpanded = true;
        newElement.Parent = Parent;
        var currentChildIndex = Parent.Elements.IndexOf(this);
        Parent.Elements.Insert(currentChildIndex + 1, newElement);
        return this;
    }

    public void AssignParentReferences()
    {
        if (!HasChildren) { return; }
        foreach (var child in Elements)
        {
            child._parent = this;
            child.AssignParentReferences();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) { return false; }
        field = value;
        if (PropertyChanged is null) { return false; }
        SetAreChangesPending();
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void SetAreChangesPending()
        => Parent?.SetAreChangesPending();
}
