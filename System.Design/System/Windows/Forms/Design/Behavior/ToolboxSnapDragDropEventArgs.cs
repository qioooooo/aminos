using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class ToolboxSnapDragDropEventArgs : DragEventArgs
	{
		public ToolboxSnapDragDropEventArgs(ToolboxSnapDragDropEventArgs.SnapDirection snapDirections, Point offset, DragEventArgs origArgs)
			: base(origArgs.Data, origArgs.KeyState, origArgs.X, origArgs.Y, origArgs.AllowedEffect, origArgs.Effect)
		{
			this.snapDirections = snapDirections;
			this.offset = offset;
		}

		public ToolboxSnapDragDropEventArgs.SnapDirection SnapDirections
		{
			get
			{
				return this.snapDirections;
			}
		}

		public Point Offset
		{
			get
			{
				return this.offset;
			}
		}

		private ToolboxSnapDragDropEventArgs.SnapDirection snapDirections;

		private Point offset;

		[Flags]
		public enum SnapDirection
		{
			None = 0,
			Top = 1,
			Bottom = 2,
			Right = 4,
			Left = 8
		}
	}
}
