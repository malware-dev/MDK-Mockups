using System.Windows;
using System.Windows.Controls;
using MDK_UI.MockupExtensions;

namespace MDK_UI.TemplateSelectors
{
    public class MyTerminalBlockDetail: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element
                && item is IMockupDataTemplateProvider provider)
            {
                return GetTemplate(element, provider.DataTemplateName);
            }

            return null;
        }

        private DataTemplate GetTemplate(FrameworkElement element, string template)
            => element.FindResource("dt" + template) as DataTemplate;
    }
}
