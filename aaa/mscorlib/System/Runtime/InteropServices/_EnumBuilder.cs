using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x020007F3 RID: 2035
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C7BD73DE-9F85-3290-88EE-090B8BDFE2DF")]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(EnumBuilder))]
	public interface _EnumBuilder
	{
		// Token: 0x0600488F RID: 18575
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004890 RID: 18576
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004891 RID: 18577
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004892 RID: 18578
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
