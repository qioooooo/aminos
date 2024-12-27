using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x02000046 RID: 70
	[Guid("B3CA4E79-0107-4CA7-9708-3BE0A97957FB")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IManagedDeploymentServiceCom
	{
		// Token: 0x06000236 RID: 566
		void ActivateDeployment(string deploymentLocation, bool isShortcut);

		// Token: 0x06000237 RID: 567
		void ActivateDeploymentEx(string deploymentLocation, int unsignedPolicy, int signedPolicy);

		// Token: 0x06000238 RID: 568
		void ActivateApplicationExtension(string textualSubId, string deploymentProviderUrl, string targetAssociatedFile);

		// Token: 0x06000239 RID: 569
		void MaintainSubscription(string textualSubId);

		// Token: 0x0600023A RID: 570
		void CheckForDeploymentUpdate(string textualSubId);

		// Token: 0x0600023B RID: 571
		void EndServiceRightNow();

		// Token: 0x0600023C RID: 572
		void CleanOnlineAppCache();
	}
}
