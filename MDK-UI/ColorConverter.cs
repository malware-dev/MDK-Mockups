using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MDK_UI
{
    class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is VRageMath.Color color && targetType == typeof(Color?))
            {
                return new Color()
                {
                    R = color.R,
                    G = color.G,
                    B = color.B,
                    A = color.A
                };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color && targetType == typeof(VRageMath.Color))
            {
                return new VRageMath.Color()
                {
                    R = color.R,
                    G = color.G,
                    B = color.B,
                    A = color.A
                };
            }

            return null;
        }
    }
}
