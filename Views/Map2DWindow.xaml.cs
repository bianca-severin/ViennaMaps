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
            /*Map myMap = new Map(Basemap.CreateDarkGrayCanvasVector());

            ArcGISMapImageLayer layer = new ArcGISMapImageLayer(new Uri("https://services1.arcgis.com/YfxQKFk1MjjurGb5/arcgis/rest/services/B%C3%BCchereien/FeatureServer"));
            myMap.OperationalLayers.Add(layer);
            MyMapView.Map = myMap;*/
        }

        private void Initialize()
        {

            // Create a new Scene with a topographic basemap.
            Scene scene = new Scene(BasemapStyle.ArcGISDarkGray);

            // Add a base surface with elevation data.
            Surface elevationSurface = new Surface();
            Uri elevationService = new Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer");
            elevationSurface.ElevationSources.Add(new ArcGISTiledElevationSource(elevationService));
            scene.BaseSurface = elevationSurface;

            // Add a scene layer.
            Uri buildingsService = new Uri("https://tiles.arcgis.com/tiles/YfxQKFk1MjjurGb5/arcgis/rest/services/Prozessiertes_Baukörpermodell_Wien/SceneServer/layers/0");
            ArcGISSceneLayer buildingsLayer = new ArcGISSceneLayer(buildingsService);
            scene.OperationalLayers.Add(buildingsLayer);

            // Assign the Scene to the SceneView.
            MySceneView.Scene = scene;

            try
            {
                // Create a camera with an interesting view.
                Camera viewCamera = new Camera(
                    latitude: 48.210033,
                    longitude: 16.363449,
                    altitude: 400,
                    heading: 50,
                    pitch: 30,
                    roll: 0
                    );

                // Set the viewpoint with the camera.
                MySceneView.SetViewpointCamera(viewCamera);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }

        }
        private async void SceneViewTapped(object sender, Esri.ArcGISRuntime.UI.Controls.GeoViewInputEventArgs e)
        {
            // Get the scene layer from the scene (first and only operational layer).
            ArcGISSceneLayer sceneLayer = (ArcGISSceneLayer)MySceneView.Scene.OperationalLayers.First();

            // Clear any existing selection.
            sceneLayer.ClearSelection();

            try
            {
                // Identify the layer at the tap point.
                // Use a 10-pixel tolerance around the point and return a maximum of one feature.
                IdentifyLayerResult result = await MySceneView.IdentifyLayerAsync(sceneLayer, e.Position, 10, false, 1);

                // Get the GeoElements that were identified (will be 0 or 1 element).
                IReadOnlyList<GeoElement> geoElements = result.GeoElements;

                // If a GeoElement was identified, select it in the scene.
                if (geoElements.Any())
                {
                    GeoElement geoElement = geoElements.FirstOrDefault();
                    if (geoElement != null)
                    {
                        // Select the feature to highlight it in the scene view.
                        sceneLayer.SelectFeature((Feature)geoElement);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

    }
}
