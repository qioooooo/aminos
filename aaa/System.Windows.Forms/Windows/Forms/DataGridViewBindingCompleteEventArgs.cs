using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000307 RID: 775
	public class DataGridViewBindingCompleteEventArgs : EventArgs
	{
		// Token: 0x06003191 RID: 12689 RVA: 0x000AABF7 File Offset: 0x000A9BF7
		public DataGridViewBindingCompleteEventArgs(ListChangedType listChangedType)
		{
			this.listChangedType = listChangedType;
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x000AAC06 File Offset: 0x000A9C06
		public ListChangedType ListChangedType
		{
			get
			{
				return this.listChangedType;
			}
		}

		// Token: 0x04001A51 RID: 6737
		private ListChangedType listChangedType;
	}
}
