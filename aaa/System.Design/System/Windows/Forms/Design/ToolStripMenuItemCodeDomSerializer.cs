using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002CC RID: 716
	internal class ToolStripMenuItemCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x06001B36 RID: 6966 RVA: 0x00096B2B File Offset: 0x00095B2B
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			return this.GetBaseSerializer(manager).Deserialize(manager, codeObject);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00096B3B File Offset: 0x00095B3B
		private CodeDomSerializer GetBaseSerializer(IDesignerSerializationManager manager)
		{
			return (CodeDomSerializer)manager.GetSerializer(typeof(Component), typeof(CodeDomSerializer));
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00096B5C File Offset: 0x00095B5C
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
