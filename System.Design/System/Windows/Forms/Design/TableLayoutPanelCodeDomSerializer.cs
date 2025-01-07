using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	internal class TableLayoutPanelCodeDomSerializer : CodeDomSerializer
	{
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			return this.GetBaseSerializer(manager).Deserialize(manager, codeObject);
		}

		private CodeDomSerializer GetBaseSerializer(IDesignerSerializationManager manager)
		{
			return (CodeDomSerializer)manager.GetSerializer(typeof(TableLayoutPanel).BaseType, typeof(CodeDomSerializer));
		}

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

		private static readonly string LayoutSettingsPropName = "LayoutSettings";
	}
}
