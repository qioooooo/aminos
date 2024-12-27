using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000280 RID: 640
	internal class RichTextBoxDesigner : TextBoxBaseDesigner
	{
		// Token: 0x060017CE RID: 6094 RVA: 0x0007BDE4 File Offset: 0x0007ADE4
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control control = this.Control;
			if (control != null && control.Handle != IntPtr.Zero)
			{
				NativeMethods.RevokeDragDrop(control.Handle);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x0007BE20 File Offset: 0x0007AE20
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

		// Token: 0x060017D0 RID: 6096 RVA: 0x0007BE50 File Offset: 0x0007AE50
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

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0007BEB9 File Offset: 0x0007AEB9
		// (set) Token: 0x060017D2 RID: 6098 RVA: 0x0007BEC8 File Offset: 0x0007AEC8
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

		// Token: 0x040013B4 RID: 5044
		private DesignerActionListCollection _actionLists;
	}
}
