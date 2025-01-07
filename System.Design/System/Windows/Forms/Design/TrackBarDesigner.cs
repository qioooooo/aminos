using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class TrackBarDesigner : ControlDesigner
	{
		public TrackBarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				object component = base.Component;
				selectionRules |= SelectionRules.AllSizeable;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["AutoSize"];
				if (propertyDescriptor != null)
				{
					bool flag = (bool)propertyDescriptor.GetValue(component);
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(component)["Orientation"];
					Orientation orientation = Orientation.Horizontal;
					if (propertyDescriptor2 != null)
					{
						orientation = (Orientation)propertyDescriptor2.GetValue(component);
					}
					if (flag)
					{
						if (orientation == Orientation.Horizontal)
						{
							selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
						}
						else if (orientation == Orientation.Vertical)
						{
							selectionRules &= ~(SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
						}
					}
				}
				return selectionRules;
			}
		}
	}
}
