using System.Windows;
using System.Windows.Controls;
using MDK_UI.MockupExtensions;

namespace MDK_UI.TemplateSelectors
{
    public class MyTerminalBlockDetail: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement)
            {
                if (item == null)
                    return null;

                if (item is IMockupDataTemplateProvider provider)
                    return provider.DataTemplate;
            }

            return null;
        }
    }
}
