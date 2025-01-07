using System;
using System.Collections;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class NotifyIconDesigner : ComponentDesigner
	{
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			NotifyIcon notifyIcon = (NotifyIcon)base.Component;
			notifyIcon.Visible = true;
		}

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

		private DesignerActionListCollection _actionLists;
	}
}
