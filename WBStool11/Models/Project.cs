﻿using System.Collections.ObjectModel;

namespace WBStool11.Models;

public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ObservableCollection<Element> Elements { get; set; }
}
