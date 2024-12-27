using System;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002D7 RID: 727
	internal class NotifyIconActionList : DesignerActionList
	{
		// Token: 0x06001C11 RID: 7185 RVA: 0x0009DD90 File Offset: 0x0009CD90
		public NotifyIconActionList(NotifyIconDesigner designer)
			: base(designer.Component)
		{
			this._designer = designer;
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0009DDA5 File Offset: 0x0009CDA5
		public void ChooseIcon()
		{
			EditorServiceContext.EditValue(this._designer, base.Component, "Icon");
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0009DDC0 File Offset: 0x0009CDC0
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionMethodItem(this, "ChooseIcon", SR.GetString("ChooseIconDisplayName"), true)
			};
		}

		// Token: 0x040015C3 RID: 5571
		private NotifyIconDesigner _designer;
	}
}
