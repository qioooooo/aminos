using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal sealed class ToolStripAdornerWindowService : IDisposable
	{
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

		internal Control ToolStripAdornerWindowControl
		{
			get
			{
				return this.toolStripAdornerWindow;
			}
		}

		public Graphics ToolStripAdornerWindowGraphics
		{
			get
			{
				return this.toolStripAdornerWindow.CreateGraphics();
			}
		}

		internal Adorner DropDownAdorner
		{
			get
			{
				return this.dropDownAdorner;
			}
		}

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

		public Point AdornerWindowPointToScreen(Point p)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(p.X, p.Y);
			NativeMethods.MapWindowPoints(this.toolStripAdornerWindow.Handle, IntPtr.Zero, point, 1);
			return new Point(point.x, point.y);
		}

		public Point AdornerWindowToScreen()
		{
			Point point = new Point(0, 0);
			return this.AdornerWindowPointToScreen(point);
		}

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

		public void Invalidate()
		{
			this.toolStripAdornerWindow.InvalidateAdornerWindow();
		}

		public void Invalidate(Rectangle rect)
		{
			this.toolStripAdornerWindow.InvalidateAdornerWindow(rect);
		}

		public void Invalidate(Region r)
		{
			this.toolStripAdornerWindow.InvalidateAdornerWindow(r);
		}

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

		internal void ProcessPaintMessage(Rectangle paintRect)
		{
			this.toolStripAdornerWindow.Invalidate(paintRect);
		}

		private IServiceProvider serviceProvider;

		private ToolStripAdornerWindowService.ToolStripAdornerWindow toolStripAdornerWindow;

		private BehaviorService bs;

		private Adorner dropDownAdorner;

		private ArrayList dropDownCollection;

		private IOverlayService os;

		private class ToolStripAdornerWindow : Control
		{
			internal ToolStripAdornerWindow(Control designerFrame)
			{
				this.designerFrame = designerFrame;
				this.Dock = DockStyle.Fill;
				this.AllowDrop = true;
				this.Text = "ToolStripAdornerWindow";
				base.SetStyle(ControlStyles.Opaque, true);
			}

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

			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
			}

			protected override void OnHandleDestroyed(EventArgs e)
			{
				base.OnHandleDestroyed(e);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing && this.designerFrame != null)
				{
					this.designerFrame = null;
				}
				base.Dispose(disposing);
			}

			private bool DesignerFrameValid
			{
				get
				{
					return this.designerFrame != null && !this.designerFrame.IsDisposed && this.designerFrame.IsHandleCreated;
				}
			}

			internal void InvalidateAdornerWindow()
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(true);
					this.designerFrame.Update();
				}
			}

			internal void InvalidateAdornerWindow(Region region)
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(region, true);
					this.designerFrame.Update();
				}
			}

			internal void InvalidateAdornerWindow(Rectangle rectangle)
			{
				if (this.DesignerFrameValid)
				{
					this.designerFrame.Invalidate(rectangle, true);
					this.designerFrame.Update();
				}
			}

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

			private Control designerFrame;
		}
	}
}
