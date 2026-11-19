using Calculator.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.WPF
{
    /// <summary>
    /// Interaction logic for GraphingWindow.xaml
    /// </summary>
    public partial class GraphingWindow : Window
    {

        private readonly GraphingVM _vm = new();


        private double _xMin = -10, _xMax = 10, _yMin = -10, _yMax = 10;

        public GraphingWindow()
        {
            InitializeComponent();
            DataContext =_vm;

        }

        private void SetRange_Click(object sender, RoutedEventArgs e)
        {
            var rangeInput = new GraphRangeScreen(_xMin, _xMax, _yMin, _yMax) { Owner = this };
            if (rangeInput.ShowDialog() == true)
            {
                _xMin = rangeInput.XMin; _xMax = rangeInput.XMax;
                _yMin = rangeInput.YMin; _yMax = rangeInput.YMax;
                DrawAxes();
                PlotFunction(x => _vm.Expressions[0].ComputeFor(x), Brushes.Blue);
                PlotFunction(x => _vm.Expressions[1].ComputeFor(x), Brushes.Crimson);
                PlotFunction(x => _vm.Expressions[2].ComputeFor(x), Brushes.Green);
                PlotFunction(x => _vm.Expressions[3].ComputeFor(x), Brushes.Purple);
                PlotFunction(x => _vm.Expressions[4].ComputeFor(x), Brushes.Orange);
            }

        }

        private void drawbutton_Click(object sender, RoutedEventArgs e)
        {
            DrawAxes();
            PlotFunction(x => _vm.Expressions[0].ComputeFor(x), Brushes.Blue);
            PlotFunction(x => _vm.Expressions[1].ComputeFor(x), Brushes.Crimson);
            PlotFunction(x => _vm.Expressions[2].ComputeFor(x), Brushes.Green);
            PlotFunction(x => _vm.Expressions[3].ComputeFor(x), Brushes.Purple);
            PlotFunction(x => _vm.Expressions[4].ComputeFor(x), Brushes.Orange);
        }
        
        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawAxes();
        }

        private double ToPx(double x) => (x - _xMin) / (_xMax - _xMin) * GraphCanvas.ActualWidth;
        private double ToPy(double y) => (_yMax - y) / (_yMax - _yMin) * GraphCanvas.ActualHeight;





        /*
         * Draws the axes onto the screen. gets min/max x&y from the GraphRange screen
         */

        private void DrawAxes()
        {
            GraphCanvas.Children.Clear();
            double width = GraphCanvas.ActualWidth;
            double height = GraphCanvas.ActualHeight;

            double xStep = NiceStep(_xMax - _xMin); // in maths units not pixels
            double yStep = NiceStep(_yMax - _yMin);

            double axisX = Math.Clamp(ToPx(0), 0, width);
            double axisY = Math.Clamp(ToPy(0), 0, height);



            long firstVerticalLine = (long)Math.Ceiling(_xMin / xStep); // same as below
            long lastVerticalLine = (long)Math.Floor(_xMax / xStep); // _xMax / xStep is how many xSteps right from the origin the last step is, and we floor it because we have to make sure the last line is not beyond xMax

            for (long i = firstVerticalLine; i <= lastVerticalLine; i++)
            {
                double x = i * xStep; // the x coordinate of a line should be in multiples of x step
                double px = ToPx(x);
                AddLine(px, 0, px, height, Brushes.LightGray, 1); // add the line and number at bnoottom
                AddLabel(x.ToString("G4"), px+2, Math.Min(axisY + 2, height - 18)); // the random addition/subtraction stuff is just experimenting so the label doesnt sit ON hte line
            }



            long firstHorizontalLine = (long)Math.Ceiling(_yMin / yStep);
            long lastHorizontalLine = (long)Math.Floor(_yMax / yStep);

            // Horizontal grid lines + y labels
            for (long i = firstHorizontalLine; i <= lastHorizontalLine; i++) // exact same logic as the vertical stuff but horzintal now
            {
                double y = i * yStep;
                double py = ToPy(y);
                AddLine(0, py, width, py, Brushes.LightGray, 1);

                if (y != 0) // avoid repeating 0 at origin
                {
                    AddLabel(y.ToString("G4"), Math.Min(axisX + 4, width - 35), py - 9);
                }
            }


            // Axes (only drawn if 0 is within the visible range)
            if (_yMin <= 0 && 0 <= _yMax) AddLine(0, ToPy(0), width, ToPy(0), Brushes.Black, 2);
            if (_xMin <= 0 && 0 <= _xMax) AddLine(ToPx(0), 0, ToPx(0), height, Brushes.Black, 2);

        }
   

        // helper method that makes a line which draws a point from (x1 y1) to (x2 y2)
        private void AddLine(double x1, double y1, double x2, double y2, Brush stroke, double thickness)
        {
            GraphCanvas.Children.Add(new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = stroke,
                StrokeThickness = thickness
            });
        }


        // lowkey had to google how to add a label but its defined by the top and bottom point
        private void AddLabel(string text, double left, double top)
        {
            var tb = new TextBlock { Text = text, FontSize = 11, Foreground = Brushes.DimGray };
            Canvas.SetLeft(tb, left);
            Canvas.SetTop(tb, top);
            GraphCanvas.Children.Add(tb);
        }



        /*
         * 
         * on the x and y axis, we don't want to "non nice" or ugly to read values, so for example,
         * if the total range of x values is 17 but we have only 10 lines, we don't want each one to read 1.7,
         * we want to correct to the nearest clean number instead
         * 
         */
        private static double NiceStep(double range)
        {
            double rawStep = range / 10; // aim for roughly 10 gridlines across the screen

            // the power of 10 just below rawStep, e.g. 0.17 -> 0.1 and 17 -> 10
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));

            double ratio = rawStep / magnitude; // always between 1 and 10

            // round the ratio to the nearest "nice" multiplier
            double multiplier;
            if (ratio < 1.5)
            {
                multiplier = 1;
            }
            else if (ratio < 3.5)
            {
                multiplier = 2;
            }
            else if (ratio < 7.5)
            {
                multiplier = 5;
            }
            else
            {
                multiplier = 10;
            }

            return multiplier * magnitude;
        }


        private void PlotFunction(Func<double, double> f, Brush brush)
        {
            double width = GraphCanvas.ActualWidth;
            double height = GraphCanvas.ActualHeight;
            var points = new PointCollection();
            double? prevPy = null;

            void Flush()
            {
                if (points.Count > 1)
                {
                    GraphCanvas.Children.Add(new Polyline
                    {
                        Points = points,
                        Stroke = brush,
                        StrokeThickness = 2
                    });
                }
                points = new PointCollection();
                prevPy = null;
            }

            for (double px = 0; px <= width; px++)
            {
                double x = _xMin + px / width * (_xMax - _xMin);
                double y = f(x);

                // no point here (e.g. sqrt of a negative), so end the current line and start a new one later
                if (double.IsNaN(y) || double.IsInfinity(y))
                {
                    Flush();
                    continue;
                }

                double py = ToPy(y);

                // end the line when the value jumps by loads
                if (prevPy.HasValue && Math.Abs(py - prevPy.Value) > height * 2)
                {
                    Flush();
                }

                points.Add(new Point(px, py));
                prevPy = py;
            }
            Flush();
        }


    }
}
