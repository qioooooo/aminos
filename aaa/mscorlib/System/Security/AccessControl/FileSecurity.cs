using System;
using System.IO;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x02000911 RID: 2321
	public sealed class FileSecurity : FileSystemSecurity
	{
		// Token: 0x06005451 RID: 21585 RVA: 0x0013264B File Offset: 0x0013164B
		public FileSecurity()
			: base(false)
		{
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x00132654 File Offset: 0x00131654
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public FileSecurity(string fileName, AccessControlSections includeSections)
			: base(false, fileName, includeSections, false)
		{
			string fullPathInternal = Path.GetFullPathInternal(fileName);
			new FileIOPermission(FileIOPermissionAccess.NoAccess, AccessControlActions.View, fullPathInternal).Demand();
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x0013267F File Offset: 0x0013167F
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal FileSecurity(SafeFileHandle handle, string fullPath, AccessControlSections includeSections)
			: base(false, handle, includeSections, false)
		{
			if (fullPath != null)
			{
				new FileIOPermission(FileIOPermissionAccess.NoAccess, AccessControlActions.View, fullPath).Demand();
				return;
			}
			new FileIOPermission(PermissionState.Unrestricted).Demand();
		}
	}
}
