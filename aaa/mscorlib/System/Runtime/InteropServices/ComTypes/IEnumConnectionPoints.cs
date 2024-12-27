using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200055C RID: 1372
	[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumConnectionPoints
	{
		// Token: 0x06003389 RID: 13193
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] IConnectionPoint[] rgelt, IntPtr pceltFetched);

		// Token: 0x0600338A RID: 13194
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600338B RID: 13195
		void Reset();

		// Token: 0x0600338C RID: 13196
		void Clone(out IEnumConnectionPoints ppenum);
	}
}
