using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052B RID: 1323
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumVARIANT instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020404-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMIEnumVARIANT
	{
		// Token: 0x06003309 RID: 13065
		[PreserveSig]
		int Next(int celt, int rgvar, int pceltFetched);

		// Token: 0x0600330A RID: 13066
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600330B RID: 13067
		[PreserveSig]
		int Reset();

		// Token: 0x0600330C RID: 13068
		void Clone(int ppenum);
	}
}
