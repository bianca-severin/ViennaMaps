using System;
using System.Windows;

using ViennaMaps.ViewModels;

namespace ViennaMaps.Views
{
    /// <summary>
    /// Interaction logic for Map3DWindow.xaml
    /// </summary>
    public partial class Map3DWindow: Window
    {
        // Attributes
        private Map3DViewModel _viewModel;

        // Constructor
        public Map3DWindow(string location)
        {
            InitializeComponent();

            // new instance of the ViewModel
            _viewModel = new Map3DViewModel(location, My3DSceneView);

            // conncetion between the Window (view) and the ViewModel
            this.DataContext = _viewModel;       
        } 
    }
}
