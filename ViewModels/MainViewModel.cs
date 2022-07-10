using System;
using System.Collections.Generic;
using System.Linq;
using ViennaMaps.Commands;
using System.Windows.Input;
using ViennaMaps.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.Measure;

namespace ViennaMaps.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        //Events - open additional windows
        public event EventHandler OnRequestOpen3DMap;
        public event EventHandler OnRequestOpen2DMap;
        public event EventHandler OnRequestOpenNewProfile;


        //Commands - view additional windows
        public ICommand View3DMapCmd { get; set; }
        public ICommand View2DMapCmd { get; set; }
        public ICommand ViewNewProfileCmd { get; set; }

        // Properties
        // public ObservableCollection<Location> LocationList { get; set; }
       
        public List<string> DistrictName {get; set; }

        public List<string> ProjectName { get; set; }

        public string SelectedLocation { get; set; }
        public string SelectedProject { get; set; }

        public MainViewModel()
        {
            //check this - LocationList vs District name
            //LocationList = new ObservableCollection<Location>();
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
            //empty list
            //LocationList.Clear();            

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                
                //show location ordered by District Number
                var locations = context.Location.OrderBy(l => l.DistrictNumber);
                // fill location list
                foreach (Location loc in locations)
                {
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

        //documentation: https://lvcharts.com/docs/wpf/2.0.0-beta.300/CartesianChart.Cartesian%20chart%20control#axes.labels-and-axes.labelers
        //allgemeine methode - füllt alle properties
        // vielleicht 1 absrakte basis klasse - ableitung für jede datenart
        public ISeries[] CountryAnalysis01 { get; set; }
        = new ISeries[]
        {
                new LineSeries<double>
                {
                    //get values from database, depending on the chosen analysis
                    Values = new double[] { 7644818, 7943489, 8002186, 8201359, 8351643, 8584926, 8858775 },
                    Fill = null,
                    TooltipLabelFormatter = (chartPoint) => $"Population: {chartPoint.PrimaryValue} inhabitants"
                }
        };

        public Axis[] CountryAnalysis01XAxes { get; set; }
              = new Axis[]
              {
                new Axis
                {
                    Labels = new string[] { "1990", "1995", "2000", "2005", "2010", "2015", "2020" }
                }
         };

    

        public ISeries[] CountryAnalysis02{ get; set; }
              = new ISeries[]
              {
                new PieSeries<double> { Values = new List<double> { 42.7 }, InnerRadius = 50, TooltipLabelFormatter = (chartPoint) =>$"{chartPoint.PrimaryValue}% are renting" },
                new PieSeries<double> { Values = new List<double> { 48.8 }, InnerRadius = 50, TooltipLabelFormatter = (chartPoint) =>$"{chartPoint.PrimaryValue}% are owning"},
                new PieSeries<double> { Values = new List<double> { 8.5 }, InnerRadius = 50, TooltipLabelFormatter = (chartPoint) =>$"{chartPoint.PrimaryValue}% are rent-free or in unpaid housing" },
                
              };
        
        public ISeries[] CountryAnalysis03 { get; set; }
            = new ISeries[]
            {
            new StackedColumnSeries<double>
            {
                Values = new List<double> { 24, 15.9 , 9.2, 7.8, 5.6, -1.1, -2.4, -3.9, -26.3 },
                Stroke = null,
                DataLabelsPaint = new SolidColorPaint(new SKColor(45, 45, 45)),
                Fill = new SolidColorPaint(SKColors.Teal),
                DataLabelsSize = 14,
                DataLabelsPosition = DataLabelsPosition.Middle,
                TooltipLabelFormatter = (chartPoint) => $"Living space per person: {chartPoint.PrimaryValue} m² to average"
            }};

        public Axis[] CountryAnalysis03XAxes { get; set; }
              = new Axis[]
              {
                new Axis
                {
                    Labels = new string[] { "Burgenland", "Lower Austria", "Upper Austria", "Carinthia", "Styria", "Voralberg", "Tyrol", "Salzburg", "Vienna" },
                    LabelsRotation = 90,
                     UnitWidth =1
                }
              };
        public ISeries[] CountryAnalysis04 { get; set; } =
{
        new ColumnSeries<double>
        {
            Values = new double[] { 4657, 154.4, 125.6, 88.6, 78.6, 76.4, 75.1, 60.4, 59.2 },
            Stroke = null,
            Fill = new SolidColorPaint(SKColors.CornflowerBlue),
            IgnoresBarPosition = true,
            TooltipLabelFormatter = (chartPoint) => $"Population Density: {chartPoint.PrimaryValue} inhabitants per square kilometer"
        },

    };        
        public Axis[] CountryAnalysis04XAxes { get; set; } =
        {
        new Axis
        {
            Labels = new string[] { "Vienna", "Voralberg", "Upper Austria" , "Lower Austria", "Salzburg", "Styria", "Burgenland", "Tyrol", "Carinthia" },
            LabelsRotation = 30
        }
    };
    }
}
