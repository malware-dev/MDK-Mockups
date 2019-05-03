using System.Windows;
using System.Windows.Controls;

namespace MDK_UI.TemplateSelectors
{
    public class MyTerminalBlockCommon: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element)
            {
                if (item == null)
                    return GetTemplate(element, "NoBlockSelected");

                return GetTemplate(element, "BlockProperties");
            }

            return null;
        }

        private DataTemplate GetTemplate(FrameworkElement element, string template)
            => element.FindResource("dt" + template) as DataTemplate;
    }
}
