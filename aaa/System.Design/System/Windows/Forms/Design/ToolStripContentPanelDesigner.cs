using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002B8 RID: 696
	internal class ToolStripContentPanelDesigner : PanelDesigner
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0008D5A0 File Offset: 0x0008C5A0
		private ContextMenuStrip DesignerContextMenu
		{
			get
			{
				if (this.contextMenu == null)
				{
					this.contextMenu = new BaseContextMenuStrip(base.Component.Site, base.Component as Component);
					this.contextMenu.GroupOrdering.Clear();
					this.contextMenu.GroupOrdering.AddRange(new string[] { "Code", "Verbs", "Custom", "Selection", "Edit", "Properties" });
					this.contextMenu.Text = "CustomContextMenu";
				}
				return this.contextMenu;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0008D648 File Offset: 0x0008C648
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = null;
				base.AddPaddingSnapLines(ref arrayList);
				return arrayList;
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0008D660 File Offset: 0x0008C660
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return false;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0008D664 File Offset: 0x0008C664
		protected override void OnContextMenu(int x, int y)
		{
			ToolStripContentPanel toolStripContentPanel = base.Component as ToolStripContentPanel;
			if (toolStripContentPanel != null && toolStripContentPanel.Parent is ToolStripContainer)
			{
				this.DesignerContextMenu.Show(x, y);
				return;
			}
			base.OnContextMenu(x, y);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0008D6A4 File Offset: 0x0008C6A4
		protected override void PreFilterEvents(IDictionary events)
		{
			base.PreFilterEvents(events);
			string[] array = new string[]
			{
				"BindingContextChanged", "ChangeUICues", "ClientSizeChanged", "EnabledChanged", "FontChanged", "ForeColorChanged", "GiveFeedback", "ImeModeChanged", "Move", "QueryAccessibilityHelp",
				"Validated", "Validating", "VisibleChanged"
			};
			for (int i = 0; i < array.Length; i++)
			{
				EventDescriptor eventDescriptor = (EventDescriptor)events[array[i]];
				if (eventDescriptor != null)
				{
					events[array[i]] = TypeDescriptor.CreateEvent(eventDescriptor.ComponentType, eventDescriptor, new Attribute[] { BrowsableAttribute.No });
				}
			}
		}

		// Token: 0x040014E9 RID: 5353
		private BaseContextMenuStrip contextMenu;
	}
}
