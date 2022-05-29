﻿using System;
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

namespace ViennaMaps.Views
{
    /// <summary>
    /// Interaktionslogik für MapWindow.xaml
    /// </summary>
    /// 
  

    public partial class MapWindow: Window
    {
        public MapWindow()
        {
            InitializeComponent();
            Initialize();
            /*Map myMap = new Map(Basemap.CreateDarkGrayCanvasVector());

            ArcGISMapImageLayer layer = new ArcGISMapImageLayer(new Uri("https://services.arcgis.com/LG9Yn2oFqZi5PnO5/arcgis/rest/services/xgqZm/FeatureServer"));
            myMap.OperationalLayers.Add(layer);
            MyMapView.Map = myMap;*/
        }

        private void Initialize()
        {
           
            // Create a new Scene with a topographic basemap.
            Scene scene = new Scene(BasemapStyle.ArcGISTopographic);

            // Add a base surface with elevation data.
            Surface elevationSurface = new Surface();
            Uri elevationService = new Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer");
            elevationSurface.ElevationSources.Add(new ArcGISTiledElevationSource(elevationService));
            scene.BaseSurface = elevationSurface;

            // Add a scene layer.
            Uri buildingsService = new Uri("https://tiles.arcgis.com/tiles/P3ePLMYs2RVChkJx/arcgis/rest/services/Buildings_Brest/SceneServer/layers/0");
            ArcGISSceneLayer buildingsLayer = new ArcGISSceneLayer(buildingsService);
            scene.OperationalLayers.Add(buildingsLayer);

            // Assign the Scene to the SceneView.
            MySceneView.Scene = scene;

            try
            {
                // Create a camera with an interesting view.
                Camera viewCamera = new Camera(48.378, -4.494, 200, 345, 65, 0);

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
