using System;
using System.Collections;

namespace System.Windows.Forms.Design
{
	internal class StatusBarDesigner : ControlDesigner
	{
		public StatusBarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		public override ICollection AssociatedComponents
		{
			get
			{
				StatusBar statusBar = this.Control as StatusBar;
				if (statusBar != null)
				{
					return statusBar.Panels;
				}
				return base.AssociatedComponents;
			}
		}
	}
}
