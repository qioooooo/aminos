using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002E8 RID: 744
	[Guid("993634C4-E47A-32CC-BE08-85F567DC27D6")]
	[TypeLibImportClass(typeof(ParameterInfo))]
	[CLSCompliant(false)]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _ParameterInfo
	{
		// Token: 0x06001DD0 RID: 7632
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001DD1 RID: 7633
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001DD2 RID: 7634
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001DD3 RID: 7635
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
