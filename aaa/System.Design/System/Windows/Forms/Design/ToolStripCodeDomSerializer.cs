using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002AF RID: 687
	internal class ToolStripCodeDomSerializer : ControlCodeDomSerializer
	{
		// Token: 0x060019BB RID: 6587 RVA: 0x0008A3E4 File Offset: 0x000893E4
		protected override bool HasSitedNonReadonlyChildren(Control parent)
		{
			ToolStrip toolStrip = parent as ToolStrip;
			if (toolStrip == null)
			{
				return false;
			}
			if (toolStrip.Items.Count == 0)
			{
				return false;
			}
			foreach (object obj in toolStrip.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (toolStripItem.Site != null && toolStrip.Site != null && toolStripItem.Site.Container == toolStrip.Site.Container)
				{
					InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(toolStripItem)[typeof(InheritanceAttribute)];
					if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
