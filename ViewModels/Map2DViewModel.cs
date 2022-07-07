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

        //Properties
        public string Location {get; set;}
        public string Project { get; set;}
        public SceneView MyMap2DView { get; set; }


        //Commands
        public ICommand ExitCmd { get; set; }


        //Variables
        private List <FeatureLayer> _featureLayer = new List<FeatureLayer>();
        private List <string> _groupLayerLabel = new List<string>();
        
        private List <GroupLayer> _groupLayer = new List<GroupLayer>();


        //Constructor
        public Map2DViewModel( string project, string location)
        {
            ExitCmd = new RelayCommand(Exit);
            Location = location;
            Project = project;
            CreateLayers();
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

                var liste = context.View1.Include(p => p.LayerId).Include(i => i.ProjectId).Where(p => p.ProjectName == Project);

                foreach (var item in liste)
                {
                    _featureLayer.Add(new FeatureLayer(new Uri(item.ArcGisuri)));
                    _featureLayer.Last().Name = item.LayerLabel;

                    //TO DO: add both options
                    if (!_groupLayerLabel.Contains(item.GroupLabel))
                    {
                        _groupLayer.Add(new GroupLayer());
                        _groupLayer.Last().Name = item.GroupLabel;
                        _groupLayer.Last().Layers.Add(_featureLayer.Last());
                        _groupLayerLabel.Add(item.GroupLabel);

                        //check if correct
                        Trace.WriteLine($"GroupLabel {item.GroupLabel} added");
                    }                 
            
                }                   

            }
        }

        private void CreateGroups( )
        {

          

        }

        public async void Initialize()
        {                        

            // Create the scene with a basemap.
            MyMap2DView.Scene = new Scene(BasemapStyle.ArcGISLightGrayBase);

   
            foreach(var groups in _groupLayer)
                MyMap2DView.Scene.OperationalLayers.Add(groups);

            // Wait for all of the layers in the group layer to load.
            await Task.WhenAll(_groupLayer.Last().Layers.ToList().Select(m => m.LoadAsync()).ToList());

            // Zoom to the extent of the group layer.
            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                var loc = context.Location.Where(d => d.DistrictName == Location);
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
