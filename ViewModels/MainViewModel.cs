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
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;

namespace ViennaMaps.ViewModels
{
    abstract class AnalysisDiagrams
    {
        ISeries[] Analysis01 { get; set; }
        Axis[] Analysis01XAxes { get; set; }
        ISeries[] Analysis02 { get; set; }
        Axis[] Analysis02XAxes { get; set; }
        ISeries[] Analysis03 { get; set; }
        Axis[] Analysis03XAxes { get; set; }
        ISeries[] Analysis04 { get; set; }
        Axis[] Analysis04XAxes { get; set; }

    }

   class AnalysisTypes : AnalysisDiagrams
   {
        public ISeries[] Analysis01 { get; set; } 
        public Axis[] Analysis01XAxes { get; set; }
        public ISeries[] Analysis02 { get; set; }
        public  Axis[] Analysis02XAxes { get; set; }
        public ISeries[] Analysis03 { get; set; }
        public Axis[] Analysis03XAxes { get; set; }
        public ISeries[] Analysis04 { get; set; }
        public Axis[] Analysis04XAxes { get; set; }

    }
    
    internal class MainViewModel : BaseViewModel
    {

        ObservableCollection<ObservableValue> _observableValues;
        private AnalysisTypes _countryAnalysis = new AnalysisTypes();
        private AnalysisTypes _cityAnalysis = new AnalysisTypes();
        private AnalysisTypes _districtAnalysis = new AnalysisTypes();

        public ISeries[] CountryAnalysis01 { get; set; }
        public Axis[] CountryAnalysis01XAxes { get; set; }
        /*public ISeries[] CountryAnalysis02 { get; set; }
        public Axis[] CountryAnalysis02XAxes { get; set; }
        public ISeries[] CountryAnalysis03 { get; set; }
        public Axis[] CountryAnalysis03XAxes { get; set; }
        public ISeries[] CountryAnalysis04 { get; set; }
        public Axis[] CountryAnalysis04XAxes { get; set; }
        */




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
            //TO DO: check this - LocationList vs District name
            //LocationList = new ObservableCollection<Location>();
            DistrictName = new List<string>();
            ProjectName = new List<string>();
            View3DMapCmd = new RelayCommand(View3DMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ViewNewProfileCmd = new RelayCommand(ViewNewProfile);

            FillLocationList();
            FillProfileList();
            

            _observableValues = new ObservableCollection<ObservableValue>
            {
            // Use the ObservableValue or ObservablePoint types to let the chart listen for property changes // mark
            // or use any INotifyPropertyChanged implementation // mark
            //https://github.com/beto-rodriguez/LiveCharts2/blob/master/samples/ViewModelsSamples/Lines/AutoUpdate/ViewModel.cs
           // new ObservableValue(2),
            new (7644818),
            new (7943489),
            new (8002186),
            new ( 8201359),
            new (8351643),
            new (8584926),
            new (8858775)
            };

            FillAnalysis();


        }

        #region Open Windows
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
        #endregion

        #region Fill Dropdowns
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
        #endregion
        public void FillAnalysis()
        {

            _countryAnalysis.Analysis01 = new ISeries[]
             {
                new LineSeries<ObservableValue>
                {
                    //get values from database, depending on the chosen analysis
                    Values = _observableValues,
                    Fill = null,
                    TooltipLabelFormatter = (chartPoint) => $"Population: {chartPoint.PrimaryValue} inhabitants"
                }
             };
            CountryAnalysis01 = _countryAnalysis.Analysis01;

            _countryAnalysis.Analysis01XAxes= new Axis[]
              {
                new Axis
                {
                    Labels = new string[] { "1990", "1995", "2000", "2005", "2010", "2015", "2020" }
                }
            };
            CountryAnalysis01XAxes = _countryAnalysis.Analysis01XAxes;

        }

        //documentation: https://lvcharts.com/docs/wpf/2.0.0-beta.300/CartesianChart.Cartesian%20chart%20control#axes.labels-and-axes.labelers
        //allgemeine methode - füllt alle properties
        // vielleicht 1 absrakte basis klasse - ableitung für jede datenart
        
    

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
