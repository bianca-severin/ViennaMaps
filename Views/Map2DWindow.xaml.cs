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
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geotriggers;

namespace ViennaMaps.Views
{
    /// <summary>
    /// Interaktionslogik für MapWindow.xaml
    /// </summary>
    /// 
  

    public partial class Map2DWindow: Window
    {
        public Map2DWindow()
        {
            InitializeComponent();
            Initialize();
    
        }

        private async void Initialize()
        {

            // Create the layers.
            //Büchereien
            FeatureLayer devOne = new FeatureLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/ArcGIS/rest/services/B%c3%bcchereien/FeatureServer/0"));
            //Gemeindebauten heute
            FeatureLayer devTwo = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/Gemeindebauten_Wien_Vollst%c3%a4ndig_WFL1/FeatureServer/0"));
            //Fußgängerzonen
            FeatureLayer devThree = new FeatureLayer(new Uri("https://services.arcgis.com/Sf0q24s0oDKgX14j/ArcGIS/rest/services/Fu%c3%9fg%c3%a4ngerzonen_Wien_WFL1/FeatureServer/0"));
            //Bezirksgrenzen
            FeatureLayer nonDevOne = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/Gemeindebauten_Wien_Vollst%c3%a4ndig_WFL1/FeatureServer/5"));
            //Lärm innerbezirke
            FeatureLayer nonDevTwo = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/Wien_Nacht_Stra%c3%9fe/FeatureServer/12"));
            //Verkehr (loads super slow)
           /* FeatureLayer Verkehr = new FeatureLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/arcgis/rest/services/GIP_Verkehrsnetz/FeatureServer/0"));*/
            //Hiking Trails
            FeatureLayer HikingTrails = new FeatureLayer(new Uri("https://services2.arcgis.com/Eij2WK0CHxEthqHo/ArcGIS/rest/services/Vienna_City_Trails%e6%b5%8b%e8%af%95/FeatureServer/0"));

            // Create the group layer and add sublayers.
            GroupLayer gLayer = new GroupLayer();
            gLayer.Name = "Group: Dev A";
            gLayer.Layers.Add(devOne);
            gLayer.Layers.Add(devTwo);
            gLayer.Layers.Add(devThree);

            // Create the scene with a basemap.
            MyMap2DView.Scene = new Scene(BasemapStyle.ArcGISLightGrayBase);

            // Add the top-level layers to the scene.
             MyMap2DView.Scene.OperationalLayers.Add(gLayer);
             MyMap2DView.Scene.OperationalLayers.Add(nonDevOne);
             MyMap2DView.Scene.OperationalLayers.Add(nonDevTwo);
             MyMap2DView.Scene.OperationalLayers.Add(HikingTrails);

          


           // Wait for all of the layers in the group layer to load.
           await Task.WhenAll(gLayer.Layers.ToList().Select(m => m.LoadAsync()).ToList());

            // Zoom to the extent of the group layer.
            MyMap2DView.SetViewpoint(new Viewpoint(nonDevOne.FullExtent));

        }
       
    }
}
