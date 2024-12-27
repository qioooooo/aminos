using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002A9 RID: 681
	internal class TextBoxActionList : DesignerActionList
	{
		// Token: 0x0600198A RID: 6538 RVA: 0x00089B64 File Offset: 0x00088B64
		public TextBoxActionList(TextBoxDesigner designer)
			: base(designer.Component)
		{
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x00089B72 File Offset: 0x00088B72
		// (set) Token: 0x0600198C RID: 6540 RVA: 0x00089B84 File Offset: 0x00088B84
		public bool Multiline
		{
			get
			{
				return ((TextBox)base.Component).Multiline;
			}
			set
			{
				TypeDescriptor.GetProperties(base.Component)["Multiline"].SetValue(base.Component, value);
			}
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00089BAC File Offset: 0x00088BAC
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionPropertyItem("Multiline", SR.GetString("MultiLineDisplayName"), SR.GetString("PropertiesCategoryName"), SR.GetString("MultiLineDescription"))
			};
		}
	}
}
