using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000497 RID: 1175
	[ComVisible(true)]
	public class ListViewItemMouseHoverEventArgs : EventArgs
	{
		// Token: 0x0600466C RID: 18028 RVA: 0x000FFD1F File Offset: 0x000FED1F
		public ListViewItemMouseHoverEventArgs(ListViewItem item)
		{
			this.item = item;
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x0600466D RID: 18029 RVA: 0x000FFD2E File Offset: 0x000FED2E
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04002198 RID: 8600
		private readonly ListViewItem item;
	}
}
