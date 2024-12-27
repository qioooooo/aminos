using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000060 RID: 96
	[CLSCompliant(false)]
	[Guid("917B14D0-2D9E-38B8-92A9-381ACF52F7C0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[TypeLibImportClass(typeof(Attribute))]
	public interface _Attribute
	{
		// Token: 0x060005BF RID: 1471
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060005C0 RID: 1472
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060005C1 RID: 1473
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060005C2 RID: 1474
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
