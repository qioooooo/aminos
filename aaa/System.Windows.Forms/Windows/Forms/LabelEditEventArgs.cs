using System;

namespace System.Windows.Forms
{
	// Token: 0x02000464 RID: 1124
	public class LabelEditEventArgs : EventArgs
	{
		// Token: 0x0600425C RID: 16988 RVA: 0x000ED3A7 File Offset: 0x000EC3A7
		public LabelEditEventArgs(int item)
		{
			this.item = item;
			this.label = null;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x000ED3BD File Offset: 0x000EC3BD
		public LabelEditEventArgs(int item, string label)
		{
			this.item = item;
			this.label = label;
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x000ED3D3 File Offset: 0x000EC3D3
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x0600425F RID: 16991 RVA: 0x000ED3DB File Offset: 0x000EC3DB
		public int Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x000ED3E3 File Offset: 0x000EC3E3
		// (set) Token: 0x06004261 RID: 16993 RVA: 0x000ED3EB File Offset: 0x000EC3EB
		public bool CancelEdit
		{
			get
			{
				return this.cancelEdit;
			}
			set
			{
				this.cancelEdit = value;
			}
		}

		// Token: 0x04002098 RID: 8344
		private readonly string label;

		// Token: 0x04002099 RID: 8345
		private readonly int item;

		// Token: 0x0400209A RID: 8346
		private bool cancelEdit;
	}
}
