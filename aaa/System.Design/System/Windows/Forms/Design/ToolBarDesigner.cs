using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002AB RID: 683
	internal class ToolBarDesigner : ControlDesigner
	{
		// Token: 0x06001990 RID: 6544 RVA: 0x00089BF9 File Offset: 0x00088BF9
		public ToolBarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x00089C08 File Offset: 0x00088C08
		public override ICollection AssociatedComponents
		{
			get
			{
				ToolBar toolBar = this.Control as ToolBar;
				if (toolBar != null)
				{
					return toolBar.Buttons;
				}
				return base.AssociatedComponents;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x00089C34 File Offset: 0x00088C34
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				object component = base.Component;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["Dock"];
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(component)["AutoSize"];
				if (propertyDescriptor != null && propertyDescriptor2 != null)
				{
					DockStyle dockStyle = (DockStyle)propertyDescriptor.GetValue(component);
					bool flag = (bool)propertyDescriptor2.GetValue(component);
					if (flag)
					{
						selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
						if (dockStyle != DockStyle.None)
						{
							selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
						}
					}
				}
				return selectionRules;
			}
		}
	}
}
