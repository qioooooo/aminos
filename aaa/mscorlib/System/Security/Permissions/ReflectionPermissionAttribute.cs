using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062A RID: 1578
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class ReflectionPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003952 RID: 14674 RVA: 0x000C1FA7 File Offset: 0x000C0FA7
		public ReflectionPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x000C1FB0 File Offset: 0x000C0FB0
		// (set) Token: 0x06003954 RID: 14676 RVA: 0x000C1FB8 File Offset: 0x000C0FB8
		public ReflectionPermissionFlag Flags
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

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x000C1FC1 File Offset: 0x000C0FC1
		// (set) Token: 0x06003956 RID: 14678 RVA: 0x000C1FD1 File Offset: 0x000C0FD1
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool TypeInformation
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.TypeInformation) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.TypeInformation) : (this.m_flag & ~ReflectionPermissionFlag.TypeInformation));
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x000C1FEF File Offset: 0x000C0FEF
		// (set) Token: 0x06003958 RID: 14680 RVA: 0x000C1FFF File Offset: 0x000C0FFF
		public bool MemberAccess
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.MemberAccess) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.MemberAccess) : (this.m_flag & ~ReflectionPermissionFlag.MemberAccess));
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x000C201D File Offset: 0x000C101D
		// (set) Token: 0x0600395A RID: 14682 RVA: 0x000C202D File Offset: 0x000C102D
		public bool ReflectionEmit
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.ReflectionEmit) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.ReflectionEmit) : (this.m_flag & ~ReflectionPermissionFlag.ReflectionEmit));
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x000C204B File Offset: 0x000C104B
		// (set) Token: 0x0600395C RID: 14684 RVA: 0x000C205B File Offset: 0x000C105B
		public bool RestrictedMemberAccess
		{
			get
			{
				return (this.m_flag & ReflectionPermissionFlag.RestrictedMemberAccess) != ReflectionPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | ReflectionPermissionFlag.RestrictedMemberAccess) : (this.m_flag & ~ReflectionPermissionFlag.RestrictedMemberAccess));
			}
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x000C2079 File Offset: 0x000C1079
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new ReflectionPermission(PermissionState.Unrestricted);
			}
			return new ReflectionPermission(this.m_flag);
		}

		// Token: 0x04001DBC RID: 7612
		private ReflectionPermissionFlag m_flag;
	}
}
