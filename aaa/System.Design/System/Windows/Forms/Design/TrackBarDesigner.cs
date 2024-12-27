using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002D5 RID: 725
	internal class TrackBarDesigner : ControlDesigner
	{
		// Token: 0x06001C0C RID: 7180 RVA: 0x0009DCA0 File Offset: 0x0009CCA0
		public TrackBarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x0009DCB0 File Offset: 0x0009CCB0
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
