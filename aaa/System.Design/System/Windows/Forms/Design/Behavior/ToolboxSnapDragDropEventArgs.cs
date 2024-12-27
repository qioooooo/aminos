using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x0200030E RID: 782
	internal sealed class ToolboxSnapDragDropEventArgs : DragEventArgs
	{
		// Token: 0x06001DD5 RID: 7637 RVA: 0x000AA2AC File Offset: 0x000A92AC
		public ToolboxSnapDragDropEventArgs(ToolboxSnapDragDropEventArgs.SnapDirection snapDirections, Point offset, DragEventArgs origArgs)
			: base(origArgs.Data, origArgs.KeyState, origArgs.X, origArgs.Y, origArgs.AllowedEffect, origArgs.Effect)
		{
			this.snapDirections = snapDirections;
			this.offset = offset;
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x000AA2E6 File Offset: 0x000A92E6
		public ToolboxSnapDragDropEventArgs.SnapDirection SnapDirections
		{
			get
			{
				return this.snapDirections;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x000AA2EE File Offset: 0x000A92EE
		public Point Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x0400170A RID: 5898
		private ToolboxSnapDragDropEventArgs.SnapDirection snapDirections;

		// Token: 0x0400170B RID: 5899
		private Point offset;

		// Token: 0x0200030F RID: 783
		[Flags]
		public enum SnapDirection
		{
			// Token: 0x0400170D RID: 5901
			None = 0,
			// Token: 0x0400170E RID: 5902
			Top = 1,
			// Token: 0x0400170F RID: 5903
			Bottom = 2,
			// Token: 0x04001710 RID: 5904
			Right = 4,
			// Token: 0x04001711 RID: 5905
			Left = 8
		}
	}
}
