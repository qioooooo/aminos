using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class TextBoxDesigner : TextBoxBaseDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new TextBoxActionList(this));
				}
				return this._actionLists;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "PasswordChar" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(TextBoxDesigner), propertyDescriptor, array2);
				}
			}
		}

		private char PasswordChar
		{
			get
			{
				TextBox textBox = this.Control as TextBox;
				if (textBox.UseSystemPasswordChar)
				{
					textBox.UseSystemPasswordChar = false;
					char passwordChar = textBox.PasswordChar;
					textBox.UseSystemPasswordChar = true;
					return passwordChar;
				}
				return textBox.PasswordChar;
			}
			set
			{
				TextBox textBox = this.Control as TextBox;
				textBox.PasswordChar = value;
			}
		}

		private DesignerActionListCollection _actionLists;
	}
}
