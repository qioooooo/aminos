using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001A8 RID: 424
	internal class ButtonBaseDesigner : ControlDesigner
	{
		// Token: 0x06001046 RID: 4166 RVA: 0x0004A2CC File Offset: 0x000492CC
		public ButtonBaseDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0004A2DC File Offset: 0x000492DC
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["UseVisualStyleBackColor"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, true);
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0004A340 File Offset: 0x00049340
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

		// Token: 0x06001049 RID: 4169 RVA: 0x0004A45C File Offset: 0x0004945C
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

		// Token: 0x0600104A RID: 4170 RVA: 0x0004A4B6 File Offset: 0x000494B6
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

		// Token: 0x0600104B RID: 4171 RVA: 0x0004A4EC File Offset: 0x000494EC
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
