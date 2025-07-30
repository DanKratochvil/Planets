using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Planets.Models;

public class Planet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<PlanetProperty> PlanetProperties { get; set; } = new List<PlanetProperty>();
}
