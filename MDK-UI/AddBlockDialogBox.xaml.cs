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

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for AddBlockDialogBox.xaml
    /// </summary>
    public partial class AddBlockDialogBox : Window
    {
        public delegate void BlockSubmittedEventHandler(object sender, string title, Type type);
        public event BlockSubmittedEventHandler OnSubmit;

        public IEnumerable<Type> AvailableTypes { get; }

        public AddBlockDialogBox()
        {
            var baseType = typeof(MockTerminalBlock);
            AvailableTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))).ToList();
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            OnSubmit?.Invoke(this, "", null);
            this.Close();
        }

        private void BtSubmit_Click(object sender, RoutedEventArgs e)
        {
            OnSubmit?.Invoke(this, BlockName.Text, BlockType.SelectedItem as Type);
            this.Close();
        }
    }
}
