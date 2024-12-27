using System;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace System.Deployment.Application
{
	// Token: 0x0200005F RID: 95
	public class GetManifestCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060002EA RID: 746 RVA: 0x000111E8 File Offset: 0x000101E8
		internal GetManifestCompletedEventArgs(BindCompletedEventArgs e, ActivationDescription activationDescription, string logFilePath)
			: base(e.Error, e.Cancelled, e.UserState)
		{
			this._applicationIdentity = ((e.ActivationContext != null) ? e.ActivationContext.Identity : null);
			string text = this._applicationIdentity.ToString();
			DefinitionAppId definitionAppId = new DefinitionAppId(text);
			DefinitionIdentity deploymentIdentity = definitionAppId.DeploymentIdentity;
			this._subId = deploymentIdentity.ToSubscriptionId();
			this._logFilePath = logFilePath;
			this._isCached = e.IsCached;
			this._name = e.FriendlyName;
			this._actContext = e.ActivationContext;
			if (this._isCached)
			{
				this._rawDeploymentManifest = e.ActivationContext.DeploymentManifestBytes;
				this._rawApplicationManifest = e.ActivationContext.ApplicationManifestBytes;
			}
			this._activationDescription = activationDescription;
			this._version = this._activationDescription.AppId.DeploymentIdentity.Version;
			this._support = this._activationDescription.DeployManifest.Description.SupportUri;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000112E1 File Offset: 0x000102E1
		internal GetManifestCompletedEventArgs(BindCompletedEventArgs e, Exception error, string logFilePath)
			: base(error, e.Cancelled, e.UserState)
		{
			this._logFilePath = logFilePath;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000112FD File Offset: 0x000102FD
		internal GetManifestCompletedEventArgs(BindCompletedEventArgs e, string logFilePath)
			: base(e.Error, e.Cancelled, e.UserState)
		{
			this._logFilePath = logFilePath;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0001131E File Offset: 0x0001031E
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._applicationIdentity;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0001132C File Offset: 0x0001032C
		public Version Version
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._version;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0001133A File Offset: 0x0001033A
		public bool IsCached
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._isCached;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00011348 File Offset: 0x00010348
		public string ProductName
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._name;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00011356 File Offset: 0x00010356
		public Uri SupportUri
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._support;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00011364 File Offset: 0x00010364
		public string LogFilePath
		{
			get
			{
				return this._logFilePath;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0001136C File Offset: 0x0001036C
		public XmlReader DeploymentManifest
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return GetManifestCompletedEventArgs.ManifestToXml(this.RawDeploymentManifest);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0001137F File Offset: 0x0001037F
		public XmlReader ApplicationManifest
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return GetManifestCompletedEventArgs.ManifestToXml(this.RawApplicationManifest);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00011392 File Offset: 0x00010392
		public ActivationContext ActivationContext
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._actContext;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000113A0 File Offset: 0x000103A0
		public string SubscriptionIdentity
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._subId.ToString();
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000113B4 File Offset: 0x000103B4
		private static XmlReader ManifestToXml(byte[] rawManifest)
		{
			if (rawManifest == null)
			{
				return null;
			}
			return new XmlTextReader(new MemoryStream(rawManifest));
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000113D3 File Offset: 0x000103D3
		private byte[] RawDeploymentManifest
		{
			get
			{
				if (this._rawDeploymentManifest == null)
				{
					this._rawDeploymentManifest = this._activationDescription.DeployManifest.RawXmlBytes;
				}
				return this._rawDeploymentManifest;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000113F9 File Offset: 0x000103F9
		private byte[] RawApplicationManifest
		{
			get
			{
				if (this._rawApplicationManifest == null)
				{
					this._rawApplicationManifest = this._activationDescription.AppManifest.RawXmlBytes;
				}
				return this._rawApplicationManifest;
			}
		}

		// Token: 0x0400023D RID: 573
		private ActivationDescription _activationDescription;

		// Token: 0x0400023E RID: 574
		private Version _version;

		// Token: 0x0400023F RID: 575
		private ApplicationIdentity _applicationIdentity;

		// Token: 0x04000240 RID: 576
		private DefinitionIdentity _subId;

		// Token: 0x04000241 RID: 577
		private bool _isCached;

		// Token: 0x04000242 RID: 578
		private string _name;

		// Token: 0x04000243 RID: 579
		private Uri _support;

		// Token: 0x04000244 RID: 580
		private string _logFilePath;

		// Token: 0x04000245 RID: 581
		private byte[] _rawApplicationManifest;

		// Token: 0x04000246 RID: 582
		private byte[] _rawDeploymentManifest;

		// Token: 0x04000247 RID: 583
		private ActivationContext _actContext;
	}
}
