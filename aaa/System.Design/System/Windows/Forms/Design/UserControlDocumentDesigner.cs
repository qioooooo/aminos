using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002DE RID: 734
	[ToolboxItemFilter("System.Windows.Forms.MainMenu", ToolboxItemFilterType.Prevent)]
	[ToolboxItemFilter("System.Windows.Forms.UserControl", ToolboxItemFilterType.Custom)]
	internal class UserControlDocumentDesigner : DocumentDesigner
	{
		// Token: 0x06001C48 RID: 7240 RVA: 0x0009F464 File Offset: 0x0009E464
		public UserControlDocumentDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x0009F473 File Offset: 0x0009E473
		// (set) Token: 0x06001C4A RID: 7242 RVA: 0x0009F480 File Offset: 0x0009E480
		private Size Size
		{
			get
			{
				return this.Control.ClientSize;
			}
			set
			{
				this.Control.ClientSize = value;
			}
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x0009F490 File Offset: 0x0009E490
		internal override bool CanDropComponents(DragEventArgs de)
		{
			bool flag = base.CanDropComponents(de);
			if (flag)
			{
				OleDragDropHandler oleDragHandler = base.GetOleDragHandler();
				object[] draggingObjects = oleDragHandler.GetDraggingObjects(de);
				if (draggingObjects != null)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					for (int i = 0; i < draggingObjects.Length; i++)
					{
						if (designerHost != null && draggingObjects[i] != null && draggingObjects[i] is IComponent && draggingObjects[i] is MainMenu)
						{
							return false;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x0009F504 File Offset: 0x0009E504
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Size" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(UserControlDocumentDesigner), propertyDescriptor, array2);
				}
			}
		}
	}
}
