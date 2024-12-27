using System;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000087 RID: 135
	[ComVisible(true)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(Thread))]
	[Guid("C281C7F1-4AA9-3517-961A-463CFED57E75")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _Thread
	{
		// Token: 0x0600076C RID: 1900
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600076D RID: 1901
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600076E RID: 1902
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600076F RID: 1903
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
