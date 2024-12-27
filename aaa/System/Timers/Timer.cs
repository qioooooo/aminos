using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Timers
{
	// Token: 0x02000736 RID: 1846
	[DefaultProperty("Interval")]
	[DefaultEvent("Elapsed")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Timer : Component, ISupportInitialize
	{
		// Token: 0x0600383E RID: 14398 RVA: 0x000ED5A0 File Offset: 0x000EC5A0
		public Timer()
		{
			this.interval = 100.0;
			this.enabled = false;
			this.autoReset = true;
			this.initializing = false;
			this.delayedEnable = false;
			this.callback = new TimerCallback(this.MyTimerCallback);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000ED5F0 File Offset: 0x000EC5F0
		public Timer(double interval)
			: this()
		{
			if (interval <= 0.0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "interval", interval }));
			}
			int num = (int)Math.Ceiling(interval);
			if (num < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "interval", interval }));
			}
			this.interval = interval;
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003840 RID: 14400 RVA: 0x000ED674 File Offset: 0x000EC674
		// (set) Token: 0x06003841 RID: 14401 RVA: 0x000ED67C File Offset: 0x000EC67C
		[TimersDescription("TimerAutoReset")]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool AutoReset
		{
			get
			{
				return this.autoReset;
			}
			set
			{
				if (base.DesignMode)
				{
					this.autoReset = value;
					return;
				}
				if (this.autoReset != value)
				{
					this.autoReset = value;
					if (this.timer != null)
					{
						this.UpdateTimer();
					}
				}
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x000ED6AC File Offset: 0x000EC6AC
		// (set) Token: 0x06003843 RID: 14403 RVA: 0x000ED6B4 File Offset: 0x000EC6B4
		[TimersDescription("TimerEnabled")]
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (base.DesignMode)
				{
					this.delayedEnable = value;
					this.enabled = value;
					return;
				}
				if (this.initializing)
				{
					this.delayedEnable = value;
					return;
				}
				if (this.enabled != value)
				{
					if (!value)
					{
						if (this.timer != null)
						{
							this.cookie = null;
							this.timer.Dispose();
							this.timer = null;
						}
						this.enabled = value;
						return;
					}
					this.enabled = value;
					if (this.timer == null)
					{
						if (this.disposed)
						{
							throw new ObjectDisposedException(base.GetType().Name);
						}
						int num = (int)Math.Ceiling(this.interval);
						this.cookie = new object();
						this.timer = new Timer(this.callback, this.cookie, num, this.autoReset ? num : (-1));
						return;
					}
					else
					{
						this.UpdateTimer();
					}
				}
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000ED78C File Offset: 0x000EC78C
		private void UpdateTimer()
		{
			int num = (int)Math.Ceiling(this.interval);
			this.timer.Change(num, this.autoReset ? num : (-1));
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06003845 RID: 14405 RVA: 0x000ED7BF File Offset: 0x000EC7BF
		// (set) Token: 0x06003846 RID: 14406 RVA: 0x000ED7C8 File Offset: 0x000EC7C8
		[RecommendedAsConfigurable(true)]
		[DefaultValue(100.0)]
		[Category("Behavior")]
		[TimersDescription("TimerInterval")]
		public double Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException(SR.GetString("TimerInvalidInterval", new object[] { value, 0 }));
				}
				this.interval = value;
				if (this.timer != null)
				{
					this.UpdateTimer();
				}
			}
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06003847 RID: 14407 RVA: 0x000ED820 File Offset: 0x000EC820
		// (remove) Token: 0x06003848 RID: 14408 RVA: 0x000ED839 File Offset: 0x000EC839
		[TimersDescription("TimerIntervalElapsed")]
		[Category("Behavior")]
		public event ElapsedEventHandler Elapsed
		{
			add
			{
				this.onIntervalElapsed = (ElapsedEventHandler)Delegate.Combine(this.onIntervalElapsed, value);
			}
			remove
			{
				this.onIntervalElapsed = (ElapsedEventHandler)Delegate.Remove(this.onIntervalElapsed, value);
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x0600384A RID: 14410 RVA: 0x000ED86A File Offset: 0x000EC86A
		// (set) Token: 0x06003849 RID: 14409 RVA: 0x000ED852 File Offset: 0x000EC852
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (base.DesignMode)
				{
					this.enabled = true;
				}
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x0600384B RID: 14411 RVA: 0x000ED874 File Offset: 0x000EC874
		// (set) Token: 0x0600384C RID: 14412 RVA: 0x000ED8CE File Offset: 0x000EC8CE
		[Browsable(false)]
		[TimersDescription("TimerSynchronizingObject")]
		[DefaultValue(null)]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if (this.synchronizingObject == null && base.DesignMode)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						object rootComponent = designerHost.RootComponent;
						if (rootComponent != null && rootComponent is ISynchronizeInvoke)
						{
							this.synchronizingObject = (ISynchronizeInvoke)rootComponent;
						}
					}
				}
				return this.synchronizingObject;
			}
			set
			{
				this.synchronizingObject = value;
			}
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000ED8D7 File Offset: 0x000EC8D7
		public void BeginInit()
		{
			this.Close();
			this.initializing = true;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000ED8E6 File Offset: 0x000EC8E6
		public void Close()
		{
			this.initializing = false;
			this.delayedEnable = false;
			this.enabled = false;
			if (this.timer != null)
			{
				this.timer.Dispose();
				this.timer = null;
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000ED917 File Offset: 0x000EC917
		protected override void Dispose(bool disposing)
		{
			this.Close();
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000ED92D File Offset: 0x000EC92D
		public void EndInit()
		{
			this.initializing = false;
			this.Enabled = this.delayedEnable;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000ED942 File Offset: 0x000EC942
		public void Start()
		{
			this.Enabled = true;
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x000ED94B File Offset: 0x000EC94B
		public void Stop()
		{
			this.Enabled = false;
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000ED954 File Offset: 0x000EC954
		private void MyTimerCallback(object state)
		{
			if (state != this.cookie)
			{
				return;
			}
			if (!this.autoReset)
			{
				this.enabled = false;
			}
			Timer.FILE_TIME file_TIME = default(Timer.FILE_TIME);
			Timer.GetSystemTimeAsFileTime(ref file_TIME);
			ElapsedEventArgs elapsedEventArgs = new ElapsedEventArgs(file_TIME.ftTimeLow, file_TIME.ftTimeHigh);
			try
			{
				ElapsedEventHandler elapsedEventHandler = this.onIntervalElapsed;
				if (elapsedEventHandler != null)
				{
					if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
					{
						this.SynchronizingObject.BeginInvoke(elapsedEventHandler, new object[] { this, elapsedEventArgs });
					}
					else
					{
						elapsedEventHandler(this, elapsedEventArgs);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003854 RID: 14420
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern void GetSystemTimeAsFileTime(ref Timer.FILE_TIME lpSystemTimeAsFileTime);

		// Token: 0x0400322B RID: 12843
		private double interval;

		// Token: 0x0400322C RID: 12844
		private bool enabled;

		// Token: 0x0400322D RID: 12845
		private bool initializing;

		// Token: 0x0400322E RID: 12846
		private bool delayedEnable;

		// Token: 0x0400322F RID: 12847
		private ElapsedEventHandler onIntervalElapsed;

		// Token: 0x04003230 RID: 12848
		private bool autoReset;

		// Token: 0x04003231 RID: 12849
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x04003232 RID: 12850
		private bool disposed;

		// Token: 0x04003233 RID: 12851
		private Timer timer;

		// Token: 0x04003234 RID: 12852
		private TimerCallback callback;

		// Token: 0x04003235 RID: 12853
		private object cookie;

		// Token: 0x02000737 RID: 1847
		internal struct FILE_TIME
		{
			// Token: 0x04003236 RID: 12854
			internal int ftTimeLow;

			// Token: 0x04003237 RID: 12855
			internal int ftTimeHigh;
		}
	}
}
