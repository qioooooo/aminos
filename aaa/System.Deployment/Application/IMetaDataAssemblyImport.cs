using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x02000080 RID: 128
	[Guid("EE62470B-E94B-424e-9B7C-2F00C9249F93")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMetaDataAssemblyImport
	{
		// Token: 0x060003F0 RID: 1008
		void GetAssemblyProps(uint mdAsm, out IntPtr pPublicKeyPtr, out uint ucbPublicKeyPtr, out uint uHashAlg, [MarshalAs(UnmanagedType.LPArray)] char[] strName, uint cchNameIn, out uint cchNameRequired, IntPtr amdInfo, out uint dwFlags);

		// Token: 0x060003F1 RID: 1009
		void GetAssemblyRefProps(uint mdAsmRef, out IntPtr ppbPublicKeyOrToken, out uint pcbPublicKeyOrToken, [MarshalAs(UnmanagedType.LPArray)] char[] strName, uint cchNameIn, out uint pchNameOut, IntPtr amdInfo, out IntPtr ppbHashValue, out uint pcbHashValue, out uint pdwAssemblyRefFlags);

		// Token: 0x060003F2 RID: 1010
		void GetFileProps([In] uint mdFile, [MarshalAs(UnmanagedType.LPArray)] char[] strName, uint cchName, out uint cchNameRequired, out IntPtr bHashData, out uint cchHashBytes, out uint dwFileFlags);

		// Token: 0x060003F3 RID: 1011
		void GetExportedTypeProps();

		// Token: 0x060003F4 RID: 1012
		void GetManifestResourceProps();

		// Token: 0x060003F5 RID: 1013
		void EnumAssemblyRefs([In] [Out] ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] asmRefs, uint asmRefCount, out uint iFetched);

		// Token: 0x060003F6 RID: 1014
		void EnumFiles([In] [Out] ref IntPtr phEnum, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] fileRefs, uint fileRefCount, out uint iFetched);

		// Token: 0x060003F7 RID: 1015
		void EnumExportedTypes();

		// Token: 0x060003F8 RID: 1016
		void EnumManifestResources();

		// Token: 0x060003F9 RID: 1017
		void GetAssemblyFromScope(out uint mdAsm);

		// Token: 0x060003FA RID: 1018
		void FindExportedTypeByName();

		// Token: 0x060003FB RID: 1019
		void FindManifestResourceByName();

		// Token: 0x060003FC RID: 1020
		[PreserveSig]
		void CloseEnum([In] IntPtr phEnum);

		// Token: 0x060003FD RID: 1021
		void FindAssembliesByName();
	}
}
