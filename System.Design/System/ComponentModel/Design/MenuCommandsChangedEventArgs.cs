using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	[ComVisible(true)]
	public class MenuCommandsChangedEventArgs : EventArgs
	{
		public MenuCommandsChangedEventArgs(MenuCommandsChangedType changeType, MenuCommand command)
		{
			this.changeType = changeType;
			this.command = command;
		}

		public MenuCommandsChangedType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		public MenuCommand Command
		{
			get
			{
				return this.command;
			}
		}

		private MenuCommand command;

		private MenuCommandsChangedType changeType;
	}
}
