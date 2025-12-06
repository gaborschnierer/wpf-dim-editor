# WPF Dimension Editor Prototype

A compact WPF prototype demonstrating an inline numeric editor for technical drawing dimensions. This application is a minimal working sample to prototype placement, editing, and comparison of numeric dimension values.

## What This Sample Demonstrates

This prototype showcases:

- **NumericCompareEditor UserControl**: A reusable WPF control for editing numeric dimension values with inline comparison
- **Compact inline layout**: Default view shows only the editable value and a muted original value label
- **Focus-activated comparison popup**: Shows original value, current value, delta (with sign and color), and a revert button
- **Keyboard interactions**: 
  - Up/Down arrow keys to nudge values (default step: 1.0)
  - Shift modifier for larger steps (10.0)
  - Ctrl modifier for fine adjustments (0.1)
  - Esc to revert to original value
  - Enter to commit and move to next field
- **Live preview**: Changes to dimension values immediately update the sample rectangle geometry
- **MVVM architecture**: Using CommunityToolkit.Mvvm for clean separation of concerns
- **Accessibility**: AutomationProperties for screen readers with current and original values
- **Non-blocking validation**: Invalid numeric input reverts to the last valid value
- **Multiple editor placement**: Demo shows three editors controlling width, height, and position

## Environment Requirements

- **.NET 7 SDK** (or later)
- Windows OS (WPF is Windows-only)
- Visual Studio 2022 (optional, for IDE experience)

## Build and Run

### Using .NET CLI

```bash
# Navigate to repository root
cd wpf-dim-editor

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project wpf-dim-editor
```

### Using Visual Studio

1. Open `wpf-dim-editor.sln` in Visual Studio 2022
2. Press F5 to build and run

## Project Structure

```
wpf-dim-editor/
├── wpf-dim-editor.sln                          # Visual Studio solution file
├── .gitignore                                   # Git ignore patterns for .NET/VS
├── README.md                                    # This file
└── wpf-dim-editor/                             # Main WPF project
    ├── wpf-dim-editor.csproj                   # Project file (net7.0-windows, UseWPF)
    ├── App.xaml / App.xaml.cs                  # Application entry point
    ├── MainWindow.xaml / MainWindow.xaml.cs    # Demo window with sample geometry
    ├── NumericCompareEditor.xaml               # UserControl XAML
    ├── NumericCompareEditor.xaml.cs            # UserControl code-behind
    ├── NumericCompareEditorViewModel.cs        # ViewModel using CommunityToolkit.Mvvm
    └── Properties/
        └── AssemblyInfo.cs                      # Platform attributes
```

## How to Use the Prototype

1. **Launch the application** - You'll see a technical drawing area with a blue rectangle
2. **Click on any dimension editor** - A popup will appear showing the comparison details
3. **Edit values**:
   - Type directly in the text box
   - Use Up/Down arrows to nudge (hold Shift for larger steps, Ctrl for finer steps)
   - Press Esc to revert to the original value
   - Press Enter to commit and move to the next field
4. **Watch the rectangle update** - Changes are reflected in real-time on the sample geometry

### Dimension Editors

- **Width Editor** (top): Controls the rectangle width
- **Height Editor** (right): Controls the rectangle height  
- **X Position Editor** (left): Controls the rectangle horizontal position

## Replacing the Placeholder Drawing

To integrate this prototype with real technical drawings:

### 1. Replace the Canvas Background

In `MainWindow.xaml`, replace the placeholder Canvas content:

```xml
<Canvas Grid.Row="1" x:Name="DrawingCanvas" Background="#F8F9FA">
    <!-- Replace with your drawing image -->
    <Image Source="path/to/your/drawing.png" 
           Canvas.Left="0" Canvas.Top="0"
           Stretch="None"/>
    
    <!-- Keep your NumericCompareEditor instances -->
    <local:NumericCompareEditor x:Name="Editor1" 
                                Canvas.Left="X" Canvas.Top="Y"/>
</Canvas>
```

### 2. Anchor Editors to Real Geometry

- Position editors using `Canvas.Left` and `Canvas.Top` properties
- Coordinates should match the dimension line positions on your drawing
- Consider reading coordinates from a configuration file or CAD data

### 3. Wire Editors to Real Geometry Data

In `MainWindow.xaml.cs`, replace the event handlers:

```csharp
private void DimensionViewModel_CurrentValueChanged(object? sender, EventArgs e)
{
    // Update your actual CAD geometry or regenerate drawing
    // UpdateCADModel(viewModel.CurrentValue);
    // RegenerateDrawing();
}
```

### 4. Load Dimension Data

Initialize editors from your CAD/drawing data:

```csharp
private void LoadDimensionsFromDrawing()
{
    var dimensions = DrawingLoader.GetDimensions("drawing.dwg");
    foreach (var dim in dimensions)
    {
        var editor = CreateEditor(dim);
        Canvas.SetLeft(editor, dim.Position.X);
        Canvas.SetTop(editor, dim.Position.Y);
        DrawingCanvas.Children.Add(editor);
    }
}
```

## Future Improvements

### AdornerLayer Placement
Instead of Canvas positioning, consider using WPF AdornerLayer for more flexible overlay placement:
- Editors can overlay directly on drawing elements
- Better Z-order management
- Automatic repositioning when drawing scales/pans

### Unit Toggle
Add support for multiple unit systems:
- Toggle between mm, inches, pixels
- Store base value in one unit, display in user preference
- Add unit selector dropdown to each editor

### Collision Detection and Resolution
For dense technical drawings:
- Detect overlapping editor instances
- Automatically adjust positions to avoid collisions
- Use "leader lines" to connect editors to their dimensions when displaced

### Additional Features
- **Undo/Redo**: Track dimension changes with command pattern
- **Batch Edit**: Select multiple dimensions and adjust together
- **Constraints**: Set min/max limits, enforce relationships (e.g., width ≤ container)
- **Formulas**: Link dimensions with expressions (e.g., "height = width * 1.5")
- **Tooltips**: Show dimension metadata (tolerance, notes, history)
- **Export**: Save modified dimensions back to CAD file formats

## Technical Notes

- **ViewModel Pattern**: Uses `CommunityToolkit.Mvvm` for `ObservableObject` and `IRelayCommand`
- **Data Binding**: Two-way binding between control and ViewModel
- **Number Formatting**: Uses `InvariantCulture` with "0.###" format (shows up to 3 decimal places)
- **Validation**: Non-blocking - invalid input reverts to previous valid value
- **Accessibility**: Implements `AutomationProperties.Name` for screen reader support
- **Color Coding**: Delta text uses green (positive), red (negative), gray (zero) - but also includes textual sign (+/-)

## License

This is a prototype/sample project for demonstration purposes.

## Author

Created for exploring WPF dimension editing UI patterns in technical drawing applications.