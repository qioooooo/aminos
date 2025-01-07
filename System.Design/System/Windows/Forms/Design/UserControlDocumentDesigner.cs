using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	[ToolboxItemFilter("System.Windows.Forms.MainMenu", ToolboxItemFilterType.Prevent)]
	[ToolboxItemFilter("System.Windows.Forms.UserControl", ToolboxItemFilterType.Custom)]
	internal class UserControlDocumentDesigner : DocumentDesigner
	{
		public UserControlDocumentDesigner()
		{
			base.AutoResizeHandles = true;
		}

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
