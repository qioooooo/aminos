using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x020007F9 RID: 2041
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C2323C25-F57F-3880-8A4D-12EBEA7A5852")]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(MethodRental))]
	public interface _MethodRental
	{
		// Token: 0x060048A7 RID: 18599
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048A8 RID: 18600
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048A9 RID: 18601
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048AA RID: 18602
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
