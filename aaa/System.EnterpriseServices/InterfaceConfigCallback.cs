using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000A2 RID: 162
	internal class InterfaceConfigCallback : IConfigCallback
	{
		// Token: 0x060003DC RID: 988 RVA: 0x0000C95C File Offset: 0x0000B95C
		private Type[] GetInteropInterfaces(Type t)
		{
			Type type = t;
			ArrayList arrayList = new ArrayList(t.GetInterfaces());
			while (type != null)
			{
				arrayList.Add(type);
				type = type.BaseType;
			}
			arrayList.Add(typeof(IManagedObject));
			Type[] array = new Type[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000C9B0 File Offset: 0x0000B9B0
		private Type FindInterfaceByID(ICatalogObject ifcObj, Type t, Type[] interfaces)
		{
			Guid guid = new Guid((string)ifcObj.GetValue("IID"));
			foreach (Type type in interfaces)
			{
				Guid guid2 = Marshal.GenerateGuidForType(type);
				if (guid2 == guid)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000CA08 File Offset: 0x0000BA08
		private Type FindInterfaceByName(ICatalogObject ifcObj, Type t, Type[] interfaces)
		{
			string text = (string)ifcObj.GetValue("Name");
			foreach (Type type in interfaces)
			{
				if (type.IsInterface)
				{
					if (type.Name == text)
					{
						return type;
					}
				}
				else if ("_" + type.Name == text)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000CA78 File Offset: 0x0000BA78
		public InterfaceConfigCallback(ICatalogCollection coll, Type t, Hashtable cache, RegistrationDriver driver)
		{
			this._type = t;
			this._coll = coll;
			this._cache = cache;
			this._driver = driver;
			this._ifcs = this.GetInteropInterfaces(this._type);
			foreach (Type type in this._ifcs)
			{
				if (Marshal.GenerateGuidForType(type) == InterfaceConfigCallback.IID_IProcessInitializer)
				{
					try
					{
						ICatalogObject catalogObject = cache["Component"] as ICatalogObject;
						ICatalogCollection catalogCollection = cache["ComponentCollection"] as ICatalogCollection;
						catalogObject.SetValue("InitializesServerApplication", 1);
						catalogCollection.SaveChanges();
					}
					catch (Exception ex)
					{
						throw new RegistrationException(Resource.FormatString("Reg_FailPIT", this._type), ex);
					}
					catch
					{
						throw new RegistrationException(Resource.FormatString("Err_NonClsException", "InterfaceConfigCallback.InterfaceConfigCallback"));
					}
				}
			}
			RegistrationDriver.Populate(this._coll);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000CB84 File Offset: 0x0000BB84
		public object FindObject(ICatalogCollection coll, object key)
		{
			ICatalogObject catalogObject = (ICatalogObject)key;
			Type type = this.FindInterfaceByID(catalogObject, this._type, this._ifcs);
			if (type == null)
			{
				type = this.FindInterfaceByName(catalogObject, this._type, this._ifcs);
			}
			return type;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000CBC8 File Offset: 0x0000BBC8
		public void ConfigureDefaults(object a, object key)
		{
			if (!Platform.IsLessThan(Platform.W2K))
			{
				bool flag = true;
				ICatalogObject catalogObject = (ICatalogObject)key;
				if (this._cache[this._type] != null)
				{
					object obj = this._cache[this._type];
					if (obj is Hashtable && ((Hashtable)obj)[a] != null)
					{
						flag = false;
					}
				}
				if (flag)
				{
					catalogObject.SetValue("QueuingEnabled", false);
				}
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000CC3B File Offset: 0x0000BC3B
		public bool Configure(object a, object key)
		{
			return a != null && this._driver.ConfigureObject((Type)a, (ICatalogObject)key, this._coll, "Interface", this._cache);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000CC6A File Offset: 0x0000BC6A
		public bool AfterSaveChanges(object a, object key)
		{
			return a != null && this._driver.AfterSaveChanges((Type)a, (ICatalogObject)key, this._coll, "Interface", this._cache);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000CC9C File Offset: 0x0000BC9C
		public IEnumerator GetEnumerator()
		{
			IEnumerator enumerator = null;
			this._coll.GetEnumerator(out enumerator);
			return enumerator;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000CCBC File Offset: 0x0000BCBC
		public void ConfigureSubCollections(ICatalogCollection coll)
		{
			foreach (object obj in this)
			{
				ICatalogObject catalogObject = (ICatalogObject)obj;
				Type type = (Type)this.FindObject(coll, catalogObject);
				if (type != null)
				{
					ICatalogCollection catalogCollection = (ICatalogCollection)coll.GetCollection(CollectionName.Methods, catalogObject.Key());
					this._driver.ConfigureCollection(catalogCollection, new MethodConfigCallback(catalogCollection, type, this._type, this._cache, this._driver));
				}
			}
		}

		// Token: 0x040001BB RID: 443
		private static readonly Guid IID_IProcessInitializer = new Guid("1113f52d-dc7f-4943-aed6-88d04027e32a");

		// Token: 0x040001BC RID: 444
		private Type _type;

		// Token: 0x040001BD RID: 445
		private ICatalogCollection _coll;

		// Token: 0x040001BE RID: 446
		private Type[] _ifcs;

		// Token: 0x040001BF RID: 447
		private Hashtable _cache;

		// Token: 0x040001C0 RID: 448
		private RegistrationDriver _driver;
	}
}
