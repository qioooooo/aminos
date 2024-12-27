using System;

namespace System.Windows.Forms
{
	// Token: 0x020002B8 RID: 696
	public class ConvertEventArgs : EventArgs
	{
		// Token: 0x0600264F RID: 9807 RVA: 0x0005D5FE File Offset: 0x0005C5FE
		public ConvertEventArgs(object value, Type desiredType)
		{
			this.value = value;
			this.desiredType = desiredType;
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x0005D614 File Offset: 0x0005C614
		// (set) Token: 0x06002651 RID: 9809 RVA: 0x0005D61C File Offset: 0x0005C61C
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x0005D625 File Offset: 0x0005C625
		public Type DesiredType
		{
			get
			{
				return this.desiredType;
			}
		}

		// Token: 0x0400163D RID: 5693
		private object value;

		// Token: 0x0400163E RID: 5694
		private Type desiredType;
	}
}
