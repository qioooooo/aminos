using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002A5 RID: 677
	internal class TabPageDesigner : PanelDesigner
	{
		// Token: 0x06001976 RID: 6518 RVA: 0x000896A1 File Offset: 0x000886A1
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return parentDesigner != null && parentDesigner.Component is TabControl;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001977 RID: 6519 RVA: 0x000896B8 File Offset: 0x000886B8
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

		// Token: 0x06001978 RID: 6520 RVA: 0x000896E6 File Offset: 0x000886E6
		internal void OnDragDropInternal(DragEventArgs de)
		{
			this.OnDragDrop(de);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x000896EF File Offset: 0x000886EF
		internal void OnDragEnterInternal(DragEventArgs de)
		{
			this.OnDragEnter(de);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x000896F8 File Offset: 0x000886F8
		internal void OnDragLeaveInternal(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00089701 File Offset: 0x00088701
		internal void OnDragOverInternal(DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0008970A File Offset: 0x0008870A
		internal void OnGiveFeedbackInternal(GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00089714 File Offset: 0x00088714
		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			this.OnSetCursor();
			Rectangle empty = Rectangle.Empty;
			return new ControlBodyGlyph(empty, Cursor.Current, this.Control, this);
		}
	}
}
