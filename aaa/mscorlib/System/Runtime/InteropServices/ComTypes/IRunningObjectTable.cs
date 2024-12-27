using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000564 RID: 1380
	[Guid("00000010-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IRunningObjectTable
	{
		// Token: 0x060033BF RID: 13247
		int Register(int grfFlags, [MarshalAs(UnmanagedType.Interface)] object punkObject, IMoniker pmkObjectName);

		// Token: 0x060033C0 RID: 13248
		void Revoke(int dwRegister);

		// Token: 0x060033C1 RID: 13249
		[PreserveSig]
		int IsRunning(IMoniker pmkObjectName);

		// Token: 0x060033C2 RID: 13250
		[PreserveSig]
		int GetObject(IMoniker pmkObjectName, [MarshalAs(UnmanagedType.Interface)] out object ppunkObject);

		// Token: 0x060033C3 RID: 13251
		void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

		// Token: 0x060033C4 RID: 13252
		[PreserveSig]
		int GetTimeOfLastChange(IMoniker pmkObjectName, out FILETIME pfiletime);

		// Token: 0x060033C5 RID: 13253
		void EnumRunning(out IEnumMoniker ppenumMoniker);
	}
}
