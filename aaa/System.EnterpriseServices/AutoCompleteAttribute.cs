using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000075 RID: 117
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	[ComVisible(false)]
	public sealed class AutoCompleteAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000286 RID: 646 RVA: 0x00006E56 File Offset: 0x00005E56
		public AutoCompleteAttribute()
			: this(true)
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00006E5F File Offset: 0x00005E5F
		public AutoCompleteAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00006E6E File Offset: 0x00005E6E
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00006E76 File Offset: 0x00005E76
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Method";
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00006E84 File Offset: 0x00005E84
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "AutoCompleteAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Method"];
			catalogObject.SetValue("AutoComplete", this._value);
			return true;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00006EC8 File Offset: 0x00005EC8
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x04000102 RID: 258
		private bool _value;
	}
}
