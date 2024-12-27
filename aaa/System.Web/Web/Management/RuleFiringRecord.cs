using System;
using System.Security.Permissions;
using System.Threading;
using System.Web.Configuration;

namespace System.Web.Management
{
	// Token: 0x020002F9 RID: 761
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RuleFiringRecord
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000A288A File Offset: 0x000A188A
		public DateTime LastFired
		{
			get
			{
				return this._lastFired;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000A2892 File Offset: 0x000A1892
		public int TimesRaised
		{
			get
			{
				return this._timesRaised;
			}
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000A289A File Offset: 0x000A189A
		internal RuleFiringRecord(HealthMonitoringSectionHelper.RuleInfo ruleInfo)
		{
			this._ruleInfo = ruleInfo;
			this._lastFired = DateTime.MinValue;
			this._timesRaised = 0;
			this._updatingLastFired = 0;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000A28C4 File Offset: 0x000A18C4
		private void UpdateLastFired(DateTime now, bool alreadyLocked)
		{
			TimeSpan timeSpan = now - this._lastFired;
			if (timeSpan < RuleFiringRecord.TS_ONE_SECOND)
			{
				return;
			}
			if (!alreadyLocked)
			{
				if (Interlocked.CompareExchange(ref this._updatingLastFired, 1, 0) != 0)
				{
					return;
				}
				try
				{
					this._lastFired = now;
					return;
				}
				finally
				{
					Interlocked.Exchange(ref this._updatingLastFired, 0);
				}
			}
			this._lastFired = now;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000A292C File Offset: 0x000A192C
		internal bool CheckAndUpdate(WebBaseEvent eventRaised)
		{
			DateTime now = DateTime.Now;
			HealthMonitoringManager healthMonitoringManager = HealthMonitoringManager.Manager();
			int num = Interlocked.Increment(ref this._timesRaised);
			if (healthMonitoringManager == null)
			{
				return false;
			}
			if (this._ruleInfo._customEvaluatorType != null)
			{
				IWebEventCustomEvaluator webEventCustomEvaluator = (IWebEventCustomEvaluator)healthMonitoringManager._sectionHelper._customEvaluatorInstances[this._ruleInfo._customEvaluatorType];
				try
				{
					eventRaised.PreProcessEventInit();
					if (!webEventCustomEvaluator.CanFire(eventRaised, this))
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
			}
			if (num < this._ruleInfo._minInstances)
			{
				return false;
			}
			if (num > this._ruleInfo._maxLimit)
			{
				return false;
			}
			if (this._ruleInfo._minInterval == TimeSpan.Zero)
			{
				this.UpdateLastFired(now, false);
				return true;
			}
			if (now - this._lastFired <= this._ruleInfo._minInterval)
			{
				return false;
			}
			bool flag;
			lock (this)
			{
				if (now - this._lastFired <= this._ruleInfo._minInterval)
				{
					flag = false;
				}
				else
				{
					this.UpdateLastFired(now, true);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x04001D8E RID: 7566
		internal DateTime _lastFired;

		// Token: 0x04001D8F RID: 7567
		internal int _timesRaised;

		// Token: 0x04001D90 RID: 7568
		internal int _updatingLastFired;

		// Token: 0x04001D91 RID: 7569
		private static TimeSpan TS_ONE_SECOND = new TimeSpan(0, 0, 1);

		// Token: 0x04001D92 RID: 7570
		internal HealthMonitoringSectionHelper.RuleInfo _ruleInfo;
	}
}
