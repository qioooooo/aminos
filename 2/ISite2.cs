using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000133 RID: 307
	[ComVisible(true)]
	[Guid("BFF6C980-0705-4394-88B8-A03A4B8B4CD7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISite2
	{
		// Token: 0x06000DFD RID: 3581
		object[] GetParentChain(object obj);
	}
}
