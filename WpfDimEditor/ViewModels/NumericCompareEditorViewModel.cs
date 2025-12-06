using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfDimEditor.ViewModels;

public partial class NumericCompareEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private double currentValue;

    [ObservableProperty]
    private double originalValue;

    public double Delta => CurrentValue - OriginalValue;

    partial void OnCurrentValueChanged(double value)
    {
        OnPropertyChanged(nameof(Delta));
    }

    partial void OnOriginalValueChanged(double value)
    {
        OnPropertyChanged(nameof(Delta));
    }

    [RelayCommand]
    private void Revert()
    {
        CurrentValue = OriginalValue;
    }

    [RelayCommand]
    private void Increment()
    {
        CurrentValue += 1;
    }

    [RelayCommand]
    private void Decrement()
    {
        CurrentValue -= 1;
    }
}
