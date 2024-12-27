using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020001E7 RID: 487
	[Guid("458AB8A2-A1EA-4d7b-8EBE-DEE5D3D9442C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface IWin32Window
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001342 RID: 4930
		IntPtr Handle { get; }
	}
}
