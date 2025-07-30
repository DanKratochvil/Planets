using Planets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private AppDbContext _dbContext;
        public ListViewModel ListViewModel { get; }
        public DetailViewModel DetailViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public PropertyViewModel PropertyViewModel { get; }


        public MainWindowViewModel(AppDbContext dbContext, ListViewModel listViewModel, DetailViewModel detailViewModel, FilterViewModel filterViewModel, PropertyViewModel propertyViewModel)
        {
            _dbContext = dbContext;
            ListViewModel = listViewModel;
            DetailViewModel = detailViewModel;
            FilterViewModel = filterViewModel;
            PropertyViewModel = propertyViewModel;
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }
    }
}
