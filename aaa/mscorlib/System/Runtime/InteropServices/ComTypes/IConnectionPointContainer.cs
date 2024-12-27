using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000555 RID: 1365
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IConnectionPointContainer
	{
		// Token: 0x06003376 RID: 13174
		void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);

		// Token: 0x06003377 RID: 13175
		void FindConnectionPoint([In] ref Guid riid, out IConnectionPoint ppCP);
	}
}
