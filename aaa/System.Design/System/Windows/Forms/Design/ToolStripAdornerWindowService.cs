using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002AD RID: 685
	internal sealed class ToolStripAdornerWindowService : IDisposable
	{
		// Token: 0x060019A3 RID: 6563 RVA: 0x0008A01C File Offset: 0x0008901C
		internal ToolStripAdornerWindowService(IServiceProvider serviceProvider, Control windowFrame)
		{
			this.serviceProvider = serviceProvider;
			this.toolStripAdornerWindow = new ToolStripAdornerWindowService.ToolStripAdornerWindow(windowFrame);
			this.bs = (BehaviorService)serviceProvider.GetService(typeof(BehaviorService));
			int adornerWindowIndex = this.bs.AdornerWindowIndex;
			this.os = (IOverlayService)serviceProvider.GetService(typeof(IOverlayService));
			if (this.os != null)
			{
				this.os.InsertOverlay(this.toolStripAdornerWindow, adornerWindowIndex);
			}
			this.dropDownAdorner = new Adorner();
			int count = this.bs.Adorners.Count;
			if (count > 1)
			{
				this.bs.Adorners.Insert(count - 1, this.dropDownAdorner);
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x0008A0D7 File Offset: 0x000890D7
		internal Control ToolStripAdornerWindowControl
		{
			get
			{
				return this.toolStripAdornerWindow;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x0008A0DF File Offset: 0x000890DF
		public Graphics ToolStripAdornerWindowGraphics
		{
			get
			{
				return this.toolStripAdornerWindow.CreateGraphics();
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060019A6 RID: 6566 RVA: 0x0008A0EC File Offset: 0x000890EC
		internal Adorner DropDownAdorner
		{
			get
			{
				return this.dropDownAdorner;
			}
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0008A0F4 File Offset: 0x000890F4
		public void Dispose()
		{
			if (this.os != null)
			{
				this.os.RemoveOverlay(this.toolStripAdornerWindow);
			}
			this.toolStripAdornerWindow.Dispose();
			if (this.bs != null)
			{
				this.bs.Adorners.Remove(this.dropDownAdorner);
				this.bs = null;
			}
			if (this.dropDownAdorner != null)
			{
				this.dropDownAdorner.Glyphs.Clear();
				this.dropDownAdorner = null;
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0008A16C File Offset: 0x0008916C
		public Point AdornerWindowPointToScreen(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			NativeMethods.MapWindowPoints(this.toolStripAdornerWindow.Handle, IntPtr.Zero, point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0008A1B8 File Offset: 0x000891B8
		public Point AdornerWindowToScreen()
		{
			Point point = new Point(0, 0);
			return this.AdornerWindowPointToScreen(point);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0008A1D8 File Offset: 0x000891D8
		public Point ControlToAdornerWindow(Control c)
		{
			if (c.Parent == null)
			{
				return Point.Empty;
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = c.Left;
			point.y = c.Top;
			NativeMethods.MapWindowPoints(c.Parent.Handle, this.toolStripAdornerWindow.Handle, point, 1);
			return new Point(point.x, point.y);
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0008A240 File Offset: 0x00089240
		public void Invalidate()
		{
			this.toolStripAdornerWindow.InvalidateAdornerWindow();
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0008A24D File Offset: 0x0008924D
		public void Invalidate(Rectangle rect)
		{
			this.toolStripAdornerWindow.InvalidateAdornerWindow(rect);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0008A25B File Offset: 0x0008925B
		public void Invalidate(Region r)
		{
			this.toolStripAdornerWindow.InvalidateAdornerWindow(r);
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x0008A269 File Offset: 0x00089269
		// (set) Token: 0x060019AF RID: 6575 RVA: 0x0008A271 File Offset: 0x00089271
		internal ArrayList DropDowns
		{
			get
			{
				return this.dropDownCollection;
			}
			set
			{
				if (this.dropDownCollection == null)
				{
					this.dropDownCollection = new ArrayList();
				}
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0008A286 File Offset: 0x00089286
		internal void ProcessPaintMessage(Rectangle paintRect)
		{
			this.toolStripAdornerWindow.Invalidate(paintRect);
		}

		// Token: 0x040014AE RID: 5294
		private IServiceProvider serviceProvider;

		// Token: 0x040014AF RID: 5295
		private ToolStripAdornerWindowService.ToolStripAdornerWindow toolStripAdornerWindow;

		// Token: 0x040014B0 RID: 5296
		private BehaviorService bs;

		// Token: 0x040014B1 RID: 5297
		private Adorner dropDownAdorner;

		// Token: 0x040014B2 RID: 5298
		private ArrayList dropDownCollection;

		// Token: 0x040014B3 RID: 5299
		private IOverlayService os;

		// Token: 0x020002AE RID: 686
		private class ToolStripAdornerWindow : Control
		{
			// Token: 0x060019B1 RID: 6577 RVA: 0x0008A294 File Offset: 0x00089294
			internal ToolStripAdornerWindow(Control designerFrame)
			{
				this.designerFrame = designerFrame;
				this.Dock = DockStyle.Fill;
				this.AllowDrop = true;
				this.Text = "ToolStripAdornerWindow";
				base.SetStyle(ControlStyles.Opaque, true);
			}

			// Token: 0x17000467 RID: 1127
			// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0008A2C4 File Offset: 0x000892C4
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.Style &= -100663297;
					createParams.ExStyle |= 32;
					return createParams;
				}
			}

			// Token: 0x060019B3 RID: 6579 RVA: 0x0008A2FA File Offset: 0x000892FA
			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
			}

			// Token: 0x060019B4 RID: 6580 RVA: 0x0008A303 File Offset: 0x00089303
			protected override void OnHandleDestroyed(EventArgs e)
			{
				base.OnHandleDestroyed(e);
			}

			// Token: 0x060019B5 RID: 6581 RVA: 0x0008A30C File Offset: 0x0008930C
			protected override void Dispose(bool disposing)
			{
				if (disposing && this.designerFrame != null)
				{
					this.designerFrame = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x17000468 RID: 1128
			// (get) Token: 0x060019B6 RID: 6582 RVA: 0x0008A327 File Offset: 0x00089327
			private bool DesignerFrameValid
			{
				get
				{
					return this.designerFrame != null && !this.designerFrame.IsDisposed && this.designerFrame.IsHandleCreated;
				}
			}

			// Token: 0x060019B7 RID: 6583 RVA: 0x0008A34E File Offset: 0x0008934E
			internal void InvalidateAdornerWindow()
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(true);
					this.designerFrame.Update();
				}
			}

			// Token: 0x060019B8 RID: 6584 RVA: 0x0008A36F File Offset: 0x0008936F
			internal void InvalidateAdornerWindow(Region region)
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(region, true);
					this.designerFrame.Update();
				}
			}

			// Token: 0x060019B9 RID: 6585 RVA: 0x0008A391 File Offset: 0x00089391
			internal void InvalidateAdornerWindow(Rectangle rectangle)
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(rectangle, true);
					this.designerFrame.Update();
				}
			}

			// Token: 0x060019BA RID: 6586 RVA: 0x0008A3B4 File Offset: 0x000893B4
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 132)
				{
					m.Result = (IntPtr)(-1);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x040014B4 RID: 5300
			private Control designerFrame;
		}
	}
}
