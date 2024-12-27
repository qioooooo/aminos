using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E9 RID: 745
	[Guid("D002E9BA-D9E3-3749-B1D3-D565A08B13E7")]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(Module))]
	[ComVisible(true)]
	public interface _Module
	{
		// Token: 0x06001DD4 RID: 7636
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001DD5 RID: 7637
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001DD6 RID: 7638
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001DD7 RID: 7639
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
