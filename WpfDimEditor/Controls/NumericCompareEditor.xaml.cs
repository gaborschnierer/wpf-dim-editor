using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfDimEditor.ViewModels;

namespace WpfDimEditor.Controls;

public partial class NumericCompareEditor : UserControl
{
    public NumericCompareEditor()
    {
        InitializeComponent();
        DataContext = new NumericCompareEditorViewModel();
    }

    public NumericCompareEditorViewModel ViewModel => (NumericCompareEditorViewModel)DataContext;

    private void ValueTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        var viewModel = ViewModel;
        
        switch (e.Key)
        {
            case Key.Up:
                viewModel.IncrementCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Down:
                viewModel.DecrementCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Escape:
                viewModel.RevertCommand.Execute(null);
                e.Handled = true;
                break;
        }
    }

    private void ValueTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        DetailsPopup.IsOpen = true;
    }

    private void ValueTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        DetailsPopup.IsOpen = false;
    }
}
