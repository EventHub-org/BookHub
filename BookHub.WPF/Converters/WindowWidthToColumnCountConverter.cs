using System;
using System.Globalization;
using System.Windows.Data;

namespace BookHub.WPF.Converters
{
    public class WindowWidthToColumnCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                // Визначення кількості стовпців залежно від ширини вікна
                if (width < 600)
                    return 1;
                else if (width < 900)
                    return 2;
                else if (width < 1200)
                    return 3;
                else
                    return 4;
            }
            return 1; // default case
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
