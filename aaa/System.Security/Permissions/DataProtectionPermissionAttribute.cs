using System;

namespace System.Security.Permissions
{
	// Token: 0x020000CF RID: 207
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[Serializable]
	public sealed class DataProtectionPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x000198F2 File Offset: 0x000188F2
		public DataProtectionPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x000198FB File Offset: 0x000188FB
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00019903 File Offset: 0x00018903
		public DataProtectionPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				DataProtectionPermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00019912 File Offset: 0x00018912
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00019922 File Offset: 0x00018922
		public bool ProtectData
		{
			get
			{
				return (this.m_flags & DataProtectionPermissionFlags.ProtectData) != DataProtectionPermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | DataProtectionPermissionFlags.ProtectData) : (this.m_flags & ~DataProtectionPermissionFlags.ProtectData));
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00019940 File Offset: 0x00018940
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00019950 File Offset: 0x00018950
		public bool UnprotectData
		{
			get
			{
				return (this.m_flags & DataProtectionPermissionFlags.UnprotectData) != DataProtectionPermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | DataProtectionPermissionFlags.UnprotectData) : (this.m_flags & ~DataProtectionPermissionFlags.UnprotectData));
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001996E File Offset: 0x0001896E
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x0001997E File Offset: 0x0001897E
		public bool ProtectMemory
		{
			get
			{
				return (this.m_flags & DataProtectionPermissionFlags.ProtectMemory) != DataProtectionPermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | DataProtectionPermissionFlags.ProtectMemory) : (this.m_flags & ~DataProtectionPermissionFlags.ProtectMemory));
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001999C File Offset: 0x0001899C
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x000199AC File Offset: 0x000189AC
		public bool UnprotectMemory
		{
			get
			{
				return (this.m_flags & DataProtectionPermissionFlags.UnprotectMemory) != DataProtectionPermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | DataProtectionPermissionFlags.UnprotectMemory) : (this.m_flags & ~DataProtectionPermissionFlags.UnprotectMemory));
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000199CA File Offset: 0x000189CA
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new DataProtectionPermission(PermissionState.Unrestricted);
			}
			return new DataProtectionPermission(this.m_flags);
		}

		// Token: 0x040005CF RID: 1487
		private DataProtectionPermissionFlags m_flags;
	}
}
