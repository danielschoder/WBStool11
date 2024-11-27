using System.Collections.ObjectModel;

namespace WBStool11.Models;

public class Element
{
    public Guid Id { get; set; }

    public string Description { get; set; }

    public ObservableCollection<Element> Elements { get; set; }
}
