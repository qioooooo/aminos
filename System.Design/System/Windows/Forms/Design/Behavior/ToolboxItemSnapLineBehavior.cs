using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class ToolboxItemSnapLineBehavior : Behavior
	{
		public ToolboxItemSnapLineBehavior(IServiceProvider serviceProvider, BehaviorService behaviorService)
		{
			this.serviceProvider = serviceProvider;
			this.behaviorService = behaviorService;
			this.designer = null;
			this.isPushed = false;
			this.lastRectangle = Rectangle.Empty;
			this.lastOffset = Point.Empty;
			this.statusCommandUI = new StatusCommandUI(serviceProvider);
			this.targetAllowsDragBox = true;
			this.targetAllowsSnapLines = true;
		}

		public ToolboxItemSnapLineBehavior(IServiceProvider serviceProvider, BehaviorService behaviorService, ControlDesigner controlDesigner)
			: this(serviceProvider, behaviorService)
		{
			this.designer = controlDesigner;
			if (controlDesigner != null && !controlDesigner.ParticipatesWithSnapLines)
			{
				this.targetAllowsSnapLines = false;
			}
		}

		public ToolboxItemSnapLineBehavior(IServiceProvider serviceProvider, BehaviorService behaviorService, ControlDesigner controlDesigner, bool allowDragBox)
			: this(serviceProvider, behaviorService, controlDesigner)
		{
			this.designer = controlDesigner;
			this.targetAllowsDragBox = allowDragBox;
		}

		public bool IsPushed
		{
			get
			{
				return this.isPushed;
			}
			set
			{
				this.isPushed = value;
				if (this.isPushed)
				{
					if (this.dragManager == null)
					{
						this.dragManager = new DragAssistanceManager(this.serviceProvider);
						return;
					}
				}
				else
				{
					if (!this.lastRectangle.IsEmpty)
					{
						this.behaviorService.Invalidate(this.lastRectangle);
					}
					this.lastOffset = Point.Empty;
					this.lastRectangle = Rectangle.Empty;
					if (this.dragManager != null)
					{
						this.dragManager.OnMouseUp();
						this.dragManager = null;
					}
				}
			}
		}

		private ToolboxSnapDragDropEventArgs CreateToolboxSnapArgs(DragEventArgs e, Point mouseLoc)
		{
			ToolboxSnapDragDropEventArgs.SnapDirection snapDirection = ToolboxSnapDragDropEventArgs.SnapDirection.None;
			Point empty = Point.Empty;
			bool flag = false;
			bool flag2 = false;
			if (this.dragManager != null)
			{
				DragAssistanceManager.Line[] recentLines = this.dragManager.GetRecentLines();
				foreach (DragAssistanceManager.Line line in recentLines)
				{
					if (line.LineType == DragAssistanceManager.LineType.Standard)
					{
						if (!flag && line.x1 == line.x2)
						{
							if (line.x1 == this.lastRectangle.Left)
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Left;
								empty.X = this.lastRectangle.Left - mouseLoc.X;
							}
							else
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Right;
								empty.X = this.lastRectangle.Right - mouseLoc.X;
							}
							flag = true;
						}
						else if (!flag2 && line.y1 == line.y2)
						{
							if (line.y1 == this.lastRectangle.Top)
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Top;
								empty.Y = this.lastRectangle.Top - mouseLoc.Y;
							}
							else if (line.y1 == this.lastRectangle.Bottom)
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Bottom;
								empty.Y = this.lastRectangle.Bottom - mouseLoc.Y;
							}
							flag2 = true;
						}
					}
					else if (line.LineType == DragAssistanceManager.LineType.Margin || line.LineType == DragAssistanceManager.LineType.Padding)
					{
						if (!flag2 && line.x1 == line.x2)
						{
							if (Math.Max(line.y1, line.y2) <= this.lastRectangle.Top)
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Top;
								empty.Y = this.lastRectangle.Top - mouseLoc.Y;
							}
							else
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Bottom;
								empty.Y = this.lastRectangle.Bottom - mouseLoc.Y;
							}
							flag2 = true;
						}
						else if (!flag && line.y1 == line.y2)
						{
							if (Math.Max(line.x1, line.x2) <= this.lastRectangle.Left)
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Left;
								empty.X = this.lastRectangle.Left - mouseLoc.X;
							}
							else
							{
								snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Right;
								empty.X = this.lastRectangle.Right - mouseLoc.X;
							}
							flag = true;
						}
					}
					if (flag && flag2)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Left;
				empty.X = this.lastRectangle.Left - mouseLoc.X;
			}
			if (!flag2)
			{
				snapDirection |= ToolboxSnapDragDropEventArgs.SnapDirection.Top;
				empty.Y = this.lastRectangle.Top - mouseLoc.Y;
			}
			return new ToolboxSnapDragDropEventArgs(snapDirection, empty, e);
		}

		private SnapLine[] GenerateNewToolSnapLines(Rectangle r)
		{
			return new SnapLine[]
			{
				new SnapLine(SnapLineType.Left, r.Left),
				new SnapLine(SnapLineType.Right, r.Right),
				new SnapLine(SnapLineType.Bottom, r.Bottom),
				new SnapLine(SnapLineType.Top, r.Top),
				new SnapLine(SnapLineType.Horizontal, r.Top - 4, "Margin.Top", SnapLinePriority.Always),
				new SnapLine(SnapLineType.Horizontal, r.Bottom + 3, "Margin.Bottom", SnapLinePriority.Always),
				new SnapLine(SnapLineType.Vertical, r.Left - 4, "Margin.Left", SnapLinePriority.Always),
				new SnapLine(SnapLineType.Vertical, r.Right + 3, "Margin.Right", SnapLinePriority.Always)
			};
		}

		public override void OnDragDrop(Glyph g, DragEventArgs e)
		{
			this.behaviorService.PopBehavior(this);
			try
			{
				Point point = this.behaviorService.AdornerWindowToScreen();
				ToolboxSnapDragDropEventArgs toolboxSnapDragDropEventArgs = this.CreateToolboxSnapArgs(e, new Point(e.X - point.X, e.Y - point.Y));
				base.OnDragDrop(g, toolboxSnapDragDropEventArgs);
			}
			finally
			{
				this.IsPushed = false;
			}
		}

		public void OnBeginDrag()
		{
			Adorner adorner = null;
			SelectionManager selectionManager = (SelectionManager)this.serviceProvider.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				adorner = selectionManager.BodyGlyphAdorner;
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in adorner.Glyphs)
			{
				ControlBodyGlyph controlBodyGlyph = (ControlBodyGlyph)obj;
				Control control = controlBodyGlyph.RelatedComponent as Control;
				if (control != null && !control.AllowDrop)
				{
					arrayList.Add(controlBodyGlyph);
				}
			}
			foreach (object obj2 in arrayList)
			{
				Glyph glyph = (Glyph)obj2;
				adorner.Glyphs.Remove(glyph);
			}
		}

		public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
		{
			bool flag = Control.ModifierKeys == Keys.Alt;
			if (flag && this.dragManager != null)
			{
				this.dragManager.EraseSnapLines();
			}
			bool flag2 = base.OnMouseMove(g, button, mouseLoc);
			Rectangle rectangle = new Rectangle(mouseLoc.X - DesignerUtils.BOXIMAGESIZE / 2, mouseLoc.Y - DesignerUtils.BOXIMAGESIZE / 2, DesignerUtils.BOXIMAGESIZE, DesignerUtils.BOXIMAGESIZE);
			if (rectangle != this.lastRectangle)
			{
				if (this.dragManager != null && this.targetAllowsSnapLines && !flag)
				{
					this.lastOffset = this.dragManager.OnMouseMove(rectangle, this.GenerateNewToolSnapLines(rectangle));
					rectangle.Offset(this.lastOffset.X, this.lastOffset.Y);
				}
				if (!this.lastRectangle.IsEmpty)
				{
					using (Region region = new Region(this.lastRectangle))
					{
						region.Exclude(rectangle);
						this.behaviorService.Invalidate(region);
					}
				}
				if (this.targetAllowsDragBox)
				{
					using (Graphics adornerWindowGraphics = this.behaviorService.AdornerWindowGraphics)
					{
						adornerWindowGraphics.DrawImage(DesignerUtils.BoxImage, rectangle.Location);
					}
				}
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					Control control = designerHost.RootComponent as Control;
					if (control != null)
					{
						Point point = this.behaviorService.MapAdornerWindowPoint(control.Handle, new Point(0, 0));
						Rectangle rectangle2 = new Rectangle(rectangle.X - point.X, rectangle.Y - point.Y, 0, 0);
						if (this.statusCommandUI != null)
						{
							this.statusCommandUI.SetStatusInformation(rectangle2);
						}
					}
				}
				if (this.dragManager != null && this.targetAllowsSnapLines && !flag)
				{
					this.dragManager.RenderSnapLinesInternal();
				}
				this.lastRectangle = rectangle;
			}
			return flag2;
		}

		private IServiceProvider serviceProvider;

		private BehaviorService behaviorService;

		private ControlDesigner designer;

		private bool isPushed;

		private Rectangle lastRectangle;

		private Point lastOffset;

		private DragAssistanceManager dragManager;

		private bool targetAllowsSnapLines;

		private StatusCommandUI statusCommandUI;

		private bool targetAllowsDragBox;
	}
}
