using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000A3 RID: 163
	internal class MethodConfigCallback : IConfigCallback
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0000CD70 File Offset: 0x0000BD70
		public MethodConfigCallback(ICatalogCollection coll, Type t, Type impl, Hashtable cache, RegistrationDriver driver)
		{
			this._type = t;
			this._impl = impl;
			this._coll = coll;
			this._cache = cache;
			this._driver = driver;
			try
			{
				this._map = this._impl.GetInterfaceMap(this._type);
			}
			catch (ArgumentException)
			{
				this._map.InterfaceMethods = null;
				this._map.InterfaceType = null;
				this._map.TargetMethods = null;
				this._map.TargetType = null;
			}
			RegistrationDriver.Populate(coll);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000CE0C File Offset: 0x0000BE0C
		public object FindObject(ICatalogCollection coll, object key)
		{
			ICatalogObject catalogObject = (ICatalogObject)key;
			int num = (int)catalogObject.GetValue("Index");
			ComMemberType comMemberType = ComMemberType.Method;
			MemberInfo memberInfo = Marshal.GetMethodInfoForComSlot(this._type, num, ref comMemberType);
			if (memberInfo is PropertyInfo)
			{
				if (comMemberType == ComMemberType.PropSet)
				{
					memberInfo = ((PropertyInfo)memberInfo).GetSetMethod();
				}
				else if (comMemberType == ComMemberType.PropGet)
				{
					memberInfo = ((PropertyInfo)memberInfo).GetGetMethod();
				}
			}
			if (this._map.InterfaceMethods != null)
			{
				for (int i = 0; i < this._map.InterfaceMethods.Length; i++)
				{
					if (this._map.InterfaceMethods[i] == memberInfo)
					{
						return this._map.TargetMethods[i];
					}
				}
			}
			return memberInfo;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000CEB8 File Offset: 0x0000BEB8
		public void ConfigureDefaults(object a, object key)
		{
			if (!Platform.IsLessThan(Platform.W2K))
			{
				ICatalogObject catalogObject = (ICatalogObject)key;
				catalogObject.SetValue("AutoComplete", false);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000CEE9 File Offset: 0x0000BEE9
		public bool Configure(object a, object key)
		{
			return a != null && this._driver.ConfigureObject((MethodInfo)a, (ICatalogObject)key, this._coll, "Method", this._cache);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000CF18 File Offset: 0x0000BF18
		public bool AfterSaveChanges(object a, object key)
		{
			return a != null && this._driver.AfterSaveChanges((MethodInfo)a, (ICatalogObject)key, this._coll, "Method", this._cache);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000CF48 File Offset: 0x0000BF48
		public IEnumerator GetEnumerator()
		{
			IEnumerator enumerator = null;
			this._coll.GetEnumerator(out enumerator);
			return enumerator;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000CF65 File Offset: 0x0000BF65
		public void ConfigureSubCollections(ICatalogCollection coll)
		{
		}

		// Token: 0x040001C1 RID: 449
		private Type _type;

		// Token: 0x040001C2 RID: 450
		private Type _impl;

		// Token: 0x040001C3 RID: 451
		private ICatalogCollection _coll;

		// Token: 0x040001C4 RID: 452
		private Hashtable _cache;

		// Token: 0x040001C5 RID: 453
		private RegistrationDriver _driver;

		// Token: 0x040001C6 RID: 454
		private InterfaceMapping _map;
	}
}
