using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Mapping;
using ViennaMaps.Models;

/// <summary>
/// Viewmodel for <c>Map3DWindow</c> 
/// The class creates a topographic map and adds the building layer. The camera will zoom in on the chosen district
/// </summary>
namespace ViennaMaps.ViewModels
{
    internal class Map3DViewModel
    {
        // Properties
        public string Location { get; set; }
        public SceneView My3DSceneView { get; set; }


        // Constructor
        public Map3DViewModel(string location, SceneView my3DSceneView)
        {
            // assign data from the ChooseProfileViewModel to the properties
            Location = location;          
            My3DSceneView = my3DSceneView;

            // create topographic scene, add buildings layers, set camera on the selected district
            CreateTopographicScene();
            AddBuildingsLayer();
            SetCamera();
        }

        // the method creates a new scene with elevation
        private void CreateTopographicScene()
        {
            // create a new Scene with a base map
            My3DSceneView.Scene = new Scene(BasemapStyle.ArcGISDarkGray);            

            // add a base surface with elevation data - topography
            Surface elevationSurface = new Surface();
            Uri elevationService = new Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer");
            elevationSurface.ElevationSources.Add(new ArcGISTiledElevationSource(elevationService));

            // add the topography to the scene
            My3DSceneView.Scene.BaseSurface = elevationSurface;      

        }

        // a building layer is added to the scene
        private void AddBuildingsLayer()
        {
            // Uri buildings in LOD2 definition for Vienna
            Uri buildingsService = new Uri("https://tiles.arcgis.com/tiles/YfxQKFk1MjjurGb5/arcgis/rest/services/Prozessiertes_Baukörpermodell_Wien/SceneServer/layers/0");

            // create buildings layer
            ArcGISSceneLayer buildingsLayer = new ArcGISSceneLayer(buildingsService);

            // add buildings layer to the scene
            My3DSceneView.Scene.OperationalLayers.Add(buildingsLayer);
        }

        // the method sets the camera on the coordinates of the chosen location
        private void SetCamera()
        {
            // get latitude and longitude from the database to set camera on selected location
            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                // get location for chosen district in the Main Window
                Location loc = context.Location.Single(d => d.DistrictName == Location);

                // create a camera over the selected district
                Camera viewCamera = new Camera(
                    latitude: float.Parse(loc.Latitude),
                    longitude: float.Parse(loc.Longitude),
                    altitude: 400,
                    heading: 50,
                    pitch: 30,
                    roll: 0
                    );

                // set the camera viewpoint on the created camera
                My3DSceneView.SetViewpointCamera(viewCamera);
            }         
        }      
    }
}
