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
using ViennaMaps.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace ViennaMaps.ViewModels
{
    /*
    abstract class AnalysisDiagrams
    {
        ISeries[] Analysis01 { get; set; }
        Axis[] Analysis01XAxes { get; set; }
        string Analysis01Label { get; set; }
        ISeries[] Analysis02 { get; set; }   
        Axis[] Analysis02XAxes { get; set; }
        string Analysis02Label { get; set; }
        ISeries[] Analysis03 { get; set; }
        Axis[] Analysis03XAxes { get; set; }
        string Analysis03Label { get; set; }
        ISeries[] Analysis04 { get; set; }
        Axis[] Analysis04XAxes { get; set; }
        string Analysis04Label { get; set; }

    }

   class AnalysisTypes : AnalysisDiagrams
   {
        public ISeries[] Analysis01 { get; set; } 
        public Axis[] Analysis01XAxes { get; set; }
        public string Analysis01Label { get; set; }
        public ISeries[] Analysis02 { get; set; }
        public  Axis[] Analysis02XAxes { get; set; }
        public string Analysis02Label { get; set; }
        public ISeries[] Analysis03 { get; set; }
        public Axis[] Analysis03XAxes { get; set; }
        string Analysis03Label { get; set; }
        public ISeries[] Analysis04 { get; set; }
        public Axis[] Analysis04XAxes { get; set; }
        string Analysis04Label { get; set; }
    }*/
    
    internal class MainViewModel : BaseViewModel
    {

       //Events 
        public event EventHandler OnRequestOpen3DMap;
        public event EventHandler OnRequestOpen2DMap;
        public event EventHandler OnRequestClose;


        //Commands
        public ICommand View3DMapCmd { get; set; }
        public ICommand View2DMapCmd { get; set; }
        public ICommand ExitCmd { get; set; }


        // Properties     
        public string SelectedLocation { get; set; }
        public string SelectedProject { get; set; }
        public List<ISeries[]> AnalysisDiagram { get; set; }
        public List<Axis[]> AnalysisXAxes { get; set; }
        public List<string> AnalysisLabel { get; set; }

        //Variables
        public List <double>[] _observableValues;
        List<string> _axisLabels;
        string _tooltip;

        public MainViewModel(string project, string location)
        {
            SelectedLocation = location;
            SelectedProject = project;

            _observableValues = new List<double>[12];
            _axisLabels = new List<string>();

            View3DMapCmd = new RelayCommand(View3DMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ExitCmd = new RelayCommand(Exit);

            AnalysisDiagram = new List<ISeries[]>();
            AnalysisXAxes = new List<Axis[]>();
            AnalysisLabel = new List<string>();

            //TO DO: Add i<12 when I inserted all data in the database
            //creating the analysis diagram for all 12 analysis slots
            FillAnalysis(1);
            FillAnalysis(2);
            for (int i = 1; i < 2; i++)
            {
                
            }
        }

        #region Open/Close Windows
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

        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }
        #endregion

        public void FillAnalysis( int analysisUIlocation)
        {
            //clear any old values from the list
                
            _axisLabels.Clear();
            _observableValues[analysisUIlocation - 1] = new List<double>();
            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                //where untereinader
                var liste = context.ProjectAnalysisView.Include(p=>p.Label).Where(p => p.DistrictName == SelectedLocation).Where(p => p.ProjectName == SelectedProject).Where(p => p.UilocationName == analysisUIlocation);
                
                //Analysis Label is the title of the Analyis
                AnalysisLabel.Add(liste.FirstOrDefault().AnalysisName);
                
                //tooltip will be shown when hovering over the analysis with the mouse 
                _tooltip = liste.FirstOrDefault().MeasurmentUnit;

                //add the data to the analysis diagram and to the axis labels
                foreach (var item in liste)
                {                   
                    _observableValues[analysisUIlocation-1].Add(double.Parse(item.Value));
                    _axisLabels.Add(item.Label);
                }
            }

            //TO DO: switch statement for diagram type (line vs pie chart)
            //create analysis diagram
            if (analysisUIlocation == 1)
            {
                ISeries[] analysis = new ISeries[]
                 {
                new LineSeries<double>
                {
                    Values = _observableValues[analysisUIlocation-1],
                    Fill = null,
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip}"
                }
                 };
                AnalysisDiagram.Add(analysis);
            }
            //TO DO: switch statement 
            //create axis with labels for the diagram
            Axis[] axis = new Axis[]
            {
                  new Axis
                  {
                    Labels = _axisLabels
                  }
            };

            //add the new diagram and axis to the list
          
            AnalysisXAxes.Add(axis);                   
        }

        #region Old - To Input in the Database

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

        #endregion
    }
}
