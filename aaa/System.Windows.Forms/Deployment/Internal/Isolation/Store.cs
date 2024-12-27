using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000116 RID: 278
	internal class Store
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00008620 File Offset: 0x00007620
		public IStore InternalStore
		{
			get
			{
				return this._pStore;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00008628 File Offset: 0x00007628
		public Store(IStore pStore)
		{
			if (pStore == null)
			{
				throw new ArgumentNullException("pStore");
			}
			this._pStore = pStore;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00008648 File Offset: 0x00007648
		public uint[] Transact(StoreTransactionOperation[] operations)
		{
			if (operations == null || operations.Length == 0)
			{
				throw new ArgumentException("operations");
			}
			uint[] array = new uint[operations.Length];
			int[] array2 = new int[operations.Length];
			this._pStore.Transact(new IntPtr(operations.Length), operations, array, array2);
			return array;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00008691 File Offset: 0x00007691
		public void Transact(StoreTransactionOperation[] operations, uint[] rgDispositions, int[] rgResults)
		{
			if (operations == null || operations.Length == 0)
			{
				throw new ArgumentException("operations");
			}
			this._pStore.Transact(new IntPtr(operations.Length), operations, rgDispositions, rgResults);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000086BC File Offset: 0x000076BC
		public IDefinitionIdentity BindReferenceToAssemblyIdentity(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)obj;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000086E8 File Offset: 0x000076E8
		public void CalculateDelimiterOfDeploymentsBasedOnQuota(uint dwFlags, uint cDeployments, IDefinitionAppId[] rgpIDefinitionAppId_Deployments, ref StoreApplicationReference InstallerReference, ulong ulonglongQuota, ref uint Delimiter, ref ulong SizeSharedWithExternalDeployment, ref ulong SizeConsumedByInputDeploymentArray)
		{
			IntPtr zero = IntPtr.Zero;
			this._pStore.CalculateDelimiterOfDeploymentsBasedOnQuota(dwFlags, new IntPtr((long)((ulong)cDeployments)), rgpIDefinitionAppId_Deployments, ref InstallerReference, ulonglongQuota, ref zero, ref SizeSharedWithExternalDeployment, ref SizeConsumedByInputDeploymentArray);
			Delimiter = (uint)zero.ToInt64();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00008724 File Offset: 0x00007724
		public ICMS BindReferenceToAssemblyManifest(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_ICMS);
			return (ICMS)obj;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00008750 File Offset: 0x00007750
		public ICMS GetAssemblyManifest(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_ICMS);
			return (ICMS)assemblyInformation;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000877C File Offset: 0x0000777C
		public IDefinitionIdentity GetAssemblyIdentity(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)assemblyInformation;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x000087A5 File Offset: 0x000077A5
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags)
		{
			return this.EnumAssemblies(Flags, null);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000087B0 File Offset: 0x000077B0
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags, IReferenceIdentity refToMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));
			object obj = this._pStore.EnumAssemblies((uint)Flags, refToMatch, ref guidOfType);
			return new StoreAssemblyEnumeration((IEnumSTORE_ASSEMBLY)obj);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000087E8 File Offset: 0x000077E8
		public StoreAssemblyFileEnumeration EnumFiles(Store.EnumAssemblyFilesFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumFiles((uint)Flags, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00008820 File Offset: 0x00007820
		public StoreAssemblyFileEnumeration EnumPrivateFiles(Store.EnumApplicationPrivateFiles Flags, IDefinitionAppId Application, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumPrivateFiles((uint)Flags, Application, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000885C File Offset: 0x0000785C
		public IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE EnumInstallationReferences(Store.EnumAssemblyInstallReferenceFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE));
			object obj = this._pStore.EnumInstallationReferences((uint)Flags, Assembly, ref guidOfType);
			return (IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE)obj;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00008890 File Offset: 0x00007890
		public Store.IPathLock LockAssemblyPath(IDefinitionIdentity asm)
		{
			IntPtr intPtr;
			string text = this._pStore.LockAssemblyPath(0U, asm, out intPtr);
			return new Store.AssemblyPathLock(this._pStore, intPtr, text);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000088BC File Offset: 0x000078BC
		public Store.IPathLock LockApplicationPath(IDefinitionAppId app)
		{
			IntPtr intPtr;
			string text = this._pStore.LockApplicationPath(0U, app, out intPtr);
			return new Store.ApplicationPathLock(this._pStore, intPtr, text);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000088E8 File Offset: 0x000078E8
		public ulong QueryChangeID(IDefinitionIdentity asm)
		{
			return this._pStore.QueryChangeID(asm);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00008904 File Offset: 0x00007904
		public StoreCategoryEnumeration EnumCategories(Store.EnumCategoriesFlags Flags, IReferenceIdentity CategoryMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));
			object obj = this._pStore.EnumCategories((uint)Flags, CategoryMatch, ref guidOfType);
			return new StoreCategoryEnumeration((IEnumSTORE_CATEGORY)obj);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000893C File Offset: 0x0000793C
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity CategoryMatch)
		{
			return this.EnumSubcategories(Flags, CategoryMatch, null);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00008948 File Offset: 0x00007948
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity Category, string SearchPattern)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_SUBCATEGORY));
			object obj = this._pStore.EnumSubcategories((uint)Flags, Category, SearchPattern, ref guidOfType);
			return new StoreSubcategoryEnumeration((IEnumSTORE_CATEGORY_SUBCATEGORY)obj);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00008984 File Offset: 0x00007984
		public StoreCategoryInstanceEnumeration EnumCategoryInstances(Store.EnumCategoryInstancesFlags Flags, IDefinitionIdentity Category, string SubCat)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));
			object obj = this._pStore.EnumCategoryInstances((uint)Flags, Category, SubCat, ref guidOfType);
			return new StoreCategoryInstanceEnumeration((IEnumSTORE_CATEGORY_INSTANCE)obj);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000089C0 File Offset: 0x000079C0
		public byte[] GetDeploymentProperty(Store.GetPackagePropertyFlags Flags, IDefinitionAppId Deployment, StoreApplicationReference Reference, Guid PropertySet, string PropertyName)
		{
			BLOB blob = default(BLOB);
			byte[] array = null;
			try
			{
				this._pStore.GetDeploymentProperty((uint)Flags, Deployment, ref Reference, ref PropertySet, PropertyName, out blob);
				array = new byte[blob.Size];
				Marshal.Copy(blob.BlobData, array, 0, (int)blob.Size);
			}
			finally
			{
				blob.Dispose();
			}
			return array;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00008A2C File Offset: 0x00007A2C
		public StoreDeploymentMetadataEnumeration EnumInstallerDeployments(Guid InstallerId, string InstallerName, string InstallerMetadata, IReferenceAppId DeploymentFilter)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadata(0U, ref storeApplicationReference, DeploymentFilter, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA);
			return new StoreDeploymentMetadataEnumeration((IEnumSTORE_DEPLOYMENT_METADATA)obj);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00008A6C File Offset: 0x00007A6C
		public StoreDeploymentMetadataPropertyEnumeration EnumInstallerDeploymentProperties(Guid InstallerId, string InstallerName, string InstallerMetadata, IDefinitionAppId Deployment)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadataProperties(0U, ref storeApplicationReference, Deployment, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY);
			return new StoreDeploymentMetadataPropertyEnumeration((IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY)obj);
		}

		// Token: 0x04000E24 RID: 3620
		private IStore _pStore;

		// Token: 0x02000117 RID: 279
		[Flags]
		public enum EnumAssembliesFlags
		{
			// Token: 0x04000E26 RID: 3622
			Nothing = 0,
			// Token: 0x04000E27 RID: 3623
			VisibleOnly = 1,
			// Token: 0x04000E28 RID: 3624
			MatchServicing = 2,
			// Token: 0x04000E29 RID: 3625
			ForceLibrarySemantics = 4
		}

		// Token: 0x02000118 RID: 280
		[Flags]
		public enum EnumAssemblyFilesFlags
		{
			// Token: 0x04000E2B RID: 3627
			Nothing = 0,
			// Token: 0x04000E2C RID: 3628
			IncludeInstalled = 1,
			// Token: 0x04000E2D RID: 3629
			IncludeMissing = 2
		}

		// Token: 0x02000119 RID: 281
		[Flags]
		public enum EnumApplicationPrivateFiles
		{
			// Token: 0x04000E2F RID: 3631
			Nothing = 0,
			// Token: 0x04000E30 RID: 3632
			IncludeInstalled = 1,
			// Token: 0x04000E31 RID: 3633
			IncludeMissing = 2
		}

		// Token: 0x0200011A RID: 282
		[Flags]
		public enum EnumAssemblyInstallReferenceFlags
		{
			// Token: 0x04000E33 RID: 3635
			Nothing = 0
		}

		// Token: 0x0200011B RID: 283
		public interface IPathLock : IDisposable
		{
			// Token: 0x1700014F RID: 335
			// (get) Token: 0x06000409 RID: 1033
			string Path { get; }
		}

		// Token: 0x0200011C RID: 284
		private class AssemblyPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x0600040A RID: 1034 RVA: 0x00008AAB File Offset: 0x00007AAB
			public AssemblyPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x0600040B RID: 1035 RVA: 0x00008AD3 File Offset: 0x00007AD3
			private void Dispose(bool fDisposing)
			{
				if (fDisposing)
				{
					GC.SuppressFinalize(this);
				}
				if (this._pLockCookie != IntPtr.Zero)
				{
					this._pSourceStore.ReleaseAssemblyPath(this._pLockCookie);
					this._pLockCookie = IntPtr.Zero;
				}
			}

			// Token: 0x0600040C RID: 1036 RVA: 0x00008B0C File Offset: 0x00007B0C
			~AssemblyPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x0600040D RID: 1037 RVA: 0x00008B3C File Offset: 0x00007B3C
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x17000150 RID: 336
			// (get) Token: 0x0600040E RID: 1038 RVA: 0x00008B45 File Offset: 0x00007B45
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x04000E34 RID: 3636
			private IStore _pSourceStore;

			// Token: 0x04000E35 RID: 3637
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x04000E36 RID: 3638
			private string _path;
		}

		// Token: 0x0200011D RID: 285
		private class ApplicationPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x0600040F RID: 1039 RVA: 0x00008B4D File Offset: 0x00007B4D
			public ApplicationPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x06000410 RID: 1040 RVA: 0x00008B75 File Offset: 0x00007B75
			private void Dispose(bool fDisposing)
			{
				if (fDisposing)
				{
					GC.SuppressFinalize(this);
				}
				if (this._pLockCookie != IntPtr.Zero)
				{
					this._pSourceStore.ReleaseApplicationPath(this._pLockCookie);
					this._pLockCookie = IntPtr.Zero;
				}
			}

			// Token: 0x06000411 RID: 1041 RVA: 0x00008BB0 File Offset: 0x00007BB0
			~ApplicationPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x06000412 RID: 1042 RVA: 0x00008BE0 File Offset: 0x00007BE0
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x17000151 RID: 337
			// (get) Token: 0x06000413 RID: 1043 RVA: 0x00008BE9 File Offset: 0x00007BE9
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x04000E37 RID: 3639
			private IStore _pSourceStore;

			// Token: 0x04000E38 RID: 3640
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x04000E39 RID: 3641
			private string _path;
		}

		// Token: 0x0200011E RID: 286
		[Flags]
		public enum EnumCategoriesFlags
		{
			// Token: 0x04000E3B RID: 3643
			Nothing = 0
		}

		// Token: 0x0200011F RID: 287
		[Flags]
		public enum EnumSubcategoriesFlags
		{
			// Token: 0x04000E3D RID: 3645
			Nothing = 0
		}

		// Token: 0x02000120 RID: 288
		[Flags]
		public enum EnumCategoryInstancesFlags
		{
			// Token: 0x04000E3F RID: 3647
			Nothing = 0
		}

		// Token: 0x02000121 RID: 289
		[Flags]
		public enum GetPackagePropertyFlags
		{
			// Token: 0x04000E41 RID: 3649
			Nothing = 0
		}
	}
}
