using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050C RID: 1292
	[ComVisible(true)]
	public interface ICustomAdapter
	{
		// Token: 0x06003292 RID: 12946
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetUnderlyingObject();
	}
}
