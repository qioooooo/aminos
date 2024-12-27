using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal
{
	// Token: 0x0200006B RID: 107
	[ComVisible(false)]
	public static class InternalActivationContextHelper
	{
		// Token: 0x06000634 RID: 1588 RVA: 0x00015616 File Offset: 0x00014616
		public static object GetActivationContextData(ActivationContext appInfo)
		{
			return appInfo.ActivationContextData;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001561E File Offset: 0x0001461E
		public static object GetApplicationComponentManifest(ActivationContext appInfo)
		{
			return appInfo.ApplicationComponentManifest;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00015626 File Offset: 0x00014626
		public static object GetDeploymentComponentManifest(ActivationContext appInfo)
		{
			return appInfo.DeploymentComponentManifest;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001562E File Offset: 0x0001462E
		public static void PrepareForExecution(ActivationContext appInfo)
		{
			appInfo.PrepareForExecution();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00015636 File Offset: 0x00014636
		public static bool IsFirstRun(ActivationContext appInfo)
		{
			return appInfo.LastApplicationStateResult == ActivationContext.ApplicationStateDisposition.RunningFirstTime;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00015645 File Offset: 0x00014645
		public static byte[] GetApplicationManifestBytes(ActivationContext appInfo)
		{
			if (appInfo == null)
			{
				throw new ArgumentNullException("appInfo");
			}
			return appInfo.GetApplicationManifestBytes();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001565B File Offset: 0x0001465B
		public static byte[] GetDeploymentManifestBytes(ActivationContext appInfo)
		{
			if (appInfo == null)
			{
				throw new ArgumentNullException("appInfo");
			}
			return appInfo.GetDeploymentManifestBytes();
		}
	}
}
