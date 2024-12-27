using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x02000018 RID: 24
	internal class Deployment
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000058D8 File Offset: 0x000048D8
		public Deployment(DeploymentMetadataEntry deploymentMetadataEntry)
		{
			this._disallowUrlActivation = (deploymentMetadataEntry.DeploymentFlags & 128U) != 0U;
			this._install = (deploymentMetadataEntry.DeploymentFlags & 32U) != 0U;
			this._trustURLParameters = (deploymentMetadataEntry.DeploymentFlags & 64U) != 0U;
			this._mapFileExtensions = (deploymentMetadataEntry.DeploymentFlags & 256U) != 0U;
			this._createDesktopShortcut = (deploymentMetadataEntry.DeploymentFlags & 512U) != 0U;
			this._update = new DeploymentUpdate(deploymentMetadataEntry);
			this._minimumRequiredVersion = ((deploymentMetadataEntry.MinimumRequiredVersion != null) ? new Version(deploymentMetadataEntry.MinimumRequiredVersion) : null);
			this._codebaseUri = AssemblyManifest.UriFromMetadataEntry(deploymentMetadataEntry.DeploymentProviderCodebase, "Ex_DepProviderNotValid");
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000599B File Offset: 0x0000499B
		public Uri ProviderCodebaseUri
		{
			get
			{
				return this._codebaseUri;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000059A3 File Offset: 0x000049A3
		public DeploymentUpdate DeploymentUpdate
		{
			get
			{
				return this._update;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000059AB File Offset: 0x000049AB
		public Version MinimumRequiredVersion
		{
			get
			{
				return this._minimumRequiredVersion;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000059B3 File Offset: 0x000049B3
		public bool DisallowUrlActivation
		{
			get
			{
				return this._disallowUrlActivation;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000059BB File Offset: 0x000049BB
		public bool Install
		{
			get
			{
				return this._install;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000059C3 File Offset: 0x000049C3
		public bool TrustURLParameters
		{
			get
			{
				return this._trustURLParameters;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000059CB File Offset: 0x000049CB
		public bool MapFileExtensions
		{
			get
			{
				return this._mapFileExtensions;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000059D3 File Offset: 0x000049D3
		public bool CreateDesktopShortcut
		{
			get
			{
				return this._createDesktopShortcut;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000059DB File Offset: 0x000049DB
		public bool IsUpdateSectionPresent
		{
			get
			{
				return this.DeploymentUpdate.BeforeApplicationStartup || this.DeploymentUpdate.MaximumAgeSpecified;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000059FA File Offset: 0x000049FA
		public bool IsInstalledAndNoDeploymentProvider
		{
			get
			{
				return this.Install && this.ProviderCodebaseUri == null;
			}
		}

		// Token: 0x04000075 RID: 117
		private readonly Uri _codebaseUri;

		// Token: 0x04000076 RID: 118
		private readonly DeploymentUpdate _update;

		// Token: 0x04000077 RID: 119
		private readonly Version _minimumRequiredVersion;

		// Token: 0x04000078 RID: 120
		private readonly bool _disallowUrlActivation;

		// Token: 0x04000079 RID: 121
		private readonly bool _install;

		// Token: 0x0400007A RID: 122
		private readonly bool _trustURLParameters;

		// Token: 0x0400007B RID: 123
		private readonly bool _mapFileExtensions;

		// Token: 0x0400007C RID: 124
		private readonly bool _createDesktopShortcut;
	}
}
