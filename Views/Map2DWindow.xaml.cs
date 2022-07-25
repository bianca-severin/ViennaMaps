using System;
using System.Windows;
using ViennaMaps.ViewModels;

namespace ViennaMaps.Views
{
    /// <summary>
    /// Interaction logic for Map2DWindow.xaml
    /// </summary>
    public partial class Map2DWindow: Window
    {
        // Attributes
        private Map2DViewModel _viewModel;
        
        // Constructor
        public Map2DWindow(string project, string location)
        {
            InitializeComponent();
            // new instance of the ViewModel
            _viewModel = new Map2DViewModel(project, location, MyMap2DView);

            // connection between the Window (view) and the ViewModel
            this.DataContext = _viewModel;
            _viewModel.OnRequestClose += (sender, args) => this.Close();      
        }
    }
}
