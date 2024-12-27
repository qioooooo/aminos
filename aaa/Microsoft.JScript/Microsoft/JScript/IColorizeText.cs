using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200001B RID: 27
	[Guid("DB283E60-7ADB-4cf6-9758-2931893A12FC")]
	[ComVisible(true)]
	public interface IColorizeText
	{
		// Token: 0x0600012A RID: 298
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		ITokenEnumerator Colorize(string sourceCode, SourceState state);

		// Token: 0x0600012B RID: 299
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		SourceState GetStateForText(string sourceCode, SourceState currentState);
	}
}
