using System;
using System.Deployment.Application.Manifest;
using System.Security.Policy;

namespace System.Deployment.Application
{
	// Token: 0x02000006 RID: 6
	internal class CommitApplicationParams
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00003A54 File Offset: 0x00002A54
		public CommitApplicationParams()
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003A68 File Offset: 0x00002A68
		public CommitApplicationParams(CommitApplicationParams src)
		{
			this.AppId = src.AppId;
			this.CommitApp = src.CommitApp;
			this.AppManifest = src.AppManifest;
			this.AppSourceUri = src.AppSourceUri;
			this.AppManifestPath = src.AppManifestPath;
			this.AppPayloadPath = src.AppPayloadPath;
			this.AppGroup = src.AppGroup;
			this.CommitDeploy = src.CommitDeploy;
			this.DeployManifest = src.DeployManifest;
			this.DeploySourceUri = src.DeploySourceUri;
			this.DeployManifestPath = src.DeployManifestPath;
			this.TimeStamp = src.TimeStamp;
			this.IsConfirmed = src.IsConfirmed;
			this.IsUpdate = src.IsUpdate;
			this.IsRequiredUpdate = src.IsRequiredUpdate;
			this.appType = src.appType;
			this.Trust = src.Trust;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00003B52 File Offset: 0x00002B52
		public Description EffectiveDescription
		{
			get
			{
				if (this.AppManifest != null && this.AppManifest.UseManifestForTrust)
				{
					return this.AppManifest.Description;
				}
				if (this.DeployManifest == null)
				{
					return null;
				}
				return this.DeployManifest.Description;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00003B8C File Offset: 0x00002B8C
		public string EffectiveCertificatePublicKeyToken
		{
			get
			{
				if (this.AppManifest != null && this.AppManifest.UseManifestForTrust)
				{
					return this.AppManifest.Identity.PublicKeyToken;
				}
				if (this.DeployManifest == null)
				{
					return null;
				}
				return this.DeployManifest.Identity.PublicKeyToken;
			}
		}

		// Token: 0x04000016 RID: 22
		public DefinitionAppId AppId;

		// Token: 0x04000017 RID: 23
		public bool CommitApp;

		// Token: 0x04000018 RID: 24
		public AssemblyManifest AppManifest;

		// Token: 0x04000019 RID: 25
		public Uri AppSourceUri;

		// Token: 0x0400001A RID: 26
		public string AppManifestPath;

		// Token: 0x0400001B RID: 27
		public string AppPayloadPath;

		// Token: 0x0400001C RID: 28
		public string AppGroup;

		// Token: 0x0400001D RID: 29
		public bool CommitDeploy;

		// Token: 0x0400001E RID: 30
		public AssemblyManifest DeployManifest;

		// Token: 0x0400001F RID: 31
		public Uri DeploySourceUri;

		// Token: 0x04000020 RID: 32
		public string DeployManifestPath;

		// Token: 0x04000021 RID: 33
		public DateTime TimeStamp = DateTime.MinValue;

		// Token: 0x04000022 RID: 34
		public bool IsConfirmed;

		// Token: 0x04000023 RID: 35
		public bool IsUpdate;

		// Token: 0x04000024 RID: 36
		public bool IsRequiredUpdate;

		// Token: 0x04000025 RID: 37
		public AppType appType;

		// Token: 0x04000026 RID: 38
		public ApplicationTrust Trust;
	}
}
