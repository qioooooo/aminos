using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x020007FB RID: 2043
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("36329EBA-F97A-3565-BC07-0ED5C6EF19FC")]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(ParameterBuilder))]
	public interface _ParameterBuilder
	{
		// Token: 0x060048AF RID: 18607
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048B0 RID: 18608
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048B1 RID: 18609
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048B2 RID: 18610
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
