using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class RichTextBoxDesigner : TextBoxBaseDesigner
	{
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control control = this.Control;
			if (control != null && control.Handle != IntPtr.Zero)
			{
				NativeMethods.RevokeDragDrop(control.Handle);
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new RichTextBoxActionList(this));
				}
				return this._actionLists;
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
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(RichTextBoxDesigner), propertyDescriptor, array2);
				}
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
				string text = this.Control.Text;
				if (value != null)
				{
					value = value.Replace("\r\n", "\n");
				}
				if (text != value)
				{
					this.Control.Text = value;
				}
			}
		}

		private DesignerActionListCollection _actionLists;
	}
}
