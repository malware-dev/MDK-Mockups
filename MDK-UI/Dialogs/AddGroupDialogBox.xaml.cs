using System.Windows;

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
