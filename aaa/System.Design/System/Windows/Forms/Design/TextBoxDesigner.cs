using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002A8 RID: 680
	internal class TextBoxDesigner : TextBoxBaseDesigner
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x00089A60 File Offset: 0x00088A60
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

		// Token: 0x06001986 RID: 6534 RVA: 0x00089A90 File Offset: 0x00088A90
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

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x00089AFC File Offset: 0x00088AFC
		// (set) Token: 0x06001988 RID: 6536 RVA: 0x00089B3C File Offset: 0x00088B3C
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

		// Token: 0x040014A8 RID: 5288
		private DesignerActionListCollection _actionLists;
	}
}
