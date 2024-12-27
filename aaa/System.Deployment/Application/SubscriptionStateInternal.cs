using System;
using System.Deployment.Application.Manifest;

namespace System.Deployment.Application
{
	// Token: 0x0200002D RID: 45
	internal class SubscriptionStateInternal
	{
		// Token: 0x0600019B RID: 411 RVA: 0x0000B790 File Offset: 0x0000A790
		public SubscriptionStateInternal()
		{
			this.Reset();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B7A0 File Offset: 0x0000A7A0
		public SubscriptionStateInternal(SubscriptionState subState)
		{
			this.IsInstalled = subState.IsInstalled;
			this.IsShellVisible = subState.IsShellVisible;
			this.CurrentBind = subState.CurrentBind;
			this.PreviousBind = subState.PreviousBind;
			this.PendingBind = subState.PreviousBind;
			this.PendingDeployment = subState.PendingDeployment;
			this.ExcludedDeployment = subState.ExcludedDeployment;
			this.DeploymentProviderUri = subState.DeploymentProviderUri;
			this.MinimumRequiredVersion = subState.MinimumRequiredVersion;
			this.LastCheckTime = subState.LastCheckTime;
			this.UpdateSkippedDeployment = subState.UpdateSkippedDeployment;
			this.UpdateSkipTime = subState.UpdateSkipTime;
			this.appType = subState.appType;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B850 File Offset: 0x0000A850
		public void Reset()
		{
			this.IsInstalled = (this.IsShellVisible = false);
			this.CurrentBind = (this.PreviousBind = (this.PendingBind = null));
			this.ExcludedDeployment = (this.PendingDeployment = null);
			this.DeploymentProviderUri = null;
			this.MinimumRequiredVersion = null;
			this.LastCheckTime = DateTime.MinValue;
			this.UpdateSkippedDeployment = null;
			this.UpdateSkipTime = DateTime.MinValue;
			this.CurrentDeployment = null;
			this.RollbackDeployment = null;
			this.CurrentDeploymentManifest = null;
			this.CurrentDeploymentSourceUri = null;
			this.CurrentApplication = null;
			this.CurrentApplicationManifest = null;
			this.CurrentApplicationSourceUri = null;
			this.PreviousApplication = null;
			this.PreviousApplicationManifest = null;
			this.appType = AppType.None;
		}

		// Token: 0x040000F0 RID: 240
		public bool IsInstalled;

		// Token: 0x040000F1 RID: 241
		public bool IsShellVisible;

		// Token: 0x040000F2 RID: 242
		public DefinitionAppId CurrentBind;

		// Token: 0x040000F3 RID: 243
		public DefinitionAppId PreviousBind;

		// Token: 0x040000F4 RID: 244
		public DefinitionAppId PendingBind;

		// Token: 0x040000F5 RID: 245
		public DefinitionIdentity PendingDeployment;

		// Token: 0x040000F6 RID: 246
		public DefinitionIdentity ExcludedDeployment;

		// Token: 0x040000F7 RID: 247
		public Uri DeploymentProviderUri;

		// Token: 0x040000F8 RID: 248
		public Version MinimumRequiredVersion;

		// Token: 0x040000F9 RID: 249
		public DateTime LastCheckTime;

		// Token: 0x040000FA RID: 250
		public DateTime UpdateSkipTime;

		// Token: 0x040000FB RID: 251
		public DefinitionIdentity UpdateSkippedDeployment;

		// Token: 0x040000FC RID: 252
		public AppType appType;

		// Token: 0x040000FD RID: 253
		public DefinitionIdentity CurrentDeployment;

		// Token: 0x040000FE RID: 254
		public DefinitionIdentity RollbackDeployment;

		// Token: 0x040000FF RID: 255
		public AssemblyManifest CurrentDeploymentManifest;

		// Token: 0x04000100 RID: 256
		public Uri CurrentDeploymentSourceUri;

		// Token: 0x04000101 RID: 257
		public DefinitionIdentity CurrentApplication;

		// Token: 0x04000102 RID: 258
		public AssemblyManifest CurrentApplicationManifest;

		// Token: 0x04000103 RID: 259
		public Uri CurrentApplicationSourceUri;

		// Token: 0x04000104 RID: 260
		public DefinitionIdentity PreviousApplication;

		// Token: 0x04000105 RID: 261
		public AssemblyManifest PreviousApplicationManifest;
	}
}
