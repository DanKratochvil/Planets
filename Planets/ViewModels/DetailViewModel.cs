using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Planets.Models;

namespace Planets.ViewModels
{
    /// <summary>
    /// class for Properties for selected Planet administration 
    /// </summary>
    public class DetailViewModel : BaseViewModel
    {
        private AppDbContext dbContext;
        public RelayCommand AddPropertyCommand { get; }
        public RelayCommand UpdatePropertyCommand { get; }
        public RelayCommand DeletePropertyCommand { get; }

        public DetailViewModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            AddPropertyCommand = new RelayCommand(AddProperty, CanAddProperty);
            UpdatePropertyCommand = new RelayCommand(UpdateProperty, CanUpdateProperty);
            DeletePropertyCommand = new RelayCommand(DeleteProperty, CanDeleteProperty);
        }

        private PlanetViewModel? _planet;
        public PlanetViewModel? Planet
        {
            get => _planet;
            set
            {
                if (_planet != value)
                {
                    _planet = value;
                    PlanetProperties = new ObservableCollection<PlanetProperty>(Planet?.PlanetProperties?.ToList() ?? new List<PlanetProperty>());
                    OnPropertyChanged(nameof(PlanetProperties));

                    AvailableProperties = GetAvailableNewPropertiesforPlanet;
                }
            }
        }

        public ObservableCollection<PlanetProperty> PlanetProperties { get; set; } = new ObservableCollection<PlanetProperty>();

        private List<Property> _availableProperties;
        public List<Property> AvailableProperties
        {
            get => _availableProperties;
            set => SetProperty(ref _availableProperties, value);
        }

        private List<Property> GetAvailableNewPropertiesforPlanet =>
            dbContext.Properties?.AsEnumerable()
            ?.Where(p => !PlanetProperties.Any(pp => pp.Property.Name == p.Name))
            .ToList() ?? new List<Property>();

        private PlanetProperty? _selectedPlanetProperty;
        public PlanetProperty? SelectedPlanetProperty
        {
            get => _selectedPlanetProperty;
            set
            {
                SetProperty(ref _selectedPlanetProperty, value);
                AvailableProperties = SelectedPlanetProperty != null ? dbContext.Properties.Where(p => p.Id == SelectedPlanetProperty.PropertyId).ToList() : dbContext.Properties.ToList();
                EditedPropertyName = AvailableProperties.FirstOrDefault();
                EditedPropertyValue = SelectedPlanetProperty?.Value ?? string.Empty;
                AddPropertyCommand?.RaiseCanExecuteChanged();
                UpdatePropertyCommand?.RaiseCanExecuteChanged();
                DeletePropertyCommand?.RaiseCanExecuteChanged();
            }
        }

        private Property? _editedPropertyName;
        public Property? EditedPropertyName
        {
            get => _editedPropertyName;
            set => SetProperty(ref _editedPropertyName, value);
        }

        private string _editedPropertyValue;
        public string EditedPropertyValue
        {
            get => _editedPropertyValue;
            set
            {
                value = value?.Replace("\"", "") ?? String.Empty;
                SetProperty(ref _editedPropertyValue, value);
            }
        }

        private bool CanAddProperty()
        {
            return SelectedPlanetProperty == null ? true:false;
        }

        private bool CanUpdateProperty()
        {
            return SelectedPlanetProperty != null ? true : false;
        }

        private bool CanDeleteProperty()
        {
            return SelectedPlanetProperty != null ? true : false;
        }


        private void AddProperty()
        {
            if (Planet == null)
                return;
            var newProperty = new PlanetProperty { Property = new Property() };
            newProperty.Property.Name = EditedPropertyName?.Name ?? String.Empty;
            newProperty.Planet = Planet.Planet;
            newProperty.PlanetId = Planet.Planet.Id;
            newProperty.Property = EditedPropertyName;
            newProperty.PropertyId = EditedPropertyName?.Id ?? 0;
            newProperty.Value = EditedPropertyValue;
            Planet.PlanetProperties.Add(newProperty);
            PlanetProperties.Add(newProperty);
            dbContext.PlanetProperties.Add(newProperty);
            dbContext.SaveChanges();
        }

        private void UpdateProperty()
        {
            if (SelectedPlanetProperty == null)
                return;
            SelectedPlanetProperty.Value = EditedPropertyValue;
            OnPropertyChanged(SelectedPlanetProperty.Value);
            dbContext.SaveChanges();
        }

        private void DeleteProperty()
        {
            if (Planet == null || PlanetProperties.Count == 0)
                return;
            var propertyToDelete = PlanetProperties.FirstOrDefault();
            if (propertyToDelete != null)
            {
                Planet.PlanetProperties.Remove(propertyToDelete);
                PlanetProperties.Remove(propertyToDelete);
                dbContext.PlanetProperties.Remove(propertyToDelete);
                dbContext.SaveChanges();
            }
        }
    }
}
