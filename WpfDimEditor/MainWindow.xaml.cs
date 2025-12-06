using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDimEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeEditors();
    }

    private void InitializeEditors()
    {
        // Set initial values for the editors
        EditorX.ViewModel.OriginalValue = 100;
        EditorX.ViewModel.CurrentValue = 100;

        EditorY.ViewModel.OriginalValue = 100;
        EditorY.ViewModel.CurrentValue = 100;

        EditorWidth.ViewModel.OriginalValue = 200;
        EditorWidth.ViewModel.CurrentValue = 200;

        // Wire up property change handlers to update the rectangle
        EditorX.ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(EditorX.ViewModel.CurrentValue))
            {
                Canvas.SetLeft(DemoRectangle, EditorX.ViewModel.CurrentValue);
            }
        };

        EditorY.ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(EditorY.ViewModel.CurrentValue))
            {
                Canvas.SetTop(DemoRectangle, EditorY.ViewModel.CurrentValue);
            }
        };

        EditorWidth.ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(EditorWidth.ViewModel.CurrentValue))
            {
                DemoRectangle.Width = EditorWidth.ViewModel.CurrentValue;
            }
        };
    }
}