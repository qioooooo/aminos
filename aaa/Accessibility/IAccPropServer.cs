using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x02000005 RID: 5
	[Guid("76C0DBBB-15E0-4E7B-B61B-20EEEA2001E0")]
	[InterfaceType(1)]
	[ComImport]
	public interface IAccPropServer
	{
		// Token: 0x06000018 RID: 24
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetPropValue([In] ref byte pIDString, [In] uint dwIDStringLen, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] out object pvarValue, out int pfHasProp);
	}
}
