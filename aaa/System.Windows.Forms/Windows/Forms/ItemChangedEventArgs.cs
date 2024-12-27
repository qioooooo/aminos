using System;

namespace System.Windows.Forms
{
	// Token: 0x02000452 RID: 1106
	public class ItemChangedEventArgs : EventArgs
	{
		// Token: 0x060041A0 RID: 16800 RVA: 0x000EB191 File Offset: 0x000EA191
		internal ItemChangedEventArgs(int index)
		{
			this.index = index;
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060041A1 RID: 16801 RVA: 0x000EB1A0 File Offset: 0x000EA1A0
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x04001FA7 RID: 8103
		private int index;
	}
}
