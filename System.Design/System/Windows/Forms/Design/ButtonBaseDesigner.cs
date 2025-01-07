using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ButtonBaseDesigner : ControlDesigner
	{
		public ButtonBaseDesigner()
		{
			base.AutoResizeHandles = true;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["UseVisualStyleBackColor"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, true);
			}
		}

		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				FlatStyle flatStyle = FlatStyle.Standard;
				ContentAlignment contentAlignment = ContentAlignment.MiddleCenter;
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				PropertyDescriptor propertyDescriptor;
				if ((propertyDescriptor = properties["TextAlign"]) != null)
				{
					contentAlignment = (ContentAlignment)propertyDescriptor.GetValue(base.Component);
				}
				if ((propertyDescriptor = properties["FlatStyle"]) != null)
				{
					flatStyle = (FlatStyle)propertyDescriptor.GetValue(base.Component);
				}
				int num = DesignerUtils.GetTextBaseline(this.Control, contentAlignment);
				if (this.Control is CheckBox || this.Control is RadioButton)
				{
					Appearance appearance = Appearance.Normal;
					if ((propertyDescriptor = properties["Appearance"]) != null)
					{
						appearance = (Appearance)propertyDescriptor.GetValue(base.Component);
					}
					if (appearance == Appearance.Normal)
					{
						if (this.Control is CheckBox)
						{
							num += this.CheckboxBaselineOffset(contentAlignment, flatStyle);
						}
						else
						{
							num += this.RadiobuttonBaselineOffset(contentAlignment, flatStyle);
						}
					}
					else
					{
						num += this.DefaultBaselineOffset(contentAlignment, flatStyle);
					}
				}
				else
				{
					num += this.DefaultBaselineOffset(contentAlignment, flatStyle);
				}
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}

		private int CheckboxBaselineOffset(ContentAlignment alignment, FlatStyle flatStyle)
		{
			if ((alignment & DesignerUtils.anyMiddleAlignment) != (ContentAlignment)0)
			{
				if (flatStyle == FlatStyle.Standard || flatStyle == FlatStyle.System)
				{
					return -1;
				}
				return 0;
			}
			else if ((alignment & DesignerUtils.anyTopAlignment) != (ContentAlignment)0)
			{
				if (flatStyle == FlatStyle.Standard)
				{
					return 1;
				}
				if (flatStyle == FlatStyle.System)
				{
					return 0;
				}
				if (flatStyle == FlatStyle.Flat || flatStyle == FlatStyle.Popup)
				{
					return 2;
				}
				return 0;
			}
			else
			{
				if (flatStyle == FlatStyle.Standard)
				{
					return -3;
				}
				if (flatStyle == FlatStyle.System)
				{
					return 0;
				}
				if (flatStyle == FlatStyle.Flat || flatStyle == FlatStyle.Popup)
				{
					return -2;
				}
				return 0;
			}
		}

		private int RadiobuttonBaselineOffset(ContentAlignment alignment, FlatStyle flatStyle)
		{
			if ((alignment & DesignerUtils.anyMiddleAlignment) != (ContentAlignment)0)
			{
				if (flatStyle == FlatStyle.System)
				{
					return -1;
				}
				return 0;
			}
			else if (flatStyle == FlatStyle.Standard || flatStyle == FlatStyle.Flat || flatStyle == FlatStyle.Popup)
			{
				if ((alignment & DesignerUtils.anyTopAlignment) == (ContentAlignment)0)
				{
					return -2;
				}
				return 2;
			}
			else
			{
				if (flatStyle == FlatStyle.System)
				{
					return 0;
				}
				return 0;
			}
		}

		private int DefaultBaselineOffset(ContentAlignment alignment, FlatStyle flatStyle)
		{
			if ((alignment & DesignerUtils.anyMiddleAlignment) != (ContentAlignment)0)
			{
				return 0;
			}
			if (flatStyle == FlatStyle.Standard || flatStyle == FlatStyle.Popup)
			{
				if ((alignment & DesignerUtils.anyTopAlignment) == (ContentAlignment)0)
				{
					return -4;
				}
				return 4;
			}
			else if (flatStyle == FlatStyle.System)
			{
				if ((alignment & DesignerUtils.anyTopAlignment) == (ContentAlignment)0)
				{
					return -3;
				}
				return 3;
			}
			else
			{
				if (flatStyle != FlatStyle.Flat)
				{
					return 0;
				}
				if ((alignment & DesignerUtils.anyTopAlignment) == (ContentAlignment)0)
				{
					return -5;
				}
				return 5;
			}
		}
	}
}
