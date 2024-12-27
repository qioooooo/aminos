using System;
using System.Collections;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003F3 RID: 1011
	public class BulletedListDesigner : ListControlDesigner
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000C84C5 File Offset: 0x000C74C5
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000C84C8 File Offset: 0x000C74C8
		protected override void PostFilterEvents(IDictionary events)
		{
			base.PostFilterEvents(events);
			events.Remove("SelectedIndexChanged");
		}
	}
}
