using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000253 RID: 595
	internal interface IMouseHandler
	{
		// Token: 0x060016B0 RID: 5808
		void OnMouseDoubleClick(IComponent component);

		// Token: 0x060016B1 RID: 5809
		void OnMouseDown(IComponent component, MouseButtons button, int x, int y);

		// Token: 0x060016B2 RID: 5810
		void OnMouseHover(IComponent component);

		// Token: 0x060016B3 RID: 5811
		void OnMouseMove(IComponent component, int x, int y);

		// Token: 0x060016B4 RID: 5812
		void OnMouseUp(IComponent component, MouseButtons button);

		// Token: 0x060016B5 RID: 5813
		void OnSetCursor(IComponent component);
	}
}
