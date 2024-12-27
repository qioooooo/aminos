using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200006B RID: 107
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class MustRunInClientContextAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000681F File Offset: 0x0000581F
		public MustRunInClientContextAttribute()
			: this(true)
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00006828 File Offset: 0x00005828
		public MustRunInClientContextAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00006837 File Offset: 0x00005837
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000683F File Offset: 0x0000583F
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000684C File Offset: 0x0000584C
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "MustRunInClientContextAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("MustRunInClientContext", this._value);
			return true;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00006890 File Offset: 0x00005890
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000F3 RID: 243
		private bool _value;
	}
}
