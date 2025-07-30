using Planets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Models
{
    public class Property : BaseViewModel
    {
        public int Id { get; set; }

        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ICollection<PlanetProperty> PlanetProperties { get; set; } = new List<PlanetProperty>();
    }
}
