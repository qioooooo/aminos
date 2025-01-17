﻿using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000522 RID: 1314
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IConnectionPointContainer instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface UCOMIConnectionPointContainer
	{
		// Token: 0x060032EE RID: 13038
		void EnumConnectionPoints(out UCOMIEnumConnectionPoints ppEnum);

		// Token: 0x060032EF RID: 13039
		void FindConnectionPoint(ref Guid riid, out UCOMIConnectionPoint ppCP);
	}
}
