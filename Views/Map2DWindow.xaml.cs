using System;
using System.Windows;
using ViennaMaps.ViewModels;

namespace ViennaMaps.Views
{
    public partial class Map2DWindow: Window
    {
        private Map2DViewModel _viewModel;
        
        public Map2DWindow(string project, string location)
        {
            InitializeComponent();
            _viewModel = new Map2DViewModel(project, location, MyMap2DView);
            this.DataContext = _viewModel;
            _viewModel.OnRequestClose += (sender, args) => this.Close();      
        }
    }
}
