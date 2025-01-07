using System;
using System.Collections;

namespace System.Windows.Forms.Design.Behavior
{
	public class BehaviorDragDropEventArgs : EventArgs
	{
		public BehaviorDragDropEventArgs(ICollection dragComponents)
		{
			this.dragComponents = dragComponents;
		}

		public ICollection DragComponents
		{
			get
			{
				return this.dragComponents;
			}
		}

		private ICollection dragComponents;
	}
}
