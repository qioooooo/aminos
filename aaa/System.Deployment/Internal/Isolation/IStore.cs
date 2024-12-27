using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200014F RID: 335
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a5c62f6d-5e3e-4cd9-b345-6b281d7a1d1e")]
	[ComImport]
	internal interface IStore
	{
		// Token: 0x060006FE RID: 1790
		void Transact([In] IntPtr cOperation, [MarshalAs(UnmanagedType.LPArray)] [In] StoreTransactionOperation[] rgOperations, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] rgDispositions, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgResults);

		// Token: 0x060006FF RID: 1791
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object BindReferenceToAssembly([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity, [In] uint cDeploymentsToIgnore, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore, [In] ref Guid riid);

		// Token: 0x06000700 RID: 1792
		void CalculateDelimiterOfDeploymentsBasedOnQuota([In] uint dwFlags, [In] IntPtr cDeployments, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionAppId[] rgpIDefinitionAppId_Deployments, [In] ref StoreApplicationReference InstallerReference, [In] ulong ulonglongQuota, [In] [Out] ref IntPtr Delimiter, [In] [Out] ref ulong SizeSharedWithExternalDeployment, [In] [Out] ref ulong SizeConsumedByInputDeploymentArray);

		// Token: 0x06000701 RID: 1793
		IntPtr BindDefinitions([In] uint Flags, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr Count, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefsToBind, [In] uint DeploymentsToIgnore, [MarshalAs(UnmanagedType.LPArray)] [In] IDefinitionIdentity[] DefsToIgnore);

		// Token: 0x06000702 RID: 1794
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAssemblyInformation([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x06000703 RID: 1795
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumAssemblies([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity_ToMatch, [In] ref Guid riid);

		// Token: 0x06000704 RID: 1796
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumFiles([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x06000705 RID: 1797
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallationReferences([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x06000706 RID: 1798
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string LockAssemblyPath([In] uint Flags, [In] IDefinitionIdentity DefinitionIdentity, out IntPtr Cookie);

		// Token: 0x06000707 RID: 1799
		void ReleaseAssemblyPath([In] IntPtr Cookie);

		// Token: 0x06000708 RID: 1800
		ulong QueryChangeID([In] IDefinitionIdentity DefinitionIdentity);

		// Token: 0x06000709 RID: 1801
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumCategories([In] uint Flags, [In] IReferenceIdentity ReferenceIdentity_ToMatch, [In] ref Guid riid);

		// Token: 0x0600070A RID: 1802
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumSubcategories([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPathPattern, [In] ref Guid riid);

		// Token: 0x0600070B RID: 1803
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumCategoryInstances([In] uint Flags, [In] IDefinitionIdentity CategoryId, [MarshalAs(UnmanagedType.LPWStr)] [In] string SubcategoryPath, [In] ref Guid riid);

		// Token: 0x0600070C RID: 1804
		void GetDeploymentProperty([In] uint Flags, [In] IDefinitionAppId DeploymentInPackage, [In] ref StoreApplicationReference Reference, [In] ref Guid PropertySet, [MarshalAs(UnmanagedType.LPWStr)] [In] string pcwszPropertyName, out BLOB blob);

		// Token: 0x0600070D RID: 1805
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string LockApplicationPath([In] uint Flags, [In] IDefinitionAppId ApId, out IntPtr Cookie);

		// Token: 0x0600070E RID: 1806
		void ReleaseApplicationPath([In] IntPtr Cookie);

		// Token: 0x0600070F RID: 1807
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumPrivateFiles([In] uint Flags, [In] IDefinitionAppId Application, [In] IDefinitionIdentity DefinitionIdentity, [In] ref Guid riid);

		// Token: 0x06000710 RID: 1808
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallerDeploymentMetadata([In] uint Flags, [In] ref StoreApplicationReference Reference, [In] IReferenceAppId Filter, [In] ref Guid riid);

		// Token: 0x06000711 RID: 1809
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object EnumInstallerDeploymentMetadataProperties([In] uint Flags, [In] ref StoreApplicationReference Reference, [In] IDefinitionAppId Filter, [In] ref Guid riid);
	}
}
