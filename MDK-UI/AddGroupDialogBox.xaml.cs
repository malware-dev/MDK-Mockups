using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for TextDialog.xaml
    /// </summary>
    public partial class AddGroupDialogBox : Window
    {
        public delegate void GroupSubmittedEventHandler(object sender, string title);
        public event GroupSubmittedEventHandler OnSubmit;

        public AddGroupDialogBox()
        {
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            OnSubmit?.Invoke(this, "");
            this.Close();
        }

        private void BtSubmit_Click(object sender, RoutedEventArgs e)
        {
            OnSubmit?.Invoke(this, GroupName.Text);
            this.Close();
        }
    }
}
