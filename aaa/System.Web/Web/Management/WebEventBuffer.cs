using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Configuration;

namespace System.Web.Management
{
	// Token: 0x020002DC RID: 732
	internal sealed class WebEventBuffer
	{
		// Token: 0x06002523 RID: 9507 RVA: 0x0009F264 File Offset: 0x0009E264
		internal WebEventBuffer(BufferedWebEventProvider provider, string bufferMode, WebEventBufferFlushCallback callback)
		{
			this._provider = provider;
			HealthMonitoringSection healthMonitoring = RuntimeConfig.GetAppLKGConfig().HealthMonitoring;
			BufferModesCollection bufferModes = healthMonitoring.BufferModes;
			BufferModeSettings bufferModeSettings = bufferModes[bufferMode];
			if (bufferModeSettings == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Health_mon_buffer_mode_not_found", new object[] { bufferMode }));
			}
			if (bufferModeSettings.RegularFlushInterval == TimeSpan.MaxValue)
			{
				this._regularFlushIntervalMs = WebEventBuffer.Infinite;
			}
			else
			{
				try
				{
					this._regularFlushIntervalMs = (long)bufferModeSettings.RegularFlushInterval.TotalMilliseconds;
				}
				catch (OverflowException)
				{
					this._regularFlushIntervalMs = WebEventBuffer.Infinite;
				}
			}
			if (bufferModeSettings.UrgentFlushInterval == TimeSpan.MaxValue)
			{
				this._urgentFlushIntervalMs = WebEventBuffer.Infinite;
			}
			else
			{
				try
				{
					this._urgentFlushIntervalMs = (long)bufferModeSettings.UrgentFlushInterval.TotalMilliseconds;
				}
				catch (OverflowException)
				{
					this._urgentFlushIntervalMs = WebEventBuffer.Infinite;
				}
			}
			this._urgentFlushThreshold = bufferModeSettings.UrgentFlushThreshold;
			this._maxBufferSize = bufferModeSettings.MaxBufferSize;
			this._maxFlushSize = bufferModeSettings.MaxFlushSize;
			this._maxBufferThreads = bufferModeSettings.MaxBufferThreads;
			this._burstWaitTimeMs = Math.Min(this._burstWaitTimeMs, this._urgentFlushIntervalMs);
			this._flushCallback = callback;
			this._buffer = new Queue();
			if (this._regularFlushIntervalMs != WebEventBuffer.Infinite)
			{
				this._startTime = DateTime.UtcNow;
				this._regularTimeoutUsed = true;
				this._urgentFlushScheduled = false;
				this.SetTimer(this.GetNextRegularFlushDueTimeInMs());
			}
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x0009F420 File Offset: 0x0009E420
		private void FlushTimerCallback(object state)
		{
			this.Flush(this._maxFlushSize, FlushCallReason.Timer);
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x0009F430 File Offset: 0x0009E430
		private bool AnticipateBurst(DateTime now)
		{
			return this._urgentFlushThreshold == 1 && this._buffer.Count == 1 && (now - this._lastAdd).TotalMilliseconds >= (double)this._urgentFlushIntervalMs;
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x0009F478 File Offset: 0x0009E478
		private long GetNextRegularFlushDueTimeInMs()
		{
			long regularFlushIntervalMs = this._regularFlushIntervalMs;
			if (this._regularFlushIntervalMs == WebEventBuffer.Infinite)
			{
				return WebEventBuffer.Infinite;
			}
			DateTime utcNow = DateTime.UtcNow;
			long num = (long)(utcNow - this._startTime).TotalMilliseconds;
			long num2 = (num + regularFlushIntervalMs + 499L) / regularFlushIntervalMs * regularFlushIntervalMs;
			return num2 - num;
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x0009F4CE File Offset: 0x0009E4CE
		private void SetTimer(long waitTimeMs)
		{
			if (this._timer == null)
			{
				this._timer = new Timer(new TimerCallback(this.FlushTimerCallback), null, waitTimeMs, -1L);
				return;
			}
			this._timer.Change(waitTimeMs, -1L);
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x0009F504 File Offset: 0x0009E504
		internal void Flush(int max, FlushCallReason reason)
		{
			WebBaseEvent[] array = null;
			DateTime utcNow = DateTime.UtcNow;
			long num = 0L;
			DateTime dateTime = DateTime.MaxValue;
			int num2 = -1;
			int num3 = -1;
			int num4 = 0;
			EventNotificationType eventNotificationType = EventNotificationType.Regular;
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			lock (this._buffer)
			{
				if (this._buffer.Count == 0)
				{
					flag = false;
				}
				switch (reason)
				{
				case FlushCallReason.UrgentFlushThresholdExceeded:
				{
					if (this._urgentFlushScheduled)
					{
						return;
					}
					flag = false;
					flag2 = true;
					flag3 = true;
					if (this.AnticipateBurst(utcNow))
					{
						num = this._burstWaitTimeMs;
					}
					else
					{
						num = 0L;
					}
					long num5 = (long)(utcNow - this._lastScheduledFlushTime).TotalMilliseconds;
					if (num5 + num < this._urgentFlushIntervalMs)
					{
						num = this._urgentFlushIntervalMs - num5;
					}
					break;
				}
				case FlushCallReason.Timer:
					if (this._regularFlushIntervalMs != WebEventBuffer.Infinite)
					{
						flag2 = true;
						num = this.GetNextRegularFlushDueTimeInMs();
					}
					break;
				}
				if (flag)
				{
					if (this._threadsInFlush >= this._maxBufferThreads)
					{
						num4 = 0;
					}
					else
					{
						num4 = Math.Min(this._buffer.Count, max);
					}
				}
				if (flag)
				{
					if (num4 > 0)
					{
						array = new WebBaseEvent[num4];
						for (int i = 0; i < num4; i++)
						{
							array[i] = (WebBaseEvent)this._buffer.Dequeue();
						}
						dateTime = this._lastFlushTime;
						this._lastFlushTime = utcNow;
						if (reason == FlushCallReason.Timer)
						{
							this._lastScheduledFlushTime = utcNow;
						}
						num2 = this._discardedSinceLastFlush;
						this._discardedSinceLastFlush = 0;
						if (reason == FlushCallReason.StaticFlush)
						{
							eventNotificationType = EventNotificationType.Flush;
						}
						else
						{
							eventNotificationType = (this._regularTimeoutUsed ? EventNotificationType.Regular : EventNotificationType.Urgent);
						}
					}
					num3 = this._buffer.Count;
					if (num3 >= this._urgentFlushThreshold)
					{
						flag2 = true;
						flag3 = true;
						num = this._urgentFlushIntervalMs;
					}
				}
				this._urgentFlushScheduled = false;
				if (flag2)
				{
					if (flag3)
					{
						long nextRegularFlushDueTimeInMs = this.GetNextRegularFlushDueTimeInMs();
						if (nextRegularFlushDueTimeInMs < num)
						{
							num = nextRegularFlushDueTimeInMs;
							this._regularTimeoutUsed = true;
						}
						else
						{
							this._regularTimeoutUsed = false;
						}
					}
					else
					{
						this._regularTimeoutUsed = true;
					}
					this.SetTimer(num);
					this._urgentFlushScheduled = flag3;
				}
				if (reason == FlushCallReason.Timer && !flag2)
				{
					((IDisposable)this._timer).Dispose();
					this._timer = null;
					this._urgentFlushScheduled = false;
				}
				if (array != null)
				{
					Interlocked.Increment(ref this._threadsInFlush);
				}
			}
			if (array != null)
			{
				using (new ApplicationImpersonationContext())
				{
					try
					{
						WebEventBufferFlushInfo webEventBufferFlushInfo = new WebEventBufferFlushInfo(new WebBaseEventCollection(array), eventNotificationType, Interlocked.Increment(ref this._notificationSequence), dateTime, num2, num3);
						this._flushCallback(webEventBufferFlushInfo);
					}
					catch (Exception ex)
					{
						try
						{
							this._provider.LogException(ex);
						}
						catch
						{
						}
					}
					catch
					{
						try
						{
							this._provider.LogException(new Exception(SR.GetString("Provider_Error")));
						}
						catch
						{
						}
					}
				}
				Interlocked.Decrement(ref this._threadsInFlush);
			}
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x0009F848 File Offset: 0x0009E848
		internal void AddEvent(WebBaseEvent webEvent)
		{
			lock (this._buffer)
			{
				if (this._buffer.Count == this._maxBufferSize)
				{
					this._buffer.Dequeue();
					this._discardedSinceLastFlush++;
				}
				this._buffer.Enqueue(webEvent);
				if (this._buffer.Count >= this._urgentFlushThreshold)
				{
					this.Flush(this._maxFlushSize, FlushCallReason.UrgentFlushThresholdExceeded);
				}
				this._lastAdd = DateTime.UtcNow;
			}
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x0009F8E0 File Offset: 0x0009E8E0
		internal void Shutdown()
		{
			if (this._timer != null)
			{
				this._timer.Dispose();
				this._timer = null;
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x0009F8FC File Offset: 0x0009E8FC
		private string PrintTime(DateTime t)
		{
			return t.ToString("T", DateTimeFormatInfo.InvariantInfo) + "." + t.Millisecond.ToString("d03", CultureInfo.InvariantCulture);
		}

		// Token: 0x04001CCC RID: 7372
		private static long Infinite = long.MaxValue;

		// Token: 0x04001CCD RID: 7373
		private long _burstWaitTimeMs = 2000L;

		// Token: 0x04001CCE RID: 7374
		private BufferedWebEventProvider _provider;

		// Token: 0x04001CCF RID: 7375
		private long _regularFlushIntervalMs;

		// Token: 0x04001CD0 RID: 7376
		private int _urgentFlushThreshold;

		// Token: 0x04001CD1 RID: 7377
		private int _maxBufferSize;

		// Token: 0x04001CD2 RID: 7378
		private int _maxFlushSize;

		// Token: 0x04001CD3 RID: 7379
		private long _urgentFlushIntervalMs;

		// Token: 0x04001CD4 RID: 7380
		private int _maxBufferThreads;

		// Token: 0x04001CD5 RID: 7381
		private Queue _buffer;

		// Token: 0x04001CD6 RID: 7382
		private Timer _timer;

		// Token: 0x04001CD7 RID: 7383
		private DateTime _lastFlushTime = DateTime.MinValue;

		// Token: 0x04001CD8 RID: 7384
		private DateTime _lastScheduledFlushTime = DateTime.MinValue;

		// Token: 0x04001CD9 RID: 7385
		private DateTime _lastAdd = DateTime.MinValue;

		// Token: 0x04001CDA RID: 7386
		private DateTime _startTime = DateTime.MinValue;

		// Token: 0x04001CDB RID: 7387
		private bool _urgentFlushScheduled;

		// Token: 0x04001CDC RID: 7388
		private int _discardedSinceLastFlush;

		// Token: 0x04001CDD RID: 7389
		private int _threadsInFlush;

		// Token: 0x04001CDE RID: 7390
		private int _notificationSequence;

		// Token: 0x04001CDF RID: 7391
		private bool _regularTimeoutUsed;

		// Token: 0x04001CE0 RID: 7392
		private WebEventBufferFlushCallback _flushCallback;
	}
}
