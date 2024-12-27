using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000020 RID: 32
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("14056581-E16C-11D2-BB90-00C04F8EE6C0")]
	[ComImport]
	internal interface ISpDataKey
	{
		// Token: 0x060000AF RID: 175
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

		// Token: 0x060000B0 RID: 176
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x060000B1 RID: 177
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x060000B2 RID: 178
		[PreserveSig]
		int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] out string value);

		// Token: 0x060000B3 RID: 179
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint dwValue);

		// Token: 0x060000B4 RID: 180
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pdwValue);

		// Token: 0x060000B5 RID: 181
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string subKeyName, out ISpDataKey ppSubKey);

		// Token: 0x060000B6 RID: 182
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x060000B7 RID: 183
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKey);

		// Token: 0x060000B8 RID: 184
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName);

		// Token: 0x060000B9 RID: 185
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x060000BA RID: 186
		[PreserveSig]
		int EnumValues(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string valueName);
	}
}
