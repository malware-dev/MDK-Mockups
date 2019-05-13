using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MDK_UI.TemplateConverters;
using Xceed.Wpf.Toolkit;

namespace MDK_UI.Extensions
{
    static class PropertyInfoExtensions
    {
        public static UIElement ToUiElement(this PropertyInfo prop, object target, PropertyInfo parent = null)
        {
            switch (prop.GetDataType())
            {
                case DataType.MultilineText:
                    var textArea = new TextBox()
                    {
                        Height = 200,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        AcceptsReturn = true,
                        IsReadOnly = prop.IsReadOnly()
                    };

                    textArea.SetBinding(TextBox.TextProperty, prop.GetBinding(target));
                    return textArea;
                default:
                    var type = parent?.PropertyType ?? prop.PropertyType;

                    if (type == typeof(bool))
                    {
                        var checkBox = new CheckBox
                        {
                            IsEnabled = !prop.IsReadOnly(),
                            VerticalAlignment = VerticalAlignment.Center
                        };

                        checkBox.SetBinding(ToggleButton.IsCheckedProperty, prop.GetBinding(target));
                        return checkBox;
                    }
                    else if (type == typeof(int) || type == typeof(float))
                    {
                        if (prop.HasAttribute<RangeAttribute>())
                        {
                            var values = prop.GetCustomAttribute<RangeAttribute>();
                            var range = new Slider
                            {
                                Minimum = (double)values.Minimum,
                                Maximum = (double)values.Maximum,
                                IsEnabled = !prop.IsReadOnly(),
                                VerticalAlignment = VerticalAlignment.Center
                            };

                            if (type == typeof(int))
                            {
                                range.TickFrequency = 1;
                            }
                            else
                            {
                                range.TickFrequency = 0.01;
                            }

                            range.SetBinding(RangeBase.ValueProperty, prop.GetBinding(target));
                            return range;
                        }
                        else
                        {
                            var range = new SingleUpDown
                            {
                                IsReadOnly = !prop.IsReadOnly(),
                                VerticalAlignment = VerticalAlignment.Center
                            };

                            if (type == typeof(int))
                            {
                                range.Increment = 1;
                            }
                            else
                            {
                                range.Increment = 0.01f;
                            }

                            range.SetBinding(SingleUpDown.ValueProperty, prop.GetBinding(target));
                            return range;
                        }
                    }
                    else if (type == typeof(VRageMath.Color))
                    {
                        var colorPicker = new ColorPicker
                        {
                            VerticalAlignment = VerticalAlignment.Center
                        };

                        var binder = prop.GetBinding(target);
                        binder.Converter = new ColorConverter();

                        colorPicker.SetBinding(ColorPicker.SelectedColorProperty, binder);
                        return colorPicker;
                    }
                    else if (type == typeof(string))
                    {
                        var textBox = new TextBox
                        {
                            AcceptsReturn = false,
                            IsReadOnly = prop.IsReadOnly()
                        };

                        textBox.SetBinding(TextBox.TextProperty, prop.GetBinding(target));

                        return textBox;
                    }
                    else if (type.IsEnum)
                    {
                        var comboBox = new ComboBox
                        {
                            IsReadOnly = prop.IsReadOnly(),
                        };

                        foreach (var value in Enum.GetValues(type))
                        {
                            comboBox.Items.Add(value);
                        }

                        var binding = prop.GetBinding(target);
                        binding.Converter = new DisplayNameConverter();

                        comboBox.SetBinding(Selector.SelectedItemProperty, binding);
                        return comboBox;
                    }
                    else
                    {
                        return new TextBlock()
                        {
                            Text = $"Unsupported property type {type.Name}."
                        };
                    }
            }
        }

        private static bool IsReadOnly(this PropertyInfo prop)
            => !prop.CanWrite || (prop.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? false);

        private static DataType GetDataType(this PropertyInfo prop)
            => prop.GetCustomAttribute<DataTypeAttribute>()?.DataType ?? DataType.Text;

        private static Binding GetBinding(this PropertyInfo prop, object target) 
            => new Binding(prop.Name)
            {
                Mode = prop.IsReadOnly() ? BindingMode.OneWay : BindingMode.Default,
                Source = target
            };
    }
}
