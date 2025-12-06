using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Automation;

namespace wpf_dim_editor
{
    /// <summary>
    /// Interaction logic for NumericCompareEditor.xaml
    /// A reusable UserControl for inline numeric editing with comparison to original value
    /// </summary>
    public partial class NumericCompareEditor : UserControl
    {
        private string _lastValidText = "0";
        private bool _isUpdatingText = false;

        public NumericCompareEditor()
        {
            InitializeComponent();
            
            // Initialize with a default ViewModel if none is set
            if (DataContext == null)
            {
                ViewModel = new NumericCompareEditorViewModel();
            }
        }

        /// <summary>
        /// ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(NumericCompareEditorViewModel),
                typeof(NumericCompareEditor),
                new PropertyMetadata(null, OnViewModelChanged));

        /// <summary>
        /// ViewModel for this control
        /// </summary>
        public NumericCompareEditorViewModel? ViewModel
        {
            get => (NumericCompareEditorViewModel?)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericCompareEditor)d;
            control.DataContext = e.NewValue;
            
            if (e.NewValue is NumericCompareEditorViewModel vm)
            {
                // Initialize text box with current value
                control._lastValidText = vm.CurrentValue.ToString("0.###", CultureInfo.InvariantCulture);
                control._isUpdatingText = true;
                control.ValueTextBox.Text = control._lastValidText;
                control._isUpdatingText = false;
                
                // Update AutomationProperties for accessibility
                control.UpdateAccessibilityProperties();
            }
        }

        /// <summary>
        /// Update accessibility properties with current and original values
        /// </summary>
        private void UpdateAccessibilityProperties()
        {
            if (ViewModel != null)
            {
                var accessibleName = $"Dimension editor - Current: {ViewModel.CurrentValueText}, Original: {ViewModel.OriginalValueText}";
                AutomationProperties.SetName(this, accessibleName);
                AutomationProperties.SetName(ValueTextBox, accessibleName);
            }
        }

        /// <summary>
        /// Handle TextBox GotFocus - show comparison popup
        /// </summary>
        private void ValueTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ComparePopup.IsOpen = true;
            ValueTextBox.SelectAll();
        }

        /// <summary>
        /// Handle TextBox LostFocus - close popup and validate input
        /// </summary>
        private void ValueTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ComparePopup.IsOpen = false;
            ValidateAndUpdateValue();
        }

        /// <summary>
        /// Handle TextBox text changes
        /// </summary>
        private void ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText || ViewModel == null)
                return;

            // Try to parse in real-time for live preview (optional)
            // For now, we validate only on lost focus or Enter key
        }

        /// <summary>
        /// Handle keyboard input for nudging and special keys
        /// </summary>
        private void ValueTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ViewModel == null)
                return;

            // Determine step size based on modifiers
            double step = 1.0; // Default step
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                step = 10.0; // Larger step with Shift
            }
            else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                step = 0.1; // Fine adjustment with Ctrl
            }

            switch (e.Key)
            {
                case Key.Up:
                    ViewModel.Nudge(step);
                    UpdateTextBoxFromViewModel();
                    e.Handled = true;
                    break;

                case Key.Down:
                    ViewModel.Nudge(-step);
                    UpdateTextBoxFromViewModel();
                    e.Handled = true;
                    break;

                case Key.Escape:
                    ViewModel.RevertToOriginal();
                    UpdateTextBoxFromViewModel();
                    ComparePopup.IsOpen = false;
                    e.Handled = true;
                    break;

                case Key.Enter:
                    ValidateAndUpdateValue();
                    ComparePopup.IsOpen = false;
                    // Move focus away
                    ValueTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Validate and update the value from text input
        /// </summary>
        private void ValidateAndUpdateValue()
        {
            if (ViewModel == null)
                return;

            var text = ValueTextBox.Text.Trim();

            // Try to parse the input
            if (ViewModel.TryParseAndUpdate(text))
            {
                // Parse succeeded - update last valid text
                _lastValidText = ViewModel.CurrentValue.ToString("0.###", CultureInfo.InvariantCulture);
                UpdateAccessibilityProperties();
            }
            else
            {
                // Parse failed - revert to last valid text (non-blocking validation)
                _isUpdatingText = true;
                ValueTextBox.Text = _lastValidText;
                _isUpdatingText = false;
            }
        }

        /// <summary>
        /// Update TextBox text from ViewModel
        /// </summary>
        private void UpdateTextBoxFromViewModel()
        {
            if (ViewModel == null)
                return;

            _lastValidText = ViewModel.CurrentValue.ToString("0.###", CultureInfo.InvariantCulture);
            _isUpdatingText = true;
            ValueTextBox.Text = _lastValidText;
            _isUpdatingText = false;
            UpdateAccessibilityProperties();
        }
    }
}
