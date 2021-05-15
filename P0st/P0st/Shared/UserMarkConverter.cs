using System;
using System.Globalization;
using OpenPr0gramm;
using Pr0mpp.Shared;
using Xamarin.Forms;

namespace P0st.Shared
{
    public class UserMarkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserMark? mark = null;
            
            if (value is UserMark m)
            {
                mark = m;
            }
            else if (value is int mi)
            {
                mark = (UserMark) mi;
            }

            if (mark == null)
                return null;

            var color = mark switch
            {
                UserMark.Unbekannt => Color.Gray,
                UserMark.Schwuchtel => Color.White,
                UserMark.Neuschwuchtel => Color.Violet,
                UserMark.Altschwuchtel => Color.FromHex("#5BB91C"),
                UserMark.Administrator => Color.FromHex("#FF9900"),
                UserMark.Gebannt => Color.DarkGray,
                UserMark.Fliesentisch => Color.SaddleBrown,
                UserMark.Mittelaltschwuchtel => Color.FromHex("#addc8d"),
                UserMark.AltHelfer => Color.FromHex("#ea9fa1"),
                _ => Color.Teal
            };
            
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NullBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}