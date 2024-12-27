using System;
using System.Deployment.Application.Manifest;

namespace System.Deployment.Application
{
	// Token: 0x02000007 RID: 7
	internal class ActivationDescription : CommitApplicationParams
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00003BDC File Offset: 0x00002BDC
		public void SetApplicationManifest(AssemblyManifest manifest, Uri manifestUri, string manifestPath)
		{
			this.AppManifest = manifest;
			this.AppSourceUri = manifestUri;
			this.AppManifestPath = manifestPath;
			if (this.AppManifest.EntryPoints[0].CustomHostSpecified)
			{
				this.appType = AppType.CustomHostSpecified;
			}
			if (this.AppManifest.EntryPoints[0].CustomUX)
			{
				this.appType = AppType.CustomUX;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003C34 File Offset: 0x00002C34
		public void SetDeploymentManifest(AssemblyManifest manifest, Uri manifestUri, string manifestPath)
		{
			this.DeploySourceUri = manifestUri;
			this.DeployManifest = manifest;
			this.DeployManifestPath = manifestPath;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003C4C File Offset: 0x00002C4C
		public string ToAppCodebase()
		{
			Uri uri = ((this.DeploySourceUri.Query != null && this.DeploySourceUri.Query.Length > 0) ? new Uri(this.DeploySourceUri.GetLeftPart(UriPartial.Path)) : this.DeploySourceUri);
			return uri.AbsoluteUri;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003C9C File Offset: 0x00002C9C
		public ActivationContext ToActivationContext()
		{
			ApplicationIdentity applicationIdentity = this.AppId.ToApplicationIdentity();
			return ActivationContext.CreatePartialActivationContext(applicationIdentity, new string[] { this.DeployManifestPath, this.AppManifestPath });
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00003CD5 File Offset: 0x00002CD5
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00003CDD File Offset: 0x00002CDD
		public ActivationType ActType
		{
			get
			{
				return this.activationType;
			}
			set
			{
				this.activationType = value;
			}
		}

		// Token: 0x04000027 RID: 39
		private ActivationType activationType;
	}
}
