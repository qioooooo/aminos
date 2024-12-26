using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000020 RID: 32
	[ComVisible(true)]
	[Guid("C1468187-3DA1-49df-ADF8-5F8600E59EA8")]
	public interface IParseText
	{
		// Token: 0x06000131 RID: 305
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Parse(string code, IErrorHandler error);
	}
}
