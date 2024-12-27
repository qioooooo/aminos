using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x02000301 RID: 769
	internal class ResizeBehavior : Behavior
	{
		// Token: 0x06001D8E RID: 7566 RVA: 0x000A668C File Offset: 0x000A568C
		internal ResizeBehavior(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
			this.dragging = false;
			this.pushedBehavior = false;
			this.lastSnapOffset = Point.Empty;
			this.didSnap = false;
			this.statusCommandUI = new StatusCommandUI(serviceProvider);
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x000A66DD File Offset: 0x000A56DD
		private BehaviorService BehaviorService
		{
			get
			{
				if (this.behaviorService == null)
				{
					this.behaviorService = (BehaviorService)this.serviceProvider.GetService(typeof(BehaviorService));
				}
				return this.behaviorService;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x000A670D File Offset: 0x000A570D
		public override Cursor Cursor
		{
			get
			{
				return this.cursor;
			}
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000A6718 File Offset: 0x000A5718
		private Rectangle AdjustToGrid(Rectangle controlBounds, SelectionRules rules)
		{
			Rectangle rectangle = controlBounds;
			if ((rules & SelectionRules.RightSizeable) != SelectionRules.None)
			{
				int num = controlBounds.Right % this.parentGridSize.Width;
				if (num > this.parentGridSize.Width / 2)
				{
					rectangle.Width += this.parentGridSize.Width - num;
				}
				else
				{
					rectangle.Width -= num;
				}
			}
			else if ((rules & SelectionRules.LeftSizeable) != SelectionRules.None)
			{
				int num2 = controlBounds.Left % this.parentGridSize.Width;
				if (num2 > this.parentGridSize.Width / 2)
				{
					rectangle.X += this.parentGridSize.Width - num2;
					rectangle.Width -= this.parentGridSize.Width - num2;
				}
				else
				{
					rectangle.X -= num2;
					rectangle.Width += num2;
				}
			}
			if ((rules & SelectionRules.BottomSizeable) != SelectionRules.None)
			{
				int num3 = controlBounds.Bottom % this.parentGridSize.Height;
				if (num3 > this.parentGridSize.Height / 2)
				{
					rectangle.Height += this.parentGridSize.Height - num3;
				}
				else
				{
					rectangle.Height -= num3;
				}
			}
			else if ((rules & SelectionRules.TopSizeable) != SelectionRules.None)
			{
				int num4 = controlBounds.Top % this.parentGridSize.Height;
				if (num4 > this.parentGridSize.Height / 2)
				{
					rectangle.Y += this.parentGridSize.Height - num4;
					rectangle.Height -= this.parentGridSize.Height - num4;
				}
				else
				{
					rectangle.Y -= num4;
					rectangle.Height += num4;
				}
			}
			rectangle.Width = Math.Max(rectangle.Width, this.parentGridSize.Width);
			rectangle.Height = Math.Max(rectangle.Height, this.parentGridSize.Height);
			return rectangle;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000A6924 File Offset: 0x000A5924
		private SnapLine[] GenerateSnapLines(SelectionRules rules, Point loc)
		{
			ArrayList arrayList = new ArrayList(2);
			if ((rules & SelectionRules.BottomSizeable) != SelectionRules.None)
			{
				arrayList.Add(new SnapLine(SnapLineType.Bottom, loc.Y - 1));
				if (this.primaryControl != null)
				{
					arrayList.Add(new SnapLine(SnapLineType.Horizontal, loc.Y + this.primaryControl.Margin.Bottom, "Margin.Bottom", SnapLinePriority.Always));
				}
			}
			else if ((rules & SelectionRules.TopSizeable) != SelectionRules.None)
			{
				arrayList.Add(new SnapLine(SnapLineType.Top, loc.Y));
				if (this.primaryControl != null)
				{
					arrayList.Add(new SnapLine(SnapLineType.Horizontal, loc.Y - this.primaryControl.Margin.Top, "Margin.Top", SnapLinePriority.Always));
				}
			}
			if ((rules & SelectionRules.RightSizeable) != SelectionRules.None)
			{
				arrayList.Add(new SnapLine(SnapLineType.Right, loc.X - 1));
				if (this.primaryControl != null)
				{
					arrayList.Add(new SnapLine(SnapLineType.Vertical, loc.X + this.primaryControl.Margin.Right, "Margin.Right", SnapLinePriority.Always));
				}
			}
			else if ((rules & SelectionRules.LeftSizeable) != SelectionRules.None)
			{
				arrayList.Add(new SnapLine(SnapLineType.Left, loc.X));
				if (this.primaryControl != null)
				{
					arrayList.Add(new SnapLine(SnapLineType.Vertical, loc.X - this.primaryControl.Margin.Left, "Margin.Left", SnapLinePriority.Always));
				}
			}
			SnapLine[] array = new SnapLine[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000A6A98 File Offset: 0x000A5A98
		private void InitiateResize()
		{
			bool useSnapLines = this.BehaviorService.UseSnapLines;
			ArrayList arrayList = new ArrayList();
			IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			for (int i = 0; i < this.resizeComponents.Length; i++)
			{
				this.resizeComponents[i].resizeBounds = ((Control)this.resizeComponents[i].resizeControl).Bounds;
				if (useSnapLines)
				{
					arrayList.Add(this.resizeComponents[i].resizeControl);
				}
				if (designerHost != null)
				{
					ControlDesigner controlDesigner = designerHost.GetDesigner(this.resizeComponents[i].resizeControl as Component) as ControlDesigner;
					if (controlDesigner != null)
					{
						this.resizeComponents[i].resizeRules = controlDesigner.SelectionRules;
					}
					else
					{
						this.resizeComponents[i].resizeRules = SelectionRules.None;
					}
				}
			}
			foreach (Adorner adorner in this.BehaviorService.Adorners)
			{
				adorner.Enabled = false;
			}
			IDesignerHost designerHost2 = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			if (designerHost2 != null)
			{
				string text2;
				if (this.resizeComponents.Length == 1)
				{
					string text = TypeDescriptor.GetComponentName(this.resizeComponents[0].resizeControl);
					if (text == null || text.Length == 0)
					{
						text = this.resizeComponents[0].resizeControl.GetType().Name;
					}
					text2 = SR.GetString("BehaviorServiceResizeControl", new object[] { text });
				}
				else
				{
					text2 = SR.GetString("BehaviorServiceResizeControls", new object[] { this.resizeComponents.Length });
				}
				this.resizeTransaction = designerHost2.CreateTransaction(text2);
			}
			this.initialResize = true;
			if (useSnapLines)
			{
				this.dragManager = new DragAssistanceManager(this.serviceProvider, arrayList, true);
			}
			else if (this.resizeComponents.Length > 0)
			{
				Control control = this.resizeComponents[0].resizeControl as Control;
				if (control != null && control.Parent != null)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control.Parent)["SnapToGrid"];
					if (propertyDescriptor != null && (bool)propertyDescriptor.GetValue(control.Parent))
					{
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control.Parent)["GridSize"];
						if (propertyDescriptor2 != null)
						{
							this.parentGridSize = (Size)propertyDescriptor2.GetValue(control.Parent);
							this.parentLocation = this.behaviorService.ControlToAdornerWindow(control);
							this.parentLocation.X = this.parentLocation.X - control.Location.X;
							this.parentLocation.Y = this.parentLocation.Y - control.Location.Y;
						}
					}
				}
			}
			this.captureLost = false;
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000A6DC8 File Offset: 0x000A5DC8
		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (button != MouseButtons.Left)
			{
				return this.pushedBehavior;
			}
			this.targetResizeRules = SelectionRules.None;
			SelectionGlyphBase selectionGlyphBase = g as SelectionGlyphBase;
			if (selectionGlyphBase != null)
			{
				this.targetResizeRules = selectionGlyphBase.SelectionRules;
				this.cursor = selectionGlyphBase.HitTestCursor;
			}
			if (this.targetResizeRules == SelectionRules.None)
			{
				return false;
			}
			ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
			if (selectionService == null)
			{
				return false;
			}
			this.initialPoint = mouseLoc;
			this.lastMouseLoc = mouseLoc;
			this.primaryControl = selectionService.PrimarySelection as Control;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in selectionService.GetSelectedComponents())
			{
				if (obj is Control)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj)["Locked"];
					if (propertyDescriptor == null || !(bool)propertyDescriptor.GetValue(obj))
					{
						arrayList.Add(obj);
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return false;
			}
			this.resizeComponents = new ResizeBehavior.ResizeComponent[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				this.resizeComponents[i].resizeControl = arrayList[i];
			}
			this.pushedBehavior = true;
			this.BehaviorService.PushCaptureBehavior(this);
			return false;
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000A6F38 File Offset: 0x000A5F38
		public override void OnLoseCapture(Glyph g, EventArgs e)
		{
			this.captureLost = true;
			if (this.pushedBehavior)
			{
				this.pushedBehavior = false;
				if (this.BehaviorService != null)
				{
					if (this.dragging)
					{
						this.dragging = false;
						int num = 0;
						while (!this.captureLost && num < this.resizeComponents.Length)
						{
							Control control = this.resizeComponents[num].resizeControl as Control;
							Rectangle rectangle = this.BehaviorService.ControlRectInAdornerWindow(control);
							if (!rectangle.IsEmpty)
							{
								using (Graphics adornerWindowGraphics = this.BehaviorService.AdornerWindowGraphics)
								{
									adornerWindowGraphics.SetClip(rectangle);
									using (Region region = new Region(rectangle))
									{
										region.Exclude(Rectangle.Inflate(rectangle, -2, -2));
										this.BehaviorService.Invalidate(region);
									}
									adornerWindowGraphics.ResetClip();
								}
							}
							num++;
						}
						foreach (Adorner adorner in this.BehaviorService.Adorners)
						{
							adorner.Enabled = true;
						}
					}
					this.BehaviorService.PopBehavior(this);
					if (this.lastResizeRegion != null)
					{
						this.BehaviorService.Invalidate(this.lastResizeRegion);
						this.lastResizeRegion.Dispose();
						this.lastResizeRegion = null;
					}
				}
			}
			if (this.resizeTransaction != null)
			{
				DesignerTransaction designerTransaction = this.resizeTransaction;
				this.resizeTransaction = null;
				using (designerTransaction)
				{
					designerTransaction.Cancel();
				}
			}
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000A7104 File Offset: 0x000A6104
		internal static int AdjustPixelsForIntegralHeight(Control control, int pixelsMoved)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["IntegralHeight"];
			if (propertyDescriptor != null)
			{
				object value = propertyDescriptor.GetValue(control);
				if (value is bool && (bool)value)
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control)["ItemHeight"];
					if (propertyDescriptor2 != null)
					{
						if (pixelsMoved >= 0)
						{
							return pixelsMoved - pixelsMoved % (int)propertyDescriptor2.GetValue(control);
						}
						int num = (int)propertyDescriptor2.GetValue(control);
						return pixelsMoved - (num - Math.Abs(pixelsMoved) % num);
					}
				}
			}
			return pixelsMoved;
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000A7184 File Offset: 0x000A6184
		public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (!this.pushedBehavior)
			{
				return false;
			}
			bool flag = Control.ModifierKeys == Keys.Alt;
			if (flag && this.dragManager != null)
			{
				this.dragManager.EraseSnapLines();
			}
			if (!flag && mouseLoc.Equals(this.lastMouseLoc))
			{
				return true;
			}
			if (this.lastMouseAbs != null)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(mouseLoc.X, mouseLoc.Y);
				UnsafeNativeMethods.ClientToScreen(new HandleRef(this, this.behaviorService.AdornerWindowControl.Handle), point);
				if (point.x == this.lastMouseAbs.x && point.y == this.lastMouseAbs.y)
				{
					return true;
				}
			}
			if (!this.dragging)
			{
				if (Math.Abs(this.initialPoint.X - mouseLoc.X) <= DesignerUtils.MinDragSize.Width / 2 && Math.Abs(this.initialPoint.Y - mouseLoc.Y) <= DesignerUtils.MinDragSize.Height / 2)
				{
					return false;
				}
				this.InitiateResize();
				this.dragging = true;
			}
			if (this.resizeComponents == null || this.resizeComponents.Length == 0)
			{
				return false;
			}
			PropertyDescriptor propertyDescriptor = null;
			PropertyDescriptor propertyDescriptor2 = null;
			PropertyDescriptor propertyDescriptor3 = null;
			PropertyDescriptor propertyDescriptor4 = null;
			if (this.initialResize)
			{
				propertyDescriptor = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Width"];
				propertyDescriptor2 = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Height"];
				propertyDescriptor3 = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Top"];
				propertyDescriptor4 = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Left"];
				if (propertyDescriptor != null && !typeof(int).IsAssignableFrom(propertyDescriptor.PropertyType))
				{
					propertyDescriptor = null;
				}
				if (propertyDescriptor2 != null && !typeof(int).IsAssignableFrom(propertyDescriptor2.PropertyType))
				{
					propertyDescriptor2 = null;
				}
				if (propertyDescriptor3 != null && !typeof(int).IsAssignableFrom(propertyDescriptor3.PropertyType))
				{
					propertyDescriptor3 = null;
				}
				if (propertyDescriptor4 != null && !typeof(int).IsAssignableFrom(propertyDescriptor4.PropertyType))
				{
					propertyDescriptor4 = null;
				}
			}
			Control control = this.resizeComponents[0].resizeControl as Control;
			this.lastMouseLoc = mouseLoc;
			this.lastMouseAbs = new NativeMethods.POINT(mouseLoc.X, mouseLoc.Y);
			UnsafeNativeMethods.ClientToScreen(new HandleRef(this, this.behaviorService.AdornerWindowControl.Handle), this.lastMouseAbs);
			int num = Math.Max(control.MinimumSize.Height, 10);
			int num2 = Math.Max(control.MinimumSize.Width, 10);
			if (this.dragManager != null)
			{
				bool flag2 = true;
				bool flag3 = true;
				if (((this.targetResizeRules & SelectionRules.BottomSizeable) != SelectionRules.None || (this.targetResizeRules & SelectionRules.TopSizeable) != SelectionRules.None) && control.Height == num)
				{
					flag2 = false;
				}
				else if (((this.targetResizeRules & SelectionRules.RightSizeable) != SelectionRules.None || (this.targetResizeRules & SelectionRules.LeftSizeable) != SelectionRules.None) && control.Width == num2)
				{
					flag2 = false;
				}
				PropertyDescriptor propertyDescriptor5 = TypeDescriptor.GetProperties(control)["IntegralHeight"];
				if (propertyDescriptor5 != null)
				{
					object value = propertyDescriptor5.GetValue(control);
					if (value is bool && (bool)value)
					{
						flag3 = false;
					}
				}
				if (!flag && flag2)
				{
					this.lastSnapOffset = this.dragManager.OnMouseMove(control, this.GenerateSnapLines(this.targetResizeRules, mouseLoc), ref this.didSnap, flag3);
				}
				else
				{
					this.dragManager.OnMouseMove(new Rectangle(-100, -100, 0, 0));
				}
				mouseLoc.X += this.lastSnapOffset.X;
				mouseLoc.Y += this.lastSnapOffset.Y;
			}
			Rectangle rectangle = new Rectangle(this.resizeComponents[0].resizeBounds.X, this.resizeComponents[0].resizeBounds.Y, this.resizeComponents[0].resizeBounds.Width, this.resizeComponents[0].resizeBounds.Height);
			if (this.didSnap && control.Parent != null)
			{
				rectangle.Location = this.behaviorService.MapAdornerWindowPoint(control.Parent.Handle, rectangle.Location);
				if (control.Parent.IsMirrored)
				{
					rectangle.Offset(-rectangle.Width, 0);
				}
			}
			Rectangle rectangle2 = Rectangle.Empty;
			Rectangle rectangle3 = Rectangle.Empty;
			bool flag4 = true;
			Color color = ((control.Parent != null) ? control.Parent.BackColor : Color.Empty);
			for (int i = 0; i < this.resizeComponents.Length; i++)
			{
				Control control2 = this.resizeComponents[i].resizeControl as Control;
				Rectangle rectangle4 = control2.Bounds;
				Rectangle rectangle5 = rectangle4;
				Rectangle resizeBounds = this.resizeComponents[i].resizeBounds;
				Rectangle rectangle6 = this.BehaviorService.ControlRectInAdornerWindow(control2);
				bool flag5 = true;
				UnsafeNativeMethods.SendMessage(control2.Handle, 11, false, 0);
				try
				{
					bool flag6 = false;
					if (control2.Parent != null && control2.Parent.IsMirrored)
					{
						flag6 = true;
					}
					BoundsSpecified boundsSpecified = BoundsSpecified.None;
					SelectionRules resizeRules = this.resizeComponents[i].resizeRules;
					if ((this.targetResizeRules & SelectionRules.BottomSizeable) != SelectionRules.None && (resizeRules & SelectionRules.BottomSizeable) != SelectionRules.None)
					{
						int num3;
						if (this.didSnap)
						{
							num3 = mouseLoc.Y - rectangle.Bottom;
						}
						else
						{
							num3 = ResizeBehavior.AdjustPixelsForIntegralHeight(control2, mouseLoc.Y - this.initialPoint.Y);
						}
						rectangle4.Height = Math.Max(num, resizeBounds.Height + num3);
						boundsSpecified |= BoundsSpecified.Height;
					}
					if ((this.targetResizeRules & SelectionRules.TopSizeable) != SelectionRules.None && (resizeRules & SelectionRules.TopSizeable) != SelectionRules.None)
					{
						int num4;
						if (this.didSnap)
						{
							num4 = rectangle.Y - mouseLoc.Y;
						}
						else
						{
							num4 = ResizeBehavior.AdjustPixelsForIntegralHeight(control2, this.initialPoint.Y - mouseLoc.Y);
						}
						boundsSpecified |= BoundsSpecified.Height;
						rectangle4.Height = Math.Max(num, resizeBounds.Height + num4);
						if (rectangle4.Height != num || (rectangle4.Height == num && rectangle5.Height != num))
						{
							boundsSpecified |= BoundsSpecified.Y;
							rectangle4.Y = Math.Min(resizeBounds.Bottom - num, resizeBounds.Y - num4);
						}
					}
					if (((this.targetResizeRules & SelectionRules.RightSizeable) != SelectionRules.None && (resizeRules & SelectionRules.RightSizeable) != SelectionRules.None && !flag6) || ((this.targetResizeRules & SelectionRules.LeftSizeable) != SelectionRules.None && (resizeRules & SelectionRules.LeftSizeable) != SelectionRules.None && flag6))
					{
						boundsSpecified |= BoundsSpecified.Width;
						int num5 = this.initialPoint.X;
						if (this.didSnap)
						{
							num5 = ((!flag6) ? rectangle.Right : rectangle.Left);
						}
						rectangle4.Width = Math.Max(num2, resizeBounds.Width + ((!flag6) ? (mouseLoc.X - num5) : (num5 - mouseLoc.X)));
					}
					if (((this.targetResizeRules & SelectionRules.RightSizeable) != SelectionRules.None && (resizeRules & SelectionRules.RightSizeable) != SelectionRules.None && flag6) || ((this.targetResizeRules & SelectionRules.LeftSizeable) != SelectionRules.None && (resizeRules & SelectionRules.LeftSizeable) != SelectionRules.None && !flag6))
					{
						boundsSpecified |= BoundsSpecified.Width;
						int num6 = this.initialPoint.X;
						if (this.didSnap)
						{
							num6 = ((!flag6) ? rectangle.Left : rectangle.Right);
						}
						int num7 = ((!flag6) ? (num6 - mouseLoc.X) : (mouseLoc.X - num6));
						rectangle4.Width = Math.Max(num2, resizeBounds.Width + num7);
						if (rectangle4.Width != num2 || (rectangle4.Width == num2 && rectangle5.Width != num2))
						{
							boundsSpecified |= BoundsSpecified.X;
							rectangle4.X = Math.Min(resizeBounds.Right - num2, resizeBounds.X - num7);
						}
					}
					if (!this.parentGridSize.IsEmpty)
					{
						rectangle4 = this.AdjustToGrid(rectangle4, this.targetResizeRules);
					}
					if ((boundsSpecified & BoundsSpecified.Width) == BoundsSpecified.Width && this.dragging && this.initialResize && propertyDescriptor != null)
					{
						propertyDescriptor.SetValue(this.resizeComponents[i].resizeControl, rectangle4.Width);
					}
					if ((boundsSpecified & BoundsSpecified.Height) == BoundsSpecified.Height && this.dragging && this.initialResize && propertyDescriptor2 != null)
					{
						propertyDescriptor2.SetValue(this.resizeComponents[i].resizeControl, rectangle4.Height);
					}
					if ((boundsSpecified & BoundsSpecified.X) == BoundsSpecified.X && this.dragging && this.initialResize && propertyDescriptor4 != null)
					{
						propertyDescriptor4.SetValue(this.resizeComponents[i].resizeControl, rectangle4.X);
					}
					if ((boundsSpecified & BoundsSpecified.Y) == BoundsSpecified.Y && this.dragging && this.initialResize && propertyDescriptor3 != null)
					{
						propertyDescriptor3.SetValue(this.resizeComponents[i].resizeControl, rectangle4.Y);
					}
					if (this.dragging)
					{
						control2.SetBounds(rectangle4.X, rectangle4.Y, rectangle4.Width, rectangle4.Height, boundsSpecified);
						rectangle2 = this.BehaviorService.ControlRectInAdornerWindow(control2);
						if (control2.Equals(control))
						{
							rectangle3 = rectangle2;
						}
						if (control2.Bounds == rectangle5)
						{
							flag5 = false;
						}
						if (control2.Bounds != rectangle4)
						{
							flag4 = false;
						}
					}
					if (control2 == this.primaryControl && this.statusCommandUI != null)
					{
						this.statusCommandUI.SetStatusInformation(control2);
					}
				}
				finally
				{
					UnsafeNativeMethods.SendMessage(control2.Handle, 11, true, 0);
					if (flag5)
					{
						Control parent = control2.Parent;
						if (parent != null)
						{
							control2.Invalidate(true);
							parent.Invalidate(rectangle5, true);
							parent.Update();
						}
						else
						{
							control2.Refresh();
						}
					}
					if (!rectangle2.IsEmpty)
					{
						using (Region region = new Region(rectangle2))
						{
							region.Exclude(Rectangle.Inflate(rectangle2, -2, -2));
							if (flag5)
							{
								using (Region region2 = new Region(rectangle6))
								{
									region2.Exclude(Rectangle.Inflate(rectangle6, -2, -2));
									this.BehaviorService.Invalidate(region2);
								}
							}
							if (!this.captureLost)
							{
								using (Graphics adornerWindowGraphics = this.BehaviorService.AdornerWindowGraphics)
								{
									if (this.lastResizeRegion != null && !this.lastResizeRegion.Equals(region, adornerWindowGraphics))
									{
										this.lastResizeRegion.Exclude(region);
										this.BehaviorService.Invalidate(this.lastResizeRegion);
										this.lastResizeRegion.Dispose();
										this.lastResizeRegion = null;
									}
									DesignerUtils.DrawResizeBorder(adornerWindowGraphics, region, color);
								}
								if (this.lastResizeRegion == null)
								{
									this.lastResizeRegion = region.Clone();
								}
							}
						}
					}
				}
			}
			if (flag4 && !flag && this.dragManager != null)
			{
				this.dragManager.RenderSnapLinesInternal(rectangle3);
			}
			this.initialResize = false;
			return true;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000A7CEC File Offset: 0x000A6CEC
		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			try
			{
				if (this.dragging)
				{
					if (this.dragManager != null)
					{
						this.dragManager.OnMouseUp();
						this.dragManager = null;
						this.lastSnapOffset = Point.Empty;
						this.didSnap = false;
					}
					if (this.resizeComponents != null && this.resizeComponents.Length > 0)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Width"];
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Height"];
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Top"];
						PropertyDescriptor propertyDescriptor4 = TypeDescriptor.GetProperties(this.resizeComponents[0].resizeControl)["Left"];
						for (int i = 0; i < this.resizeComponents.Length; i++)
						{
							if (propertyDescriptor != null && ((Control)this.resizeComponents[i].resizeControl).Width != this.resizeComponents[i].resizeBounds.Width)
							{
								propertyDescriptor.SetValue(this.resizeComponents[i].resizeControl, ((Control)this.resizeComponents[i].resizeControl).Width);
							}
							if (propertyDescriptor2 != null && ((Control)this.resizeComponents[i].resizeControl).Height != this.resizeComponents[i].resizeBounds.Height)
							{
								propertyDescriptor2.SetValue(this.resizeComponents[i].resizeControl, ((Control)this.resizeComponents[i].resizeControl).Height);
							}
							if (propertyDescriptor3 != null && ((Control)this.resizeComponents[i].resizeControl).Top != this.resizeComponents[i].resizeBounds.Y)
							{
								propertyDescriptor3.SetValue(this.resizeComponents[i].resizeControl, ((Control)this.resizeComponents[i].resizeControl).Top);
							}
							if (propertyDescriptor4 != null && ((Control)this.resizeComponents[i].resizeControl).Left != this.resizeComponents[i].resizeBounds.X)
							{
								propertyDescriptor4.SetValue(this.resizeComponents[i].resizeControl, ((Control)this.resizeComponents[i].resizeControl).Left);
							}
							if (this.resizeComponents[i].resizeControl == this.primaryControl && this.statusCommandUI != null)
							{
								this.statusCommandUI.SetStatusInformation(this.primaryControl);
							}
						}
					}
				}
				if (this.resizeTransaction != null)
				{
					DesignerTransaction designerTransaction = this.resizeTransaction;
					this.resizeTransaction = null;
					using (designerTransaction)
					{
						designerTransaction.Commit();
					}
				}
			}
			finally
			{
				this.OnLoseCapture(g, EventArgs.Empty);
			}
			return false;
		}

		// Token: 0x0400169D RID: 5789
		private const int MINSIZE = 10;

		// Token: 0x0400169E RID: 5790
		private const int borderSize = 2;

		// Token: 0x0400169F RID: 5791
		private ResizeBehavior.ResizeComponent[] resizeComponents;

		// Token: 0x040016A0 RID: 5792
		private IServiceProvider serviceProvider;

		// Token: 0x040016A1 RID: 5793
		private BehaviorService behaviorService;

		// Token: 0x040016A2 RID: 5794
		private SelectionRules targetResizeRules;

		// Token: 0x040016A3 RID: 5795
		private Point initialPoint;

		// Token: 0x040016A4 RID: 5796
		private bool dragging;

		// Token: 0x040016A5 RID: 5797
		private bool pushedBehavior;

		// Token: 0x040016A6 RID: 5798
		private bool initialResize;

		// Token: 0x040016A7 RID: 5799
		private DesignerTransaction resizeTransaction;

		// Token: 0x040016A8 RID: 5800
		private DragAssistanceManager dragManager;

		// Token: 0x040016A9 RID: 5801
		private Point lastMouseLoc;

		// Token: 0x040016AA RID: 5802
		private Point parentLocation;

		// Token: 0x040016AB RID: 5803
		private Size parentGridSize;

		// Token: 0x040016AC RID: 5804
		private NativeMethods.POINT lastMouseAbs;

		// Token: 0x040016AD RID: 5805
		private Point lastSnapOffset;

		// Token: 0x040016AE RID: 5806
		private bool didSnap;

		// Token: 0x040016AF RID: 5807
		private Control primaryControl;

		// Token: 0x040016B0 RID: 5808
		private Cursor cursor = Cursors.Default;

		// Token: 0x040016B1 RID: 5809
		private StatusCommandUI statusCommandUI;

		// Token: 0x040016B2 RID: 5810
		private Region lastResizeRegion;

		// Token: 0x040016B3 RID: 5811
		private bool captureLost;

		// Token: 0x02000302 RID: 770
		private struct ResizeComponent
		{
			// Token: 0x040016B4 RID: 5812
			public object resizeControl;

			// Token: 0x040016B5 RID: 5813
			public Rectangle resizeBounds;

			// Token: 0x040016B6 RID: 5814
			public SelectionRules resizeRules;
		}
	}
}
