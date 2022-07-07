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
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geotriggers;
using ViennaMaps.Models;
using ViennaMaps.ViewModels;

namespace ViennaMaps.Views
{
    /// <summary>
    /// Interaktionslogik für MapWindow.xaml
    /// </summary>
    ///   

    public partial class Map2DWindow: Window
    {
        private Map2DViewModel _viewModel;

        
        public Map2DWindow(string project, string location)
        {
            InitializeComponent();
            _viewModel = new Map2DViewModel(project, location);
            this.DataContext = _viewModel;
            _viewModel.OnRequestClose += (sender, args) => this.Close();
            _viewModel.MyMap2DView = MyMap2DView;
            _viewModel.Initialize();           
        }
    }
}
