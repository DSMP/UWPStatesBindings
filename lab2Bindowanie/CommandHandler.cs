using System;
using System.Windows.Input;

namespace lab2Bindowanie
{
	internal class CommandHandler : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private Action _action;

		public CommandHandler(Action action)
		{
			this._action = action;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			this._action();
		}
	}
}