using System;
using System.Windows;
using System.Windows.Controls;

namespace wpf_dim_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Demo window showing NumericCompareEditor instances controlling a sample rectangle
    /// </summary>
    public partial class MainWindow : Window
    {
        // ViewModels for each dimension editor
        private NumericCompareEditorViewModel? _widthViewModel;
        private NumericCompareEditorViewModel? _heightViewModel;
        private NumericCompareEditorViewModel? _xPositionViewModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDimensionEditors();
        }

        /// <summary>
        /// Initialize the dimension editors with ViewModels and wire them to the rectangle
        /// </summary>
        private void InitializeDimensionEditors()
        {
            // Width Editor - controls rectangle width
            _widthViewModel = new NumericCompareEditorViewModel
            {
                OriginalValue = 400.0,
                CurrentValue = 400.0,
                UnitSuffix = "mm"
            };
            _widthViewModel.CurrentValueChanged += WidthViewModel_CurrentValueChanged;
            WidthEditor.ViewModel = _widthViewModel;

            // Height Editor - controls rectangle height
            _heightViewModel = new NumericCompareEditorViewModel
            {
                OriginalValue = 250.0,
                CurrentValue = 250.0,
                UnitSuffix = "mm"
            };
            _heightViewModel.CurrentValueChanged += HeightViewModel_CurrentValueChanged;
            HeightEditor.ViewModel = _heightViewModel;

            // X Position Editor - controls rectangle left position
            _xPositionViewModel = new NumericCompareEditorViewModel
            {
                OriginalValue = 200.0,
                CurrentValue = 200.0,
                UnitSuffix = "mm"
            };
            _xPositionViewModel.CurrentValueChanged += XPositionViewModel_CurrentValueChanged;
            XPositionEditor.ViewModel = _xPositionViewModel;
        }

        /// <summary>
        /// Handle width changes - update rectangle width and dimension lines
        /// </summary>
        private void WidthViewModel_CurrentValueChanged(object? sender, EventArgs e)
        {
            if (_widthViewModel != null)
            {
                var newWidth = Math.Max(50, _widthViewModel.CurrentValue); // Minimum width
                SampleRectangle.Width = newWidth;
                
                // Update width dimension line endpoints
                var canvas = DrawingCanvas;
                var left = Canvas.GetLeft(SampleRectangle);
                var top = Canvas.GetTop(SampleRectangle);
                
                // Find the width dimension line (first Line element)
                foreach (var child in canvas.Children)
                {
                    if (child is System.Windows.Shapes.Line line && 
                        Math.Abs(line.Y1 - 130) < 1 && Math.Abs(line.Y2 - 130) < 1)
                    {
                        line.X2 = left + newWidth;
                        break;
                    }
                }
                
                // Update right endpoint tick mark
                var lineIndex = 0;
                foreach (var child in canvas.Children)
                {
                    if (child is System.Windows.Shapes.Line line)
                    {
                        lineIndex++;
                        // This is the right tick mark (3rd line)
                        if (lineIndex == 3)
                        {
                            line.X1 = left + newWidth;
                            line.X2 = left + newWidth;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle height changes - update rectangle height and dimension lines
        /// </summary>
        private void HeightViewModel_CurrentValueChanged(object? sender, EventArgs e)
        {
            if (_heightViewModel != null)
            {
                var newHeight = Math.Max(50, _heightViewModel.CurrentValue); // Minimum height
                SampleRectangle.Height = newHeight;
                
                // Update height dimension line
                var canvas = DrawingCanvas;
                var left = Canvas.GetLeft(SampleRectangle);
                var top = Canvas.GetTop(SampleRectangle);
                
                var lineIndex = 0;
                foreach (var child in canvas.Children)
                {
                    if (child is System.Windows.Shapes.Line line)
                    {
                        lineIndex++;
                        // Height dimension line (4th line)
                        if (lineIndex == 4 && Math.Abs(line.X1 - 670) < 1)
                        {
                            line.Y2 = top + newHeight;
                        }
                        // Height bottom tick mark (6th line)
                        else if (lineIndex == 6 && Math.Abs(line.X1 - 665) < 1)
                        {
                            line.Y1 = top + newHeight;
                            line.Y2 = top + newHeight;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle X position changes - update rectangle position and dimension line
        /// </summary>
        private void XPositionViewModel_CurrentValueChanged(object? sender, EventArgs e)
        {
            if (_xPositionViewModel != null)
            {
                var newX = Math.Max(50, _xPositionViewModel.CurrentValue);
                Canvas.SetLeft(SampleRectangle, newX);
                
                // Update X position dimension line
                var canvas = DrawingCanvas;
                var lineIndex = 0;
                foreach (var child in canvas.Children)
                {
                    if (child is System.Windows.Shapes.Line line)
                    {
                        lineIndex++;
                        // X position dimension line (7th line - dashed orange)
                        if (lineIndex == 7)
                        {
                            line.X2 = newX;
                        }
                        // X position right tick mark (9th line)
                        else if (lineIndex == 9)
                        {
                            line.X1 = newX;
                            line.X2 = newX;
                        }
                    }
                }
                
                // Also update width dimension line when X position changes
                if (_widthViewModel != null)
                {
                    lineIndex = 0;
                    foreach (var child in canvas.Children)
                    {
                        if (child is System.Windows.Shapes.Line line)
                        {
                            lineIndex++;
                            // Width dimension line (1st line)
                            if (lineIndex == 1)
                            {
                                line.X1 = newX;
                                line.X2 = newX + SampleRectangle.Width;
                            }
                            // Width left tick mark (2nd line)
                            else if (lineIndex == 2)
                            {
                                line.X1 = newX;
                                line.X2 = newX;
                            }
                            // Width right tick mark (3rd line)
                            else if (lineIndex == 3)
                            {
                                line.X1 = newX + SampleRectangle.Width;
                                line.X2 = newX + SampleRectangle.Width;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
