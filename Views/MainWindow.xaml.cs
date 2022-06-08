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



namespace ViennaMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
            _viewModel.OnRequestOpenMap += (sender, args) => this.StartMapWindow();
            _viewModel.OnRequestOpen2DMap += (sender, args) => this.Start2DMapWindow();
            _viewModel.OnRequestOpenNewProfile += (sender, args) => this.StartNewProfile();

        }

        private void StartMapWindow()
        {
            // anlegen eines Adress window
            MapWindow mapwin = new MapWindow();
            // Anzeigen eines modalen windows
            mapwin.ShowDialog();

        }

        private void Start2DMapWindow()
        {
            // anlegen eines Adress window
            Map2DWindow mapwin = new Map2DWindow();
            // Anzeigen eines modalen windows
            mapwin.ShowDialog();

        }

        private void StartNewProfile()
        {
            // anlegen eines Adress window
            NewProfileWindow profileWin = new NewProfileWindow();
            // Anzeigen eines modalen windows
            profileWin.ShowDialog();

        }
    }
}
