using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000079 RID: 121
	[Guid("B638799F-6598-4c56-B3ED-509CA3F35B22")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpCategory
	{
		// Token: 0x060001EF RID: 495
		void GetType(out SPCATEGORYTYPE peCategoryType);

		// Token: 0x060001F0 RID: 496
		void SetPrefix([MarshalAs(UnmanagedType.LPWStr)] string pszPrefix);

		// Token: 0x060001F1 RID: 497
		void GetPrefix([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemPrefix);

		// Token: 0x060001F2 RID: 498
		void SetIsPrefixRequired([MarshalAs(UnmanagedType.Bool)] bool fRequired);

		// Token: 0x060001F3 RID: 499
		void GetIsPrefixRequired([MarshalAs(UnmanagedType.Bool)] out bool pfRequired);

		// Token: 0x060001F4 RID: 500
		void SetState(SPCATEGORYSTATE eCategoryState);

		// Token: 0x060001F5 RID: 501
		void GetState(out SPCATEGORYSTATE peCategoryState);

		// Token: 0x060001F6 RID: 502
		void SetName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x060001F7 RID: 503
		void GetName([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemName);

		// Token: 0x060001F8 RID: 504
		void SetIcon([MarshalAs(UnmanagedType.LPWStr)] string pszIcon);

		// Token: 0x060001F9 RID: 505
		void GetIcon([MarshalAs(UnmanagedType.LPWStr)] out string ppszCoMemIcon);
	}
}
