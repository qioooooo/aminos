using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000556 RID: 1366
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IConnectionPoint
	{
		// Token: 0x06003378 RID: 13176
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x06003379 RID: 13177
		void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);

		// Token: 0x0600337A RID: 13178
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		// Token: 0x0600337B RID: 13179
		void Unadvise(int dwCookie);

		// Token: 0x0600337C RID: 13180
		void EnumConnections(out IEnumConnections ppEnum);
	}
}
