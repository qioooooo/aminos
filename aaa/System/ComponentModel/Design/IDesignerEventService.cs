using System;

namespace System.ComponentModel.Design
{
	// Token: 0x0200017D RID: 381
	public interface IDesignerEventService
	{
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000C34 RID: 3124
		IDesignerHost ActiveDesigner { get; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000C35 RID: 3125
		DesignerCollection Designers { get; }

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000C36 RID: 3126
		// (remove) Token: 0x06000C37 RID: 3127
		event ActiveDesignerEventHandler ActiveDesignerChanged;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000C38 RID: 3128
		// (remove) Token: 0x06000C39 RID: 3129
		event DesignerEventHandler DesignerCreated;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000C3A RID: 3130
		// (remove) Token: 0x06000C3B RID: 3131
		event DesignerEventHandler DesignerDisposed;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000C3C RID: 3132
		// (remove) Token: 0x06000C3D RID: 3133
		event EventHandler SelectionChanged;
	}
}
