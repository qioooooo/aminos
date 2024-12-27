using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000094 RID: 148
	[ComVisible(true)]
	[Guid("0E4EFFC0-2387-11d3-B372-00105A98B7CE")]
	public class JSAuthor : IAuthorServices
	{
		// Token: 0x060006A5 RID: 1701 RVA: 0x0002ECB7 File Offset: 0x0002DCB7
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual IColorizeText GetColorizer()
		{
			return new JSColorizer();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002ECBE File Offset: 0x0002DCBE
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual IParseText GetCodeSense()
		{
			return new JSCodeSense();
		}
	}
}
