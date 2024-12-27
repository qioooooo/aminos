using System;

namespace System.DirectoryServices
{
	// Token: 0x0200002B RID: 43
	public class DirectoryVirtualListViewContext
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00006031 File Offset: 0x00005031
		public DirectoryVirtualListViewContext()
			: this(new byte[0])
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006040 File Offset: 0x00005040
		internal DirectoryVirtualListViewContext(byte[] context)
		{
			if (context == null)
			{
				this.context = new byte[0];
				return;
			}
			this.context = new byte[context.Length];
			for (int i = 0; i < context.Length; i++)
			{
				this.context[i] = context[i];
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000608C File Offset: 0x0000508C
		public DirectoryVirtualListViewContext Copy()
		{
			return new DirectoryVirtualListViewContext(this.context);
		}

		// Token: 0x040001BA RID: 442
		internal byte[] context;
	}
}
