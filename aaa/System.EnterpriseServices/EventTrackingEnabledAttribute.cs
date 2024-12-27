using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000070 RID: 112
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[ComVisible(false)]
	public sealed class EventTrackingEnabledAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000267 RID: 615 RVA: 0x00006BDB File Offset: 0x00005BDB
		public EventTrackingEnabledAttribute()
			: this(true)
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00006BE4 File Offset: 0x00005BE4
		public EventTrackingEnabledAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00006BF3 File Offset: 0x00005BF3
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00006BFB File Offset: 0x00005BFB
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00006C08 File Offset: 0x00005C08
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "EventTrackingEnabledAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("EventTrackingEnabled", this._value);
			return true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00006C4C File Offset: 0x00005C4C
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000FC RID: 252
		private bool _value;
	}
}
