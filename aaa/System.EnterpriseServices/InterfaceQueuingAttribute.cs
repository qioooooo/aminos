using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200007A RID: 122
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
	public sealed class InterfaceQueuingAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x000071C6 File Offset: 0x000061C6
		public InterfaceQueuingAttribute()
		{
			this._enabled = true;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000071D5 File Offset: 0x000061D5
		public InterfaceQueuingAttribute(bool enabled)
		{
			this._enabled = enabled;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002AB RID: 683 RVA: 0x000071E4 File Offset: 0x000061E4
		// (set) Token: 0x060002AC RID: 684 RVA: 0x000071EC File Offset: 0x000061EC
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000071F5 File Offset: 0x000061F5
		// (set) Token: 0x060002AE RID: 686 RVA: 0x000071FD File Offset: 0x000061FD
		public string Interface
		{
			get
			{
				return this._interface;
			}
			set
			{
				this._interface = value;
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00007206 File Offset: 0x00006206
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			if (this._interface == null)
			{
				return s == "Interface";
			}
			return s == "Component";
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00007228 File Offset: 0x00006228
		private bool ConfigureInterface(ICatalogObject obj)
		{
			bool flag = (bool)obj.GetValue("QueuingSupported");
			if (this._enabled && flag)
			{
				obj.SetValue("QueuingEnabled", this._enabled);
			}
			else
			{
				if (this._enabled)
				{
					throw new RegistrationException(Resource.FormatString("Reg_QueueingNotSupported", (string)obj.Name()));
				}
				obj.SetValue("QueuingEnabled", this._enabled);
			}
			return true;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000072A4 File Offset: 0x000062A4
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "InterfaceQueuingAttribute");
			if (this._interface == null)
			{
				ICatalogObject catalogObject = (ICatalogObject)info["Interface"];
				this.ConfigureInterface(catalogObject);
			}
			return true;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000072E4 File Offset: 0x000062E4
		internal static Type ResolveTypeRelativeTo(string typeName, Type serverType)
		{
			Type type = null;
			Type type2 = null;
			bool flag = false;
			bool flag2 = false;
			foreach (Type type in serverType.GetInterfaces())
			{
				string fullName = type.FullName;
				int num = fullName.Length - typeName.Length;
				if (num >= 0 && string.CompareOrdinal(typeName, 0, fullName, num, typeName.Length) == 0 && (num == 0 || (num > 0 && fullName[num - 1] == '.')))
				{
					if (type2 == null)
					{
						type2 = type;
						flag = num == 0;
					}
					else
					{
						if (type2 != null)
						{
							flag2 = true;
						}
						if (type2 != null && flag)
						{
							if (num == 0)
							{
								throw new AmbiguousMatchException(Resource.FormatString("Reg_IfcAmbiguousMatch", typeName, type, type2));
							}
						}
						else if (type2 != null && !flag && num == 0)
						{
							type2 = type;
							flag = true;
						}
					}
				}
			}
			if (flag2 && !flag)
			{
				throw new AmbiguousMatchException(Resource.FormatString("Reg_IfcAmbiguousMatch", typeName, type, type2));
			}
			return type2;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000073C0 File Offset: 0x000063C0
		internal static Type FindInterfaceByName(string name, Type component)
		{
			Type type = InterfaceQueuingAttribute.ResolveTypeRelativeTo(name, component);
			if (type == null)
			{
				type = Type.GetType(name, false);
			}
			return type;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000073E4 File Offset: 0x000063E4
		private void FindInterfaceByKey(string key, ICatalogCollection coll, Type comp, out ICatalogObject ifcObj, out Type ifcType)
		{
			ifcType = InterfaceQueuingAttribute.FindInterfaceByName(key, comp);
			if (ifcType == null)
			{
				throw new RegistrationException(Resource.FormatString("Reg_TypeFindError", key, comp.ToString()));
			}
			Guid guid = Marshal.GenerateGuidForType(ifcType);
			object[] array = new object[] { "{" + guid + "}" };
			coll.PopulateByKey(array);
			if (coll.Count() != 1)
			{
				throw new RegistrationException(Resource.FormatString("Reg_TypeFindError", key, comp.ToString()));
			}
			ifcObj = (ICatalogObject)coll.Item(0);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00007477 File Offset: 0x00006477
		private void StashModification(Hashtable cache, Type comp, Type ifc)
		{
			if (cache[comp] == null)
			{
				cache[comp] = new Hashtable();
			}
			((Hashtable)cache[comp])[ifc] = true;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000074A8 File Offset: 0x000064A8
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			if (this._interface != null)
			{
				ICatalogCollection catalogCollection = (ICatalogCollection)info["ComponentCollection"];
				ICatalogObject catalogObject = (ICatalogObject)info["Component"];
				Type type = (Type)info["ComponentType"];
				ICatalogCollection catalogCollection2 = (ICatalogCollection)catalogCollection.GetCollection("InterfacesForComponent", catalogObject.Key());
				ICatalogObject catalogObject2;
				Type type2;
				this.FindInterfaceByKey(this._interface, catalogCollection2, type, out catalogObject2, out type2);
				this.ConfigureInterface(catalogObject2);
				catalogCollection2.SaveChanges();
				this.StashModification(info, type, type2);
			}
			return false;
		}

		// Token: 0x0400010B RID: 267
		private bool _enabled;

		// Token: 0x0400010C RID: 268
		private string _interface;
	}
}
