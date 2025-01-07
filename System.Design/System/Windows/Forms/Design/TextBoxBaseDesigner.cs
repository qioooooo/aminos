using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class TextBoxBaseDesigner : ControlDesigner
	{
		public TextBoxBaseDesigner()
		{
			base.AutoResizeHandles = true;
		}

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

		private bool ShouldSerializeText()
		{
			return TypeDescriptor.GetProperties(typeof(TextBoxBase))["Text"].ShouldSerializeValue(base.Component);
		}

		private void ResetText()
		{
			this.Control.Text = "";
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, "");
			}
		}

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
