using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000521 RID: 1313
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IBindCtx instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000e-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMIBindCtx
	{
		// Token: 0x060032E4 RID: 13028
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060032E5 RID: 13029
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060032E6 RID: 13030
		void ReleaseBoundObjects();

		// Token: 0x060032E7 RID: 13031
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		// Token: 0x060032E8 RID: 13032
		void GetBindOptions(ref BIND_OPTS pbindopts);

		// Token: 0x060032E9 RID: 13033
		void GetRunningObjectTable(out UCOMIRunningObjectTable pprot);

		// Token: 0x060032EA RID: 13034
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060032EB RID: 13035
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x060032EC RID: 13036
		void EnumObjectParam(out UCOMIEnumString ppenum);

		// Token: 0x060032ED RID: 13037
		void RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}
