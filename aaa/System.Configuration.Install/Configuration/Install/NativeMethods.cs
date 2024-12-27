using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Install
{
	// Token: 0x02000017 RID: 23
	internal static class NativeMethods
	{
		// Token: 0x06000089 RID: 137
		[DllImport("msi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int MsiCreateRecord(int cParams);

		// Token: 0x0600008A RID: 138
		[DllImport("msi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int MsiRecordSetInteger(int hRecord, int iField, int iValue);

		// Token: 0x0600008B RID: 139
		[DllImport("msi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int MsiRecordSetStringW(int hRecord, int iField, string szValue);

		// Token: 0x0600008C RID: 140
		[DllImport("msi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int MsiProcessMessage(int hInstall, int messageType, int hRecord);

		// Token: 0x040000F4 RID: 244
		public const int INSTALLMESSAGE_ERROR = 16777216;
	}
}
