using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200006C RID: 108
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[ComVisible(false)]
	public sealed class ConstructionEnabledAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000243 RID: 579 RVA: 0x00006893 File Offset: 0x00005893
		public ConstructionEnabledAttribute()
		{
			this._enabled = true;
			this._default = "";
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000068AD File Offset: 0x000058AD
		public ConstructionEnabledAttribute(bool val)
		{
			this._enabled = val;
			this._default = "";
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000245 RID: 581 RVA: 0x000068C7 File Offset: 0x000058C7
		// (set) Token: 0x06000246 RID: 582 RVA: 0x000068CF File Offset: 0x000058CF
		public string Default
		{
			get
			{
				return this._default;
			}
			set
			{
				this._default = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000247 RID: 583 RVA: 0x000068D8 File Offset: 0x000058D8
		// (set) Token: 0x06000248 RID: 584 RVA: 0x000068E0 File Offset: 0x000058E0
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000068E9 File Offset: 0x000058E9
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000068F8 File Offset: 0x000058F8
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "ConstructionEnabledAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("ConstructionEnabled", this._enabled);
			if (this._default != null && this._default != "")
			{
				catalogObject.SetValue("ConstructorString", this._default);
			}
			return true;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00006967 File Offset: 0x00005967
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000F4 RID: 244
		private bool _enabled;

		// Token: 0x040000F5 RID: 245
		private string _default;
	}
}
