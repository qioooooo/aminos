using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000093 RID: 147
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
	[ComVisible(false)]
	public sealed class SecurityRoleAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000B6BA File Offset: 0x0000A6BA
		private static string EveryoneAccount
		{
			get
			{
				if (SecurityRoleAttribute._everyone == null)
				{
					SecurityRoleAttribute._everyone = Security.GetEveryoneAccountName();
				}
				return SecurityRoleAttribute._everyone;
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000B6D2 File Offset: 0x0000A6D2
		public SecurityRoleAttribute(string role)
			: this(role, false)
		{
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000B6DC File Offset: 0x0000A6DC
		public SecurityRoleAttribute(string role, bool everyone)
		{
			this._role = role;
			this._setEveryoneAccess = everyone;
			this._description = null;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000B6F9 File Offset: 0x0000A6F9
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000B701 File Offset: 0x0000A701
		public string Role
		{
			get
			{
				return this._role;
			}
			set
			{
				this._role = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000B70A File Offset: 0x0000A70A
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000B712 File Offset: 0x0000A712
		public bool SetEveryoneAccess
		{
			get
			{
				return this._setEveryoneAccess;
			}
			set
			{
				this._setEveryoneAccess = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000B71B File Offset: 0x0000A71B
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000B723 File Offset: 0x0000A723
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000B72C File Offset: 0x0000A72C
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component" || s == "Method" || s == "Application" || s == "Interface";
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000B768 File Offset: 0x0000A768
		private ICatalogObject Search(ICatalogCollection coll, string key, string value)
		{
			for (int i = 0; i < coll.Count(); i++)
			{
				ICatalogObject catalogObject = (ICatalogObject)coll.Item(i);
				string text = (string)catalogObject.GetValue(key);
				if (text == value)
				{
					return catalogObject;
				}
			}
			return null;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000B7AC File Offset: 0x0000A7AC
		private void EnsureRole(Hashtable cache)
		{
			Hashtable hashtable = (Hashtable)cache[SecurityRoleAttribute.RoleCacheString];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				cache[SecurityRoleAttribute.RoleCacheString] = hashtable;
			}
			if (hashtable[this._role] != null)
			{
				return;
			}
			ICatalogCollection catalogCollection = (ICatalogCollection)cache["ApplicationCollection"];
			ICatalogObject catalogObject = (ICatalogObject)cache["Application"];
			ICatalogCollection catalogCollection2 = (ICatalogCollection)catalogCollection.GetCollection(CollectionName.Roles, catalogObject.Key());
			catalogCollection2.Populate();
			if (this.Search(catalogCollection2, "Name", this._role) == null)
			{
				ICatalogObject catalogObject2 = (ICatalogObject)catalogCollection2.Add();
				catalogObject2.SetValue("Name", this._role);
				if (this._description != null)
				{
					catalogObject2.SetValue("Description", this._description);
				}
				catalogCollection2.SaveChanges();
				if (this._setEveryoneAccess)
				{
					ICatalogCollection catalogCollection3 = (ICatalogCollection)catalogCollection2.GetCollection(CollectionName.UsersInRole, catalogObject2.Key());
					catalogCollection3.Populate();
					ICatalogObject catalogObject3 = (ICatalogObject)catalogCollection3.Add();
					catalogObject3.SetValue("User", SecurityRoleAttribute.EveryoneAccount);
					catalogCollection3.SaveChanges();
				}
			}
			hashtable[this._role] = true;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000B8F0 File Offset: 0x0000A8F0
		private void AddRoleFor(string target, Hashtable cache)
		{
			ICatalogCollection catalogCollection = (ICatalogCollection)cache[target + "Collection"];
			ICatalogObject catalogObject = (ICatalogObject)cache[target];
			ICatalogCollection catalogCollection2 = (ICatalogCollection)catalogCollection.GetCollection(CollectionName.RolesFor(target), catalogObject.Key());
			catalogCollection2.Populate();
			if (Platform.IsLessThan(Platform.W2K))
			{
				IRoleAssociationUtil roleAssociationUtil = (IRoleAssociationUtil)catalogCollection2.GetUtilInterface();
				roleAssociationUtil.AssociateRoleByName(this._role);
				return;
			}
			ICatalogObject catalogObject2 = this.Search(catalogCollection2, "Name", this._role);
			if (catalogObject2 != null)
			{
				return;
			}
			ICatalogObject catalogObject3 = (ICatalogObject)catalogCollection2.Add();
			catalogObject3.SetValue("Name", this._role);
			catalogCollection2.SaveChanges();
			catalogCollection2.Populate();
			for (int i = 0; i < catalogCollection2.Count(); i++)
			{
				ICatalogObject catalogObject4 = (ICatalogObject)catalogCollection2.Item(i);
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000B9CC File Offset: 0x0000A9CC
		bool IConfigurationAttribute.Apply(Hashtable cache)
		{
			this.EnsureRole(cache);
			string text = (string)cache["CurrentTarget"];
			if (text == "Method")
			{
				cache["SecurityOnMethods"] = true;
			}
			return true;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000BA10 File Offset: 0x0000AA10
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable cache)
		{
			string text = (string)cache["CurrentTarget"];
			if (text == "Component")
			{
				Platform.Assert(Platform.MTS, "SecurityRoleAttribute");
				this.AddRoleFor("Component", cache);
			}
			else if (text == "Method")
			{
				Platform.Assert(Platform.W2K, "SecurityRoleAttribute");
				this.AddRoleFor("Method", cache);
			}
			else if (text == "Interface")
			{
				this.AddRoleFor("Interface", cache);
			}
			else
			{
				text == "Application";
			}
			return true;
		}

		// Token: 0x04000168 RID: 360
		private string _role;

		// Token: 0x04000169 RID: 361
		private bool _setEveryoneAccess;

		// Token: 0x0400016A RID: 362
		private string _description;

		// Token: 0x0400016B RID: 363
		private static readonly string RoleCacheString = "RoleAttribute::ApplicationRoleCache";

		// Token: 0x0400016C RID: 364
		private static string _everyone;
	}
}
