using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000072 RID: 114
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class LoadBalancingSupportedAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00006CB6 File Offset: 0x00005CB6
		public LoadBalancingSupportedAttribute()
			: this(true)
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00006CBF File Offset: 0x00005CBF
		public LoadBalancingSupportedAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00006CCE File Offset: 0x00005CCE
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00006CD6 File Offset: 0x00005CD6
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00006CE4 File Offset: 0x00005CE4
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "LoadBalancingSupportedAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("LoadBalancingSupported", this._value);
			return true;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00006D28 File Offset: 0x00005D28
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000FE RID: 254
		private bool _value;
	}
}
