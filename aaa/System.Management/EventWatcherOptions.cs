using System;

namespace System.Management
{
	// Token: 0x0200002F RID: 47
	public class EventWatcherOptions : ManagementOptions
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000875F File Offset: 0x0000775F
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00008767 File Offset: 0x00007767
		public int BlockSize
		{
			get
			{
				return this.blockSize;
			}
			set
			{
				this.blockSize = value;
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008776 File Offset: 0x00007776
		public EventWatcherOptions()
			: this(null, ManagementOptions.InfiniteTimeout, 1)
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008785 File Offset: 0x00007785
		public EventWatcherOptions(ManagementNamedValueCollection context, TimeSpan timeout, int blockSize)
			: base(context, timeout)
		{
			base.Flags = 48;
			this.BlockSize = blockSize;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000087A8 File Offset: 0x000077A8
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new EventWatcherOptions(managementNamedValueCollection, base.Timeout, this.blockSize);
		}

		// Token: 0x0400013E RID: 318
		private int blockSize = 1;
	}
}
