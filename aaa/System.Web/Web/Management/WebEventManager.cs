using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web.Management
{
	// Token: 0x020002FC RID: 764
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class WebEventManager
	{
		// Token: 0x0600260B RID: 9739 RVA: 0x000A2D34 File Offset: 0x000A1D34
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static void Flush(string providerName)
		{
			HealthMonitoringSectionHelper.ProviderInstances providerInstances = HealthMonitoringManager.ProviderInstances;
			if (providerInstances == null)
			{
				return;
			}
			if (!providerInstances.ContainsKey(providerName))
			{
				throw new ArgumentException(SR.GetString("Health_mon_provider_not_found", new object[] { providerName }));
			}
			using (new ApplicationImpersonationContext())
			{
				providerInstances[providerName].Flush();
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000A2DA0 File Offset: 0x000A1DA0
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
		public static void Flush()
		{
			HealthMonitoringSectionHelper.ProviderInstances providerInstances = HealthMonitoringManager.ProviderInstances;
			if (providerInstances == null)
			{
				return;
			}
			using (new ApplicationImpersonationContext())
			{
				foreach (object obj in providerInstances)
				{
					WebEventProvider webEventProvider = (WebEventProvider)((DictionaryEntry)obj).Value;
					webEventProvider.Flush();
				}
			}
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000A2E30 File Offset: 0x000A1E30
		internal static void Shutdown()
		{
			HealthMonitoringSectionHelper.ProviderInstances providerInstances = HealthMonitoringManager.ProviderInstances;
			if (providerInstances == null)
			{
				return;
			}
			foreach (object obj in providerInstances)
			{
				WebEventProvider webEventProvider = (WebEventProvider)((DictionaryEntry)obj).Value;
				webEventProvider.Shutdown();
			}
		}
	}
}
