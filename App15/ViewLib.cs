using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace App15
{
    class ViewLib
    {
        public static readonly string defaultImage = "ms-appx:///Assets/default.png";
    }

    public class SelectedToOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null)
            {
                var skillIsNew = (bool)value;
                if (skillIsNew)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PathToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null)
            {
                string url = (string)value;
                if (!url.Equals(string.Empty))
                    return (new BitmapImage(new Uri(url, UriKind.Absolute)));
                else
                    return (new BitmapImage(new Uri(ViewLib.defaultImage, UriKind.Absolute)));
            }
            else
            {
                return (new BitmapImage(new Uri(ViewLib.defaultImage, UriKind.Absolute)));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
