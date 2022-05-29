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
using Esri.ArcGISRuntime.Mapping;


namespace ViennaMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Map myMap = new Map(Basemap.CreateDarkGrayCanvasVector());

            ArcGISMapImageLayer layer = new ArcGISMapImageLayer(new Uri("https://services.arcgis.com/LG9Yn2oFqZi5PnO5/arcgis/rest/services/xgqZm/FeatureServer"));
            myMap.OperationalLayers.Add(layer);
            MyMapView.Map = myMap;
        }
    }
}
