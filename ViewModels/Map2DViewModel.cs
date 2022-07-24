using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ViennaMaps.Commands;
using ViennaMaps.Models;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using System.Data.Entity;
using Location = ViennaMaps.Models.Location;

namespace ViennaMaps.ViewModels
{
    internal class Map2DViewModel : BaseViewModel
    {
        // Events
        public event EventHandler OnRequestClose;

        // Commands
        public ICommand ExitCmd { get; set; }

        // Properties
        public string Location { get; set; }
        public string Project { get; set; }
        public SceneView MyMap2DView { get; set; }

        // Attributes
        private List<string> _groupLayerLabel = new List<string>();
        private List<GroupLayer> _groupLayer = new List<GroupLayer>();


        // Constructor
        public Map2DViewModel(string project, string location, SceneView myMap2DView)
        {
            ExitCmd = new RelayCommand(Exit);
            Location = location;
            Project = project;
            MyMap2DView = myMap2DView;

            //create scene, create layers, add groups to the scene and zoom in on selected district
            CreateScene();
            CreateLayers();
            AddGroupsToScene();
            ZoomDistrict();
        }

        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }

        private void CreateScene()
        {
            // Create the scene with a basemap.
            MyMap2DView.Scene = new Scene(BasemapStyle.ArcGISLightGrayBase);
        }

        private void CreateLayers()
        {
            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                //check which layers should be created for the chosen project in the main window
                var liste = context.ProjectLayersView.Include(p => p.LayerId).Where(p => p.ProjectName == Project);

                //for each project layer, a new feature layer will be created 
                foreach (var item in liste)
                {
                    //new feature layer created
                    FeatureLayer tempLayer = new FeatureLayer(new Uri(item.ArcGisuri)) { Name = item.LayerLabel };

                    //create and/or add layer to correspinding group
                    CreateGroups(tempLayer, item);

                }
            }
        }

        private void CreateGroups(FeatureLayer layer, ProjectLayersView item)
        {
            //if layer for the group does not exist yet, create the group
            if (!_groupLayerLabel.Contains(item.GroupLabel))
            {
                //create new Group Layer
                GroupLayer tempGroupLayer = new GroupLayer() { Name = item.GroupLabel};

                //add layer to new Group Layer
                tempGroupLayer.Layers.Add(layer);

                //add group layer and label to the group layer list
                _groupLayer.Add(tempGroupLayer);
                _groupLayerLabel.Add(item.GroupLabel);
            }
            else
            {
                //add layer to existing group layer
                _groupLayer[_groupLayerLabel.IndexOf(item.GroupLabel)].Layers.Add(layer);
            }

        }

        private async void AddGroupsToScene()
        {

            // add created groups to the scene
            foreach (var groups in _groupLayer)
                MyMap2DView.Scene.OperationalLayers.Add(groups);

            // wait for all of the layers in the group layer to load.
            await Task.WhenAll(_groupLayer.LastOrDefault().Layers.ToList().Select(m => m.LoadAsync()).ToList());

        }

        private async void ZoomDistrict()
        {
            //get latitude and longitude from the database and zoom in on selected location
            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                //get location for chosen district in the Main Window
                Location loc = context.Location.Single(d => d.DistrictName == Location);
                //create a new Viewport to zoom in on chosen location
                await MyMap2DView.SetViewpointAsync(new Viewpoint(float.Parse(loc.Latitude), float.Parse(loc.Longitude), 8000.0));                
            }
        }
    }
}
