using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200006F RID: 111
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class IISIntrinsicsAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000261 RID: 609 RVA: 0x00006B67 File Offset: 0x00005B67
		public IISIntrinsicsAttribute()
			: this(true)
		{
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00006B70 File Offset: 0x00005B70
		public IISIntrinsicsAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00006B7F File Offset: 0x00005B7F
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00006B87 File Offset: 0x00005B87
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00006B94 File Offset: 0x00005B94
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "IISIntrinsicsAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("IISIntrinsics", this._value);
			return true;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00006BD8 File Offset: 0x00005BD8
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000FB RID: 251
		private bool _value;
	}
}
