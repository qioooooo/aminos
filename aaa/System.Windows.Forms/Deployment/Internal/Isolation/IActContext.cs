using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000132 RID: 306
	[Guid("0af57545-a72a-4fbe-813c-8554ed7d4528")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IActContext
	{
		// Token: 0x0600045E RID: 1118
		void GetAppId([MarshalAs(UnmanagedType.Interface)] out object AppId);

		// Token: 0x0600045F RID: 1119
		void EnumCategories([In] uint Flags, [In] IReferenceIdentity CategoryToMatch, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x06000460 RID: 1120
		void EnumSubcategories([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPattern, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x06000461 RID: 1121
		void EnumCategoryInstances([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string Subcategory, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object EnumOut);

		// Token: 0x06000462 RID: 1122
		void ReplaceStringMacros([In] uint Flags, [MarshalAs(UnmanagedType.LPWStr)] [In] string Culture, [MarshalAs(UnmanagedType.LPWStr)] [In] string ReplacementPattern, [MarshalAs(UnmanagedType.LPWStr)] out string Replaced);

		// Token: 0x06000463 RID: 1123
		void GetComponentStringTableStrings([In] uint Flags, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr ComponentIndex, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr StringCount, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] SourceStrings, [MarshalAs(UnmanagedType.LPArray)] out string[] DestinationStrings, [MarshalAs(UnmanagedType.SysUInt)] [In] IntPtr CultureFallbacks);

		// Token: 0x06000464 RID: 1124
		void GetApplicationProperties([In] uint Flags, [In] UIntPtr cProperties, [MarshalAs(UnmanagedType.LPArray)] [In] string[] PropertyNames, [MarshalAs(UnmanagedType.LPArray)] out string[] PropertyValues, [MarshalAs(UnmanagedType.LPArray)] out UIntPtr[] ComponentIndicies);

		// Token: 0x06000465 RID: 1125
		void ApplicationBasePath([In] uint Flags, [MarshalAs(UnmanagedType.LPWStr)] out string ApplicationPath);

		// Token: 0x06000466 RID: 1126
		void GetComponentManifest([In] uint Flags, [In] IDefinitionIdentity ComponentId, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ManifestInteface);

		// Token: 0x06000467 RID: 1127
		void GetComponentPayloadPath([In] uint Flags, [In] IDefinitionIdentity ComponentId, [MarshalAs(UnmanagedType.LPWStr)] out string PayloadPath);

		// Token: 0x06000468 RID: 1128
		void FindReferenceInContext([In] uint dwFlags, [In] IReferenceIdentity Reference, [MarshalAs(UnmanagedType.Interface)] out object MatchedDefinition);

		// Token: 0x06000469 RID: 1129
		void CreateActContextFromCategoryInstance([In] uint dwFlags, [In] ref CATEGORY_INSTANCE CategoryInstance, [MarshalAs(UnmanagedType.Interface)] out object ppCreatedAppContext);

		// Token: 0x0600046A RID: 1130
		void EnumComponents([In] uint dwFlags, [MarshalAs(UnmanagedType.Interface)] out object ppIdentityEnum);

		// Token: 0x0600046B RID: 1131
		void PrepareForExecution([MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Inputs, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Outputs);

		// Token: 0x0600046C RID: 1132
		void SetApplicationRunningState([In] uint dwFlags, [In] uint ulState, out uint ulDisposition);

		// Token: 0x0600046D RID: 1133
		void GetApplicationStateFilesystemLocation([In] uint dwFlags, [In] UIntPtr Component, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr pCoordinateList, [MarshalAs(UnmanagedType.LPWStr)] out string ppszPath);

		// Token: 0x0600046E RID: 1134
		void FindComponentsByDefinition([In] uint dwFlags, [In] UIntPtr ComponentCount, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] Components, [MarshalAs(UnmanagedType.LPArray)] [Out] UIntPtr[] Indicies, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] Dispositions);

		// Token: 0x0600046F RID: 1135
		void FindComponentsByReference([In] uint dwFlags, [In] UIntPtr Components, [MarshalAs(UnmanagedType.LPArray)] [In] IReferenceIdentity[] References, [MarshalAs(UnmanagedType.LPArray)] [Out] UIntPtr[] Indicies, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] Dispositions);
	}
}
