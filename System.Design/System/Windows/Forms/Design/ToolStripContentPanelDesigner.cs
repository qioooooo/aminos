using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class ToolStripContentPanelDesigner : PanelDesigner
	{
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

		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = null;
				base.AddPaddingSnapLines(ref arrayList);
				return arrayList;
			}
		}

		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return false;
		}

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

		private BaseContextMenuStrip contextMenu;
	}
}
