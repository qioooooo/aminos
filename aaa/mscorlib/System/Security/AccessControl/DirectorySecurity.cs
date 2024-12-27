using System;
using System.IO;
using System.Security.Permissions;

namespace System.Security.AccessControl
{
	// Token: 0x02000912 RID: 2322
	public sealed class DirectorySecurity : FileSystemSecurity
	{
		// Token: 0x06005454 RID: 21588 RVA: 0x001326A7 File Offset: 0x001316A7
		public DirectorySecurity()
			: base(true)
		{
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x001326B0 File Offset: 0x001316B0
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		public DirectorySecurity(string name, AccessControlSections includeSections)
			: base(true, name, includeSections, true)
		{
			string fullPathInternal = Path.GetFullPathInternal(name);
			new FileIOPermission(FileIOPermissionAccess.NoAccess, AccessControlActions.View, fullPathInternal).Demand();
		}
	}
}
