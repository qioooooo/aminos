using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200021B RID: 539
	internal class Store
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x000373EC File Offset: 0x000363EC
		public IStore InternalStore
		{
			get
			{
				return this._pStore;
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000373F4 File Offset: 0x000363F4
		public Store(IStore pStore)
		{
			if (pStore == null)
			{
				throw new ArgumentNullException("pStore");
			}
			this._pStore = pStore;
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00037414 File Offset: 0x00036414
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

		// Token: 0x06001576 RID: 5494 RVA: 0x00037460 File Offset: 0x00036460
		public IDefinitionIdentity BindReferenceToAssemblyIdentity(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)obj;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0003748C File Offset: 0x0003648C
		public void CalculateDelimiterOfDeploymentsBasedOnQuota(uint dwFlags, uint cDeployments, IDefinitionAppId[] rgpIDefinitionAppId_Deployments, ref StoreApplicationReference InstallerReference, ulong ulonglongQuota, ref uint Delimiter, ref ulong SizeSharedWithExternalDeployment, ref ulong SizeConsumedByInputDeploymentArray)
		{
			IntPtr zero = IntPtr.Zero;
			this._pStore.CalculateDelimiterOfDeploymentsBasedOnQuota(dwFlags, new IntPtr((long)((ulong)cDeployments)), rgpIDefinitionAppId_Deployments, ref InstallerReference, ulonglongQuota, ref zero, ref SizeSharedWithExternalDeployment, ref SizeConsumedByInputDeploymentArray);
			Delimiter = (uint)zero.ToInt64();
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x000374C8 File Offset: 0x000364C8
		public ICMS BindReferenceToAssemblyManifest(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_ICMS);
			return (ICMS)obj;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000374F4 File Offset: 0x000364F4
		public ICMS GetAssemblyManifest(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_ICMS);
			return (ICMS)assemblyInformation;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00037520 File Offset: 0x00036520
		public IDefinitionIdentity GetAssemblyIdentity(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)assemblyInformation;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00037549 File Offset: 0x00036549
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags)
		{
			return this.EnumAssemblies(Flags, null);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00037554 File Offset: 0x00036554
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags, IReferenceIdentity refToMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));
			object obj = this._pStore.EnumAssemblies((uint)Flags, refToMatch, ref guidOfType);
			return new StoreAssemblyEnumeration((IEnumSTORE_ASSEMBLY)obj);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0003758C File Offset: 0x0003658C
		public StoreAssemblyFileEnumeration EnumFiles(Store.EnumAssemblyFilesFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumFiles((uint)Flags, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x000375C4 File Offset: 0x000365C4
		public StoreAssemblyFileEnumeration EnumPrivateFiles(Store.EnumApplicationPrivateFiles Flags, IDefinitionAppId Application, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumPrivateFiles((uint)Flags, Application, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00037600 File Offset: 0x00036600
		public IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE EnumInstallationReferences(Store.EnumAssemblyInstallReferenceFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE));
			object obj = this._pStore.EnumInstallationReferences((uint)Flags, Assembly, ref guidOfType);
			return (IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE)obj;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00037634 File Offset: 0x00036634
		public Store.IPathLock LockAssemblyPath(IDefinitionIdentity asm)
		{
			IntPtr intPtr;
			string text = this._pStore.LockAssemblyPath(0U, asm, out intPtr);
			return new Store.AssemblyPathLock(this._pStore, intPtr, text);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00037660 File Offset: 0x00036660
		public Store.IPathLock LockApplicationPath(IDefinitionAppId app)
		{
			IntPtr intPtr;
			string text = this._pStore.LockApplicationPath(0U, app, out intPtr);
			return new Store.ApplicationPathLock(this._pStore, intPtr, text);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0003768C File Offset: 0x0003668C
		public ulong QueryChangeID(IDefinitionIdentity asm)
		{
			return this._pStore.QueryChangeID(asm);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x000376A8 File Offset: 0x000366A8
		public StoreCategoryEnumeration EnumCategories(Store.EnumCategoriesFlags Flags, IReferenceIdentity CategoryMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));
			object obj = this._pStore.EnumCategories((uint)Flags, CategoryMatch, ref guidOfType);
			return new StoreCategoryEnumeration((IEnumSTORE_CATEGORY)obj);
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x000376E0 File Offset: 0x000366E0
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity CategoryMatch)
		{
			return this.EnumSubcategories(Flags, CategoryMatch, null);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x000376EC File Offset: 0x000366EC
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity Category, string SearchPattern)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_SUBCATEGORY));
			object obj = this._pStore.EnumSubcategories((uint)Flags, Category, SearchPattern, ref guidOfType);
			return new StoreSubcategoryEnumeration((IEnumSTORE_CATEGORY_SUBCATEGORY)obj);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00037728 File Offset: 0x00036728
		public StoreCategoryInstanceEnumeration EnumCategoryInstances(Store.EnumCategoryInstancesFlags Flags, IDefinitionIdentity Category, string SubCat)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));
			object obj = this._pStore.EnumCategoryInstances((uint)Flags, Category, SubCat, ref guidOfType);
			return new StoreCategoryInstanceEnumeration((IEnumSTORE_CATEGORY_INSTANCE)obj);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00037764 File Offset: 0x00036764
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

		// Token: 0x06001588 RID: 5512 RVA: 0x000377D0 File Offset: 0x000367D0
		public StoreDeploymentMetadataEnumeration EnumInstallerDeployments(Guid InstallerId, string InstallerName, string InstallerMetadata, IReferenceAppId DeploymentFilter)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadata(0U, ref storeApplicationReference, DeploymentFilter, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA);
			return new StoreDeploymentMetadataEnumeration((IEnumSTORE_DEPLOYMENT_METADATA)obj);
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00037810 File Offset: 0x00036810
		public StoreDeploymentMetadataPropertyEnumeration EnumInstallerDeploymentProperties(Guid InstallerId, string InstallerName, string InstallerMetadata, IDefinitionAppId Deployment)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadataProperties(0U, ref storeApplicationReference, Deployment, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY);
			return new StoreDeploymentMetadataPropertyEnumeration((IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY)obj);
		}

		// Token: 0x040008B0 RID: 2224
		private IStore _pStore;

		// Token: 0x0200021C RID: 540
		[Flags]
		public enum EnumAssembliesFlags
		{
			// Token: 0x040008B2 RID: 2226
			Nothing = 0,
			// Token: 0x040008B3 RID: 2227
			VisibleOnly = 1,
			// Token: 0x040008B4 RID: 2228
			MatchServicing = 2,
			// Token: 0x040008B5 RID: 2229
			ForceLibrarySemantics = 4
		}

		// Token: 0x0200021D RID: 541
		[Flags]
		public enum EnumAssemblyFilesFlags
		{
			// Token: 0x040008B7 RID: 2231
			Nothing = 0,
			// Token: 0x040008B8 RID: 2232
			IncludeInstalled = 1,
			// Token: 0x040008B9 RID: 2233
			IncludeMissing = 2
		}

		// Token: 0x0200021E RID: 542
		[Flags]
		public enum EnumApplicationPrivateFiles
		{
			// Token: 0x040008BB RID: 2235
			Nothing = 0,
			// Token: 0x040008BC RID: 2236
			IncludeInstalled = 1,
			// Token: 0x040008BD RID: 2237
			IncludeMissing = 2
		}

		// Token: 0x0200021F RID: 543
		[Flags]
		public enum EnumAssemblyInstallReferenceFlags
		{
			// Token: 0x040008BF RID: 2239
			Nothing = 0
		}

		// Token: 0x02000220 RID: 544
		public interface IPathLock : IDisposable
		{
			// Token: 0x170002EB RID: 747
			// (get) Token: 0x0600158A RID: 5514
			string Path { get; }
		}

		// Token: 0x02000221 RID: 545
		private class AssemblyPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x0600158B RID: 5515 RVA: 0x0003784F File Offset: 0x0003684F
			public AssemblyPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x0600158C RID: 5516 RVA: 0x00037877 File Offset: 0x00036877
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

			// Token: 0x0600158D RID: 5517 RVA: 0x000378B0 File Offset: 0x000368B0
			~AssemblyPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x0600158E RID: 5518 RVA: 0x000378E0 File Offset: 0x000368E0
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x0600158F RID: 5519 RVA: 0x000378E9 File Offset: 0x000368E9
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x040008C0 RID: 2240
			private IStore _pSourceStore;

			// Token: 0x040008C1 RID: 2241
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x040008C2 RID: 2242
			private string _path;
		}

		// Token: 0x02000222 RID: 546
		private class ApplicationPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x06001590 RID: 5520 RVA: 0x000378F1 File Offset: 0x000368F1
			public ApplicationPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x06001591 RID: 5521 RVA: 0x00037919 File Offset: 0x00036919
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

			// Token: 0x06001592 RID: 5522 RVA: 0x00037954 File Offset: 0x00036954
			~ApplicationPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x06001593 RID: 5523 RVA: 0x00037984 File Offset: 0x00036984
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x06001594 RID: 5524 RVA: 0x0003798D File Offset: 0x0003698D
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x040008C3 RID: 2243
			private IStore _pSourceStore;

			// Token: 0x040008C4 RID: 2244
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x040008C5 RID: 2245
			private string _path;
		}

		// Token: 0x02000223 RID: 547
		[Flags]
		public enum EnumCategoriesFlags
		{
			// Token: 0x040008C7 RID: 2247
			Nothing = 0
		}

		// Token: 0x02000224 RID: 548
		[Flags]
		public enum EnumSubcategoriesFlags
		{
			// Token: 0x040008C9 RID: 2249
			Nothing = 0
		}

		// Token: 0x02000225 RID: 549
		[Flags]
		public enum EnumCategoryInstancesFlags
		{
			// Token: 0x040008CB RID: 2251
			Nothing = 0
		}

		// Token: 0x02000226 RID: 550
		[Flags]
		public enum GetPackagePropertyFlags
		{
			// Token: 0x040008CD RID: 2253
			Nothing = 0
		}
	}
}
