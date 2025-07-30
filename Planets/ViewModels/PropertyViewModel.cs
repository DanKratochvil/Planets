using Planets.Models;
using Planets.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.ViewModels
{
    /// <summary>
    /// class for all defined Properties administration
    /// </summary>
    public class PropertyViewModel : BaseViewModel
    {
        private AppDbContext dbContext;
        public IDialogService dialogService; 

        public RelayCommand AddPropertyCommand { get; }
        public RelayCommand UpdatePropertyCommand { get; }
        public RelayCommand DeletePropertyCommand { get; }

     

        public PropertyViewModel(AppDbContext dbContext, IDialogService dialogService)
        {
            this.dbContext = dbContext;

            AddPropertyCommand = new RelayCommand(AddProperty, CanAddProperty);
            UpdatePropertyCommand = new RelayCommand(UpdateProperty, CanUpdateProperty);
            DeletePropertyCommand = new RelayCommand(DeleteProperty,CanDeleteProperty);
            this.dialogService = dialogService;
            LoadProperties();
        }

        public ObservableCollection<Property> Properties { get; set; } = new();

        private Property? _selectedProperty;
        public Property? SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                SetProperty<Property?>(ref _selectedProperty, value);
                EditedPropertyName = SelectedProperty?.Name ?? "";
                UpdatePropertyCommand?.RaiseCanExecuteChanged();
                DeletePropertyCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _editedPropertyName;
        public string? EditedPropertyName
        {
            get => _editedPropertyName;
            set
            {
                SetProperty<string?>(ref _editedPropertyName, value);
                AddPropertyCommand?.RaiseCanExecuteChanged();
            }
        }

        public void LoadProperties()
        {
            Properties.Clear();
            var properties = dbContext.Properties;

            foreach (var property in properties)
            {
                Properties.Add(property);
            }
        }

        private bool CanAddProperty()
        {
            if (String.IsNullOrEmpty(EditedPropertyName) || Properties.Any(p => p.Name == EditedPropertyName))
                return false;

            return true;
        }

        private bool CanUpdateProperty()
        {
            return SelectedProperty != null ? true : false;
        }

        private bool CanDeleteProperty()
        {
            return SelectedProperty != null ? true : false;
        }

        private void AddProperty()
        {
            if (string.IsNullOrEmpty(EditedPropertyName))
                return;
            var newProperty = new Property { Name = EditedPropertyName };
            dbContext.Properties.Add(newProperty);
            dbContext.SaveChanges();

            Properties.Add(newProperty);
        }

        private void UpdateProperty()
        {
            if (SelectedProperty == null || string.IsNullOrEmpty(EditedPropertyName))
                return;
            SelectedProperty.Name = EditedPropertyName;
            dbContext.Properties.Update(SelectedProperty);
            dbContext.SaveChanges();
        }

        private void DeleteProperty()
        {
            if (SelectedProperty == null)
                return;
            var planets = dbContext.PlanetProperties.Where(pp => pp.PropertyId == SelectedProperty.Id).Select(pp => pp.Planet.Name).ToList();
            if (planets.Count > 0)
            {
                dialogService.ShowMessage($"Property '{SelectedProperty.Name}' is used by the following planets:\n {string.Join(", ", planets)}","Warning");
                return;
            }

            dbContext.Properties.Remove(SelectedProperty);
            dbContext.SaveChanges();
            Properties.Remove(SelectedProperty);
            SelectedProperty = null;
        }
    }
}
