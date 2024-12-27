using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000037 RID: 55
	[Guid("2D3D3845-39AF-4850-BBF9-40B49780011D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpObjectTokenCategory : ISpDataKey
	{
		// Token: 0x0600016B RID: 363
		[PreserveSig]
		int SetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint cbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data);

		// Token: 0x0600016C RID: 364
		[PreserveSig]
		int GetData([MarshalAs(UnmanagedType.LPWStr)] string valueName, ref uint pcbData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] data);

		// Token: 0x0600016D RID: 365
		[PreserveSig]
		int SetStringValue([MarshalAs(UnmanagedType.LPWStr)] string valueName, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x0600016E RID: 366
		[PreserveSig]
		void GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValue);

		// Token: 0x0600016F RID: 367
		[PreserveSig]
		int SetDWORD([MarshalAs(UnmanagedType.LPWStr)] string valueName, uint dwValue);

		// Token: 0x06000170 RID: 368
		[PreserveSig]
		int GetDWORD([MarshalAs(UnmanagedType.LPWStr)] string pszValueName, ref uint pdwValue);

		// Token: 0x06000171 RID: 369
		[PreserveSig]
		int OpenKey([MarshalAs(UnmanagedType.LPWStr)] string pszSubKeyName, out ISpDataKey ppSubKey);

		// Token: 0x06000172 RID: 370
		[PreserveSig]
		int CreateKey([MarshalAs(UnmanagedType.LPWStr)] string subKey, out ISpDataKey ppSubKey);

		// Token: 0x06000173 RID: 371
		[PreserveSig]
		int DeleteKey([MarshalAs(UnmanagedType.LPWStr)] string subKey);

		// Token: 0x06000174 RID: 372
		[PreserveSig]
		int DeleteValue([MarshalAs(UnmanagedType.LPWStr)] string valueName);

		// Token: 0x06000175 RID: 373
		[PreserveSig]
		int EnumKeys(uint index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszSubKeyName);

		// Token: 0x06000176 RID: 374
		[PreserveSig]
		int EnumValues(uint Index, [MarshalAs(UnmanagedType.LPWStr)] out string ppszValueName);

		// Token: 0x06000177 RID: 375
		void SetId([MarshalAs(UnmanagedType.LPWStr)] string pszCategoryId, [MarshalAs(UnmanagedType.Bool)] bool fCreateIfNotExist);

		// Token: 0x06000178 RID: 376
		void GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemCategoryId);

		// Token: 0x06000179 RID: 377
		void Slot14();

		// Token: 0x0600017A RID: 378
		void EnumTokens([MarshalAs(UnmanagedType.LPWStr)] string pzsReqAttribs, [MarshalAs(UnmanagedType.LPWStr)] string pszOptAttribs, out IEnumSpObjectTokens ppEnum);

		// Token: 0x0600017B RID: 379
		void Slot16();

		// Token: 0x0600017C RID: 380
		void GetDefaultTokenId([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemTokenId);
	}
}
