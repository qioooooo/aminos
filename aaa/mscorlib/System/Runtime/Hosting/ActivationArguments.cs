using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Hosting
{
	// Token: 0x02000064 RID: 100
	[ComVisible(true)]
	[Serializable]
	public sealed class ActivationArguments
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x00014DDD File Offset: 0x00013DDD
		private ActivationArguments()
		{
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00014DE5 File Offset: 0x00013DE5
		internal bool UseFusionActivationContext
		{
			get
			{
				return this.m_useFusionActivationContext;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00014DED File Offset: 0x00013DED
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00014DF5 File Offset: 0x00013DF5
		internal bool ActivateInstance
		{
			get
			{
				return this.m_activateInstance;
			}
			set
			{
				this.m_activateInstance = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00014DFE File Offset: 0x00013DFE
		internal string ApplicationFullName
		{
			get
			{
				return this.m_appFullName;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00014E06 File Offset: 0x00013E06
		internal string[] ApplicationManifestPaths
		{
			get
			{
				return this.m_appManifestPaths;
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00014E0E File Offset: 0x00013E0E
		public ActivationArguments(ApplicationIdentity applicationIdentity)
			: this(applicationIdentity, null)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00014E18 File Offset: 0x00013E18
		public ActivationArguments(ApplicationIdentity applicationIdentity, string[] activationData)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this.m_appFullName = applicationIdentity.FullName;
			this.m_activationData = activationData;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00014E41 File Offset: 0x00013E41
		public ActivationArguments(ActivationContext activationData)
			: this(activationData, null)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00014E4C File Offset: 0x00013E4C
		public ActivationArguments(ActivationContext activationContext, string[] activationData)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			this.m_appFullName = activationContext.Identity.FullName;
			this.m_appManifestPaths = activationContext.ManifestPaths;
			this.m_activationData = activationData;
			this.m_useFusionActivationContext = true;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00014E98 File Offset: 0x00013E98
		internal ActivationArguments(string appFullName, string[] appManifestPaths, string[] activationData)
		{
			if (appFullName == null)
			{
				throw new ArgumentNullException("appFullName");
			}
			this.m_appFullName = appFullName;
			this.m_appManifestPaths = appManifestPaths;
			this.m_activationData = activationData;
			this.m_useFusionActivationContext = true;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00014ECA File Offset: 0x00013ECA
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return new ApplicationIdentity(this.m_appFullName);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00014ED7 File Offset: 0x00013ED7
		public ActivationContext ActivationContext
		{
			get
			{
				if (!this.UseFusionActivationContext)
				{
					return null;
				}
				if (this.m_appManifestPaths == null)
				{
					return new ActivationContext(new ApplicationIdentity(this.m_appFullName));
				}
				return new ActivationContext(new ApplicationIdentity(this.m_appFullName), this.m_appManifestPaths);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00014F12 File Offset: 0x00013F12
		public string[] ActivationData
		{
			get
			{
				return this.m_activationData;
			}
		}

		// Token: 0x040001D6 RID: 470
		private bool m_useFusionActivationContext;

		// Token: 0x040001D7 RID: 471
		private bool m_activateInstance;

		// Token: 0x040001D8 RID: 472
		private string m_appFullName;

		// Token: 0x040001D9 RID: 473
		private string[] m_appManifestPaths;

		// Token: 0x040001DA RID: 474
		private string[] m_activationData;
	}
}
