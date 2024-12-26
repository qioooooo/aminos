using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000139 RID: 313
	[Guid("D1A19408-BB6B-43eb-BB6F-E7CF6AF047D7")]
	[ComVisible(true)]
	public interface IDefineEvent
	{
		// Token: 0x06000E10 RID: 3600
		[return: MarshalAs(UnmanagedType.Interface)]
		object AddEvent(string code, int startLine);
	}
}
