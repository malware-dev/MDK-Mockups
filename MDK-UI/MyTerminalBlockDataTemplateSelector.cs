using System.Windows;
using System.Windows.Controls;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace MDK_UI
{
    public class MyTerminalBlockDataTemplateSelector: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element)
            {
                if (item == null)
                    return null;

                if (item is IMyAirVent)
                    return GetTemplate(element, "AirVent");
                
                if (item is IMyDoor)
                    return GetTemplate(element, "Door");

                if (item is IMyLightingBlock)
                    return GetTemplate(element, "LightingBlock");

                if (item is IMyTextSurface)
                    return GetTemplate(element, "TextSurface");

                return GetTemplate(element, "UnsupportedBlock");
            }

            return null;
        }

        private DataTemplate GetTemplate(FrameworkElement element, string template) 
            => element.FindResource("dt" + template) as DataTemplate;
    }
}
