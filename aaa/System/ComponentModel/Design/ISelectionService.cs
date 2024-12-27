using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x0200018E RID: 398
	[ComVisible(true)]
	public interface ISelectionService
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000C97 RID: 3223
		object PrimarySelection { get; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000C98 RID: 3224
		int SelectionCount { get; }

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000C99 RID: 3225
		// (remove) Token: 0x06000C9A RID: 3226
		event EventHandler SelectionChanged;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000C9B RID: 3227
		// (remove) Token: 0x06000C9C RID: 3228
		event EventHandler SelectionChanging;

		// Token: 0x06000C9D RID: 3229
		bool GetComponentSelected(object component);

		// Token: 0x06000C9E RID: 3230
		ICollection GetSelectedComponents();

		// Token: 0x06000C9F RID: 3231
		void SetSelectedComponents(ICollection components);

		// Token: 0x06000CA0 RID: 3232
		void SetSelectedComponents(ICollection components, SelectionTypes selectionType);
	}
}
