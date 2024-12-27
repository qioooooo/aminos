using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000124 RID: 292
	[Guid("a5c62f6d-5e3e-4cd9-b345-6b281d7a1d1e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IStore
	{
		// Token: 0x06000414 RID: 1044
		void Transact([In] IntPtr cOperation, [MarshalAs(UnmanagedType.LPArray)] [In] StoreTransactionOperation[] rgOperations, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] rgDispositions, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgResults);

		// Token: 0x06000415 RID: 1045
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object BindReferenceToAssembly([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity, [In] uint cDeploymentsToIgnore, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore, [In] ref Guid riid);

		// Token: 0x06000416 RID: 1046
		void CalculateDelimiterOfDeploymentsBasedOnQuota([In] uint dwFlags, [In] IntPtr cDeployments, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionAppId[] rgpIDefinitionAppId_Deployments, [In] ref StoreApplicationReference InstallerReference, [In] ulong ulonglongQuota, [In] [Out] ref IntPtr Delimiter, [In] [Out] ref ulong SizeSharedWithExternalDeployment, [In] [Out] ref ulong SizeConsumedByInputDeploymentArray);

		// Token: 0x06000417 RID: 1047
		IntPtr BindDefinitions([In] uint Flags, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Count, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefsToBind, [In] uint DeploymentsToIgnore, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefsToIgnore);

		// Token: 0x06000418 RID: 1048
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAssemblyInformation([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x06000419 RID: 1049
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumAssemblies([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity_ToMatch, [In] ref Guid riid);

		// Token: 0x0600041A RID: 1050
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumFiles([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x0600041B RID: 1051
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallationReferences([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x0600041C RID: 1052
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string LockAssemblyPath([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, out IntPtr Cookie);

		// Token: 0x0600041D RID: 1053
		void ReleaseAssemblyPath([In] IntPtr Cookie);

		// Token: 0x0600041E RID: 1054
		ulong QueryChangeID([In] IDefinitionIdentity DefinitionIdentity);

		// Token: 0x0600041F RID: 1055
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumCategories([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity_ToMatch, [In] ref Guid riid);

		// Token: 0x06000420 RID: 1056
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumSubcategories([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPathPattern, [In] ref Guid riid);

		// Token: 0x06000421 RID: 1057
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumCategoryInstances([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPath, [In] ref Guid riid);

		// Token: 0x06000422 RID: 1058
		void GetDeploymentProperty([In] uint Flags, [In] IDefinitionAppId DeploymentInPackage, [In] ref StoreApplicationReference Reference, [In] ref Guid PropertySet, [MarshalAs(UnmanagedType.LPWStr)] [In] string pcwszPropertyName, out BLOB blob);

		// Token: 0x06000423 RID: 1059
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string LockApplicationPath([In] uint Flags, [In] IDefinitionAppId ApId, out IntPtr Cookie);

		// Token: 0x06000424 RID: 1060
		void ReleaseApplicationPath([In] IntPtr Cookie);

		// Token: 0x06000425 RID: 1061
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumPrivateFiles([In] uint Flags, [In] IDefinitionAppId Application, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x06000426 RID: 1062
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallerDeploymentMetadata([In] uint Flags, [In] ref StoreApplicationReference Reference, [In] IReferenceAppId Filter, [In] ref Guid riid);

		// Token: 0x06000427 RID: 1063
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallerDeploymentMetadataProperties([In] uint Flags, [In] ref StoreApplicationReference Reference, [In] IDefinitionAppId Filter, [In] ref Guid riid);
	}
}
