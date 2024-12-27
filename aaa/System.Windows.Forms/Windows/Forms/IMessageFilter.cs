using System;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200024B RID: 587
	public interface IMessageFilter
	{
		// Token: 0x06001E46 RID: 7750
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		bool PreFilterMessage(ref Message m);
	}
}
