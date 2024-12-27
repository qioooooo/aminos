using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000BC RID: 188
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7D8805A0-2EA7-11D1-B1CC-00AA00BA3258")]
	[ComImport]
	internal interface IObjPool
	{
		// Token: 0x06000467 RID: 1127
		void Init([MarshalAs(UnmanagedType.Interface)] object pClassInfo);

		// Token: 0x06000468 RID: 1128
		[return: MarshalAs(UnmanagedType.Interface)]
		object Get();

		// Token: 0x06000469 RID: 1129
		void SetOption(int eOption, int dwOption);

		// Token: 0x0600046A RID: 1130
		void PutNew([MarshalAs(UnmanagedType.Interface)] [In] object pObj);

		// Token: 0x0600046B RID: 1131
		void PutEndTx([MarshalAs(UnmanagedType.Interface)] [In] object pObj);

		// Token: 0x0600046C RID: 1132
		void PutDeactivated([MarshalAs(UnmanagedType.Interface)] [In] object pObj);

		// Token: 0x0600046D RID: 1133
		void Shutdown();
	}
}
