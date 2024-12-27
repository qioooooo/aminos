using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200008E RID: 142
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[ComVisible(false)]
	public sealed class ComponentAccessControlAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000364 RID: 868 RVA: 0x0000B4A3 File Offset: 0x0000A4A3
		public ComponentAccessControlAttribute()
			: this(true)
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000B4AC File Offset: 0x0000A4AC
		public ComponentAccessControlAttribute(bool val)
		{
			this._value = val;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000B4BB File Offset: 0x0000A4BB
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000B4C3 File Offset: 0x0000A4C3
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000B4D0 File Offset: 0x0000A4D0
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.MTS, "ComponentAccessControlAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			if (Platform.IsLessThan(Platform.W2K))
			{
				catalogObject.SetValue("SecurityEnabled", this._value ? "Y" : "N");
			}
			else
			{
				catalogObject.SetValue("ComponentAccessChecksEnabled", this._value);
			}
			return true;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000B541 File Offset: 0x0000A541
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x04000152 RID: 338
		private bool _value;
	}
}
