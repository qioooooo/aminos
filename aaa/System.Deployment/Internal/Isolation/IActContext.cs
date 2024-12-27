using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200015B RID: 347
	[Guid("0af57545-a72a-4fbe-813c-8554ed7d4528")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IActContext
	{
		// Token: 0x06000738 RID: 1848
		void GetAppId([MarshalAs(UnmanagedType.Interface)] out object AppId);

		// Token: 0x06000739 RID: 1849
		void EnumCategories([In] uint Flags, [In] IReferenceIdentity CategoryToMatch, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x0600073A RID: 1850
		void EnumSubcategories([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPattern, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x0600073B RID: 1851
		void EnumCategoryInstances([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string Subcategory, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x0600073C RID: 1852
		void ReplaceStringMacros([In] uint Flags, [MarshalAs(UnmanagedType.LPWStr)] [In] string Culture, [MarshalAs(UnmanagedType.LPWStr)] [In] string ReplacementPattern, [MarshalAs(UnmanagedType.LPWStr)] out string Replaced);

		// Token: 0x0600073D RID: 1853
		void GetComponentStringTableStrings([In] uint Flags, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr ComponentIndex, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr StringCount, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] SourceStrings, [MarshalAs(UnmanagedType.LPArray)] out string[] DestinationStrings, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr CultureFallbacks);

		// Token: 0x0600073E RID: 1854
		void GetApplicationProperties([In] uint Flags, [In] UIntPtr cProperties, [MarshalAs(UnmanagedType.LPArray)] [In] string[] PropertyNames, [MarshalAs(UnmanagedType.LPArray)] out string[] PropertyValues, [MarshalAs(UnmanagedType.LPArray)] out UIntPtr[] ComponentIndicies);

		// Token: 0x0600073F RID: 1855
		void ApplicationBasePath([In] uint Flags, [MarshalAs(UnmanagedType.LPWStr)] out string ApplicationPath);

		// Token: 0x06000740 RID: 1856
		void GetComponentManifest([In] uint Flags, [In] IDefinitionIdentity ComponentId, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ManifestInteface);

		// Token: 0x06000741 RID: 1857
		void GetComponentPayloadPath([In] uint Flags, [In] IDefinitionIdentity ComponentId, [MarshalAs(UnmanagedType.LPWStr)] out string PayloadPath);

		// Token: 0x06000742 RID: 1858
		void FindReferenceInContext([In] uint dwFlags, [In] IReferenceIdentity Reference, [MarshalAs(UnmanagedType.Interface)] out object MatchedDefinition);

		// Token: 0x06000743 RID: 1859
		void CreateActContextFromCategoryInstance([In] uint dwFlags, [In] ref CATEGORY_INSTANCE CategoryInstance, [MarshalAs(UnmanagedType.Interface)] out object ppCreatedAppContext);

		// Token: 0x06000744 RID: 1860
		void EnumComponents([In] uint dwFlags, [MarshalAs(UnmanagedType.Interface)] out object ppIdentityEnum);

		// Token: 0x06000745 RID: 1861
		void PrepareForExecution([MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Inputs, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Outputs);

		// Token: 0x06000746 RID: 1862
		void SetApplicationRunningState([In] uint dwFlags, [In] uint ulState, out uint ulDisposition);

		// Token: 0x06000747 RID: 1863
		void GetApplicationStateFilesystemLocation([In] uint dwFlags, [In] UIntPtr Component, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr pCoordinateList, [MarshalAs(UnmanagedType.LPWStr)] out string ppszPath);

		// Token: 0x06000748 RID: 1864
		void FindComponentsByDefinition([In] uint dwFlags, [In] UIntPtr ComponentCount, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] Components, [MarshalAs(UnmanagedType.LPArray)] [Out] UIntPtr[] Indicies, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] Dispositions);

		// Token: 0x06000749 RID: 1865
		void FindComponentsByReference([In] uint dwFlags, [In] UIntPtr Components, [MarshalAs(UnmanagedType.LPArray)] [In] IReferenceIdentity[] References, [MarshalAs(UnmanagedType.LPArray)] [Out] UIntPtr[] Indicies, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] Dispositions);
	}
}
