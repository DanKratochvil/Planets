using Planets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Planets.Views
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class DetailView : UserControl
    {
        public DetailView()
        {
            InitializeComponent();
        }

        private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            DetailViewModel? viewModel = DataContext as DetailViewModel;
            if (viewModel != null)
            {
             //   viewModel.SelectedPlanetProperty = null;
            }
        }
    }
}
