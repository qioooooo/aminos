using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000141 RID: 321
	internal class Store
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001FCD8 File Offset: 0x0001ECD8
		public IStore InternalStore
		{
			get
			{
				return this._pStore;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001FCE0 File Offset: 0x0001ECE0
		public Store(IStore pStore)
		{
			if (pStore == null)
			{
				throw new ArgumentNullException("pStore");
			}
			this._pStore = pStore;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001FD00 File Offset: 0x0001ED00
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

		// Token: 0x060006DE RID: 1758 RVA: 0x0001FD49 File Offset: 0x0001ED49
		public void Transact(StoreTransactionOperation[] operations, uint[] rgDispositions, int[] rgResults)
		{
			if (operations == null || operations.Length == 0)
			{
				throw new ArgumentException("operations");
			}
			this._pStore.Transact(new IntPtr(operations.Length), operations, rgDispositions, rgResults);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001FD74 File Offset: 0x0001ED74
		public IDefinitionIdentity BindReferenceToAssemblyIdentity(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)obj;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001FDA0 File Offset: 0x0001EDA0
		public void CalculateDelimiterOfDeploymentsBasedOnQuota(uint dwFlags, uint cDeployments, IDefinitionAppId[] rgpIDefinitionAppId_Deployments, ref StoreApplicationReference InstallerReference, ulong ulonglongQuota, ref uint Delimiter, ref ulong SizeSharedWithExternalDeployment, ref ulong SizeConsumedByInputDeploymentArray)
		{
			IntPtr zero = IntPtr.Zero;
			this._pStore.CalculateDelimiterOfDeploymentsBasedOnQuota(dwFlags, new IntPtr((long)((ulong)cDeployments)), rgpIDefinitionAppId_Deployments, ref InstallerReference, ulonglongQuota, ref zero, ref SizeSharedWithExternalDeployment, ref SizeConsumedByInputDeploymentArray);
			Delimiter = (uint)zero.ToInt64();
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001FDDC File Offset: 0x0001EDDC
		public ICMS BindReferenceToAssemblyManifest(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_ICMS);
			return (ICMS)obj;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001FE08 File Offset: 0x0001EE08
		public ICMS GetAssemblyManifest(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_ICMS);
			return (ICMS)assemblyInformation;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001FE34 File Offset: 0x0001EE34
		public IDefinitionIdentity GetAssemblyIdentity(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)assemblyInformation;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001FE5D File Offset: 0x0001EE5D
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags)
		{
			return this.EnumAssemblies(Flags, null);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001FE68 File Offset: 0x0001EE68
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags, IReferenceIdentity refToMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));
			object obj = this._pStore.EnumAssemblies((uint)Flags, refToMatch, ref guidOfType);
			return new StoreAssemblyEnumeration((IEnumSTORE_ASSEMBLY)obj);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001FEA0 File Offset: 0x0001EEA0
		public StoreAssemblyFileEnumeration EnumFiles(Store.EnumAssemblyFilesFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumFiles((uint)Flags, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001FED8 File Offset: 0x0001EED8
		public StoreAssemblyFileEnumeration EnumPrivateFiles(Store.EnumApplicationPrivateFiles Flags, IDefinitionAppId Application, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumPrivateFiles((uint)Flags, Application, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001FF14 File Offset: 0x0001EF14
		public IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE EnumInstallationReferences(Store.EnumAssemblyInstallReferenceFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE));
			object obj = this._pStore.EnumInstallationReferences((uint)Flags, Assembly, ref guidOfType);
			return (IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE)obj;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001FF48 File Offset: 0x0001EF48
		public Store.IPathLock LockAssemblyPath(IDefinitionIdentity asm)
		{
			IntPtr intPtr;
			string text = this._pStore.LockAssemblyPath(0U, asm, out intPtr);
			return new Store.AssemblyPathLock(this._pStore, intPtr, text);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001FF74 File Offset: 0x0001EF74
		public Store.IPathLock LockApplicationPath(IDefinitionAppId app)
		{
			IntPtr intPtr;
			string text = this._pStore.LockApplicationPath(0U, app, out intPtr);
			return new Store.ApplicationPathLock(this._pStore, intPtr, text);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001FFA0 File Offset: 0x0001EFA0
		public ulong QueryChangeID(IDefinitionIdentity asm)
		{
			return this._pStore.QueryChangeID(asm);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001FFBC File Offset: 0x0001EFBC
		public StoreCategoryEnumeration EnumCategories(Store.EnumCategoriesFlags Flags, IReferenceIdentity CategoryMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));
			object obj = this._pStore.EnumCategories((uint)Flags, CategoryMatch, ref guidOfType);
			return new StoreCategoryEnumeration((IEnumSTORE_CATEGORY)obj);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001FFF4 File Offset: 0x0001EFF4
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity CategoryMatch)
		{
			return this.EnumSubcategories(Flags, CategoryMatch, null);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00020000 File Offset: 0x0001F000
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity Category, string SearchPattern)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_SUBCATEGORY));
			object obj = this._pStore.EnumSubcategories((uint)Flags, Category, SearchPattern, ref guidOfType);
			return new StoreSubcategoryEnumeration((IEnumSTORE_CATEGORY_SUBCATEGORY)obj);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002003C File Offset: 0x0001F03C
		public StoreCategoryInstanceEnumeration EnumCategoryInstances(Store.EnumCategoryInstancesFlags Flags, IDefinitionIdentity Category, string SubCat)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));
			object obj = this._pStore.EnumCategoryInstances((uint)Flags, Category, SubCat, ref guidOfType);
			return new StoreCategoryInstanceEnumeration((IEnumSTORE_CATEGORY_INSTANCE)obj);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00020078 File Offset: 0x0001F078
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

		// Token: 0x060006F1 RID: 1777 RVA: 0x000200E4 File Offset: 0x0001F0E4
		public StoreDeploymentMetadataEnumeration EnumInstallerDeployments(Guid InstallerId, string InstallerName, string InstallerMetadata, IReferenceAppId DeploymentFilter)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadata(0U, ref storeApplicationReference, DeploymentFilter, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA);
			return new StoreDeploymentMetadataEnumeration((IEnumSTORE_DEPLOYMENT_METADATA)obj);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00020124 File Offset: 0x0001F124
		public StoreDeploymentMetadataPropertyEnumeration EnumInstallerDeploymentProperties(Guid InstallerId, string InstallerName, string InstallerMetadata, IDefinitionAppId Deployment)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadataProperties(0U, ref storeApplicationReference, Deployment, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY);
			return new StoreDeploymentMetadataPropertyEnumeration((IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY)obj);
		}

		// Token: 0x04000598 RID: 1432
		private IStore _pStore;

		// Token: 0x02000142 RID: 322
		[Flags]
		public enum EnumAssembliesFlags
		{
			// Token: 0x0400059A RID: 1434
			Nothing = 0,
			// Token: 0x0400059B RID: 1435
			VisibleOnly = 1,
			// Token: 0x0400059C RID: 1436
			MatchServicing = 2,
			// Token: 0x0400059D RID: 1437
			ForceLibrarySemantics = 4
		}

		// Token: 0x02000143 RID: 323
		[Flags]
		public enum EnumAssemblyFilesFlags
		{
			// Token: 0x0400059F RID: 1439
			Nothing = 0,
			// Token: 0x040005A0 RID: 1440
			IncludeInstalled = 1,
			// Token: 0x040005A1 RID: 1441
			IncludeMissing = 2
		}

		// Token: 0x02000144 RID: 324
		[Flags]
		public enum EnumApplicationPrivateFiles
		{
			// Token: 0x040005A3 RID: 1443
			Nothing = 0,
			// Token: 0x040005A4 RID: 1444
			IncludeInstalled = 1,
			// Token: 0x040005A5 RID: 1445
			IncludeMissing = 2
		}

		// Token: 0x02000145 RID: 325
		[Flags]
		public enum EnumAssemblyInstallReferenceFlags
		{
			// Token: 0x040005A7 RID: 1447
			Nothing = 0
		}

		// Token: 0x02000146 RID: 326
		public interface IPathLock : IDisposable
		{
			// Token: 0x17000165 RID: 357
			// (get) Token: 0x060006F3 RID: 1779
			string Path { get; }
		}

		// Token: 0x02000147 RID: 327
		private class AssemblyPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x060006F4 RID: 1780 RVA: 0x00020163 File Offset: 0x0001F163
			public AssemblyPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x060006F5 RID: 1781 RVA: 0x0002018B File Offset: 0x0001F18B
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

			// Token: 0x060006F6 RID: 1782 RVA: 0x000201C4 File Offset: 0x0001F1C4
			~AssemblyPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x000201F4 File Offset: 0x0001F1F4
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x17000166 RID: 358
			// (get) Token: 0x060006F8 RID: 1784 RVA: 0x000201FD File Offset: 0x0001F1FD
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x040005A8 RID: 1448
			private IStore _pSourceStore;

			// Token: 0x040005A9 RID: 1449
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x040005AA RID: 1450
			private string _path;
		}

		// Token: 0x02000148 RID: 328
		private class ApplicationPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x060006F9 RID: 1785 RVA: 0x00020205 File Offset: 0x0001F205
			public ApplicationPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x060006FA RID: 1786 RVA: 0x0002022D File Offset: 0x0001F22D
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

			// Token: 0x060006FB RID: 1787 RVA: 0x00020268 File Offset: 0x0001F268
			~ApplicationPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x060006FC RID: 1788 RVA: 0x00020298 File Offset: 0x0001F298
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x17000167 RID: 359
			// (get) Token: 0x060006FD RID: 1789 RVA: 0x000202A1 File Offset: 0x0001F2A1
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x040005AB RID: 1451
			private IStore _pSourceStore;

			// Token: 0x040005AC RID: 1452
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x040005AD RID: 1453
			private string _path;
		}

		// Token: 0x02000149 RID: 329
		[Flags]
		public enum EnumCategoriesFlags
		{
			// Token: 0x040005AF RID: 1455
			Nothing = 0
		}

		// Token: 0x0200014A RID: 330
		[Flags]
		public enum EnumSubcategoriesFlags
		{
			// Token: 0x040005B1 RID: 1457
			Nothing = 0
		}

		// Token: 0x0200014B RID: 331
		[Flags]
		public enum EnumCategoryInstancesFlags
		{
			// Token: 0x040005B3 RID: 1459
			Nothing = 0
		}

		// Token: 0x0200014C RID: 332
		[Flags]
		public enum GetPackagePropertyFlags
		{
			// Token: 0x040005B5 RID: 1461
			Nothing = 0
		}
	}
}
