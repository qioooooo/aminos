using System;
using System.Runtime.InteropServices;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000A3 RID: 163
	[ComVisible(true)]
	[Guid("DC3691BC-F188-4b67-8338-326671E0F3F6")]
	public interface IVsaFullErrorInfo : IVsaError
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000797 RID: 1943
		int EndLine { get; }
	}
}
