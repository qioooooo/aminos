using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200026F RID: 623
	internal class TextBoxBaseDesigner : ControlDesigner
	{
		// Token: 0x06001780 RID: 6016 RVA: 0x0007A448 File Offset: 0x00079448
		public TextBoxBaseDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0007A458 File Offset: 0x00079458
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				int num = DesignerUtils.GetTextBaseline(this.Control, ContentAlignment.TopLeft);
				BorderStyle borderStyle = BorderStyle.Fixed3D;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
				if (propertyDescriptor != null)
				{
					borderStyle = (BorderStyle)propertyDescriptor.GetValue(base.Component);
				}
				if (borderStyle == BorderStyle.None)
				{
					num = num;
				}
				else if (borderStyle == BorderStyle.FixedSingle)
				{
					num += 2;
				}
				else if (borderStyle == BorderStyle.Fixed3D)
				{
					num += 3;
				}
				else
				{
					num = num;
				}
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x0007A4D8 File Offset: 0x000794D8
		// (set) Token: 0x06001783 RID: 6019 RVA: 0x0007A4E5 File Offset: 0x000794E5
		private string Text
		{
			get
			{
				return this.Control.Text;
			}
			set
			{
				this.Control.Text = value;
				((TextBoxBase)this.Control).Select(0, 0);
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0007A505 File Offset: 0x00079505
		private bool ShouldSerializeText()
		{
			return TypeDescriptor.GetProperties(typeof(TextBoxBase))["Text"].ShouldSerializeValue(base.Component);
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0007A52B File Offset: 0x0007952B
		private void ResetText()
		{
			this.Control.Text = "";
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0007A540 File Offset: 0x00079540
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, "");
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0007A5A0 File Offset: 0x000795A0
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Text" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(TextBoxBaseDesigner), propertyDescriptor, array2);
				}
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x0007A60C File Offset: 0x0007960C
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				object component = base.Component;
				selectionRules |= SelectionRules.AllSizeable;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["Multiline"];
				if (propertyDescriptor != null)
				{
					object value = propertyDescriptor.GetValue(component);
					if (value is bool && !(bool)value)
					{
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(component)["AutoSize"];
						if (propertyDescriptor2 != null)
						{
							object value2 = propertyDescriptor2.GetValue(component);
							if (value2 is bool && (bool)value2)
							{
								selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
							}
						}
					}
				}
				return selectionRules;
			}
		}
	}
}
