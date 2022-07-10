using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViennaMaps.Commands;
using System.Windows.Forms;
using ViennaMaps.Models;
using System.Collections.ObjectModel;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Layer = ViennaMaps.Models.Layer;
using System.Diagnostics;
using System.Data.Entity;

namespace ViennaMaps.ViewModels
{
    internal class Map2DViewModel : BaseViewModel
    {

        //Event
        public event EventHandler OnRequestClose;

        //Commands
        public ICommand ExitCmd { get; set; }

        //Properties
        public string Location { get; set; }
        public string Project { get; set; }
        public SceneView MyMap2DView { get; set; }


        //Variables
        private List <string> _groupLayerLabel = new List<string>();        
        private List <GroupLayer> _groupLayer = new List<GroupLayer>();


        //Constructor
        public Map2DViewModel( string project, string location, SceneView myMap2DView)
        {
            ExitCmd = new RelayCommand(Exit);
            Location = location;
            Project = project;
            MyMap2DView = myMap2DView;

            // Create the scene with a basemap.
            MyMap2DView.Scene = new Scene(BasemapStyle.ArcGISLightGrayBase);            
            CreateLayers();
            Initialize();
        }

        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
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
                    FeatureLayer tempLayer = new FeatureLayer(new Uri(item.ArcGisuri)) { Name = item.LayerLabel};
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
                GroupLayer tempGroupLayer = new GroupLayer() { Name = item.GroupLabel };
                //add layer to new Group Layer
                tempGroupLayer.Layers.Add(layer);

                //add group layer and label to local list
                _groupLayer.Add(tempGroupLayer);
                _groupLayerLabel.Add(item.GroupLabel);
            }
            else
            {
                _groupLayer[_groupLayerLabel.IndexOf(item.GroupLabel)].Layers.Add(layer);
            }

        }

        public async void Initialize()
        {                                  

            //Add created groups to the scene
            foreach(var groups in _groupLayer)
                MyMap2DView.Scene.OperationalLayers.Add(groups);

            // Wait for all of the layers in the group layer to load.
            await Task.WhenAll(_groupLayer.LastOrDefault().Layers.ToList().Select(m => m.LoadAsync()).ToList());

            // Zoom on chosen location
            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                var loc = context.Location.Where(d => d.DistrictName == Location);
                //first or single 
                foreach (Models.Location l in loc)
                {
                    float la = float.Parse(l.Latitude);
                    float lo = float.Parse(l.Longitude);
                    await MyMap2DView.SetViewpointAsync(new Viewpoint(la,lo , 8000.0));
                }
            }
        }
    }
}
