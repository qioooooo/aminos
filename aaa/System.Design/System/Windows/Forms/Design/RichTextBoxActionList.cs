using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000281 RID: 641
	internal class RichTextBoxActionList : DesignerActionList
	{
		// Token: 0x060017D4 RID: 6100 RVA: 0x0007BF13 File Offset: 0x0007AF13
		public RichTextBoxActionList(RichTextBoxDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0007BF28 File Offset: 0x0007AF28
		public void EditLines()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Lines");
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0007BF44 File Offset: 0x0007AF44
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "EditLines", SR.GetString("EditLinesDisplayName"), SR.GetString("LinksCategoryName"), SR.GetString("EditLinesDescription"), true)
			};
		}

		// Token: 0x040013B5 RID: 5045
		private RichTextBoxDesigner _designer;
	}
}
