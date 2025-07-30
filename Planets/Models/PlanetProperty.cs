using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planets.ViewModels;

namespace Planets.Models
{
    public class PlanetProperty : BaseViewModel
    {
        public int Id { get; set; }

        public int PlanetId { get; set; }
        public Planet? Planet { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }

        private string? _value;
        public string? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
