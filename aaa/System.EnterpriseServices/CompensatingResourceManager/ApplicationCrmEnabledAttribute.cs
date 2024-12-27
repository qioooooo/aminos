using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000B4 RID: 180
	[ComVisible(false)]
	[ProgId("System.EnterpriseServices.Crm.ApplicationCrmEnabledAttribute")]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = true)]
	public sealed class ApplicationCrmEnabledAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x0000D7B4 File Offset: 0x0000C7B4
		public ApplicationCrmEnabledAttribute()
			: this(true)
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000D7BD File Offset: 0x0000C7BD
		public ApplicationCrmEnabledAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0000D7CC File Offset: 0x0000C7CC
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000D7D4 File Offset: 0x0000C7D4
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application";
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000D7E4 File Offset: 0x0000C7E4
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "CrmEnabledAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Application"];
			catalogObject.SetValue("CRMEnabled", this._value);
			return true;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000D828 File Offset: 0x0000C828
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040001EF RID: 495
		private bool _value;
	}
}
