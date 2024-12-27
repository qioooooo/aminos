using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000456 RID: 1110
	[ComVisible(true)]
	public class ItemCheckEventArgs : EventArgs
	{
		// Token: 0x060041AC RID: 16812 RVA: 0x000EB1BF File Offset: 0x000EA1BF
		public ItemCheckEventArgs(int index, CheckState newCheckValue, CheckState currentValue)
		{
			this.index = index;
			this.newValue = newCheckValue;
			this.currentValue = currentValue;
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x000EB1DC File Offset: 0x000EA1DC
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x000EB1E4 File Offset: 0x000EA1E4
		// (set) Token: 0x060041AF RID: 16815 RVA: 0x000EB1EC File Offset: 0x000EA1EC
		public CheckState NewValue
		{
			get
			{
				return this.newValue;
			}
			set
			{
				this.newValue = value;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x000EB1F5 File Offset: 0x000EA1F5
		public CheckState CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x04001FA9 RID: 8105
		private readonly int index;

		// Token: 0x04001FAA RID: 8106
		private CheckState newValue;

		// Token: 0x04001FAB RID: 8107
		private readonly CheckState currentValue;
	}
}
