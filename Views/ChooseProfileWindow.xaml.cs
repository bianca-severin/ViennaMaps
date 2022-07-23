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
    /// Interaktionslogik für ChooseProfileWindow.xaml
    /// </summary>
    public partial class ChooseProfileWindow : Window
    {
        private ChooseProfileViewModel _viewModel;
        public ChooseProfileWindow()
        {
            InitializeComponent();
            _viewModel = new ChooseProfileViewModel();
            this.DataContext = _viewModel;
            _viewModel.OnRequestClose += (sender, args) => this.Close();

            _viewModel.OnRequestOpenMainWindow += (sender, args) => this.StartMainWindow();
        }

        private void StartMainWindow()
        {
            // create a new profile window
            MainWindow mainWin = new(_viewModel.SelectedProject, _viewModel.SelectedLocation);
            // show the window
            mainWin.ShowDialog();
        }

    }
}
