using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class ToolStripCodeDomSerializer : ControlCodeDomSerializer
	{
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
