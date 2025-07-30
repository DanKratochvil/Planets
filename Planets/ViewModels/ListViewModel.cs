using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Planets.Models;
using Planets.Services;

namespace Planets.ViewModels
{
    /// <summary>
    /// class for Planets administration
    /// </summary>
    public class ListViewModel : BaseViewModel
    {
        private AppDbContext dbContext;
        private IDialogService dialogService;

        public RelayCommand AddPlanetCommand { get; }
        public RelayCommand UpdatePlanetCommand { get; }
        public RelayCommand DeletePlanetCommand { get; }
        public DetailViewModel DetailViewModel { get; }



        public ListViewModel(AppDbContext dbContext, DetailViewModel detailViewModel, IDialogService dialogService)
        {
            this.dbContext = dbContext;
            this.dialogService = dialogService;
            DetailViewModel = detailViewModel;
            AddPlanetCommand = new RelayCommand(AddPlanet, CanAddPlanet);
            UpdatePlanetCommand = new RelayCommand(UpdatePlanet, CanUpdatePlanet);
            DeletePlanetCommand = new RelayCommand(DeletePlanet, CanDeletePlanet);
            LoadPlanets(new FilteredProperty("", ""));
        }

        public ObservableCollection<PlanetViewModel> Planets { get; set; } = new();

        private PlanetViewModel? _selectedPlanet;
        public PlanetViewModel? SelectedPlanet
        {
            get => _selectedPlanet;
            set
            {
                SetProperty<PlanetViewModel?>(ref _selectedPlanet, value);
                DetailViewModel.Planet = value;
                EditedPlanetName = SelectedPlanet?.Name ?? "";
                UpdatePlanetCommand?.RaiseCanExecuteChanged();
                DeletePlanetCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _editedPlanetName;
        public string? EditedPlanetName
        {
            get => _editedPlanetName;
            set
            {
                SetProperty<string?>(ref _editedPlanetName, value);
                AddPlanetCommand?.RaiseCanExecuteChanged();
            }
        }

        public void LoadPlanets(FilteredProperty filteredProperty)
        {
            Planets.Clear();
            var planets = dbContext.Planets
                .Include(p => p.PlanetProperties)
                .ThenInclude(p => p.Property)
                .Where(p =>
                    string.IsNullOrEmpty(filteredProperty.Name) ||
                    p.PlanetProperties.Any(pp => pp.Property.Name == filteredProperty.Name && (filteredProperty.Value == string.Empty || pp.Value.ToLower().Contains(filteredProperty.Value))))
                .ToList();

            foreach (var planet in planets)
            {
                Planets.Add(new PlanetViewModel(planet));
            }
        }

        private bool CanAddPlanet()
        {
            if (String.IsNullOrEmpty(EditedPlanetName) || Planets.Any(p => p.Name == EditedPlanetName))
                return false;

            return true;
        }


        private void AddPlanet()
        {
            if (String.IsNullOrEmpty(EditedPlanetName))
                return;
            var newPlanet = new Planet { Name = EditedPlanetName };
            dbContext.Planets.Add(newPlanet);
            dbContext.SaveChanges();

            var newPlanetViewModel = new PlanetViewModel(newPlanet);
            Planets.Add(newPlanetViewModel);
            SelectedPlanet = new PlanetViewModel(newPlanet);
        }

        private bool CanUpdatePlanet()
        {
            return SelectedPlanet != null ? true : false;
        }

        private void UpdatePlanet()
        {
            if (SelectedPlanet == null || string.IsNullOrEmpty(EditedPlanetName))
                return;
            SelectedPlanet.Planet.Name = EditedPlanetName;
            dbContext.Planets.Update(SelectedPlanet.Planet);
            dbContext.SaveChanges();
            SelectedPlanet.Name = EditedPlanetName;
        }

        private bool CanDeletePlanet()
        {
            return SelectedPlanet != null ? true : false;
        }

        private void DeletePlanet()
        {
            if (SelectedPlanet == null)
                return;

            var properties = dbContext.PlanetProperties.Where(pp => pp.PlanetId == SelectedPlanet.Planet.Id).ToList();
            if (properties.Count > 0)
            {
                dialogService.ShowMessage($"Planet '{SelectedPlanet.Name}' has {properties.Count} Propeties\nDelete them first", "Warning");
                return;
            }

            dbContext.Planets.Remove(SelectedPlanet.Planet);
            dbContext.SaveChanges();
            Planets.Remove(SelectedPlanet);
            SelectedPlanet = null;
        }
    }
}
