using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class TabPageDesigner : PanelDesigner
	{
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return parentDesigner != null && parentDesigner.Component is TabControl;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				Control control = this.Control;
				if (control.Parent is TabControl)
				{
					selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
				}
				return selectionRules;
			}
		}

		internal void OnDragDropInternal(DragEventArgs de)
		{
			this.OnDragDrop(de);
		}

		internal void OnDragEnterInternal(DragEventArgs de)
		{
			this.OnDragEnter(de);
		}

		internal void OnDragLeaveInternal(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		internal void OnDragOverInternal(DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		internal void OnGiveFeedbackInternal(GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			this.OnSetCursor();
			Rectangle empty = Rectangle.Empty;
			return new ControlBodyGlyph(empty, Cursor.Current, this.Control, this);
		}
	}
}
