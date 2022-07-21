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

        // Variables
        ObservableCollection<ObservableValue>[] _observableValues;
        List<string>[] _axisLabels;
        string[] _tooltip = new string[12];
        string _diagramType;

        public MainViewModel(string project, string location)
        {
            SelectedLocation = location;
            SelectedProject = project;

            View3DMapCmd = new RelayCommand(View3DMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ExitCmd = new RelayCommand(Exit);

            _observableValues = new ObservableCollection<ObservableValue>[12];
            _axisLabels = new List<string>[12];

            AnalysisDiagram = new List<ISeries[]>();
            AnalysisXAxes = new List<Axis[]>();
            AnalysisLabel = new List<string>();

            // creating the analysis chart for all 12 analysis slots
            for (int i = 0; i < 12; i++)
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

        // FillAnalyis fills the analysis slot with data from the database
        public void FillAnalysis(int analysisUILocation)
        {
            // list of values for the analysis charts             
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

                // tooltip will be shown when hovering over the analysis chart with the mouse in the Main Window
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
                ISeries[] analysis = new ISeries[]
                {
                    new LineSeries<ObservableValue>
                    {
                        Values = _observableValues[analysisUILocation],
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}"
                    }
                };
                AnalysisDiagram.Add(analysis);
            }
            else if (_diagramType == "LineSeriesDashed")
            {
                var strokeThickness = 3;
                var strokeDashArray = new float[] { 3 * strokeThickness, 2 * strokeThickness };

                ISeries[] analysis = new ISeries[]
                {
                    new LineSeries<ObservableValue>
                    {
                        Values = _observableValues[analysisUILocation],
                        Fill = null,
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}",
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
                    }
                };
                AnalysisDiagram.Add(analysis);
            }
            else if (_diagramType == "StackedColumnSeries")
            {
                ISeries[] analysis = new ISeries[]
                {
                    new StackedColumnSeries<ObservableValue>
                    {
                        Values =  _observableValues[analysisUILocation],
                        Stroke = null,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        Fill = new SolidColorPaint(SKColors.Orange),
                        DataLabelsSize = 14,
                        DataLabelsPosition = DataLabelsPosition.Middle,
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}"
                    }
                };
                AnalysisDiagram.Add(analysis);
            }
            else if (_diagramType == "ColumnSeries")
            {
                ISeries[] analysis =
{
                    new ColumnSeries<ObservableValue>
                    {
                        Values =  _observableValues[analysisUILocation],
                        Stroke = null,
                        Fill = new SolidColorPaint(SKColors.Teal),
                        IgnoresBarPosition = true,
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}",
                        DataLabelsPosition = DataLabelsPosition.Top,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    }
                };
                AnalysisDiagram.Add(analysis);
            }
            else if (_diagramType == "RowSeries")
            {
                ISeries[] analysis =
{
                    new RowSeries <ObservableValue>
                    {
                        Values =  _observableValues[analysisUILocation],
                        Stroke = null,
                        Fill = new SolidColorPaint(SKColors.Teal),
                        DataLabelsSize = 14,
                        DataLabelsPosition = DataLabelsPosition.Middle,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue} {_tooltip[analysisUILocation]}",
                    }
                };
                AnalysisDiagram.Add(analysis);
            }

            // create axis with labels for the chosen diagram type   
            Axis[] axis;
            if (_diagramType == "StackedColumnSeries")
            {
                axis = new Axis[]
                {
                    new Axis
                    {
                        Labels = _axisLabels[analysisUILocation],
                        LabelsRotation = 90,
                        UnitWidth = 1
                    }
                };
            }
            else if (_diagramType == "ColumnSeries")
            {
                axis = new Axis[]
                {
                    new Axis
                    {
                        Labels = _axisLabels[analysisUILocation],
                        LabelsRotation = 30,
                        UnitWidth = 1
                    }
                };
            }
            else
            {
                axis = new Axis[]
                {
                    new Axis
                    {
                        Labels = _axisLabels[analysisUILocation]
                    }
                };
            }

            // add the new diagram and axis to the list          
            AnalysisXAxes.Add(axis);
        }

    }
}
