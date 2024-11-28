using System.ComponentModel;

namespace WBStool11.Models;

public class Project : Element, INotifyPropertyChanged
{
    public Guid Id { get; set; }

    public static Project Create(string Name)
        => new()
        {
            Id = Guid.NewGuid(),
            _name = Name,
            Elements = [new Element { Name = "Phase 1" }, new Element { Name = "Phase 2" }]
        };
}
