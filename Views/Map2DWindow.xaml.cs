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
            //Verkehr (loads super slow)
            FeatureLayer transportRoads = new FeatureLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/arcgis/rest/services/GIP_Verkehrsnetz/FeatureServer/0"));
            //Hiking Trails
            FeatureLayer hikingTrails = new FeatureLayer(new Uri("https://services2.arcgis.com/Eij2WK0CHxEthqHo/ArcGIS/rest/services/Vienna_City_Trails%e6%b5%8b%e8%af%95/FeatureServer/0"));
            //ForeingerPercentage
            FeatureLayer foreingersPercentage = new FeatureLayer(new Uri("https://services.arcgis.com/Ok7pjj1jT9rEneC7/ArcGIS/rest/services/AuslaenderanteilWien2001/FeatureServer/0"));
            //POIs Points
            FeatureLayer poisPoints = new FeatureLayer(new Uri("https://services.arcgis.com/E3H3dLmHwo799BQr/ArcGIS/rest/services/pois_wien_v6_bereinigt/FeatureServer/0"));
            //POIs Points
            FeatureLayer doctors = new FeatureLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/ArcGIS/rest/services/%c3%84rzte/FeatureServer/0"));


            hikingTrails.Name = "Hiking trails";
            libraries.Name = "Libraries";
            communityBuildings.Name = "Community buildings";
            pedestrianAreas.Name = "Pedestrian areas";
            districtBoundary.Name = "District Boundaries";
            foreingersPercentage.Name = "Foreingers Percentage";
            poisPoints.Name = "Points of interest";
            transportRoads.Name = "Streets, Roads & Paths";

            // Create one layer and add sublayers.
            GroupLayer cityMorphologyLayer = new GroupLayer();
            cityMorphologyLayer.Name = "City morphology";
            cityMorphologyLayer.Layers.Add(districtBoundary);
            cityMorphologyLayer.Layers.Add(communityBuildings);

            GroupLayer servicesLayer = new GroupLayer();
            servicesLayer.Name = "Services, functions and uses";
            servicesLayer.Layers.Add(libraries);      
           

            GroupLayer greenAndPublicSpaceLayer = new GroupLayer();
            greenAndPublicSpaceLayer.Name = "Green and public spaces";
            greenAndPublicSpaceLayer.Layers.Add(pedestrianAreas);
            greenAndPublicSpaceLayer.Layers.Add(hikingTrails);
            greenAndPublicSpaceLayer.Layers.Add(poisPoints);

            GroupLayer educationLayer= new GroupLayer();
            educationLayer.Name = "Education";

            GroupLayer healthLayer = new GroupLayer();
            healthLayer.Name = "Health";
            healthLayer.Layers.Add(doctors);

            GroupLayer cultureLayer = new GroupLayer();
            cultureLayer.Name = "Culture";

            GroupLayer publicFacilitiesLayer = new GroupLayer();
            publicFacilitiesLayer.Name = "Public Facilities";


            GroupLayer transportLayer = new GroupLayer();
            transportLayer.Name = "Transport";
            transportLayer.Layers.Add(transportRoads);


            // Create the scene with a basemap.
            MyMap2DView.Scene = new Scene(BasemapStyle.ArcGISLightGrayBase);

            // Add the top-level layers to the scene.
            MyMap2DView.Scene.OperationalLayers.Add(cityMorphologyLayer);
            MyMap2DView.Scene.OperationalLayers.Add(servicesLayer);
            MyMap2DView.Scene.OperationalLayers.Add(greenAndPublicSpaceLayer);
            MyMap2DView.Scene.OperationalLayers.Add(educationLayer);
            MyMap2DView.Scene.OperationalLayers.Add(healthLayer);
            MyMap2DView.Scene.OperationalLayers.Add(cultureLayer);
            MyMap2DView.Scene.OperationalLayers.Add(publicFacilitiesLayer);
            MyMap2DView.Scene.OperationalLayers.Add(transportLayer);                     
            

           // Wait for all of the layers in the group layer to load.
           await Task.WhenAll(servicesLayer.Layers.ToList().Select(m => m.LoadAsync()).ToList());

            // Zoom to the extent of the group layer.
            MyMap2DView.SetViewpointAsync(new Viewpoint(48.21, 16.36, 8000.0));

        }
       
    }
}
