using System;
using System.Runtime.InteropServices;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000137 RID: 311
	[Guid("ED4BAE22-2F3C-419a-B487-CF869E716B95")]
	[ComVisible(true)]
	public interface IVsaScriptScope : IVsaItem
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000E01 RID: 3585
		IVsaScriptScope Parent { get; }

		// Token: 0x06000E02 RID: 3586
		IVsaItem AddItem(string itemName, VsaItemType type);

		// Token: 0x06000E03 RID: 3587
		IVsaItem GetItem(string itemName);

		// Token: 0x06000E04 RID: 3588
		void RemoveItem(string itemName);

		// Token: 0x06000E05 RID: 3589
		void RemoveItem(IVsaItem item);

		// Token: 0x06000E06 RID: 3590
		int GetItemCount();

		// Token: 0x06000E07 RID: 3591
		IVsaItem GetItemAtIndex(int index);

		// Token: 0x06000E08 RID: 3592
		void RemoveItemAtIndex(int index);

		// Token: 0x06000E09 RID: 3593
		object GetObject();

		// Token: 0x06000E0A RID: 3594
		IVsaItem CreateDynamicItem(string itemName, VsaItemType type);
	}
}
