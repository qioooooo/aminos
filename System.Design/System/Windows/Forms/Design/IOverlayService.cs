using System;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal interface IOverlayService
	{
		int PushOverlay(Control control);

		void RemoveOverlay(Control control);

		void InsertOverlay(Control control, int index);

		void InvalidateOverlays(Rectangle screenRectangle);

		void InvalidateOverlays(Region screenRegion);
	}
}
