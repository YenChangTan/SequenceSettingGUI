using System;
using System.Globalization;
using Avalonia.Controls.Converters;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SequenceSettingGUI.Converters;

public class BooleanToStatusFillConverter : IValueConverter
{

    public Brush NoColor = new SolidColorBrush(Colors.White);
    public Brush Green = new SolidColorBrush(Colors.Green);
    public Brush Red = new SolidColorBrush(Colors.Red);

    public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
    {
        if (value is bool IsGood)
        {
            if (IsGood)
            {
                return Green;
            }
            else
            {
                return Red;
            }
        }
        else
        {
            return NoColor;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo) =>
        Avalonia.Data.BindingOperations.DoNothing;
}
