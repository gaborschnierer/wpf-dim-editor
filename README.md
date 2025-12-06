# wpf-dim-editor

A WPF application demonstrating a numeric value comparison editor control built with .NET 7 and CommunityToolkit.Mvvm.

## Features

### NumericCompareEditor Control

A custom WPF user control for editing numeric values with comparison capabilities:

- **Editable TextBox**: Displays and allows editing of the current numeric value
- **Original Value Display**: Shows the original value in a muted style for comparison
- **Focus Popup**: When the TextBox gains focus, displays a popup with:
  - Original value
  - Current value
  - Delta (Δ) - the difference between current and original
  - Revert button to restore the original value
- **Keyboard Support**:
  - **Up Arrow**: Increment value by 1
  - **Down Arrow**: Decrement value by 1
  - **Esc**: Revert to original value
- **Basic Validation**: Numeric input validation

### Demo Application

The MainWindow demonstrates the control with:
- A canvas containing a blue rectangle
- Three NumericCompareEditor instances controlling:
  - X Position
  - Y Position
  - Width
- Real-time updates: changing values in the editors updates the rectangle properties

## Technologies

- .NET 7.0 (net7.0-windows)
- WPF (Windows Presentation Foundation)
- CommunityToolkit.Mvvm for MVVM implementation

## Building and Running

1. Ensure you have .NET 7 SDK installed
2. Clone the repository
3. Navigate to the WpfDimEditor directory
4. Build: `dotnet build`
5. Run: `dotnet run`

Note: This is a Windows-only application due to WPF requirements.
