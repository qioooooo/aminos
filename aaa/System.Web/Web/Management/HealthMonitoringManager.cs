using System;
using System.Threading;
using System.Web.Configuration;

namespace System.Web.Management
{
	// Token: 0x020002FA RID: 762
	internal class HealthMonitoringManager
	{
		// Token: 0x060025FC RID: 9724 RVA: 0x000A2A78 File Offset: 0x000A1A78
		internal static HealthMonitoringManager Manager()
		{
			if (HealthMonitoringManager.s_initing)
			{
				return null;
			}
			if (HealthMonitoringManager.s_inited)
			{
				return HealthMonitoringManager.s_manager;
			}
			lock (HealthMonitoringManager.s_lockObject)
			{
				if (HealthMonitoringManager.s_inited)
				{
					return HealthMonitoringManager.s_manager;
				}
				try
				{
					HealthMonitoringManager.s_initing = true;
					HealthMonitoringManager.s_manager = new HealthMonitoringManager();
				}
				finally
				{
					HealthMonitoringManager.s_initing = false;
					HealthMonitoringManager.s_inited = true;
				}
			}
			return HealthMonitoringManager.s_manager;
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x000A2B04 File Offset: 0x000A1B04
		internal static bool Enabled
		{
			get
			{
				HealthMonitoringManager healthMonitoringManager = HealthMonitoringManager.Manager();
				return healthMonitoringManager != null && healthMonitoringManager._enabled;
			}
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000A2B24 File Offset: 0x000A1B24
		internal static void StartHealthMonitoringHeartbeat()
		{
			HealthMonitoringManager healthMonitoringManager = HealthMonitoringManager.Manager();
			if (healthMonitoringManager == null)
			{
				return;
			}
			if (!healthMonitoringManager._enabled)
			{
				return;
			}
			healthMonitoringManager.StartHeartbeatTimer();
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000A2B4A File Offset: 0x000A1B4A
		private HealthMonitoringManager()
		{
			this._sectionHelper = HealthMonitoringSectionHelper.GetHelper();
			this._enabled = this._sectionHelper.Enabled;
			if (!this._enabled)
			{
			}
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000A2B76 File Offset: 0x000A1B76
		internal static void Shutdown()
		{
			WebEventManager.Shutdown();
			HealthMonitoringManager.Dispose();
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000A2B84 File Offset: 0x000A1B84
		internal static void Dispose()
		{
			try
			{
				if (HealthMonitoringManager.s_heartbeatTimer != null)
				{
					HealthMonitoringManager.s_heartbeatTimer.Dispose();
					HealthMonitoringManager.s_heartbeatTimer = null;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000A2BC0 File Offset: 0x000A1BC0
		internal void HeartbeatCallback(object state)
		{
			WebBaseEvent.RaiseSystemEvent(null, 1005);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000A2BD0 File Offset: 0x000A1BD0
		internal void StartHeartbeatTimer()
		{
			TimeSpan heartbeatInterval = this._sectionHelper.HealthMonitoringSection.HeartbeatInterval;
			if (heartbeatInterval == TimeSpan.Zero)
			{
				return;
			}
			HealthMonitoringManager.s_heartbeatTimer = new Timer(new TimerCallback(this.HeartbeatCallback), null, TimeSpan.Zero, heartbeatInterval);
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x000A2C1C File Offset: 0x000A1C1C
		internal static HealthMonitoringSectionHelper.ProviderInstances ProviderInstances
		{
			get
			{
				HealthMonitoringManager healthMonitoringManager = HealthMonitoringManager.Manager();
				if (healthMonitoringManager == null)
				{
					return null;
				}
				if (!healthMonitoringManager._enabled)
				{
					return null;
				}
				return healthMonitoringManager._sectionHelper._providerInstances;
			}
		}

		// Token: 0x04001D93 RID: 7571
		internal HealthMonitoringSectionHelper _sectionHelper;

		// Token: 0x04001D94 RID: 7572
		internal bool _enabled;

		// Token: 0x04001D95 RID: 7573
		private static Timer s_heartbeatTimer = null;

		// Token: 0x04001D96 RID: 7574
		private static HealthMonitoringManager s_manager = null;

		// Token: 0x04001D97 RID: 7575
		private static bool s_inited = false;

		// Token: 0x04001D98 RID: 7576
		private static bool s_initing = false;

		// Token: 0x04001D99 RID: 7577
		private static object s_lockObject = new object();
	}
}
