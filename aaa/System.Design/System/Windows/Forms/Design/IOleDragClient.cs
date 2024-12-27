using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001B6 RID: 438
	internal interface IOleDragClient
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060010C3 RID: 4291
		IComponent Component { get; }

		// Token: 0x060010C4 RID: 4292
		bool AddComponent(IComponent component, string name, bool firstAdd);

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060010C5 RID: 4293
		bool CanModifyComponents { get; }

		// Token: 0x060010C6 RID: 4294
		bool IsDropOk(IComponent component);

		// Token: 0x060010C7 RID: 4295
		Control GetDesignerControl();

		// Token: 0x060010C8 RID: 4296
		Control GetControlForComponent(object component);
	}
}
