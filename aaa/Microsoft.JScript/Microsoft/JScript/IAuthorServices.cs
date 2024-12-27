using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200001A RID: 26
	[ComVisible(true)]
	[Guid("9E2B453C-6EAA-4329-A619-62E4889C8C8A")]
	public interface IAuthorServices
	{
		// Token: 0x06000128 RID: 296
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		IColorizeText GetColorizer();

		// Token: 0x06000129 RID: 297
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		IParseText GetCodeSense();
	}
}
