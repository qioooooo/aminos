using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200000B RID: 11
	[Guid("74C08646-CEDB-11CF-8B49-00AA00B8A790")]
	[ComImport]
	internal interface IDispatchContext
	{
		// Token: 0x0600001A RID: 26
		void CreateInstance([MarshalAs(UnmanagedType.BStr)] [In] string bstrProgID, out object pObject);

		// Token: 0x0600001B RID: 27
		void SetComplete();

		// Token: 0x0600001C RID: 28
		void SetAbort();

		// Token: 0x0600001D RID: 29
		void EnableCommit();

		// Token: 0x0600001E RID: 30
		void DisableCommit();

		// Token: 0x0600001F RID: 31
		bool IsInTransaction();

		// Token: 0x06000020 RID: 32
		bool IsSecurityEnabled();

		// Token: 0x06000021 RID: 33
		bool IsCallerInRole([MarshalAs(UnmanagedType.BStr)] [In] string bstrRole);

		// Token: 0x06000022 RID: 34
		void Count(out int plCount);

		// Token: 0x06000023 RID: 35
		void Item([MarshalAs(UnmanagedType.BStr)] [In] string name, out object pItem);

		// Token: 0x06000024 RID: 36
		void _NewEnum([MarshalAs(UnmanagedType.Interface)] out object ppEnum);

		// Token: 0x06000025 RID: 37
		[return: MarshalAs(UnmanagedType.Interface)]
		object Security();

		// Token: 0x06000026 RID: 38
		[return: MarshalAs(UnmanagedType.Interface)]
		object ContextInfo();
	}
}
