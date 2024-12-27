using System;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x02000186 RID: 390
	public abstract class Behavior
	{
		// Token: 0x06000E7E RID: 3710 RVA: 0x0003C6B0 File Offset: 0x0003B6B0
		protected Behavior()
		{
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0003C6B8 File Offset: 0x0003B6B8
		protected Behavior(bool callParentBehavior, BehaviorService behaviorService)
		{
			if (callParentBehavior && behaviorService == null)
			{
				throw new ArgumentException("behaviorService");
			}
			this.callParentBehavior = callParentBehavior;
			this.bhvSvc = behaviorService;
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0003C6DF File Offset: 0x0003B6DF
		private Behavior GetNextBehavior
		{
			get
			{
				if (this.bhvSvc != null)
				{
					return this.bhvSvc.GetNextBehavior(this);
				}
				return null;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0003C6F7 File Offset: 0x0003B6F7
		public virtual Cursor Cursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0003C6FE File Offset: 0x0003B6FE
		public virtual bool DisableAllCommands
		{
			get
			{
				return this.callParentBehavior && this.GetNextBehavior != null && this.GetNextBehavior.DisableAllCommands;
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0003C720 File Offset: 0x0003B720
		public virtual MenuCommand FindCommand(CommandID commandId)
		{
			MenuCommand menuCommand;
			try
			{
				if (this.callParentBehavior && this.GetNextBehavior != null)
				{
					menuCommand = this.GetNextBehavior.FindCommand(commandId);
				}
				else
				{
					menuCommand = null;
				}
			}
			catch
			{
				menuCommand = null;
			}
			return menuCommand;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0003C768 File Offset: 0x0003B768
		private bool GlyphIsValid(Glyph g)
		{
			return g != null && g.Behavior != null && g.Behavior != this;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0003C783 File Offset: 0x0003B783
		public virtual void OnLoseCapture(Glyph g, EventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnLoseCapture(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnLoseCapture(g, e);
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0003C7B9 File Offset: 0x0003B7B9
		public virtual bool OnMouseDoubleClick(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseDoubleClick(g, button, mouseLoc);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseDoubleClick(g, button, mouseLoc);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0003C7F3 File Offset: 0x0003B7F3
		public virtual bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseDown(g, button, mouseLoc);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseDown(g, button, mouseLoc);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0003C82D File Offset: 0x0003B82D
		public virtual bool OnMouseEnter(Glyph g)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseEnter(g);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseEnter(g);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0003C863 File Offset: 0x0003B863
		public virtual bool OnMouseHover(Glyph g, Point mouseLoc)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseHover(g, mouseLoc);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseHover(g, mouseLoc);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0003C89B File Offset: 0x0003B89B
		public virtual bool OnMouseLeave(Glyph g)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseLeave(g);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseLeave(g);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0003C8D1 File Offset: 0x0003B8D1
		public virtual bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseMove(g, button, mouseLoc);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseMove(g, button, mouseLoc);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0003C90B File Offset: 0x0003B90B
		public virtual bool OnMouseUp(Glyph g, MouseButtons button)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				return this.GetNextBehavior.OnMouseUp(g, button);
			}
			return this.GlyphIsValid(g) && g.Behavior.OnMouseUp(g, button);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0003C943 File Offset: 0x0003B943
		public virtual void OnDragDrop(Glyph g, DragEventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnDragDrop(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnDragDrop(g, e);
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0003C979 File Offset: 0x0003B979
		public virtual void OnDragEnter(Glyph g, DragEventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnDragEnter(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnDragEnter(g, e);
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0003C9AF File Offset: 0x0003B9AF
		public virtual void OnDragLeave(Glyph g, EventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnDragLeave(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnDragLeave(g, e);
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0003C9E8 File Offset: 0x0003B9E8
		public virtual void OnDragOver(Glyph g, DragEventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnDragOver(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnDragOver(g, e);
				return;
			}
			if (e.Effect != DragDropEffects.None)
			{
				e.Effect = ((Control.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
			}
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0003CA48 File Offset: 0x0003BA48
		public virtual void OnGiveFeedback(Glyph g, GiveFeedbackEventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnGiveFeedback(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnGiveFeedback(g, e);
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0003CA7E File Offset: 0x0003BA7E
		public virtual void OnQueryContinueDrag(Glyph g, QueryContinueDragEventArgs e)
		{
			if (this.callParentBehavior && this.GetNextBehavior != null)
			{
				this.GetNextBehavior.OnQueryContinueDrag(g, e);
				return;
			}
			if (this.GlyphIsValid(g))
			{
				g.Behavior.OnQueryContinueDrag(g, e);
			}
		}

		// Token: 0x04000F6F RID: 3951
		private bool callParentBehavior;

		// Token: 0x04000F70 RID: 3952
		private BehaviorService bhvSvc;
	}
}
