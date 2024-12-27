using System;
using System.Runtime.InteropServices;

namespace System.Web
{
	// Token: 0x02000033 RID: 51
	// (Invoke) Token: 0x0600010C RID: 268
	internal delegate void NativeFileChangeNotification(FileAction action, [MarshalAs(UnmanagedType.LPWStr)] [In] string fileName, long ticks);
}
