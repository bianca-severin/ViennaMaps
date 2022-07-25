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
using ViennaMaps.ViewModels;

namespace ViennaMaps.Views
{
    /// <summary>
    /// Interaction logic for ChooseProfileWindow.xaml
    /// </summary>
    public partial class ChooseProfileWindow : Window
    {
        // Attributes
        private ChooseProfileViewModel _viewModel;

        // Constructor
        public ChooseProfileWindow()
        {
            InitializeComponent();

            // new instance of the ViewModel
            _viewModel = new ChooseProfileViewModel();

            // connection between the Window (view) and the ViewModel
            this.DataContext = _viewModel;
            _viewModel.OnRequestClose += (sender, args) => this.Close();
            _viewModel.OnRequestOpenMainWindow += (sender, args) => this.StartMainWindow();
        }

        // starts/shows the the Main Window
        private void StartMainWindow()
        {
            // create a new profile window
            MainWindow mainWin = new(_viewModel.SelectedProject, _viewModel.SelectedLocation);
            // show the window
            mainWin.ShowDialog();
        }
    }
}
