using System;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000219 RID: 537
	internal interface IOverlayService
	{
		// Token: 0x0600142B RID: 5163
		int PushOverlay(Control control);

		// Token: 0x0600142C RID: 5164
		void RemoveOverlay(Control control);

		// Token: 0x0600142D RID: 5165
		void InsertOverlay(Control control, int index);

		// Token: 0x0600142E RID: 5166
		void InvalidateOverlays(Rectangle screenRectangle);

		// Token: 0x0600142F RID: 5167
		void InvalidateOverlays(Region screenRegion);
	}
}
