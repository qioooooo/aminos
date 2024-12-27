using System;
using System.Deployment.Application.Manifest;

namespace System.Deployment.Application
{
	// Token: 0x020000C9 RID: 201
	internal class SubscriptionState
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x0001AFC2 File Offset: 0x00019FC2
		public SubscriptionState(SubscriptionStore subStore, DefinitionIdentity subId)
		{
			this.Initialize(subStore, subId);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001AFD2 File Offset: 0x00019FD2
		public SubscriptionState(SubscriptionStore subStore, AssemblyManifest deployment)
		{
			this.Initialize(subStore, deployment.Identity.ToSubscriptionId());
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001AFEC File Offset: 0x00019FEC
		public void Invalidate()
		{
			this._stateIsValid = false;
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001AFF5 File Offset: 0x00019FF5
		public DefinitionIdentity SubscriptionId
		{
			get
			{
				return this._subId;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0001AFFD File Offset: 0x00019FFD
		public SubscriptionStore SubscriptionStore
		{
			get
			{
				return this._subStore;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001B005 File Offset: 0x0001A005
		public bool IsInstalled
		{
			get
			{
				this.Validate();
				return this.state.IsInstalled;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001B018 File Offset: 0x0001A018
		public bool IsShellVisible
		{
			get
			{
				this.Validate();
				return this.state.IsShellVisible;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001B02B File Offset: 0x0001A02B
		public DefinitionAppId CurrentBind
		{
			get
			{
				this.Validate();
				return this.state.CurrentBind;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001B03E File Offset: 0x0001A03E
		public DefinitionAppId PreviousBind
		{
			get
			{
				this.Validate();
				return this.state.PreviousBind;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001B051 File Offset: 0x0001A051
		public DefinitionAppId PendingBind
		{
			get
			{
				this.Validate();
				return this.state.PendingBind;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001B064 File Offset: 0x0001A064
		public DefinitionIdentity PendingDeployment
		{
			get
			{
				this.Validate();
				return this.state.PendingDeployment;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001B077 File Offset: 0x0001A077
		public DefinitionIdentity ExcludedDeployment
		{
			get
			{
				this.Validate();
				return this.state.ExcludedDeployment;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001B08A File Offset: 0x0001A08A
		public Uri DeploymentProviderUri
		{
			get
			{
				this.Validate();
				return this.state.DeploymentProviderUri;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001B09D File Offset: 0x0001A09D
		public Version MinimumRequiredVersion
		{
			get
			{
				this.Validate();
				return this.state.MinimumRequiredVersion;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001B0B0 File Offset: 0x0001A0B0
		public DateTime LastCheckTime
		{
			get
			{
				this.Validate();
				return this.state.LastCheckTime;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001B0C3 File Offset: 0x0001A0C3
		public DefinitionIdentity UpdateSkippedDeployment
		{
			get
			{
				this.Validate();
				return this.state.UpdateSkippedDeployment;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001B0D6 File Offset: 0x0001A0D6
		public DateTime UpdateSkipTime
		{
			get
			{
				this.Validate();
				return this.state.UpdateSkipTime;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001B0E9 File Offset: 0x0001A0E9
		public AppType appType
		{
			get
			{
				this.Validate();
				return this.state.appType;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001B0FC File Offset: 0x0001A0FC
		public DefinitionIdentity CurrentDeployment
		{
			get
			{
				this.Validate();
				return this.state.CurrentDeployment;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001B10F File Offset: 0x0001A10F
		public DefinitionIdentity RollbackDeployment
		{
			get
			{
				this.Validate();
				return this.state.RollbackDeployment;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001B122 File Offset: 0x0001A122
		public AssemblyManifest CurrentDeploymentManifest
		{
			get
			{
				this.Validate();
				return this.state.CurrentDeploymentManifest;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001B135 File Offset: 0x0001A135
		public Uri CurrentDeploymentSourceUri
		{
			get
			{
				this.Validate();
				return this.state.CurrentDeploymentSourceUri;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001B148 File Offset: 0x0001A148
		public AssemblyManifest CurrentApplicationManifest
		{
			get
			{
				this.Validate();
				return this.state.CurrentApplicationManifest;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0001B15B File Offset: 0x0001A15B
		public Uri CurrentApplicationSourceUri
		{
			get
			{
				this.Validate();
				return this.state.CurrentApplicationSourceUri;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0001B16E File Offset: 0x0001A16E
		public AssemblyManifest PreviousApplicationManifest
		{
			get
			{
				this.Validate();
				return this.state.PreviousApplicationManifest;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0001B184 File Offset: 0x0001A184
		public DefinitionIdentity PKTGroupId
		{
			get
			{
				DefinitionIdentity definitionIdentity = (DefinitionIdentity)this._subId.Clone();
				definitionIdentity["publicKeyToken"] = null;
				return definitionIdentity;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001B1AF File Offset: 0x0001A1AF
		public Description EffectiveDescription
		{
			get
			{
				if (this.CurrentApplicationManifest != null && this.CurrentApplicationManifest.UseManifestForTrust)
				{
					return this.CurrentApplicationManifest.Description;
				}
				if (this.CurrentDeploymentManifest == null)
				{
					return null;
				}
				return this.CurrentDeploymentManifest.Description;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001B1E8 File Offset: 0x0001A1E8
		public string EffectiveCertificatePublicKeyToken
		{
			get
			{
				if (this.CurrentApplicationManifest != null && this.CurrentApplicationManifest.UseManifestForTrust)
				{
					return this.CurrentApplicationManifest.Identity.PublicKeyToken;
				}
				if (this.CurrentDeploymentManifest == null)
				{
					return null;
				}
				return this.CurrentDeploymentManifest.Identity.PublicKeyToken;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001B235 File Offset: 0x0001A235
		private void Validate()
		{
			if (!this._stateIsValid)
			{
				this.state = this._subStore.GetSubscriptionStateInternal(this);
				this._stateIsValid = true;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001B258 File Offset: 0x0001A258
		private void Initialize(SubscriptionStore subStore, DefinitionIdentity subId)
		{
			this._subStore = subStore;
			this._subId = subId;
			this.Invalidate();
		}

		// Token: 0x04000466 RID: 1126
		private SubscriptionStore _subStore;

		// Token: 0x04000467 RID: 1127
		private DefinitionIdentity _subId;

		// Token: 0x04000468 RID: 1128
		private bool _stateIsValid;

		// Token: 0x04000469 RID: 1129
		private SubscriptionStateInternal state;
	}
}
