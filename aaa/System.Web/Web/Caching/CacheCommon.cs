using System;
using System.Threading;
using System.Web.Configuration;

namespace System.Web.Caching
{
	// Token: 0x020000FD RID: 253
	internal class CacheCommon
	{
		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002EA48 File Offset: 0x0002DA48
		internal CacheCommon()
		{
			this._cachePublic = new Cache(0);
			this._cacheMemoryStats = new CacheMemoryStats();
			this._enableMemoryCollection = true;
			this._enableExpiration = true;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002EAA1 File Offset: 0x0002DAA1
		internal void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.EnableCacheMemoryTimer(false);
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002EAAD File Offset: 0x0002DAAD
		internal void SetCacheInternal(CacheInternal cacheInternal)
		{
			this._cacheInternal = cacheInternal;
			this._cachePublic.SetCacheInternal(cacheInternal);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002EAC4 File Offset: 0x0002DAC4
		internal void ReadCacheInternalConfig(CacheSection cacheSection)
		{
			if (this._internalConfigRead)
			{
				return;
			}
			lock (this)
			{
				if (!this._internalConfigRead)
				{
					this._internalConfigRead = true;
					if (cacheSection != null)
					{
						this._enableMemoryCollection = !cacheSection.DisableMemoryCollection;
						this._enableExpiration = !cacheSection.DisableExpiration;
						this._cacheMemoryStats.ReadConfig(cacheSection);
						this._currentPollInterval = CacheMemoryPrivateBytesPressure.PollInterval;
						this.ResetFromConfigSettings();
					}
				}
			}
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002EB4C File Offset: 0x0002DB4C
		internal void ResetFromConfigSettings()
		{
			this.EnableCacheMemoryTimer(this._enableMemoryCollection);
			this._cacheInternal.EnableExpirationTimer(this._enableExpiration);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002EB6C File Offset: 0x0002DB6C
		internal void EnableCacheMemoryTimer(bool enable)
		{
			lock (this._timerMemoryStatsLock)
			{
				if (enable)
				{
					if (this._timerMemoryStats == null)
					{
						this._timerMemoryStats = new Timer(new TimerCallback(this.MemoryStatusTimerCallback), null, this._currentPollInterval, this._currentPollInterval);
					}
					else
					{
						this._timerMemoryStats.Change(this._currentPollInterval, this._currentPollInterval);
					}
				}
				else
				{
					Timer timerMemoryStats = this._timerMemoryStats;
					if (timerMemoryStats != null && Interlocked.CompareExchange<Timer>(ref this._timerMemoryStats, null, timerMemoryStats) == timerMemoryStats)
					{
						timerMemoryStats.Dispose();
					}
				}
			}
			if (!enable)
			{
				while (this._inMemoryStatsUpdate != 0)
				{
					Thread.Sleep(100);
				}
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002EC20 File Offset: 0x0002DC20
		private void AdjustTimer()
		{
			lock (this._timerMemoryStatsLock)
			{
				if (this._timerMemoryStats != null)
				{
					if (this._cacheMemoryStats.IsAboveHighPressure())
					{
						if (this._currentPollInterval > 5000)
						{
							this._currentPollInterval = 5000;
							this._timerMemoryStats.Change(this._currentPollInterval, this._currentPollInterval);
						}
					}
					else if (this._cacheMemoryStats.PrivateBytesPressure.PressureLast > this._cacheMemoryStats.PrivateBytesPressure.PressureLow / 2 || this._cacheMemoryStats.TotalMemoryPressure.PressureLast > this._cacheMemoryStats.TotalMemoryPressure.PressureLow / 2)
					{
						int num = Math.Min(CacheMemoryPrivateBytesPressure.PollInterval, 30000);
						if (this._currentPollInterval != num)
						{
							this._currentPollInterval = num;
							this._timerMemoryStats.Change(this._currentPollInterval, this._currentPollInterval);
						}
					}
					else if (this._currentPollInterval != CacheMemoryPrivateBytesPressure.PollInterval)
					{
						this._currentPollInterval = CacheMemoryPrivateBytesPressure.PollInterval;
						this._timerMemoryStats.Change(this._currentPollInterval, this._currentPollInterval);
					}
				}
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0002ED60 File Offset: 0x0002DD60
		private void MemoryStatusTimerCallback(object state)
		{
			if (Interlocked.Exchange(ref this._inMemoryStatsUpdate, 1) != 0)
			{
				return;
			}
			try
			{
				if (DateTime.UtcNow >= this._timerSuspendTime)
				{
					this._cacheMemoryStats.Update();
					this.AdjustTimer();
					bool flag = this._cacheInternal.ReviewMemoryStats();
					if (this._cacheMemoryStats.IsAboveHighPressure() && flag)
					{
						this.GcCollect();
					}
				}
			}
			finally
			{
				Interlocked.Exchange(ref this._inMemoryStatsUpdate, 0);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002EDE4 File Offset: 0x0002DDE4
		internal void GcCollect()
		{
			int num = CacheMemoryPrivateBytesPressure.PollInterval / 1000;
			int num2 = Math.Max(Math.Min(num, 60), 5);
			bool flag = this._cacheMemoryStats.TotalMemoryPressure.IsAboveHighPressure();
			long num3 = 0L;
			bool flag2;
			UnsafeNativeMethods.SetGCLastCalledTime(out flag2, flag ? num2 : 5);
			if (flag2)
			{
				long privateBytes = CacheMemoryPrivateBytesPressure.GetPrivateBytes(true);
				long totalMemory = GC.GetTotalMemory(false);
				GC.Collect();
				this._gcCollectCount++;
				long privateBytes2 = CacheMemoryPrivateBytesPressure.GetPrivateBytes(true);
				long totalMemory2 = GC.GetTotalMemory(false);
				num3 = Math.Max(privateBytes - privateBytes2, totalMemory - totalMemory2);
			}
			if (!flag2 || flag || this._cacheMemoryStats.IsGcCollectIneffective(num3))
			{
				this._timerSuspendTime = DateTime.UtcNow.AddSeconds((double)num2);
			}
		}

		// Token: 0x040013B1 RID: 5041
		private const int MEMORYSTATUS_INTERVAL_5_SECONDS = 5000;

		// Token: 0x040013B2 RID: 5042
		private const int MEMORYSTATUS_INTERVAL_30_SECONDS = 30000;

		// Token: 0x040013B3 RID: 5043
		private const int GC_BACKOFF_INTERVAL = 60;

		// Token: 0x040013B4 RID: 5044
		private const int GC_INTERVAL = 5;

		// Token: 0x040013B5 RID: 5045
		internal CacheInternal _cacheInternal;

		// Token: 0x040013B6 RID: 5046
		internal Cache _cachePublic;

		// Token: 0x040013B7 RID: 5047
		protected internal CacheMemoryStats _cacheMemoryStats;

		// Token: 0x040013B8 RID: 5048
		internal object _timerMemoryStatsLock = new object();

		// Token: 0x040013B9 RID: 5049
		internal Timer _timerMemoryStats;

		// Token: 0x040013BA RID: 5050
		internal int _currentPollInterval = 30000;

		// Token: 0x040013BB RID: 5051
		internal DateTime _timerSuspendTime = DateTime.MinValue;

		// Token: 0x040013BC RID: 5052
		internal int _inMemoryStatsUpdate;

		// Token: 0x040013BD RID: 5053
		internal bool _enableMemoryCollection;

		// Token: 0x040013BE RID: 5054
		internal bool _enableExpiration;

		// Token: 0x040013BF RID: 5055
		internal bool _internalConfigRead;

		// Token: 0x040013C0 RID: 5056
		private int _gcCollectCount;
	}
}
