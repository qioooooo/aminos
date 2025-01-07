using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class ToolBarDesigner : ControlDesigner
	{
		public ToolBarDesigner()
		{
			base.AutoResizeHandles = true;
		}

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
