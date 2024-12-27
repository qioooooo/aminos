using System;
using System.Collections;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002D6 RID: 726
	internal class NotifyIconDesigner : ComponentDesigner
	{
		// Token: 0x06001C0E RID: 7182 RVA: 0x0009DD34 File Offset: 0x0009CD34
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			NotifyIcon notifyIcon = (NotifyIcon)base.Component;
			notifyIcon.Visible = true;
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x0009DD5B File Offset: 0x0009CD5B
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new NotifyIconActionList(this));
				}
				return this._actionLists;
			}
		}

		// Token: 0x040015C2 RID: 5570
		private DesignerActionListCollection _actionLists;
	}
}
