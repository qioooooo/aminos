using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000A1 RID: 161
	internal class ComponentConfigCallback : IConfigCallback
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x0000C4F5 File Offset: 0x0000B4F5
		public ComponentConfigCallback(ICatalogCollection coll, ApplicationSpec spec, Hashtable cache, RegistrationDriver driver, InstallationFlags installFlags)
		{
			this._spec = spec;
			this._coll = coll;
			this._cache = cache;
			this._driver = driver;
			this._installFlags = installFlags;
			RegistrationDriver.Populate(coll);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000C528 File Offset: 0x0000B528
		public object FindObject(ICatalogCollection coll, object key)
		{
			Guid guid = Marshal.GenerateGuidForType((Type)key);
			for (int i = 0; i < coll.Count(); i++)
			{
				ICatalogObject catalogObject = (ICatalogObject)coll.Item(i);
				Guid guid2 = new Guid((string)catalogObject.Key());
				if (guid2 == guid)
				{
					return catalogObject;
				}
			}
			throw new RegistrationException(Resource.FormatString("Reg_ComponentMissing", ((Type)key).FullName));
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000C598 File Offset: 0x0000B598
		public void ConfigureDefaults(object a, object key)
		{
			ICatalogObject catalogObject = (ICatalogObject)a;
			if (Platform.IsLessThan(Platform.W2K))
			{
				catalogObject.SetValue("Transaction", "Not Supported");
				catalogObject.SetValue("SecurityEnabled", "N");
			}
			else
			{
				catalogObject.SetValue("AllowInprocSubscribers", true);
				catalogObject.SetValue("ComponentAccessChecksEnabled", false);
				catalogObject.SetValue("COMTIIntrinsics", false);
				catalogObject.SetValue("ConstructionEnabled", false);
				catalogObject.SetValue("EventTrackingEnabled", false);
				catalogObject.SetValue("FireInParallel", false);
				catalogObject.SetValue("IISIntrinsics", false);
				catalogObject.SetValue("JustInTimeActivation", false);
				catalogObject.SetValue("LoadBalancingSupported", false);
				catalogObject.SetValue("MustRunInClientContext", false);
				catalogObject.SetValue("ObjectPoolingEnabled", false);
				catalogObject.SetValue("Synchronization", SynchronizationOption.Disabled);
				catalogObject.SetValue("Transaction", TransactionOption.Disabled);
				catalogObject.SetValue("ComponentTransactionTimeoutEnabled", false);
			}
			if (!Platform.IsLessThan(Platform.Whistler))
			{
				catalogObject.SetValue("TxIsolationLevel", TransactionIsolationLevel.Serializable);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000C6E8 File Offset: 0x0000B6E8
		public bool Configure(object a, object key)
		{
			return this._driver.ConfigureObject((Type)key, (ICatalogObject)a, this._coll, "Component", this._cache);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000C712 File Offset: 0x0000B712
		public bool AfterSaveChanges(object a, object key)
		{
			return this._driver.AfterSaveChanges((Type)key, (ICatalogObject)a, this._coll, "Component", this._cache);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000C73C File Offset: 0x0000B73C
		public IEnumerator GetEnumerator()
		{
			return this._spec.ConfigurableTypes.GetEnumerator();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000C750 File Offset: 0x0000B750
		public void ConfigureSubCollections(ICatalogCollection coll)
		{
			if ((this._installFlags & InstallationFlags.ConfigureComponentsOnly) == InstallationFlags.Default)
			{
				foreach (Type type in this._spec.ConfigurableTypes)
				{
					ICatalogObject catalogObject = (ICatalogObject)this.FindObject(coll, type);
					ICatalogCollection catalogCollection = (ICatalogCollection)coll.GetCollection(CollectionName.Interfaces, catalogObject.Key());
					this._cache["Component"] = catalogObject;
					this._cache["ComponentType"] = type;
					InterfaceConfigCallback interfaceConfigCallback = new InterfaceConfigCallback(catalogCollection, type, this._cache, this._driver);
					this._driver.ConfigureCollection(catalogCollection, interfaceConfigCallback);
					if (this._cache["SecurityOnMethods"] != null || ServicedComponentInfo.AreMethodsSecure(type))
					{
						this.FixupMethodSecurity(catalogCollection);
						this._cache["SecurityOnMethods"] = null;
					}
				}
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000C831 File Offset: 0x0000B831
		private void FixupMethodSecurity(ICatalogCollection coll)
		{
			this.FixupMethodSecurityForInterface(coll, typeof(IManagedObject));
			this.FixupMethodSecurityForInterface(coll, typeof(IServicedComponentInfo));
			this.FixupMethodSecurityForInterface(coll, typeof(IDisposable));
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000C868 File Offset: 0x0000B868
		private void FixupMethodSecurityForInterface(ICatalogCollection coll, Type InterfaceType)
		{
			ICatalogObject catalogObject = null;
			Guid guid = Marshal.GenerateGuidForType(InterfaceType);
			int num = coll.Count();
			for (int i = 0; i < num; i++)
			{
				ICatalogObject catalogObject2 = (ICatalogObject)coll.Item(i);
				if (new Guid((string)catalogObject2.Key()) == guid)
				{
					catalogObject = catalogObject2;
					break;
				}
			}
			if (catalogObject != null)
			{
				IConfigurationAttribute configurationAttribute = new SecurityRoleAttribute("Marshaler", false)
				{
					Description = Resource.FormatString("Reg_MarshalerDesc")
				};
				this._cache["CurrentTarget"] = "Interface";
				this._cache["InterfaceCollection"] = coll;
				this._cache["Interface"] = catalogObject;
				this._cache["InterfaceType"] = InterfaceType;
				if (configurationAttribute.Apply(this._cache))
				{
					coll.SaveChanges();
				}
				if (configurationAttribute.AfterSaveChanges(this._cache))
				{
					coll.SaveChanges();
				}
			}
		}

		// Token: 0x040001B6 RID: 438
		private ApplicationSpec _spec;

		// Token: 0x040001B7 RID: 439
		private ICatalogCollection _coll;

		// Token: 0x040001B8 RID: 440
		private Hashtable _cache;

		// Token: 0x040001B9 RID: 441
		private RegistrationDriver _driver;

		// Token: 0x040001BA RID: 442
		private InstallationFlags _installFlags;
	}
}
