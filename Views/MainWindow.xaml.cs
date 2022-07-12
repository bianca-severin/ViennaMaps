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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        private MainViewModel _viewModel;

        //ChooseProfileWindow chooseProfilePopup = new ChooseProfileWindow();


        public MainWindow(string project, string location)
        {
            InitializeComponent();
            _viewModel = new MainViewModel(project,location);
            this.DataContext = _viewModel;
           //viewModel.OnRequestClose += (sender, args) => this.Close();
            _viewModel.OnRequestOpen3DMap += (sender, args) => this.Start3DMapWindow();
            _viewModel.OnRequestOpen2DMap += (sender, args) => this.Start2DMapWindow();
            _viewModel.OnRequestClose += (sender, args) => this.Close();
        }

        private void Start3DMapWindow()
        {
            // create a new 2D Map window, using the selected location
            Map3DWindow map3Dwin = new Map3DWindow(_viewModel.SelectedLocation);
            // show the 3D Map window
            map3Dwin.ShowDialog();
        }

        private void Start2DMapWindow()
        {
            // create a new 2D Map window, using the selected project and selected location by the user
            Map2DWindow map2Dwin = new Map2DWindow(_viewModel.SelectedProject, _viewModel.SelectedLocation);
            // show the window
            map2Dwin.ShowDialog();
        }

    }
}
