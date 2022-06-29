using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaMaps.Commands;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using ViennaMaps.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace ViennaMaps.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public event EventHandler OnRequestOpen3DMap;
        public event EventHandler OnRequestOpen2DMap;
        public event EventHandler OnRequestOpenNewProfile;

        //Commands
        public ICommand View3DMapCmd { get; set; }
        public ICommand View2DMapCmd { get; set; }

        public ICommand ViewNewProfileCmd { get; set; }

        // Properties
        public ObservableCollection<Location> LocationList { get; set; }
       
        public List<string> DistrictName {get; set; }

        public List<string> ProjectName { get; set; }

        public MainViewModel()
        {
            LocationList = new ObservableCollection<Location>();
            DistrictName = new List<string>();
            ProjectName = new List<string>();
            View3DMapCmd = new RelayCommand(View3DMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ViewNewProfileCmd = new RelayCommand(ViewNewProfile);

            FillLocationList();
            FillProfileList();
        }

        private void View3DMap()
        {
            if (OnRequestOpen3DMap != null)
                OnRequestOpen3DMap(this, new EventArgs());
        }

        private void View2DMap()
        {
            if (OnRequestOpen2DMap != null)
                OnRequestOpen2DMap(this, new EventArgs());
        }

        private void ViewNewProfile()
        {
            if (OnRequestOpenNewProfile != null)
                OnRequestOpenNewProfile(this, new EventArgs());
        }

        public void FillLocationList()
        {

            LocationList.Clear();
            

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                // Join zwischen Adressen und Plz
                var locations = context.Location.OrderBy(l => l.DistrictNumber);
                // in Schleife Adresseliste füllen
                foreach (Location loc in locations)
                {
                    //LocationList.Add(loc);
                    DistrictName.Add(loc.DistrictName);
                }

                
            }
        }

        public void FillProfileList()
        {

            ProjectName.Clear();


            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {

                var project = context.Project.OrderBy(l => l.ProjectName);

                foreach (Project p in project)
                {
                    ProjectName.Add(p.ProjectName);
                }


            }
        }

        public void FillAnalysis()
        {

        }

        public ISeries[] Series { get; set; }
        = new ISeries[]
        {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                    Fill = null
                }
        };
    }
}
