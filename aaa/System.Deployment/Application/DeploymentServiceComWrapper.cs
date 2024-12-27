using System;

namespace System.Deployment.Application
{
	// Token: 0x02000047 RID: 71
	internal class DeploymentServiceComWrapper : IManagedDeploymentServiceCom
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000E4D8 File Offset: 0x0000D4D8
		public DeploymentServiceComWrapper()
		{
			this.m_deploymentServiceCom = new DeploymentServiceCom();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E4EB File Offset: 0x0000D4EB
		public void ActivateApplicationExtension(string textualSubId, string deploymentProviderUrl, string targetAssociatedFile)
		{
			this.m_deploymentServiceCom.ActivateApplicationExtension(textualSubId, deploymentProviderUrl, targetAssociatedFile);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E4FB File Offset: 0x0000D4FB
		public void ActivateDeployment(string deploymentLocation, bool isShortcut)
		{
			this.m_deploymentServiceCom.ActivateDeployment(deploymentLocation, isShortcut);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E50A File Offset: 0x0000D50A
		public void ActivateDeploymentEx(string deploymentLocation, int unsignedPolicy, int signedPolicy)
		{
			this.m_deploymentServiceCom.ActivateDeploymentEx(deploymentLocation, unsignedPolicy, signedPolicy);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E51A File Offset: 0x0000D51A
		public void CheckForDeploymentUpdate(string textualSubId)
		{
			this.m_deploymentServiceCom.CheckForDeploymentUpdate(textualSubId);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000E528 File Offset: 0x0000D528
		public void CleanOnlineAppCache()
		{
			this.m_deploymentServiceCom.CleanOnlineAppCache();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000E535 File Offset: 0x0000D535
		public void EndServiceRightNow()
		{
			this.m_deploymentServiceCom.EndServiceRightNow();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E542 File Offset: 0x0000D542
		public void MaintainSubscription(string textualSubId)
		{
			this.m_deploymentServiceCom.MaintainSubscription(textualSubId);
		}

		// Token: 0x040001D1 RID: 465
		private DeploymentServiceCom m_deploymentServiceCom;
	}
}
