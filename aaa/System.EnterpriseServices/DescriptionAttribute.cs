using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200007B RID: 123
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface, Inherited = true)]
	public sealed class DescriptionAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x00007534 File Offset: 0x00006534
		public DescriptionAttribute(string desc)
		{
			this._desc = desc;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00007543 File Offset: 0x00006543
		private string Description
		{
			get
			{
				return this._desc;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000754B File Offset: 0x0000654B
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application" || s == "Component" || s == "Interface" || s == "Method";
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00007584 File Offset: 0x00006584
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.MTS, "DescriptionAttribute");
			string text = (string)info["CurrentTarget"];
			ICatalogObject catalogObject = (ICatalogObject)info[text];
			catalogObject.SetValue("Description", this.Description);
			return true;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000075D0 File Offset: 0x000065D0
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x0400010D RID: 269
		private string _desc;
	}
}
