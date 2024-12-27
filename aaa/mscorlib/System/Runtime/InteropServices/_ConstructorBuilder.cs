using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x020007F1 RID: 2033
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(ConstructorBuilder))]
	[Guid("ED3E4384-D7E2-3FA7-8FFD-8940D330519A")]
	[CLSCompliant(false)]
	public interface _ConstructorBuilder
	{
		// Token: 0x06004887 RID: 18567
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004888 RID: 18568
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004889 RID: 18569
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600488A RID: 18570
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
