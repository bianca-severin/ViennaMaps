using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaMaps.ViewModels;
using ViennaMaps.Views;
using System.Collections.ObjectModel;
using ViennaMaps.Models;

namespace ViennaMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Attributes
        private MainViewModel _viewModel;

        // Constructor
        public MainWindow(string project, string location)
        {
            InitializeComponent();

            // new instance of the ViewModel
            _viewModel = new MainViewModel(project,location);

            // connection between the Window (view) and the ViewModel
            this.DataContext = _viewModel;
            _viewModel.OnRequestOpen3DMap += (sender, args) => this.Start3DMapWindow();
            _viewModel.OnRequestOpen2DMap += (sender, args) => this.Start2DMapWindow();
            _viewModel.OnRequestClose += (sender, args) => this.Close();
        }

        // starts/shows the the 3D Map Window
        private void Start3DMapWindow()
        {
            // create a new 3D Map window, using the selected location
            Map3DWindow map3Dwin = new Map3DWindow(_viewModel.SelectedLocation);
            // show the 3D Map window
            map3Dwin.ShowDialog();
        }

        // starts/shows the 2D Map Window
        private void Start2DMapWindow()
        {
            // create a new 2D Map window, using the selected project and selected location by the user
            Map2DWindow map2Dwin = new Map2DWindow(_viewModel.SelectedProject, _viewModel.SelectedLocation);
            // show the window
            map2Dwin.ShowDialog();
        }
    }
}
