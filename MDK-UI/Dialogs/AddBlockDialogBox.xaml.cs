using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MDK_UI.MockupExtensions;
using IMyGridProgram = Sandbox.ModAPI.IMyGridProgram;
using IMyTerminalBlock = Sandbox.ModAPI.Ingame.IMyTerminalBlock;

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for AddBlockDialogBox.xaml
    /// </summary>
    public partial class AddBlockDialogBox : Window
    {
        private static Type ProgramType { get; } = typeof(IMyGridProgram);
        private static Type BaseType { get; } = typeof(IMyTerminalBlock);
        private static Type SelectorType { get; } = typeof(DisplayNameAttribute);
        private static Type OverriddenType { get; } = typeof(MockOverriddenAttribute);

        public delegate void BlockSubmittedEventHandler(object sender, string title, Type type);
        public event BlockSubmittedEventHandler OnSubmit;

        public IEnumerable<IMyTerminalBlock> AvailableTypes { get; }

        public AddBlockDialogBox()
        {
            // Load all assemblies.
            var types = AppDomain.CurrentDomain.GetAssemblies().AsQueryable()
                // Filter out assemblies containing types which implement IMyGridProgram.
                .Where(a => !a.GetTypes().Any(t => !t.IsAbstract && ProgramType.IsAssignableFrom(t)))
                // Select all types which implement IMyTerminalBlock.
                .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && BaseType.IsAssignableFrom(t)))
                // Filter to types with a DisplayNameAttribute decorator.
                .Where(t => t.CustomAttributes.Any(a => a.AttributeType == SelectorType))
                // Filter out types which have the MockOverriddenAttribute decorator.
                .Where(t => !t.CustomAttributes.Any(a => a.AttributeType == OverriddenType));

            AvailableTypes = types
                // Create an instance.
                .Select(t => Activator.CreateInstance(t))
                // Force a typecast.
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
            OnSubmit?.Invoke(this, BlockName.Text, BlockType.SelectedItem?.GetType());
            this.Close();
        }
    }
}
