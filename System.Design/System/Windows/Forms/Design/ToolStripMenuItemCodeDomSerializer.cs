using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	internal class ToolStripMenuItemCodeDomSerializer : CodeDomSerializer
	{
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			return this.GetBaseSerializer(manager).Deserialize(manager, codeObject);
		}

		private CodeDomSerializer GetBaseSerializer(IDesignerSerializationManager manager)
		{
			return (CodeDomSerializer)manager.GetSerializer(typeof(Component), typeof(CodeDomSerializer));
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			ToolStripMenuItem toolStripMenuItem = value as ToolStripMenuItem;
			ToolStrip currentParent = toolStripMenuItem.GetCurrentParent();
			if (toolStripMenuItem != null && !toolStripMenuItem.IsOnDropDown && currentParent != null && currentParent.Site == null)
			{
				return null;
			}
			CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(ImageList).BaseType, typeof(CodeDomSerializer));
			return codeDomSerializer.Serialize(manager, value);
		}
	}
}
