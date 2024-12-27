using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000023 RID: 35
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("14056589-E16C-11D2-BB90-00C04F8EE6C0")]
	[ComImport]
	internal interface ISpObjectToken : ISpDataKey
	{
		// Token: 0x060000E4 RID: 228
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pData);

		// Token: 0x060000E5 RID: 229
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pData);

		// Token: 0x060000E6 RID: 230
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] string pszValue);

		// Token: 0x060000E7 RID: 231
		[PreserveSig]
		int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValue);

		// Token: 0x060000E8 RID: 232
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, uint dwValue);

		// Token: 0x060000E9 RID: 233
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pdwValue);

		// Token: 0x060000EA RID: 234
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x060000EB RID: 235
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKey, out ISpDataKey ppSubKey);

		// Token: 0x060000EC RID: 236
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKey);

		// Token: 0x060000ED RID: 237
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName);

		// Token: 0x060000EE RID: 238
		[PreserveSig]
		int EnumKeys(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x060000EF RID: 239
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValueName);

		// Token: 0x060000F0 RID: 240
		void SetId([MarshalAs(UnmanagedType.LPWStr)] string pszCategoryId, [MarshalAs(UnmanagedType.LPWStr)] string pszTokenId, [MarshalAs(UnmanagedType.Bool)] bool fCreateIfNotExist);

		// Token: 0x060000F1 RID: 241
		void GetId(out IntPtr ppszCoMemTokenId);

		// Token: 0x060000F2 RID: 242
		void Slot15();

		// Token: 0x060000F3 RID: 243
		void Slot16();

		// Token: 0x060000F4 RID: 244
		void Slot17();

		// Token: 0x060000F5 RID: 245
		void Slot18();

		// Token: 0x060000F6 RID: 246
		void Slot19();

		// Token: 0x060000F7 RID: 247
		void Slot20();

		// Token: 0x060000F8 RID: 248
		void Slot21();

		// Token: 0x060000F9 RID: 249
		void MatchesAttributes([MarshalAs(UnmanagedType.LPWStr)] string pszAttributes, [MarshalAs(UnmanagedType.Bool)] out bool pfMatches);
	}
}
