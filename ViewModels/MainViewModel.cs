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
    /// <summary>
    /// Viewmodel for <c>MainWindow</c> 
    /// The class creates charts for the chosen location and profile in the ChooseProfileViewModel
    /// All the necessary data for the charts is saved in a View in the database. The class access the data and draws all 12 charts displayed
    /// The user can open two windows, Map2DWindow and Map3DWindow
    /// </summary>
    internal class MainViewModel : BaseViewModel
    {
        // Events 
        public event EventHandler OnRequestOpen3DMap;
        public event EventHandler OnRequestOpen2DMap;
        public event EventHandler OnRequestClose;

        // Commands
        public ICommand View3DMapCmd { get; set; }
        public ICommand View2DMapCmd { get; set; }
        public ICommand ExitCmd { get; set; }

        // Properties     
        public string SelectedLocation { get; set; }
        public string SelectedProject { get; set; }
        public List<ISeries[]> AnalysisDiagram { get; set; }
        public List<Axis[]> AnalysisXAxes { get; set; }
        public List<string> AnalysisLabel { get; set; }

        // Attributes
        ObservableCollection<ObservableValue>[] _observableValues;
        List<string>[] _axisLabels;
        string[] _tooltip = new string[12];
        string _diagramType;
        ISeries[] _analysis;
        Axis[] _axis;

        // Constructor
        public MainViewModel(string project, string location)
        {
            // assign data from the ChooseProfileViewModel to the properties
            SelectedLocation = location;
            SelectedProject = project;

            // bind commands between the ViewModel and UI elements
            View3DMapCmd = new RelayCommand(View3DMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ExitCmd = new RelayCommand(Exit);                      

            // create lists for the analysis charts, labels (titles), x-axes, values and axes labels
            AnalysisDiagram = new List<ISeries[]>();
            AnalysisXAxes = new List<Axis[]>();
            AnalysisLabel = new List<string>();
            _observableValues = new ObservableCollection<ObservableValue>[12];
            _axisLabels = new List<string>[12];

            // creating the analysis chart for all 12 analysis slots
            for (int i = 0; i < 12; i++)
            {
                FillAnalysis(i);
            }
        }

        // the method FillAnalyis fills the analysis slots with data from the database
        public void FillAnalysis(int analysisUILocation)
        {
            //  new list of values for the analysis charts pf this user interface location            
            _observableValues[analysisUILocation] = new ObservableCollection<ObservableValue>();
            _axisLabels[analysisUILocation] = new List<string>();

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                // selecting analysis values for the chosen district and project and for the current analyisUILocation
                var liste = context.ProjectAnalysisView.Include(p => p.Label)
                           .Where(p => p.DistrictName == SelectedLocation)
                           .Where(p => p.ProjectName == SelectedProject)
                           .Where(p => p.UilocationName == (analysisUILocation + 1));

                // AnalysisLabel is the displayed title of the analyis chart in the Main Window
                AnalysisLabel.Add(liste.FirstOrDefault().AnalysisName);

                // tooltip will be shown when hovering over the analysis chart in the Main Window
                _tooltip[analysisUILocation] = liste.FirstOrDefault().MeasurmentUnit;

                // depending on the diagram type, the initialization of the diagram will be different (see switch statement below)
                _diagramType = liste.FirstOrDefault().DiagramType;

                // add the data to the analysis diagram and to the axis labels
                foreach (var item in liste)
                {
                    _observableValues[analysisUILocation].Add(new(double.Parse(item.Value)));
                    _axisLabels[analysisUILocation].Add(item.Label);
                }
            }

            // create analysis chart for the chosen diagram type           
            if (_diagramType == "LineSeriesFill")
            {
                _analysis = new ISeries[]
                {
                    // create line chart
                    new LineSeries<ObservableValue>
                    {
                        // asign the values to the chart
                        Values = _observableValues[analysisUILocation],
                        // show the value of a point and the measurment unit when hovering over
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}"
                    }
                };
            }
            else if (_diagramType == "LineSeriesDashed")
            {
                // create a new line type (dashed)
                var strokeThickness = 3;
                var strokeDashArray = new float[] { 3 * strokeThickness, 2 * strokeThickness };

                _analysis = new ISeries[]
                {
                    // create line chart
                    new LineSeries<ObservableValue>
                    {
                        // assign values to the chart 
                        Values = _observableValues[analysisUILocation],
                        // show the value of a point and the measurment unit when hovering over
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}",
                        // style chart (red color line and points, no fill, dashed)
                        Fill = null,                        
                        Stroke = new SolidColorPaint
                        {
                            Color = SKColors.Crimson,
                            StrokeCap = SKStrokeCap.Round,
                            StrokeThickness = 3,
                            PathEffect = new DashEffect(strokeDashArray)
                        },
                        GeometryStroke= new SolidColorPaint
                        {
                            Color = SKColors.Crimson
                        }
                    }
                };
            }
            else if (_diagramType == "StackedColumnSeries")
            {
                _analysis = new ISeries[]
                {
                    // create stacked columns chart
                    new StackedColumnSeries<ObservableValue>
                    {
                        // assign values to the chart
                        Values =  _observableValues[analysisUILocation],
                        // style the chart (orange color, white data labels in the middle)
                        Stroke = null,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        Fill = new SolidColorPaint(SKColors.Orange),
                        DataLabelsSize = 14,
                        DataLabelsPosition = DataLabelsPosition.Middle,
                        // show the value of a point and the measurment unit when hovering over
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}"
                    }
                };
            }
            else if (_diagramType == "ColumnSeries")
            {
                _analysis = new ISeries[] {
                    // create column chart
                    new ColumnSeries<ObservableValue>
                    {
                        // assign values to the chart
                        Values =  _observableValues[analysisUILocation],
                        // style the chart (no stroke, teal color, white data labels at the top)
                        Stroke = null,
                        Fill = new SolidColorPaint(SKColors.Teal),
                        IgnoresBarPosition = true,
                        DataLabelsPosition = DataLabelsPosition.Top,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        // show the value of a point and the measurment unit when hovering over
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}",
                    }
                };
            }
            else if (_diagramType == "RowSeries")
            {
                _analysis = new ISeries[] {
                    // create vertical column chart (row chart)
                    new RowSeries <ObservableValue>
                    {
                        // assign values to the chart
                        Values =  _observableValues[analysisUILocation],
                        // style the chart (no stroke, teal color, white data labels in the middle)
                        Stroke = null,
                        Fill = new SolidColorPaint(SKColors.Teal),
                        DataLabelsSize = 14,
                        DataLabelsPosition = DataLabelsPosition.Middle,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        // show the value of a point and the measurment unit when hovering over
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}",
                    }
                };
            }

            // create axis with labels for the chosen diagram type              
            if (_diagramType == "StackedColumnSeries")
            {
                // axis with 90 degree label rotation for the stacked columns chart
                _axis = new Axis[]
                {
                    new Axis
                    {
                        // assign values to the labels
                        Labels = _axisLabels[analysisUILocation],
                        // rotate label
                        LabelsRotation = 90,
                        UnitWidth = 1
                    }
                };
            }
            else if (_diagramType == "ColumnSeries")
            {
                // axis with 30 degree label rotation for the column chart
                _axis = new Axis[]
                {
                    new Axis
                    {
                        // assign values to the labels
                        Labels = _axisLabels[analysisUILocation],
                        // rotate label
                        LabelsRotation = 30,
                        UnitWidth = 1
                    }
                };
            }
            else
            {
                // default axis
                _axis = new Axis[]
                {
                    new Axis
                    {
                        // assign values to the labels
                        Labels = _axisLabels[analysisUILocation]
                    }
                };
            }

            // add the new diagram and axis to the list     
            AnalysisDiagram.Add(_analysis);
            AnalysisXAxes.Add(_axis);
        }

        #region Open/Close Windows
        // View3DMap opens the Map3DWindow
        private void View3DMap()
        {
            if (OnRequestOpen3DMap != null)
                OnRequestOpen3DMap(this, new EventArgs());
        }

        // View2DMap opens the Map2DWindow
        private void View2DMap()
        {
            if (OnRequestOpen2DMap != null)
                OnRequestOpen2DMap(this, new EventArgs());
        }

        // Exit the current window
        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }
        #endregion     

    }
}
