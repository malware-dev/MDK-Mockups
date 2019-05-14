using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MDK_UI.TemplateConverters;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace MDK_UI.Extensions
{
    static class ReflectionBindingExtensions
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
                            IsEnabled = !prop.IsReadOnly()
                        };

                        foreach (var value in Enum.GetValues(type))
                        {
                            comboBox.Items.Add(value);
                        }

                        var binding = prop.GetBinding(target);
                        //binding.Converter = new DisplayNameConverter();

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

        public static UIElement ToUIElement(this MethodInfo method, object target, MethodInfo parent = null)
        {
            var name = method.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
            var element = new Button
            {
                Content = name
            };

            method = parent ?? method;
            if (method.GetParameters().Any())
            {
                element.Click += UnsupportedMethod;
            }
            else
            {
                element.Click += (sender, args) =>
                {
                    var result = method.Invoke(target, new object[] { });

                    if (method.ReturnType != typeof(void))
                        element.InvokeMessageBox($"{name} returned:\n{result}", "Action Result", MessageBoxButton.OK, MessageBoxImage.Information);
                };
            }

            return element;
        }

        private static void InvokeMessageBox(this UIElement element, string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            element.Dispatcher.Invoke(() => MessageBox.Show(message, caption, button, icon));
        }

        private static RoutedEventHandler UnsupportedMethod { get; } = (sender, args) =>
        {
            (sender as UIElement).InvokeMessageBox("Executing actions with parameters is not yet supported.", "Not Supported", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        };

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
