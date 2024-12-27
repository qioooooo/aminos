using System;
using System.Threading;

namespace System.Web.Util
{
	// Token: 0x02000050 RID: 80
	internal class ReadWriteObjectLock
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000CB5A File Offset: 0x0000BB5A
		internal ReadWriteObjectLock()
		{
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000CB64 File Offset: 0x0000BB64
		internal virtual void AcquireRead()
		{
			lock (this)
			{
				while (this._lock == -1)
				{
					try
					{
						Monitor.Wait(this);
					}
					catch (ThreadInterruptedException)
					{
					}
				}
				this._lock++;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000CBC4 File Offset: 0x0000BBC4
		internal virtual void ReleaseRead()
		{
			lock (this)
			{
				this._lock--;
				if (this._lock == 0)
				{
					Monitor.PulseAll(this);
				}
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000CC10 File Offset: 0x0000BC10
		internal virtual void AcquireWrite()
		{
			lock (this)
			{
				while (this._lock != 0)
				{
					try
					{
						Monitor.Wait(this);
					}
					catch (ThreadInterruptedException)
					{
					}
				}
				this._lock = -1;
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000CC68 File Offset: 0x0000BC68
		internal virtual void ReleaseWrite()
		{
			lock (this)
			{
				this._lock = 0;
				Monitor.PulseAll(this);
			}
		}

		// Token: 0x04000E69 RID: 3689
		private int _lock;
	}
}
