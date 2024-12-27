using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062C RID: 1580
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600396E RID: 14702 RVA: 0x000C21D1 File Offset: 0x000C11D1
		public SecurityPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x0600396F RID: 14703 RVA: 0x000C21DA File Offset: 0x000C11DA
		// (set) Token: 0x06003970 RID: 14704 RVA: 0x000C21E2 File Offset: 0x000C11E2
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_flag;
			}
			set
			{
				this.m_flag = value;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x000C21EB File Offset: 0x000C11EB
		// (set) Token: 0x06003972 RID: 14706 RVA: 0x000C21FB File Offset: 0x000C11FB
		public bool Assertion
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.Assertion) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.Assertion) : (this.m_flag & ~SecurityPermissionFlag.Assertion));
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003973 RID: 14707 RVA: 0x000C2219 File Offset: 0x000C1219
		// (set) Token: 0x06003974 RID: 14708 RVA: 0x000C2229 File Offset: 0x000C1229
		public bool UnmanagedCode
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.UnmanagedCode) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.UnmanagedCode) : (this.m_flag & ~SecurityPermissionFlag.UnmanagedCode));
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003975 RID: 14709 RVA: 0x000C2247 File Offset: 0x000C1247
		// (set) Token: 0x06003976 RID: 14710 RVA: 0x000C2257 File Offset: 0x000C1257
		public bool SkipVerification
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.SkipVerification) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.SkipVerification) : (this.m_flag & ~SecurityPermissionFlag.SkipVerification));
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003977 RID: 14711 RVA: 0x000C2275 File Offset: 0x000C1275
		// (set) Token: 0x06003978 RID: 14712 RVA: 0x000C2285 File Offset: 0x000C1285
		public bool Execution
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.Execution) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.Execution) : (this.m_flag & ~SecurityPermissionFlag.Execution));
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x000C22A3 File Offset: 0x000C12A3
		// (set) Token: 0x0600397A RID: 14714 RVA: 0x000C22B4 File Offset: 0x000C12B4
		public bool ControlThread
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlThread) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlThread) : (this.m_flag & ~SecurityPermissionFlag.ControlThread));
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x000C22D3 File Offset: 0x000C12D3
		// (set) Token: 0x0600397C RID: 14716 RVA: 0x000C22E4 File Offset: 0x000C12E4
		public bool ControlEvidence
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlEvidence) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlEvidence) : (this.m_flag & ~SecurityPermissionFlag.ControlEvidence));
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x000C2303 File Offset: 0x000C1303
		// (set) Token: 0x0600397E RID: 14718 RVA: 0x000C2314 File Offset: 0x000C1314
		public bool ControlPolicy
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlPolicy) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlPolicy) : (this.m_flag & ~SecurityPermissionFlag.ControlPolicy));
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x000C2333 File Offset: 0x000C1333
		// (set) Token: 0x06003980 RID: 14720 RVA: 0x000C2347 File Offset: 0x000C1347
		public bool SerializationFormatter
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.SerializationFormatter) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.SerializationFormatter) : (this.m_flag & ~SecurityPermissionFlag.SerializationFormatter));
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003981 RID: 14721 RVA: 0x000C236C File Offset: 0x000C136C
		// (set) Token: 0x06003982 RID: 14722 RVA: 0x000C2380 File Offset: 0x000C1380
		public bool ControlDomainPolicy
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlDomainPolicy) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlDomainPolicy) : (this.m_flag & ~SecurityPermissionFlag.ControlDomainPolicy));
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x000C23A5 File Offset: 0x000C13A5
		// (set) Token: 0x06003984 RID: 14724 RVA: 0x000C23B9 File Offset: 0x000C13B9
		public bool ControlPrincipal
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlPrincipal) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlPrincipal) : (this.m_flag & ~SecurityPermissionFlag.ControlPrincipal));
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003985 RID: 14725 RVA: 0x000C23DE File Offset: 0x000C13DE
		// (set) Token: 0x06003986 RID: 14726 RVA: 0x000C23F2 File Offset: 0x000C13F2
		public bool ControlAppDomain
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlAppDomain) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlAppDomain) : (this.m_flag & ~SecurityPermissionFlag.ControlAppDomain));
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003987 RID: 14727 RVA: 0x000C2417 File Offset: 0x000C1417
		// (set) Token: 0x06003988 RID: 14728 RVA: 0x000C242B File Offset: 0x000C142B
		public bool RemotingConfiguration
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.RemotingConfiguration) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.RemotingConfiguration) : (this.m_flag & ~SecurityPermissionFlag.RemotingConfiguration));
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003989 RID: 14729 RVA: 0x000C2450 File Offset: 0x000C1450
		// (set) Token: 0x0600398A RID: 14730 RVA: 0x000C2464 File Offset: 0x000C1464
		[ComVisible(true)]
		public bool Infrastructure
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.Infrastructure) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.Infrastructure) : (this.m_flag & ~SecurityPermissionFlag.Infrastructure));
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x000C2489 File Offset: 0x000C1489
		// (set) Token: 0x0600398C RID: 14732 RVA: 0x000C249D File Offset: 0x000C149D
		public bool BindingRedirects
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.BindingRedirects) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.BindingRedirects) : (this.m_flag & ~SecurityPermissionFlag.BindingRedirects));
			}
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000C24C2 File Offset: 0x000C14C2
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			return new SecurityPermission(this.m_flag);
		}

		// Token: 0x04001DC2 RID: 7618
		private SecurityPermissionFlag m_flag;
	}
}
