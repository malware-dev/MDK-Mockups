using System.Windows;

namespace MDK_UI.MockupExtensions
{
    public interface IMockupDataTemplateProvider
    {
        string TemplateDisplayName { get; }
        string DataTemplateName { get; }
    }
}
