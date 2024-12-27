using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200006A RID: 106
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class SynchronizationAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000067AA File Offset: 0x000057AA
		public SynchronizationAttribute()
			: this(SynchronizationOption.Required)
		{
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000067B3 File Offset: 0x000057B3
		public SynchronizationAttribute(SynchronizationOption val)
		{
			this._value = val;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000239 RID: 569 RVA: 0x000067C2 File Offset: 0x000057C2
		public SynchronizationOption Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x000067CA File Offset: 0x000057CA
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000067D8 File Offset: 0x000057D8
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "SynchronizationAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("Synchronization", this._value);
			return true;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000681C File Offset: 0x0000581C
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000F2 RID: 242
		private SynchronizationOption _value;
	}
}
