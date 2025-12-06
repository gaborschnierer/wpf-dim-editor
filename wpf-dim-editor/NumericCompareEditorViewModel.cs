using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Globalization;

namespace wpf_dim_editor
{
    /// <summary>
    /// ViewModel for NumericCompareEditor control using CommunityToolkit.Mvvm
    /// </summary>
    public partial class NumericCompareEditorViewModel : ObservableObject
    {
        private double _originalValue;
        private double _currentValue;
        private string _unitSuffix = "mm";

        public NumericCompareEditorViewModel()
        {
            RevertCommand = new RelayCommand(RevertToOriginal);
        }

        /// <summary>
        /// Original value (read-only reference)
        /// </summary>
        public double OriginalValue
        {
            get => _originalValue;
            set
            {
                if (SetProperty(ref _originalValue, value))
                {
                    OnPropertyChanged(nameof(OriginalValueText));
                    OnPropertyChanged(nameof(Delta));
                    OnPropertyChanged(nameof(DeltaText));
                }
            }
        }

        /// <summary>
        /// Current editable value
        /// </summary>
        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                if (SetProperty(ref _currentValue, value))
                {
                    OnPropertyChanged(nameof(CurrentValueText));
                    OnPropertyChanged(nameof(Delta));
                    OnPropertyChanged(nameof(DeltaText));
                    OnPropertyChanged(nameof(DeltaBrush));
                    CurrentValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Unit suffix (e.g., "mm")
        /// </summary>
        public string UnitSuffix
        {
            get => _unitSuffix;
            set
            {
                if (SetProperty(ref _unitSuffix, value))
                {
                    OnPropertyChanged(nameof(OriginalValueText));
                    OnPropertyChanged(nameof(CurrentValueText));
                    OnPropertyChanged(nameof(DeltaText));
                }
            }
        }

        /// <summary>
        /// Formatted original value text
        /// </summary>
        public string OriginalValueText => FormatValue(OriginalValue);

        /// <summary>
        /// Formatted current value text
        /// </summary>
        public string CurrentValueText => FormatValue(CurrentValue);

        /// <summary>
        /// Delta between current and original
        /// </summary>
        public double Delta => CurrentValue - OriginalValue;

        /// <summary>
        /// Formatted delta text with sign and units
        /// </summary>
        public string DeltaText
        {
            get
            {
                var delta = Delta;
                var sign = delta >= 0 ? "+" : "";
                return $"{sign}{delta.ToString("0.###", CultureInfo.InvariantCulture)} {UnitSuffix}";
            }
        }

        /// <summary>
        /// Brush for delta text (green for positive, red for negative, gray for zero)
        /// </summary>
        public string DeltaBrush
        {
            get
            {
                var delta = Delta;
                if (Math.Abs(delta) < 0.001) return "Gray";
                return delta > 0 ? "Green" : "Red";
            }
        }

        /// <summary>
        /// Event raised when CurrentValue changes
        /// </summary>
        public event EventHandler? CurrentValueChanged;

        /// <summary>
        /// Command to revert current value to original
        /// </summary>
        public IRelayCommand RevertCommand { get; }

        /// <summary>
        /// Revert current value to original value
        /// </summary>
        public void RevertToOriginal()
        {
            CurrentValue = OriginalValue;
        }

        /// <summary>
        /// Try to parse user input and update current value
        /// </summary>
        public bool TryParseAndUpdate(string input)
        {
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            {
                CurrentValue = result;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Nudge current value by a step amount
        /// </summary>
        public void Nudge(double step)
        {
            CurrentValue += step;
        }

        /// <summary>
        /// Format a value with units
        /// </summary>
        private string FormatValue(double value)
        {
            return $"{value.ToString("0.###", CultureInfo.InvariantCulture)} {UnitSuffix}";
        }
    }
}
