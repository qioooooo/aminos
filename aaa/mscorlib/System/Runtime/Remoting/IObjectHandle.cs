using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x02000140 RID: 320
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("C460E2B4-E199-412a-8456-84DC3E4838C3")]
	public interface IObjectHandle
	{
		// Token: 0x06001220 RID: 4640
		object Unwrap();
	}
}
