using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System
{
	// Token: 0x0200005B RID: 91
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class AppDomainManager : MarshalByRefObject
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x00012FEE File Offset: 0x00011FEE
		public virtual AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup appDomainInfo)
		{
			return AppDomainManager.CreateDomainHelper(friendlyName, securityInfo, appDomainInfo);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00012FF8 File Offset: 0x00011FF8
		[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		protected static AppDomain CreateDomainHelper(string friendlyName, Evidence securityInfo, AppDomainSetup appDomainInfo)
		{
			if (friendlyName == null)
			{
				throw new ArgumentNullException(Environment.GetResourceString("ArgumentNull_String"));
			}
			if (securityInfo != null)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			return AppDomain.nCreateDomain(friendlyName, appDomainInfo, securityInfo, (securityInfo == null) ? AppDomain.CurrentDomain.InternalEvidence : null, AppDomain.CurrentDomain.GetSecurityDescriptor());
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00013049 File Offset: 0x00012049
		public virtual void InitializeNewDomain(AppDomainSetup appDomainInfo)
		{
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001304B File Offset: 0x0001204B
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x00013053 File Offset: 0x00012053
		public AppDomainManagerInitializationOptions InitializationFlags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				this.m_flags = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001305C File Offset: 0x0001205C
		public virtual ApplicationActivator ApplicationActivator
		{
			get
			{
				if (this.m_appActivator == null)
				{
					this.m_appActivator = new ApplicationActivator();
				}
				return this.m_appActivator;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00013077 File Offset: 0x00012077
		public virtual HostSecurityManager HostSecurityManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001307A File Offset: 0x0001207A
		public virtual HostExecutionContextManager HostExecutionContextManager
		{
			get
			{
				return HostExecutionContextManager.GetInternalHostExecutionContextManager();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00013084 File Offset: 0x00012084
		public virtual Assembly EntryAssembly
		{
			get
			{
				if (this.m_entryAssembly == null)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					if (currentDomain.IsDefaultAppDomain() && currentDomain.ActivationContext != null)
					{
						ManifestRunner manifestRunner = new ManifestRunner(currentDomain, currentDomain.ActivationContext);
						this.m_entryAssembly = manifestRunner.EntryAssembly;
					}
					else
					{
						this.m_entryAssembly = AppDomainManager.nGetEntryAssembly();
					}
				}
				return this.m_entryAssembly;
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000130DB File Offset: 0x000120DB
		public virtual bool CheckSecuritySettings(SecurityState state)
		{
			return false;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x000130DE File Offset: 0x000120DE
		internal static AppDomainManager CurrentAppDomainManager
		{
			get
			{
				return AppDomain.CurrentDomain.DomainManager;
			}
		}

		// Token: 0x06000559 RID: 1369
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void nRegisterWithHost();

		// Token: 0x0600055A RID: 1370
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Assembly nGetEntryAssembly();

		// Token: 0x040001B0 RID: 432
		private AppDomainManagerInitializationOptions m_flags;

		// Token: 0x040001B1 RID: 433
		private ApplicationActivator m_appActivator;

		// Token: 0x040001B2 RID: 434
		private Assembly m_entryAssembly;
	}
}
