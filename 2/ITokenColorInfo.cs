using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200001F RID: 31
	[ComVisible(true)]
	[Guid("0F20D5C8-CBDB-4b64-AB7F-10B158407323")]
	public interface ITokenColorInfo
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600012E RID: 302
		int StartPosition
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600012F RID: 303
		int EndPosition
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000130 RID: 304
		TokenColor Color
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}
	}
}
