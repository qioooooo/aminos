using System;

namespace System.Threading
{
	// Token: 0x02000142 RID: 322
	public class HostExecutionContext
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00032F49 File Offset: 0x00031F49
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00032F51 File Offset: 0x00031F51
		protected internal object State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00032F5A File Offset: 0x00031F5A
		public HostExecutionContext()
		{
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00032F62 File Offset: 0x00031F62
		public HostExecutionContext(object state)
		{
			this.state = state;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00032F71 File Offset: 0x00031F71
		public virtual HostExecutionContext CreateCopy()
		{
			if (this.state is IUnknownSafeHandle)
			{
				((IUnknownSafeHandle)this.state).Clone();
			}
			return new HostExecutionContext(this.state);
		}

		// Token: 0x04000609 RID: 1545
		private object state;
	}
}
