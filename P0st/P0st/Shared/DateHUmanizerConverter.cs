using System;
using System.Globalization;
using Pr0mpp.Shared;
using Xamarin.Forms;

namespace P0st.Shared
{
    public class DateHUmanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;

            if (date == null)
                return null;

            return date.AsReadableString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}