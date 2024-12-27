using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200061C RID: 1564
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class HostProtectionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060038DE RID: 14558 RVA: 0x000C0FAA File Offset: 0x000BFFAA
		public HostProtectionAttribute()
			: base(SecurityAction.LinkDemand)
		{
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x000C0FB3 File Offset: 0x000BFFB3
		public HostProtectionAttribute(SecurityAction action)
			: base(action)
		{
			if (action != SecurityAction.LinkDemand)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"));
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060038E0 RID: 14560 RVA: 0x000C0FD0 File Offset: 0x000BFFD0
		// (set) Token: 0x060038E1 RID: 14561 RVA: 0x000C0FD8 File Offset: 0x000BFFD8
		public HostProtectionResource Resources
		{
			get
			{
				return this.m_resources;
			}
			set
			{
				this.m_resources = value;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060038E2 RID: 14562 RVA: 0x000C0FE1 File Offset: 0x000BFFE1
		// (set) Token: 0x060038E3 RID: 14563 RVA: 0x000C0FF1 File Offset: 0x000BFFF1
		public bool Synchronization
		{
			get
			{
				return (this.m_resources & HostProtectionResource.Synchronization) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.Synchronization) : (this.m_resources & ~HostProtectionResource.Synchronization));
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060038E4 RID: 14564 RVA: 0x000C100F File Offset: 0x000C000F
		// (set) Token: 0x060038E5 RID: 14565 RVA: 0x000C101F File Offset: 0x000C001F
		public bool SharedState
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SharedState) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SharedState) : (this.m_resources & ~HostProtectionResource.SharedState));
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060038E6 RID: 14566 RVA: 0x000C103D File Offset: 0x000C003D
		// (set) Token: 0x060038E7 RID: 14567 RVA: 0x000C104D File Offset: 0x000C004D
		public bool ExternalProcessMgmt
		{
			get
			{
				return (this.m_resources & HostProtectionResource.ExternalProcessMgmt) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.ExternalProcessMgmt) : (this.m_resources & ~HostProtectionResource.ExternalProcessMgmt));
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060038E8 RID: 14568 RVA: 0x000C106B File Offset: 0x000C006B
		// (set) Token: 0x060038E9 RID: 14569 RVA: 0x000C107B File Offset: 0x000C007B
		public bool SelfAffectingProcessMgmt
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SelfAffectingProcessMgmt) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SelfAffectingProcessMgmt) : (this.m_resources & ~HostProtectionResource.SelfAffectingProcessMgmt));
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060038EA RID: 14570 RVA: 0x000C1099 File Offset: 0x000C0099
		// (set) Token: 0x060038EB RID: 14571 RVA: 0x000C10AA File Offset: 0x000C00AA
		public bool ExternalThreading
		{
			get
			{
				return (this.m_resources & HostProtectionResource.ExternalThreading) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.ExternalThreading) : (this.m_resources & ~HostProtectionResource.ExternalThreading));
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060038EC RID: 14572 RVA: 0x000C10C9 File Offset: 0x000C00C9
		// (set) Token: 0x060038ED RID: 14573 RVA: 0x000C10DA File Offset: 0x000C00DA
		public bool SelfAffectingThreading
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SelfAffectingThreading) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SelfAffectingThreading) : (this.m_resources & ~HostProtectionResource.SelfAffectingThreading));
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060038EE RID: 14574 RVA: 0x000C10F9 File Offset: 0x000C00F9
		// (set) Token: 0x060038EF RID: 14575 RVA: 0x000C110A File Offset: 0x000C010A
		[ComVisible(true)]
		public bool SecurityInfrastructure
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SecurityInfrastructure) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SecurityInfrastructure) : (this.m_resources & ~HostProtectionResource.SecurityInfrastructure));
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060038F0 RID: 14576 RVA: 0x000C1129 File Offset: 0x000C0129
		// (set) Token: 0x060038F1 RID: 14577 RVA: 0x000C113D File Offset: 0x000C013D
		public bool UI
		{
			get
			{
				return (this.m_resources & HostProtectionResource.UI) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.UI) : (this.m_resources & ~HostProtectionResource.UI));
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060038F2 RID: 14578 RVA: 0x000C1162 File Offset: 0x000C0162
		// (set) Token: 0x060038F3 RID: 14579 RVA: 0x000C1176 File Offset: 0x000C0176
		public bool MayLeakOnAbort
		{
			get
			{
				return (this.m_resources & HostProtectionResource.MayLeakOnAbort) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.MayLeakOnAbort) : (this.m_resources & ~HostProtectionResource.MayLeakOnAbort));
			}
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x000C119B File Offset: 0x000C019B
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new HostProtectionPermission(PermissionState.Unrestricted);
			}
			return new HostProtectionPermission(this.m_resources);
		}

		// Token: 0x04001D5F RID: 7519
		private HostProtectionResource m_resources;
	}
}
