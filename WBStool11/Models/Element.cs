using System.Collections.ObjectModel;
using System.ComponentModel;
using WBStool11.Helpers;

namespace WBStool11.Models;

public class Element : ObservableObject, INotifyPropertyChanged
{
    protected string _name;
    public string Name { get => _name; set => SetProperty(ref _name, value); }

    public ObservableCollection<Element> Elements { get; set; }
}
