using System;
using System.ComponentModel;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004F5 RID: 1269
	[SupportsEventValidation]
	[ToolboxItem(false)]
	internal class ChildTable : Table
	{
		// Token: 0x06003DEA RID: 15850 RVA: 0x00103121 File Offset: 0x00102121
		internal ChildTable()
			: this(1)
		{
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0010312A File Offset: 0x0010212A
		internal ChildTable(int parentLevel)
		{
			this._parentLevel = parentLevel;
			this._parentIDSet = false;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x00103140 File Offset: 0x00102140
		internal ChildTable(string parentID)
		{
			this._parentID = parentID;
			this._parentIDSet = true;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x00103158 File Offset: 0x00102158
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			string text = this._parentID;
			if (!this._parentIDSet)
			{
				text = this.GetParentID();
			}
			if (text != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, text);
			}
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00103190 File Offset: 0x00102190
		private string GetParentID()
		{
			if (this.ID != null)
			{
				return null;
			}
			Control control = this;
			for (int i = 0; i < this._parentLevel; i++)
			{
				control = control.Parent;
				if (control == null)
				{
					break;
				}
			}
			if (control != null)
			{
				string id = control.ID;
				if (!string.IsNullOrEmpty(id))
				{
					return control.ClientID;
				}
			}
			return null;
		}

		// Token: 0x0400278C RID: 10124
		private int _parentLevel;

		// Token: 0x0400278D RID: 10125
		private string _parentID;

		// Token: 0x0400278E RID: 10126
		private bool _parentIDSet;
	}
}
