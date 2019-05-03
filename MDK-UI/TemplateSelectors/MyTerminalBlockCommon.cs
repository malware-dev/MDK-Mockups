using System.Windows;
using System.Windows.Controls;

namespace MDK_UI.TemplateSelectors
{
    public class MyTerminalBlockCommon: DataTemplateSelector
    {
        private DisplayTemplateProvider TemplateProvider { get; } = new DisplayTemplateProvider();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement)
            {
                if (item == null)
                    return GetTemplate("NoBlockSelected");

                return GetTemplate("BlockProperties");
            }

            return null;
        }

        private DataTemplate GetTemplate(string template)
            => TemplateProvider["dt" + template] as DataTemplate;
    }
}
