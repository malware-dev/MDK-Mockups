using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace MDK_UI.TemplateConverters
{
    public sealed class DisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            var type = value.GetType();
            var meta = type.GetCustomAttribute<MetadataTypeAttribute>();

            var propertyName = parameter as string;
            if (string.IsNullOrEmpty(propertyName))
            {
                var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
                if (attribute == null)
                    return new ArgumentOutOfRangeException(nameof(parameter), parameter, $"Class \"{type.Name}\" has no associated DisplayName attribute.").ToString();

                return attribute.DisplayName;
            }
            else
            {
                var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    return new ArgumentOutOfRangeException(nameof(parameter), parameter, $"Property \"{propertyName}\" not found in type \"{type.Name}\".").ToString();

                var attribute = property.GetCustomAttribute<DisplayNameAttribute>() ?? meta?.MetadataClassType?.GetProperty(propertyName)?.GetCustomAttribute<DisplayNameAttribute>();

                if (attribute == null)
                {
                    return new ArgumentOutOfRangeException(nameof(parameter), parameter, $"Property \"{propertyName}\" of type \"{type.Name}\" has no associated DisplayName attribute.").ToString();
                }
                
                return attribute.DisplayName;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
