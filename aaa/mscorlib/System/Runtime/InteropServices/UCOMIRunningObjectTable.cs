using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000531 RID: 1329
	[Guid("00000010-0000-0000-C000-000000000046")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IRunningObjectTable instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIRunningObjectTable
	{
		// Token: 0x06003337 RID: 13111
		void Register(int grfFlags, [MarshalAs(UnmanagedType.Interface)] object punkObject, UCOMIMoniker pmkObjectName, out int pdwRegister);

		// Token: 0x06003338 RID: 13112
		void Revoke(int dwRegister);

		// Token: 0x06003339 RID: 13113
		void IsRunning(UCOMIMoniker pmkObjectName);

		// Token: 0x0600333A RID: 13114
		void GetObject(UCOMIMoniker pmkObjectName, [MarshalAs(UnmanagedType.Interface)] out object ppunkObject);

		// Token: 0x0600333B RID: 13115
		void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

		// Token: 0x0600333C RID: 13116
		void GetTimeOfLastChange(UCOMIMoniker pmkObjectName, out FILETIME pfiletime);

		// Token: 0x0600333D RID: 13117
		void EnumRunning(out UCOMIEnumMoniker ppenumMoniker);
	}
}
