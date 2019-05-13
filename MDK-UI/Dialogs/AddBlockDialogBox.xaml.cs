using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MDK_UI.MockupExtensions;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for AddBlockDialogBox.xaml
    /// </summary>
    public partial class AddBlockDialogBox : Window
    {
        private static Type BaseType { get; } = typeof(IMyTerminalBlock);
        private static Type SelectorType { get; } = typeof(DisplayNameAttribute);
        private static Type OverriddenType { get; } = typeof(MockOverriddenAttribute);

        public delegate void BlockSubmittedEventHandler(object sender, string title, Type type);
        public event BlockSubmittedEventHandler OnSubmit;

        public IEnumerable<IMyTerminalBlock> AvailableTypes { get; }

        public AddBlockDialogBox()
        {
            AvailableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && BaseType.IsAssignableFrom(t)))
                .Where(t => t.CustomAttributes.Any(a => a.AttributeType == SelectorType))
                .Where(t => !t.CustomAttributes.Any(a => a.AttributeType == OverriddenType))
                .Select(t => Activator.CreateInstance(t))
                .OfType<IMyTerminalBlock>()
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
