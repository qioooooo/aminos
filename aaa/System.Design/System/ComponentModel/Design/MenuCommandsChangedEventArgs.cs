using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000133 RID: 307
	[ComVisible(true)]
	public class MenuCommandsChangedEventArgs : EventArgs
	{
		// Token: 0x06000BFA RID: 3066 RVA: 0x0002ED3B File Offset: 0x0002DD3B
		public MenuCommandsChangedEventArgs(MenuCommandsChangedType changeType, MenuCommand command)
		{
			this.changeType = changeType;
			this.command = command;
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0002ED51 File Offset: 0x0002DD51
		public MenuCommandsChangedType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0002ED59 File Offset: 0x0002DD59
		public MenuCommand Command
		{
			get
			{
				return this.command;
			}
		}

		// Token: 0x04000E73 RID: 3699
		private MenuCommand command;

		// Token: 0x04000E74 RID: 3700
		private MenuCommandsChangedType changeType;
	}
}
