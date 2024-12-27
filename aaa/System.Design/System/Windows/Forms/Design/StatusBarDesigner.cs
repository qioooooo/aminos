using System;
using System.Collections;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000295 RID: 661
	internal class StatusBarDesigner : ControlDesigner
	{
		// Token: 0x06001883 RID: 6275 RVA: 0x000818EF File Offset: 0x000808EF
		public StatusBarDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x00081900 File Offset: 0x00080900
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
