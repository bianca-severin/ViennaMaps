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
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.Measure;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;
using ViennaMaps.Models;
using System.Data.Entity;
using System.Diagnostics;
using LiveChartsCore.Geo;


namespace ViennaMaps.ViewModels
{

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
        ObservableCollection <ObservableValue>[] _observableValues;
        List<string>[] _axisLabels;
        string[] _tooltip = new string[12];
        
        public MainViewModel(string project, string location)
        {
            SelectedLocation = location;
            SelectedProject = project;

            _observableValues = new ObservableCollection<ObservableValue>[12];
            _axisLabels = new List<string>[12];
            

            View3DMapCmd = new RelayCommand(View3DMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ExitCmd = new RelayCommand(Exit);

            AnalysisDiagram = new List<ISeries[]>();
            AnalysisXAxes = new List<Axis[]>();
            AnalysisLabel = new List<string>();

            //TO DO: Add i<12 when I inserted all data in the database
            //creating the analysis diagram for all 12 analysis slots

            for (int i = 0; i <= 2; i++)
            {
                FillAnalysis(i);
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

            _axisLabels[analysisUIlocation] = new List<string>();
            _observableValues[analysisUIlocation] = new ObservableCollection<ObservableValue>();
            string _diagramType;

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                //where untereinader
                var liste = context.ProjectAnalysisView.Include(p=>p.Label).Where(p => p.DistrictName == SelectedLocation).Where(p => p.ProjectName == SelectedProject).Where(p => p.UilocationName == (analysisUIlocation+1));
                Trace.WriteLine("objects in the list: " + liste.Count() + "analyis ui location: " + analysisUIlocation);    
                //Analysis Label is the title of the Analyis
                 AnalysisLabel.Add(liste.FirstOrDefault().AnalysisName);

                //tooltip will be shown when hovering over the analysis with the mouse 
                _tooltip[analysisUIlocation]=liste.FirstOrDefault().MeasurmentUnit;
                _diagramType = liste.FirstOrDefault().DiagramType;

                //add the data to the analysis diagram and to the axis labels
                foreach (var item in liste)
                {                   
                    _observableValues[analysisUIlocation].Add(new(double.Parse(item.Value)));
                    _axisLabels[analysisUIlocation].Add(item.Label);
                }
            }

            //TO DO: switch statement for diagram type (line vs pie chart)
            //create analysis diagram
            if (_diagramType == "LineSeriesFill")
            {
                ISeries[] analysis = new ISeries[]
                {
                new LineSeries<ObservableValue>
                {
                    Values = _observableValues[analysisUIlocation],
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUIlocation]}"
                }
                };
                AnalysisDiagram.Add(analysis);
            }
            else if(_diagramType == "LineSeriesDashed")
            {
                var strokeThickness = 3;
                var strokeDashArray = new float[] { 3 * strokeThickness, 2 * strokeThickness };

                ISeries[] analysis = new ISeries[]
                {
                new LineSeries<ObservableValue>
                    {
                    Values = _observableValues[analysisUIlocation],
                    Fill = null,
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUIlocation]}",
                    Stroke = new SolidColorPaint
                    {
                            Color = SKColors.Crimson,
                            StrokeCap = SKStrokeCap.Round,
                            StrokeThickness = 3,
                            PathEffect = new DashEffect(strokeDashArray)
                    },
                    GeometryStroke= new SolidColorPaint
                    { 
                        Color= SKColors.Crimson 
                    }
                    } };
                AnalysisDiagram.Add(analysis);
            }
            else if (_diagramType == "StackedColumnSeries")
            {
                ISeries[] analysis = new ISeries[]
                {
                new StackedColumnSeries<ObservableValue>
                    {
                    Values =  _observableValues[analysisUIlocation],
                    Stroke = null,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    Fill = new SolidColorPaint(SKColors.Teal),
                    DataLabelsSize = 14,
                    DataLabelsPosition = DataLabelsPosition.Middle,
                    TooltipLabelFormatter = (chartPoint) => $"Living space per person: {chartPoint.PrimaryValue} m² to average"
                    }};
                AnalysisDiagram.Add(analysis);
            }
            else if(_diagramType == "ColumnSeries")
            {
                //TO DO: Add population density to database
                ISeries[] analysis =
{
                    new ColumnSeries<double>
                    {
                        Values = new double[] { 4657, 154.4, 125.6, 88.6, 78.6, 76.4, 75.1, 60.4, 59.2 },
                        Stroke = null,
                        Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                        IgnoresBarPosition = true,
                        TooltipLabelFormatter = (chartPoint) => $"Population Density: {chartPoint.PrimaryValue} inhabitants per square kilometer"
                    } };
                AnalysisDiagram.Add(analysis);
            }


            // create axis with labels for the diagram
            Axis[] axis;

            if (_diagramType == "StackedColumnSeries")
            {
                axis = new Axis[]
                {
                    new Axis
                    {
                        Labels = _axisLabels[analysisUIlocation],
                        LabelsRotation = 90,
                        UnitWidth =1
                    }
                };
            }
            else {
                axis = new Axis[]
                {
                    new Axis
                    {
                        Labels = _axisLabels[analysisUIlocation]
                    }
                };
            }


            // add the new diagram and axis to the list          
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
