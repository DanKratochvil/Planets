using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.Models;

namespace Planets.ViewModels;

/// <summary>
/// VM for Planet to notify when Name changed
/// </summary>
public class PlanetViewModel : BaseViewModel
{  
    public Planet Planet { get; }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    public ICollection<PlanetProperty> PlanetProperties { get; set; } = new List<PlanetProperty>();

    public PlanetViewModel(Planet planet) 
    {
        Planet = planet ?? throw new ArgumentNullException(nameof(planet));
        Name = planet.Name;
        PlanetProperties = planet.PlanetProperties ?? new List<PlanetProperty>();
    }
}
