using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.ViewModels
{
    /// <summary>
    /// class for filtering Planets
    /// </summary>
    public class FilteredProperty
    {
        public FilteredProperty(string name, string value)
        {
            Name = name;
            Value = value.ToLower();
        }

        public string Name { get; set; } 
        public string Value { get; set; }
    }

    public class FilterViewModel : BaseViewModel
    {
        private AppDbContext _dbContext;
        public ListViewModel ListViewModel { get; }
        public DetailViewModel DetailViewModel { get; }
        public FilteredProperty FilteredProperty { get; set; } = new("","");

        public RelayCommand ApplyFilterCommand { get; }
        public RelayCommand ClearFilterCommand { get; }

        public FilterViewModel(AppDbContext dbContext, ListViewModel listViewModel, DetailViewModel detailViewModel)
        {
            _dbContext = dbContext;
            ListViewModel = listViewModel;
            DetailViewModel = detailViewModel;
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ClearFilterCommand = new RelayCommand(ClearFilter);
        }

        public List<Property> Properties => _dbContext.Properties.ToList(); 

        private Property? _selectedProperty;
        public Property? SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                SetProperty<Property?>(ref _selectedProperty, value);
            }
        }

        private string? _filterValue;

        public string? FilterValue
        {
            get => _filterValue;
            set
            {
                SetProperty<string?>(ref _filterValue, value);
                ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            var fp = new FilteredProperty(SelectedProperty?.Name ?? string.Empty, FilterValue ?? string.Empty);
            ListViewModel.LoadPlanets(fp);
        }

        private void ClearFilter()
        {
            SelectedProperty = null;
            FilterValue = string.Empty;
            ListViewModel.LoadPlanets(new FilteredProperty("", ""));
        }
    }
}
