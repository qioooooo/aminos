using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x02000003 RID: 3
	[InterfaceType(1)]
	[TypeLibType(272)]
	[Guid("03022430-ABC4-11D0-BDE2-00AA001A1953")]
	[ComImport]
	public interface IAccessibleHandler
	{
		// Token: 0x06000016 RID: 22
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AccessibleObjectFromID([In] int hwnd, [In] int lObjectID, [MarshalAs(UnmanagedType.Interface)] out IAccessible pIAccessible);
	}
}
