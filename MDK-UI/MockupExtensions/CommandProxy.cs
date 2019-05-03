using System;
using System.Windows.Input;

namespace IngameScript.Mockups.Blocks
{
    public class CommandProxy : ICommand
    {
        private Action Proxy { get; }
        private Func<bool> CanExecuteTest { get; } = () => true;

        public CommandProxy(Action proxy, Func<bool> canExecute = null)
        {
            Proxy = proxy;
            if (canExecute != null)
            {
                CanExecuteTest = canExecute;
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => CanExecuteTest();
        public void Execute(object parameter) => Proxy();

        public void OnExecuteChange() => CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
