using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002DA RID: 730
	[CLSCompliant(false)]
	[Guid("B42B6AAC-317E-34D5-9FA9-093BB4160C50")]
	[TypeLibImportClass(typeof(AssemblyName))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface _AssemblyName
	{
		// Token: 0x06001CCE RID: 7374
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001CCF RID: 7375
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001CD0 RID: 7376
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001CD1 RID: 7377
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
