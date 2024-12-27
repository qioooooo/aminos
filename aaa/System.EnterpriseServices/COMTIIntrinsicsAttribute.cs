using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200006E RID: 110
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class COMTIIntrinsicsAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600025B RID: 603 RVA: 0x00006AF0 File Offset: 0x00005AF0
		public COMTIIntrinsicsAttribute()
			: this(true)
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00006AF9 File Offset: 0x00005AF9
		public COMTIIntrinsicsAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00006B08 File Offset: 0x00005B08
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00006B10 File Offset: 0x00005B10
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00006B20 File Offset: 0x00005B20
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "COMTIIntrinsicsAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("COMTIIntrinsics", this._value);
			return true;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00006B64 File Offset: 0x00005B64
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000FA RID: 250
		private bool _value;
	}
}
