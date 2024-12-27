using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.Drawing.Design
{
	// Token: 0x020000F6 RID: 246
	[Guid("4BACD258-DE64-4048-BC4E-FEDBEF9ACB76")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IToolboxService
	{
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000D8A RID: 3466
		CategoryNameCollection CategoryNames { get; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000D8B RID: 3467
		// (set) Token: 0x06000D8C RID: 3468
		string SelectedCategory { get; set; }

		// Token: 0x06000D8D RID: 3469
		void AddCreator(ToolboxItemCreatorCallback creator, string format);

		// Token: 0x06000D8E RID: 3470
		void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host);

		// Token: 0x06000D8F RID: 3471
		void AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host);

		// Token: 0x06000D90 RID: 3472
		void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host);

		// Token: 0x06000D91 RID: 3473
		void AddToolboxItem(ToolboxItem toolboxItem);

		// Token: 0x06000D92 RID: 3474
		void AddToolboxItem(ToolboxItem toolboxItem, string category);

		// Token: 0x06000D93 RID: 3475
		ToolboxItem DeserializeToolboxItem(object serializedObject);

		// Token: 0x06000D94 RID: 3476
		ToolboxItem DeserializeToolboxItem(object serializedObject, IDesignerHost host);

		// Token: 0x06000D95 RID: 3477
		ToolboxItem GetSelectedToolboxItem();

		// Token: 0x06000D96 RID: 3478
		ToolboxItem GetSelectedToolboxItem(IDesignerHost host);

		// Token: 0x06000D97 RID: 3479
		ToolboxItemCollection GetToolboxItems();

		// Token: 0x06000D98 RID: 3480
		ToolboxItemCollection GetToolboxItems(IDesignerHost host);

		// Token: 0x06000D99 RID: 3481
		ToolboxItemCollection GetToolboxItems(string category);

		// Token: 0x06000D9A RID: 3482
		ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host);

		// Token: 0x06000D9B RID: 3483
		bool IsSupported(object serializedObject, IDesignerHost host);

		// Token: 0x06000D9C RID: 3484
		bool IsSupported(object serializedObject, ICollection filterAttributes);

		// Token: 0x06000D9D RID: 3485
		bool IsToolboxItem(object serializedObject);

		// Token: 0x06000D9E RID: 3486
		bool IsToolboxItem(object serializedObject, IDesignerHost host);

		// Token: 0x06000D9F RID: 3487
		void Refresh();

		// Token: 0x06000DA0 RID: 3488
		void RemoveCreator(string format);

		// Token: 0x06000DA1 RID: 3489
		void RemoveCreator(string format, IDesignerHost host);

		// Token: 0x06000DA2 RID: 3490
		void RemoveToolboxItem(ToolboxItem toolboxItem);

		// Token: 0x06000DA3 RID: 3491
		void RemoveToolboxItem(ToolboxItem toolboxItem, string category);

		// Token: 0x06000DA4 RID: 3492
		void SelectedToolboxItemUsed();

		// Token: 0x06000DA5 RID: 3493
		object SerializeToolboxItem(ToolboxItem toolboxItem);

		// Token: 0x06000DA6 RID: 3494
		bool SetCursor();

		// Token: 0x06000DA7 RID: 3495
		void SetSelectedToolboxItem(ToolboxItem toolboxItem);
	}
}
