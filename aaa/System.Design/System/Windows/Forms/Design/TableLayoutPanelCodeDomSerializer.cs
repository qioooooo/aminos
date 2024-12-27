using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200029E RID: 670
	internal class TableLayoutPanelCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x060018DD RID: 6365 RVA: 0x00084FA0 File Offset: 0x00083FA0
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			return this.GetBaseSerializer(manager).Deserialize(manager, codeObject);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x00084FB0 File Offset: 0x00083FB0
		private CodeDomSerializer GetBaseSerializer(IDesignerSerializationManager manager)
		{
			return (CodeDomSerializer)manager.GetSerializer(typeof(TableLayoutPanel).BaseType, typeof(CodeDomSerializer));
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x00084FD8 File Offset: 0x00083FD8
		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			object obj = this.GetBaseSerializer(manager).Serialize(manager, value);
			TableLayoutPanel tableLayoutPanel = value as TableLayoutPanel;
			if (tableLayoutPanel != null)
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(tableLayoutPanel)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
				{
					IDesignerHost designerHost = (IDesignerHost)manager.GetService(typeof(IDesignerHost));
					if (this.IsLocalizable(designerHost))
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(tableLayoutPanel)[TableLayoutPanelCodeDomSerializer.LayoutSettingsPropName];
						object obj2 = ((propertyDescriptor != null) ? propertyDescriptor.GetValue(tableLayoutPanel) : null);
						if (obj2 != null)
						{
							string text = manager.GetName(tableLayoutPanel) + "." + TableLayoutPanelCodeDomSerializer.LayoutSettingsPropName;
							base.SerializeResourceInvariant(manager, text, obj2);
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00085094 File Offset: 0x00084094
		private bool IsLocalizable(IDesignerHost host)
		{
			if (host != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(host.RootComponent)["Localizable"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool))
				{
					return (bool)propertyDescriptor.GetValue(host.RootComponent);
				}
			}
			return false;
		}

		// Token: 0x04001473 RID: 5235
		private static readonly string LayoutSettingsPropName = "LayoutSettings";
	}
}
