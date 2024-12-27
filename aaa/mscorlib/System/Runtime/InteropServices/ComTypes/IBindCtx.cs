using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000554 RID: 1364
	[Guid("0000000e-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IBindCtx
	{
		// Token: 0x0600336C RID: 13164
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x0600336D RID: 13165
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x0600336E RID: 13166
		void ReleaseBoundObjects();

		// Token: 0x0600336F RID: 13167
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		// Token: 0x06003370 RID: 13168
		void GetBindOptions(ref BIND_OPTS pbindopts);

		// Token: 0x06003371 RID: 13169
		void GetRunningObjectTable(out IRunningObjectTable pprot);

		// Token: 0x06003372 RID: 13170
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x06003373 RID: 13171
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x06003374 RID: 13172
		void EnumObjectParam(out IEnumString ppenum);

		// Token: 0x06003375 RID: 13173
		[PreserveSig]
		int RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}
