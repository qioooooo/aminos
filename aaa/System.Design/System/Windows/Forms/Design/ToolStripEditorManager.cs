using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002C2 RID: 706
	internal class ToolStripEditorManager
	{
		// Token: 0x06001AC1 RID: 6849 RVA: 0x00092180 File Offset: 0x00091180
		public ToolStripEditorManager(IComponent comp)
		{
			this.comp = comp;
			this.behaviorService = (BehaviorService)comp.Site.GetService(typeof(BehaviorService));
			this.designerHost = (IDesignerHost)comp.Site.GetService(typeof(IDesignerHost));
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000921E8 File Offset: 0x000911E8
		internal void ActivateEditor(ToolStripItem item, bool clicked)
		{
			if (item != this.currentItem)
			{
				if (this.editor != null)
				{
					this.behaviorService.AdornerWindowControl.Controls.Remove(this.editor);
					this.behaviorService.Invalidate(this.editor.Bounds);
					this.editorUI = null;
					this.editor = null;
					this.currentItem = null;
					this.itemDesigner.IsEditorActive = false;
					if (this.currentItem != null)
					{
						this.currentItem = null;
					}
				}
				if (item != null)
				{
					this.currentItem = item;
					if (this.designerHost != null)
					{
						this.itemDesigner = (ToolStripItemDesigner)this.designerHost.GetDesigner(this.currentItem);
					}
					this.editorUI = this.itemDesigner.Editor;
					if (this.editorUI != null)
					{
						this.itemDesigner.IsEditorActive = true;
						this.editor = new ToolStripEditorManager.ToolStripEditorControl(this.editorUI.EditorToolStrip, this.editorUI.Bounds);
						this.behaviorService.AdornerWindowControl.Controls.Add(this.editor);
						this.lastKnownEditorBounds = this.editor.Bounds;
						this.editor.BringToFront();
						this.editorUI.ignoreFirstKeyUp = true;
						this.editorUI.FocusEditor(this.currentItem);
					}
				}
			}
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00092338 File Offset: 0x00091338
		internal void CloseManager()
		{
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0009233A File Offset: 0x0009133A
		private void OnEditorResize(object sender, EventArgs e)
		{
			this.behaviorService.Invalidate(this.lastKnownEditorBounds);
			if (this.editor != null)
			{
				this.lastKnownEditorBounds = this.editor.Bounds;
			}
		}

		// Token: 0x04001533 RID: 5427
		private BehaviorService behaviorService;

		// Token: 0x04001534 RID: 5428
		private IDesignerHost designerHost;

		// Token: 0x04001535 RID: 5429
		private IComponent comp;

		// Token: 0x04001536 RID: 5430
		private Rectangle lastKnownEditorBounds = Rectangle.Empty;

		// Token: 0x04001537 RID: 5431
		private ToolStripEditorManager.ToolStripEditorControl editor;

		// Token: 0x04001538 RID: 5432
		private ToolStripTemplateNode editorUI;

		// Token: 0x04001539 RID: 5433
		private ToolStripItem currentItem;

		// Token: 0x0400153A RID: 5434
		private ToolStripItemDesigner itemDesigner;

		// Token: 0x020002C3 RID: 707
		private class ToolStripEditorControl : Panel
		{
			// Token: 0x06001AC5 RID: 6853 RVA: 0x00092368 File Offset: 0x00091368
			public ToolStripEditorControl(Control editorToolStrip, Rectangle bounds)
			{
				this.wrappedEditor = editorToolStrip;
				this.bounds = bounds;
				this.wrappedEditor.Resize += this.OnWrappedEditorResize;
				base.Controls.Add(editorToolStrip);
				base.Location = new Point(bounds.X, bounds.Y);
				this.Text = "InSituEditorWrapper";
				this.UpdateSize();
			}

			// Token: 0x06001AC6 RID: 6854 RVA: 0x000923D6 File Offset: 0x000913D6
			private void OnWrappedEditorResize(object sender, EventArgs e)
			{
			}

			// Token: 0x06001AC7 RID: 6855 RVA: 0x000923D8 File Offset: 0x000913D8
			private void UpdateSize()
			{
				base.Size = new Size(this.wrappedEditor.Size.Width, this.wrappedEditor.Size.Height);
			}

			// Token: 0x0400153B RID: 5435
			private Control wrappedEditor;

			// Token: 0x0400153C RID: 5436
			private Rectangle bounds;
		}
	}
}
