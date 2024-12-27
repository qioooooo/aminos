using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000071 RID: 113
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[ComVisible(false)]
	public sealed class ExceptionClassAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600026D RID: 621 RVA: 0x00006C4F File Offset: 0x00005C4F
		public ExceptionClassAttribute(string name)
		{
			this._value = name;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00006C5E File Offset: 0x00005C5E
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00006C66 File Offset: 0x00005C66
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00006C74 File Offset: 0x00005C74
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "ExceptionClassAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("ExceptionClass", this._value);
			return true;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00006CB3 File Offset: 0x00005CB3
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000FD RID: 253
		private string _value;
	}
}
