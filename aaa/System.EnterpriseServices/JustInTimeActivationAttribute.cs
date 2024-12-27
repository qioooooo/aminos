using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000069 RID: 105
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[ComVisible(false)]
	public sealed class JustInTimeActivationAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000231 RID: 561 RVA: 0x0000670B File Offset: 0x0000570B
		public JustInTimeActivationAttribute()
			: this(true)
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00006714 File Offset: 0x00005714
		public JustInTimeActivationAttribute(bool val)
		{
			this._enabled = val;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00006723 File Offset: 0x00005723
		public bool Value
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000672B File Offset: 0x0000572B
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00006738 File Offset: 0x00005738
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "JustInTimeActivationAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("JustInTimeActivation", this._enabled);
			if (this._enabled && (int)catalogObject.GetValue("Synchronization") == 0)
			{
				catalogObject.SetValue("Synchronization", SynchronizationOption.Required);
			}
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000067A7 File Offset: 0x000057A7
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000F1 RID: 241
		private bool _enabled;
	}
}
