using System;
using System.Runtime.InteropServices;

namespace System.Web.Services.Interop
{
	// Token: 0x0200001E RID: 30
	[Guid("26E7F0F1-B49C-48cb-B43E-78DCD577E1D9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface INotifySource2
	{
		// Token: 0x06000071 RID: 113
		void SetNotifyFilter([In] NotifyFilter in_NotifyFilter, [In] UserThread in_pUserThreadFilter);
	}
}
