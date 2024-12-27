using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000077 RID: 119
	[AttributeUsage(AttributeTargets.Assembly, Inherited = true)]
	[ComVisible(false)]
	public sealed class ApplicationNameAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000295 RID: 661 RVA: 0x00007048 File Offset: 0x00006048
		public ApplicationNameAttribute(string name)
		{
			this._value = name;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00007057 File Offset: 0x00006057
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000705F File Offset: 0x0000605F
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application";
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000706C File Offset: 0x0000606C
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.MTS, "ApplicationNameAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Application"];
			catalogObject.SetValue("Name", this._value);
			return true;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000070AB File Offset: 0x000060AB
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x04000106 RID: 262
		private string _value;
	}
}
