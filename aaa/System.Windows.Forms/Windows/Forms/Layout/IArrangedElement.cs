using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020001E8 RID: 488
	internal interface IArrangedElement : IComponent, IDisposable
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06001343 RID: 4931
		Rectangle Bounds { get; }

		// Token: 0x06001344 RID: 4932
		void SetBounds(Rectangle bounds, BoundsSpecified specified);

		// Token: 0x06001345 RID: 4933
		Size GetPreferredSize(Size proposedSize);

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06001346 RID: 4934
		Rectangle DisplayRectangle { get; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06001347 RID: 4935
		bool ParticipatesInLayout { get; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001348 RID: 4936
		PropertyStore Properties { get; }

		// Token: 0x06001349 RID: 4937
		void PerformLayout(IArrangedElement affectedElement, string propertyName);

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600134A RID: 4938
		IArrangedElement Container { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600134B RID: 4939
		ArrangedElementCollection Children { get; }
	}
}
