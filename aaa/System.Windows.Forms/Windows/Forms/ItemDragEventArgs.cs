using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000458 RID: 1112
	[ComVisible(true)]
	public class ItemDragEventArgs : EventArgs
	{
		// Token: 0x060041B5 RID: 16821 RVA: 0x000EB1FD File Offset: 0x000EA1FD
		public ItemDragEventArgs(MouseButtons button)
		{
			this.button = button;
			this.item = null;
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x000EB213 File Offset: 0x000EA213
		public ItemDragEventArgs(MouseButtons button, object item)
		{
			this.button = button;
			this.item = item;
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x000EB229 File Offset: 0x000EA229
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x000EB231 File Offset: 0x000EA231
		public object Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04001FAC RID: 8108
		private readonly MouseButtons button;

		// Token: 0x04001FAD RID: 8109
		private readonly object item;
	}
}
