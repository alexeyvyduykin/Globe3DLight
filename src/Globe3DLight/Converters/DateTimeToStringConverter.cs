using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Globe3DLight.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        private readonly string _format = "dd-MMM-yyyy hh:mm:ss";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.ToString(_format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            }

            return DateTime.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeSpanToStringConverter : IValueConverter
    {
        private readonly string _format = @"mm\:ss";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(_format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            }

            return DateTime.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToDateConverter : IValueConverter
    {
        private readonly string _format = "dd-MMM-yyyy";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.ToString(_format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            }

            return DateTime.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class DateTimeToTimeConverter : IValueConverter
    {
        private readonly string _format = "hh:mm:ss";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.ToString(_format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            }

            return DateTime.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
