using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace System.Security.Permissions
{
	// Token: 0x02000627 RID: 1575
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class FileIOPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003926 RID: 14630 RVA: 0x000C1C6B File Offset: 0x000C0C6B
		public FileIOPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003927 RID: 14631 RVA: 0x000C1C74 File Offset: 0x000C0C74
		// (set) Token: 0x06003928 RID: 14632 RVA: 0x000C1C7C File Offset: 0x000C0C7C
		public string Read
		{
			get
			{
				return this.m_read;
			}
			set
			{
				this.m_read = value;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003929 RID: 14633 RVA: 0x000C1C85 File Offset: 0x000C0C85
		// (set) Token: 0x0600392A RID: 14634 RVA: 0x000C1C8D File Offset: 0x000C0C8D
		public string Write
		{
			get
			{
				return this.m_write;
			}
			set
			{
				this.m_write = value;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x0600392B RID: 14635 RVA: 0x000C1C96 File Offset: 0x000C0C96
		// (set) Token: 0x0600392C RID: 14636 RVA: 0x000C1C9E File Offset: 0x000C0C9E
		public string Append
		{
			get
			{
				return this.m_append;
			}
			set
			{
				this.m_append = value;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x000C1CA7 File Offset: 0x000C0CA7
		// (set) Token: 0x0600392E RID: 14638 RVA: 0x000C1CAF File Offset: 0x000C0CAF
		public string PathDiscovery
		{
			get
			{
				return this.m_pathDiscovery;
			}
			set
			{
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x000C1CB8 File Offset: 0x000C0CB8
		// (set) Token: 0x06003930 RID: 14640 RVA: 0x000C1CC0 File Offset: 0x000C0CC0
		public string ViewAccessControl
		{
			get
			{
				return this.m_viewAccess;
			}
			set
			{
				this.m_viewAccess = value;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x000C1CC9 File Offset: 0x000C0CC9
		// (set) Token: 0x06003932 RID: 14642 RVA: 0x000C1CD1 File Offset: 0x000C0CD1
		public string ChangeAccessControl
		{
			get
			{
				return this.m_changeAccess;
			}
			set
			{
				this.m_changeAccess = value;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003934 RID: 14644 RVA: 0x000C1CF8 File Offset: 0x000C0CF8
		// (set) Token: 0x06003933 RID: 14643 RVA: 0x000C1CDA File Offset: 0x000C0CDA
		[Obsolete("Please use the ViewAndModify property instead.")]
		public string All
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_read = value;
				this.m_write = value;
				this.m_append = value;
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x000C1D09 File Offset: 0x000C0D09
		// (set) Token: 0x06003936 RID: 14646 RVA: 0x000C1D1A File Offset: 0x000C0D1A
		public string ViewAndModify
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GetMethod"));
			}
			set
			{
				this.m_read = value;
				this.m_write = value;
				this.m_append = value;
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x000C1D38 File Offset: 0x000C0D38
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x000C1D40 File Offset: 0x000C0D40
		public FileIOPermissionAccess AllFiles
		{
			get
			{
				return this.m_allFiles;
			}
			set
			{
				this.m_allFiles = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003939 RID: 14649 RVA: 0x000C1D49 File Offset: 0x000C0D49
		// (set) Token: 0x0600393A RID: 14650 RVA: 0x000C1D51 File Offset: 0x000C0D51
		public FileIOPermissionAccess AllLocalFiles
		{
			get
			{
				return this.m_allLocalFiles;
			}
			set
			{
				this.m_allLocalFiles = value;
			}
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x000C1D5C File Offset: 0x000C0D5C
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new FileIOPermission(PermissionState.Unrestricted);
			}
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.None);
			if (this.m_read != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.Read, this.m_read);
			}
			if (this.m_write != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.Write, this.m_write);
			}
			if (this.m_append != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.Append, this.m_append);
			}
			if (this.m_pathDiscovery != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.PathDiscovery, this.m_pathDiscovery);
			}
			if (this.m_viewAccess != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.NoAccess, AccessControlActions.View, new string[] { this.m_viewAccess }, false);
			}
			if (this.m_changeAccess != null)
			{
				fileIOPermission.SetPathList(FileIOPermissionAccess.NoAccess, AccessControlActions.Change, new string[] { this.m_changeAccess }, false);
			}
			fileIOPermission.AllFiles = this.m_allFiles;
			fileIOPermission.AllLocalFiles = this.m_allLocalFiles;
			return fileIOPermission;
		}

		// Token: 0x04001DAB RID: 7595
		private string m_read;

		// Token: 0x04001DAC RID: 7596
		private string m_write;

		// Token: 0x04001DAD RID: 7597
		private string m_append;

		// Token: 0x04001DAE RID: 7598
		private string m_pathDiscovery;

		// Token: 0x04001DAF RID: 7599
		private string m_viewAccess;

		// Token: 0x04001DB0 RID: 7600
		private string m_changeAccess;

		// Token: 0x04001DB1 RID: 7601
		[OptionalField(VersionAdded = 2)]
		private FileIOPermissionAccess m_allLocalFiles;

		// Token: 0x04001DB2 RID: 7602
		[OptionalField(VersionAdded = 2)]
		private FileIOPermissionAccess m_allFiles;
	}
}
