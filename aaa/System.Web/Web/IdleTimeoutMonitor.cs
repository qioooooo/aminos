using System;
using System.Diagnostics;
using System.Threading;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x020000A0 RID: 160
	internal class IdleTimeoutMonitor
	{
		// Token: 0x0600083F RID: 2111 RVA: 0x00024828 File Offset: 0x00023828
		internal IdleTimeoutMonitor(TimeSpan timeout)
		{
			this._idleTimeout = timeout;
			this._timer = new Timer(new TimerCallback(this.TimerCompletionCallback), null, this._timerPeriod, this._timerPeriod);
			this._lastEvent = DateTime.UtcNow;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00024880 File Offset: 0x00023880
		internal void Stop()
		{
			if (this._timer != null)
			{
				lock (this)
				{
					if (this._timer != null)
					{
						((IDisposable)this._timer).Dispose();
						this._timer = null;
					}
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x000248D0 File Offset: 0x000238D0
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00024908 File Offset: 0x00023908
		internal DateTime LastEvent
		{
			get
			{
				DateTime lastEvent;
				lock (this)
				{
					lastEvent = this._lastEvent;
				}
				return lastEvent;
			}
			set
			{
				lock (this)
				{
					this._lastEvent = value;
				}
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00024940 File Offset: 0x00023940
		private void TimerCompletionCallback(object state)
		{
			HttpApplicationFactory.TrimApplicationInstances();
			if (this._idleTimeout == TimeSpan.MaxValue)
			{
				return;
			}
			if (HostingEnvironment.ShutdownInitiated)
			{
				return;
			}
			if (HostingEnvironment.BusyCount != 0)
			{
				return;
			}
			if (DateTime.UtcNow <= this.LastEvent.Add(this._idleTimeout))
			{
				return;
			}
			if (Debugger.IsAttached)
			{
				return;
			}
			HttpRuntime.SetShutdownReason(ApplicationShutdownReason.IdleTimeout, SR.GetString("Hosting_Env_IdleTimeout"));
			HostingEnvironment.InitiateShutdown();
		}

		// Token: 0x04001198 RID: 4504
		private TimeSpan _idleTimeout;

		// Token: 0x04001199 RID: 4505
		private DateTime _lastEvent;

		// Token: 0x0400119A RID: 4506
		private Timer _timer;

		// Token: 0x0400119B RID: 4507
		private readonly TimeSpan _timerPeriod = new TimeSpan(0, 0, 30);
	}
}
