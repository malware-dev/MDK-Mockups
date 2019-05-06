using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IngameScript.Mockups.Base;
using MDK_UI.MockupExtensions;

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for AddBlockDialogBox.xaml
    /// </summary>
    public partial class AddBlockDialogBox : Window
    {
        private static Type BaseType { get; } = typeof(IMockupDataTemplateProvider);
        private static Type OverriddenType { get; } = typeof(MockOverriddenAttribute);

        public delegate void BlockSubmittedEventHandler(object sender, string title, Type type);
        public event BlockSubmittedEventHandler OnSubmit;

        public IEnumerable<IMockupDataTemplateProvider> AvailableTypes { get; }

        public AddBlockDialogBox()
        {
            AvailableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && BaseType.IsAssignableFrom(t)))
                .Where(t => !t.CustomAttributes.Any(a => a.AttributeType == OverriddenType))
                .Select(t => Activator.CreateInstance(t))
                .OfType<IMockupDataTemplateProvider>()
                .ToList();

            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            OnSubmit?.Invoke(this, "", null);
            this.Close();
        }

        private void BtSubmit_Click(object sender, RoutedEventArgs e)
        {
            OnSubmit?.Invoke(this, BlockName.Text, BlockType.SelectedItem.GetType());
            this.Close();
        }
    }
}
