using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfPipeGenerator
{
    public class ConverterGeometry : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            return Geometry.Parse(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
