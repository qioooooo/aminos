using System;

namespace System.IO
{
	// Token: 0x02000732 RID: 1842
	public struct WaitForChangedResult
	{
		// Token: 0x0600382D RID: 14381 RVA: 0x000ED4ED File Offset: 0x000EC4ED
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, bool timedOut)
		{
			this = new WaitForChangedResult(changeType, name, null, timedOut);
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x000ED4F9 File Offset: 0x000EC4F9
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, string oldName, bool timedOut)
		{
			this.changeType = changeType;
			this.name = name;
			this.oldName = oldName;
			this.timedOut = timedOut;
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x0600382F RID: 14383 RVA: 0x000ED518 File Offset: 0x000EC518
		// (set) Token: 0x06003830 RID: 14384 RVA: 0x000ED520 File Offset: 0x000EC520
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
			set
			{
				this.changeType = value;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003831 RID: 14385 RVA: 0x000ED529 File Offset: 0x000EC529
		// (set) Token: 0x06003832 RID: 14386 RVA: 0x000ED531 File Offset: 0x000EC531
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003833 RID: 14387 RVA: 0x000ED53A File Offset: 0x000EC53A
		// (set) Token: 0x06003834 RID: 14388 RVA: 0x000ED542 File Offset: 0x000EC542
		public string OldName
		{
			get
			{
				return this.oldName;
			}
			set
			{
				this.oldName = value;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x000ED54B File Offset: 0x000EC54B
		// (set) Token: 0x06003836 RID: 14390 RVA: 0x000ED553 File Offset: 0x000EC553
		public bool TimedOut
		{
			get
			{
				return this.timedOut;
			}
			set
			{
				this.timedOut = value;
			}
		}

		// Token: 0x0400321F RID: 12831
		private WatcherChangeTypes changeType;

		// Token: 0x04003220 RID: 12832
		private string name;

		// Token: 0x04003221 RID: 12833
		private string oldName;

		// Token: 0x04003222 RID: 12834
		private bool timedOut;

		// Token: 0x04003223 RID: 12835
		internal static readonly WaitForChangedResult TimedOutResult = new WaitForChangedResult((WatcherChangeTypes)0, null, true);
	}
}
