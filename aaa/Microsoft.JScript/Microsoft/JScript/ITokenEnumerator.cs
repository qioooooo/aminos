using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200001E RID: 30
	[ComVisible(true)]
	[Guid("556BA9E0-BD6A-4837-89F0-C79B14759181")]
	public interface ITokenEnumerator
	{
		// Token: 0x0600012C RID: 300
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		ITokenColorInfo GetNext();

		// Token: 0x0600012D RID: 301
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Reset();
	}
}
