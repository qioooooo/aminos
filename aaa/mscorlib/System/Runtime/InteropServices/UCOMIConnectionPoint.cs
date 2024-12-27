using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000523 RID: 1315
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IConnectionPoint instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface UCOMIConnectionPoint
	{
		// Token: 0x060032F0 RID: 13040
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x060032F1 RID: 13041
		void GetConnectionPointContainer(out UCOMIConnectionPointContainer ppCPC);

		// Token: 0x060032F2 RID: 13042
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		// Token: 0x060032F3 RID: 13043
		void Unadvise(int dwCookie);

		// Token: 0x060032F4 RID: 13044
		void EnumConnections(out UCOMIEnumConnections ppEnum);
	}
}
