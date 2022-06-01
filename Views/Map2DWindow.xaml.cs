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
using ViennaMaps.Models;

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
            //Map2D map = new Map2D();

            // Create the layers.
            //Büchereien
            FeatureLayer libraries = new FeatureLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/ArcGIS/rest/services/B%c3%bcchereien/FeatureServer/0"));
            //Gemeindebauten heute
            FeatureLayer communityBuildings = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/Gemeindebauten_Wien_Vollst%c3%a4ndig_WFL1/FeatureServer/0"));
            //Fußgängerzonen
            FeatureLayer pedestrianAreas = new FeatureLayer(new Uri("https://services.arcgis.com/Sf0q24s0oDKgX14j/ArcGIS/rest/services/Fu%c3%9fg%c3%a4ngerzonen_Wien_WFL1/FeatureServer/0"));
            //Bezirksgrenzen
            FeatureLayer districtBoundary = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/Gemeindebauten_Wien_Vollst%c3%a4ndig_WFL1/FeatureServer/5"));
            //Lärm innerbezirke
            FeatureLayer noise = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/Wien_Nacht_Stra%c3%9fe/FeatureServer/12"));
            //Verkehr (loads super slow)
           /* FeatureLayer Verkehr = new FeatureLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/arcgis/rest/services/GIP_Verkehrsnetz/FeatureServer/0"));*/
            //Hiking Trails
            FeatureLayer hikingTrails = new FeatureLayer(new Uri("https://services2.arcgis.com/Eij2WK0CHxEthqHo/ArcGIS/rest/services/Vienna_City_Trails%e6%b5%8b%e8%af%95/FeatureServer/0"));
            //Parzellen
            FeatureLayer parcels = new FeatureLayer(new Uri("https://services3.arcgis.com/ratx1xWpTPZDmMzX/ArcGIS/rest/services/Vienna_Parcels/FeatureServer/0"));
            //ParkingLots
            FeatureLayer parkingLots = new FeatureLayer(new Uri("https://services3.arcgis.com/ratx1xWpTPZDmMzX/ArcGIS/rest/services/Vienna_ParkingLots_from_2009/FeatureServer/0"));
            //ForeingerPercentage
            FeatureLayer foreingersPercentage = new FeatureLayer(new Uri("https://services.arcgis.com/Ok7pjj1jT9rEneC7/ArcGIS/rest/services/AuslaenderanteilWien2001/FeatureServer/0"));
            //POIs Points
            FeatureLayer poisPoints = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/pois_wien_v6_bereinigt/FeatureServer/0"));


            hikingTrails.Name = "Hiking trails";
            libraries.Name = "Libraries";
            communityBuildings.Name = "Community buildings";
            pedestrianAreas.Name = "Pedestrian areas";
            districtBoundary.Name = "District Boundaries";
            noise.Name = "Noise";
            hikingTrails.Name = "Hiking trails";
            parcels.Name = "Parcels";
            parkingLots.Name = "Parking lots";
            foreingersPercentage.Name = "Foreingers Percentage";
            poisPoints.Name = "POIs";

            // Create the group layer and add sublayers.
            GroupLayer gLayer = new GroupLayer();
            gLayer.Name = "Services";
            gLayer.Layers.Add(libraries);
            
            gLayer.Layers.Add(communityBuildings);
            gLayer.Layers.Add(pedestrianAreas);

            // Create the scene with a basemap.
            MyMap2DView.Scene = new Scene(BasemapStyle.ArcGISLightGrayBase);

            // Add the top-level layers to the scene.
             MyMap2DView.Scene.OperationalLayers.Add(gLayer);
             MyMap2DView.Scene.OperationalLayers.Add(districtBoundary);
             MyMap2DView.Scene.OperationalLayers.Add(noise);
             MyMap2DView.Scene.OperationalLayers.Add(hikingTrails);

          


           // Wait for all of the layers in the group layer to load.
           await Task.WhenAll(gLayer.Layers.ToList().Select(m => m.LoadAsync()).ToList());

            // Zoom to the extent of the group layer.
            MyMap2DView.SetViewpoint(new Viewpoint(communityBuildings.FullExtent));



        }
       
    }
}
