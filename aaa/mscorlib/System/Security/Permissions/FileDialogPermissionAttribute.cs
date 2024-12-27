using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000626 RID: 1574
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class FileDialogPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003920 RID: 14624 RVA: 0x000C1BEA File Offset: 0x000C0BEA
		public FileDialogPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x000C1BF3 File Offset: 0x000C0BF3
		// (set) Token: 0x06003922 RID: 14626 RVA: 0x000C1C03 File Offset: 0x000C0C03
		public bool Open
		{
			get
			{
				return (this.m_access & FileDialogPermissionAccess.Open) != FileDialogPermissionAccess.None;
			}
			set
			{
				this.m_access = (value ? (this.m_access | FileDialogPermissionAccess.Open) : (this.m_access & ~FileDialogPermissionAccess.Open));
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x000C1C21 File Offset: 0x000C0C21
		// (set) Token: 0x06003924 RID: 14628 RVA: 0x000C1C31 File Offset: 0x000C0C31
		public bool Save
		{
			get
			{
				return (this.m_access & FileDialogPermissionAccess.Save) != FileDialogPermissionAccess.None;
			}
			set
			{
				this.m_access = (value ? (this.m_access | FileDialogPermissionAccess.Save) : (this.m_access & ~FileDialogPermissionAccess.Save));
			}
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x000C1C4F File Offset: 0x000C0C4F
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new FileDialogPermission(PermissionState.Unrestricted);
			}
			return new FileDialogPermission(this.m_access);
		}

		// Token: 0x04001DAA RID: 7594
		private FileDialogPermissionAccess m_access;
	}
}
