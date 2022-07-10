using System;
using System.Windows;

using ViennaMaps.ViewModels;

namespace ViennaMaps.Views
{
    
    public partial class Map3DWindow: Window
    {
        private Map3DViewModel _viewModel;
        public Map3DWindow(string location)
        {
            InitializeComponent();
            _viewModel = new Map3DViewModel(location, My3DSceneView);
            this.DataContext = _viewModel;

        }
   

    }
}
