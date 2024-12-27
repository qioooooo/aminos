using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020005D6 RID: 1494
	[ComVisible(true)]
	public class PropertyValueChangedEventArgs : EventArgs
	{
		// Token: 0x06004E3C RID: 20028 RVA: 0x00121001 File Offset: 0x00120001
		public PropertyValueChangedEventArgs(GridItem changedItem, object oldValue)
		{
			this.changedItem = changedItem;
			this.oldValue = oldValue;
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06004E3D RID: 20029 RVA: 0x00121017 File Offset: 0x00120017
		public GridItem ChangedItem
		{
			get
			{
				return this.changedItem;
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0012101F File Offset: 0x0012001F
		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x040032B2 RID: 12978
		private readonly GridItem changedItem;

		// Token: 0x040032B3 RID: 12979
		private object oldValue;
	}
}
