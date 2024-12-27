using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x020007FD RID: 2045
	[CLSCompliant(false)]
	[Guid("7D13DD37-5A04-393C-BBCA-A5FEA802893D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(SignatureHelper))]
	[ComVisible(true)]
	public interface _SignatureHelper
	{
		// Token: 0x060048B7 RID: 18615
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048B8 RID: 18616
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048B9 RID: 18617
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048BA RID: 18618
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
