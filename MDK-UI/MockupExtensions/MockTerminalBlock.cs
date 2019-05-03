using System.Windows;
using MDK_UI.MockupExtensions;

namespace IngameScript.Mockups.Base
{
    public partial class MockTerminalBlock: IMockupDataTemplateProvider
    {
        public virtual string DataTemplateName { get; } = "UnsupportedBlock";
        public virtual string TemplateDisplayName => GetType().Name.Substring(4);
    }
}
