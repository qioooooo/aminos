using System;
using System.Collections;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002E5 RID: 741
	public class BehaviorDragDropEventArgs : EventArgs
	{
		// Token: 0x06001C76 RID: 7286 RVA: 0x0009F94B File Offset: 0x0009E94B
		public BehaviorDragDropEventArgs(ICollection dragComponents)
		{
			this.dragComponents = dragComponents;
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x0009F95A File Offset: 0x0009E95A
		public ICollection DragComponents
		{
			get
			{
				return this.dragComponents;
			}
		}

		// Token: 0x040015E6 RID: 5606
		private ICollection dragComponents;
	}
}
