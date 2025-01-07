using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class DragAssistanceManager
	{
		internal DragAssistanceManager(IServiceProvider serviceProvider)
			: this(serviceProvider, null, null, null, false, false)
		{
		}

		internal DragAssistanceManager(IServiceProvider serviceProvider, ArrayList dragComponents)
			: this(serviceProvider, null, dragComponents, null, false, false)
		{
		}

		internal DragAssistanceManager(IServiceProvider serviceProvider, ArrayList dragComponents, bool resizing)
			: this(serviceProvider, null, dragComponents, null, resizing, false)
		{
		}

		internal DragAssistanceManager(IServiceProvider serviceProvider, Graphics graphics, ArrayList dragComponents, Image backgroundImage, bool ctrlDrag)
			: this(serviceProvider, graphics, dragComponents, backgroundImage, false, ctrlDrag)
		{
		}

		internal DragAssistanceManager(IServiceProvider serviceProvider, Graphics graphics, ArrayList dragComponents, Image backgroundImage, bool resizing, bool ctrlDrag)
		{
			this.serviceProvider = serviceProvider;
			this.behaviorService = serviceProvider.GetService(typeof(BehaviorService)) as BehaviorService;
			IDesignerHost designerHost = serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			IUIService iuiservice = serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (designerHost == null || this.behaviorService == null)
			{
				return;
			}
			if (graphics == null)
			{
				this.graphics = this.behaviorService.AdornerWindowGraphics;
			}
			else
			{
				this.graphics = graphics;
			}
			if (iuiservice != null)
			{
				if (iuiservice.Styles["VsColorSnaplines"] is Color)
				{
					this.edgePen = new Pen((Color)iuiservice.Styles["VsColorSnaplines"]);
					this.disposeEdgePen = true;
				}
				if (iuiservice.Styles["VsColorSnaplinesTextBaseline"] is Color)
				{
					this.baselinePen.Dispose();
					this.baselinePen = new Pen((Color)iuiservice.Styles["VsColorSnaplinesTextBaseline"]);
				}
				if (iuiservice.Styles["VsColorSnaplinesMarginAndPadding"] is Color)
				{
					this.marginAndPaddingPen = new Pen((Color)iuiservice.Styles["VsColorSnaplinesMarginAndPadding"]);
					this.disposeMarginPen = true;
				}
			}
			this.backgroundImage = backgroundImage;
			this.rootComponentHandle = ((designerHost.RootComponent is Control) ? ((Control)designerHost.RootComponent).Handle : IntPtr.Zero);
			this.resizing = resizing;
			this.ctrlDrag = ctrlDrag;
			this.Initialize(dragComponents, designerHost);
		}

		private void AddSnapLines(ControlDesigner controlDesigner, ArrayList horizontalList, ArrayList verticalList, bool isTarget, bool validTarget)
		{
			IList snapLines = controlDesigner.SnapLines;
			Rectangle clientRectangle = controlDesigner.Control.ClientRectangle;
			Rectangle bounds = controlDesigner.Control.Bounds;
			bounds.Location = (clientRectangle.Location = this.behaviorService.ControlToAdornerWindow(controlDesigner.Control));
			int left = bounds.Left;
			int top = bounds.Top;
			Point offsetToClientArea = controlDesigner.GetOffsetToClientArea();
			clientRectangle.X += offsetToClientArea.X;
			clientRectangle.Y += offsetToClientArea.Y;
			foreach (object obj in snapLines)
			{
				SnapLine snapLine = (SnapLine)obj;
				if (isTarget)
				{
					if (snapLine.Filter != null && snapLine.Filter.StartsWith("Padding"))
					{
						continue;
					}
					if (validTarget && !this.targetSnapLineTypes.Contains(snapLine.SnapLineType))
					{
						this.targetSnapLineTypes.Add(snapLine.SnapLineType);
					}
				}
				else
				{
					if (validTarget && !this.targetSnapLineTypes.Contains(snapLine.SnapLineType))
					{
						continue;
					}
					if (snapLine.Filter != null && snapLine.Filter.StartsWith("Padding"))
					{
						this.snapLineToBounds.Add(snapLine, clientRectangle);
					}
					else
					{
						this.snapLineToBounds.Add(snapLine, bounds);
					}
				}
				if (snapLine.IsHorizontal)
				{
					snapLine.AdjustOffset(top);
					horizontalList.Add(snapLine);
				}
				else
				{
					snapLine.AdjustOffset(left);
					verticalList.Add(snapLine);
				}
			}
		}

		private int BuildDistanceArray(ArrayList snapLines, ArrayList targetSnapLines, int[] distances, Rectangle dragBounds)
		{
			int num = 4369;
			int num2 = 0;
			for (int i = 0; i < snapLines.Count; i++)
			{
				SnapLine snapLine = (SnapLine)snapLines[i];
				if (DragAssistanceManager.IsMarginOrPaddingSnapLine(snapLine) && !this.ValidateMarginOrPaddingLine(snapLine, dragBounds))
				{
					distances[i] = 4369;
				}
				else
				{
					int num3 = 4369;
					for (int j = 0; j < targetSnapLines.Count; j++)
					{
						SnapLine snapLine2 = (SnapLine)targetSnapLines[j];
						if (SnapLine.ShouldSnap(snapLine, snapLine2))
						{
							int num4 = snapLine2.Offset - snapLine.Offset;
							if (Math.Abs(num4) < Math.Abs(num3))
							{
								num3 = num4;
							}
						}
					}
					distances[i] = num3;
					int priority = (int)((SnapLine)snapLines[i]).Priority;
					if (Math.Abs(num3) < Math.Abs(num) || (Math.Abs(num3) == Math.Abs(num) && priority > num2))
					{
						num = num3;
						if (priority != 4)
						{
							num2 = priority;
						}
					}
				}
			}
			return num;
		}

		private DragAssistanceManager.Line[] EraseOldSnapLines(DragAssistanceManager.Line[] lines, ArrayList tempLines)
		{
			Rectangle empty = Rectangle.Empty;
			if (lines != null)
			{
				foreach (DragAssistanceManager.Line line in lines)
				{
					bool flag = false;
					if (tempLines != null)
					{
						for (int j = 0; j < tempLines.Count; j++)
						{
							if (line.LineType == ((DragAssistanceManager.Line)tempLines[j]).LineType)
							{
								DragAssistanceManager.Line[] diffs = DragAssistanceManager.Line.GetDiffs(line, (DragAssistanceManager.Line)tempLines[j]);
								if (diffs != null)
								{
									for (int k = 0; k < diffs.Length; k++)
									{
										empty = new Rectangle(diffs[k].x1, diffs[k].y1, diffs[k].x2 - diffs[k].x1, diffs[k].y2 - diffs[k].y1);
										empty.Inflate(1, 1);
										if (this.backgroundImage != null)
										{
											this.graphics.DrawImage(this.backgroundImage, empty, empty, GraphicsUnit.Pixel);
										}
										else
										{
											this.behaviorService.Invalidate(empty);
										}
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						empty = new Rectangle(line.x1, line.y1, line.x2 - line.x1, line.y2 - line.y1);
						empty.Inflate(1, 1);
						if (this.backgroundImage != null)
						{
							this.graphics.DrawImage(this.backgroundImage, empty, empty, GraphicsUnit.Pixel);
						}
						else
						{
							this.behaviorService.Invalidate(empty);
						}
					}
				}
			}
			if (tempLines != null)
			{
				lines = new DragAssistanceManager.Line[tempLines.Count];
				tempLines.CopyTo(lines);
			}
			else
			{
				lines = new DragAssistanceManager.Line[0];
			}
			return lines;
		}

		internal void EraseSnapLines()
		{
			this.EraseOldSnapLines(this.vertLines, null);
			this.EraseOldSnapLines(this.horzLines, null);
		}

		internal DragAssistanceManager.Line[] GetRecentLines()
		{
			if (this.recentLines != null)
			{
				return this.recentLines;
			}
			return new DragAssistanceManager.Line[0];
		}

		private void IdentifyAndStoreValidLines(ArrayList snapLines, int[] distances, Rectangle dragBounds, int smallestDistance)
		{
			int num = 1;
			for (int i = 0; i < distances.Length; i++)
			{
				if (distances[i] == smallestDistance)
				{
					int priority = (int)((SnapLine)snapLines[i]).Priority;
					if (priority > num && priority != 4)
					{
						num = priority;
					}
				}
			}
			for (int j = 0; j < distances.Length; j++)
			{
				if (distances[j] == smallestDistance && (((SnapLine)snapLines[j]).Priority == (SnapLinePriority)num || ((SnapLine)snapLines[j]).Priority == SnapLinePriority.Always))
				{
					this.StoreSnapLine((SnapLine)snapLines[j], dragBounds);
				}
			}
		}

		private bool AddChildCompSnaplines(IComponent comp, ArrayList dragComponents, Rectangle clipBounds, Control targetControl)
		{
			Control control = comp as Control;
			if (control == null || (dragComponents != null && dragComponents.Contains(comp) && !this.ctrlDrag) || DragAssistanceManager.IsChildOfParent(control, targetControl) || !clipBounds.IntersectsWith(control.Bounds) || control.Parent == null || !control.Visible)
			{
				return false;
			}
			Control control2 = control;
			if (!control2.Equals(targetControl))
			{
				IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					ControlDesigner controlDesigner = designerHost.GetDesigner(control2) as ControlDesigner;
					if (controlDesigner != null)
					{
						return controlDesigner.ControlSupportsSnaplines;
					}
				}
			}
			return true;
		}

		private bool AddControlSnaplinesWhenResizing(ControlDesigner designer, Control control, Control targetControl)
		{
			return !this.resizing || !(designer is ParentControlDesigner) || !control.AutoSize || targetControl == null || targetControl.Parent == null || !targetControl.Parent.Equals(control);
		}

		private void Initialize(ArrayList dragComponents, IDesignerHost host)
		{
			Control control = null;
			if (dragComponents != null && dragComponents.Count > 0)
			{
				control = dragComponents[0] as Control;
			}
			Control control2 = host.RootComponent as Control;
			Rectangle rectangle = new Rectangle(0, 0, control2.ClientRectangle.Width, control2.ClientRectangle.Height);
			rectangle.Inflate(-1, -1);
			if (control != null)
			{
				this.dragOffset = this.behaviorService.ControlToAdornerWindow(control);
			}
			else
			{
				this.dragOffset = this.behaviorService.MapAdornerWindowPoint(control2.Handle, Point.Empty);
				if (control2.Parent != null && control2.Parent.IsMirrored)
				{
					this.dragOffset.Offset(-control2.Width, 0);
				}
			}
			if (control != null)
			{
				ControlDesigner controlDesigner = host.GetDesigner(control) as ControlDesigner;
				bool flag = false;
				if (controlDesigner == null)
				{
					controlDesigner = TypeDescriptor.CreateDesigner(control, typeof(IDesigner)) as ControlDesigner;
					if (controlDesigner != null)
					{
						controlDesigner.ForceVisible = false;
						controlDesigner.Initialize(control);
						flag = true;
					}
				}
				this.AddSnapLines(controlDesigner, this.targetHorizontalSnapLines, this.targetVerticalSnapLines, true, control != null);
				if (flag)
				{
					controlDesigner.Dispose();
				}
			}
			foreach (object obj in host.Container.Components)
			{
				IComponent component = (IComponent)obj;
				if (this.AddChildCompSnaplines(component, dragComponents, rectangle, control))
				{
					ControlDesigner controlDesigner2 = host.GetDesigner(component) as ControlDesigner;
					if (controlDesigner2 != null)
					{
						if (this.AddControlSnaplinesWhenResizing(controlDesigner2, component as Control, control))
						{
							this.AddSnapLines(controlDesigner2, this.horizontalSnapLines, this.verticalSnapLines, false, control != null);
						}
						int num = controlDesigner2.NumberOfInternalControlDesigners();
						for (int i = 0; i < num; i++)
						{
							ControlDesigner controlDesigner3 = controlDesigner2.InternalControlDesigner(i);
							if (controlDesigner3 != null && this.AddChildCompSnaplines(controlDesigner3.Component, dragComponents, rectangle, control) && this.AddControlSnaplinesWhenResizing(controlDesigner3, controlDesigner3.Component as Control, control))
							{
								this.AddSnapLines(controlDesigner3, this.horizontalSnapLines, this.verticalSnapLines, false, control != null);
							}
						}
					}
				}
			}
			this.verticalDistances = new int[this.verticalSnapLines.Count];
			this.horizontalDistances = new int[this.horizontalSnapLines.Count];
		}

		private static bool IsChildOfParent(Control child, Control parent)
		{
			if (child == null || parent == null)
			{
				return false;
			}
			for (Control control = child.Parent; control != null; control = control.Parent)
			{
				if (control.Equals(parent))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsMarginOrPaddingSnapLine(SnapLine snapLine)
		{
			return snapLine.Filter != null && (snapLine.Filter.StartsWith("Margin") || snapLine.Filter.StartsWith("Padding"));
		}

		internal Point OffsetToNearestSnapLocation(Control targetControl, IList targetSnaplines, Point directionOffset)
		{
			this.targetHorizontalSnapLines.Clear();
			this.targetVerticalSnapLines.Clear();
			foreach (object obj in targetSnaplines)
			{
				SnapLine snapLine = (SnapLine)obj;
				if (snapLine.IsHorizontal)
				{
					this.targetHorizontalSnapLines.Add(snapLine);
				}
				else
				{
					this.targetVerticalSnapLines.Add(snapLine);
				}
			}
			return this.OffsetToNearestSnapLocation(targetControl, directionOffset);
		}

		internal Point OffsetToNearestSnapLocation(Control targetControl, Point directionOffset)
		{
			Point empty = Point.Empty;
			Rectangle rectangle = new Rectangle(this.behaviorService.ControlToAdornerWindow(targetControl), targetControl.Size);
			if (directionOffset.X != 0)
			{
				this.BuildDistanceArray(this.verticalSnapLines, this.targetVerticalSnapLines, this.verticalDistances, rectangle);
				int num = ((directionOffset.X < 0) ? 0 : rectangle.X);
				int num2 = ((directionOffset.X < 0) ? rectangle.Right : int.MaxValue);
				empty.X = DragAssistanceManager.FindSmallestValidDistance(this.verticalSnapLines, this.verticalDistances, num, num2, directionOffset.X);
				if (empty.X != 0)
				{
					this.IdentifyAndStoreValidLines(this.verticalSnapLines, this.verticalDistances, rectangle, empty.X);
					if (directionOffset.X < 0)
					{
						empty.X *= -1;
					}
				}
			}
			if (directionOffset.Y != 0)
			{
				this.BuildDistanceArray(this.horizontalSnapLines, this.targetHorizontalSnapLines, this.horizontalDistances, rectangle);
				int num3 = ((directionOffset.Y < 0) ? 0 : rectangle.Y);
				int num4 = ((directionOffset.Y < 0) ? rectangle.Bottom : int.MaxValue);
				empty.Y = DragAssistanceManager.FindSmallestValidDistance(this.horizontalSnapLines, this.horizontalDistances, num3, num4, directionOffset.Y);
				if (empty.Y != 0)
				{
					this.IdentifyAndStoreValidLines(this.horizontalSnapLines, this.horizontalDistances, rectangle, empty.Y);
					if (directionOffset.Y < 0)
					{
						empty.Y *= -1;
					}
				}
			}
			if (!empty.IsEmpty)
			{
				this.cachedDragRect = rectangle;
				this.cachedDragRect.Offset(empty.X, empty.Y);
				if (empty.X != 0)
				{
					this.vertLines = new DragAssistanceManager.Line[this.tempVertLines.Count];
					this.tempVertLines.CopyTo(this.vertLines);
				}
				if (empty.Y != 0)
				{
					this.horzLines = new DragAssistanceManager.Line[this.tempHorzLines.Count];
					this.tempHorzLines.CopyTo(this.horzLines);
				}
			}
			return empty;
		}

		private static int FindSmallestValidDistance(ArrayList snapLines, int[] distances, int min, int max, int direction)
		{
			int num = 0;
			int num2;
			do
			{
				num2 = DragAssistanceManager.SmallestDistanceIndex(distances, direction, out num);
				if (num2 == 4369)
				{
					return 0;
				}
			}
			while (!DragAssistanceManager.IsWithinValidRange(((SnapLine)snapLines[num2]).Offset, min, max));
			distances[num2] = num;
			return num;
		}

		private static bool IsWithinValidRange(int offset, int min, int max)
		{
			return offset > min && offset < max;
		}

		private static int SmallestDistanceIndex(int[] distances, int direction, out int distanceValue)
		{
			distanceValue = 4369;
			int num = 4369;
			if (distances.Length == 0)
			{
				return num;
			}
			for (int i = 0; i < distances.Length; i++)
			{
				if (distances[i] == 0 || (distances[i] > 0 && direction > 0) || (distances[i] < 0 && direction < 0))
				{
					distances[i] = 4369;
				}
				if (Math.Abs(distances[i]) < distanceValue)
				{
					distanceValue = Math.Abs(distances[i]);
					num = i;
				}
			}
			if (num < distances.Length)
			{
				distances[num] = 4369;
			}
			return num;
		}

		private void RenderSnapLines(DragAssistanceManager.Line[] lines, Rectangle dragRect)
		{
			for (int i = 0; i < lines.Length; i++)
			{
				Pen pen;
				if (lines[i].LineType == DragAssistanceManager.LineType.Margin || lines[i].LineType == DragAssistanceManager.LineType.Padding)
				{
					pen = this.marginAndPaddingPen;
					if (lines[i].x1 == lines[i].x2)
					{
						int num = Math.Max(dragRect.Top, lines[i].OriginalBounds.Top);
						num += (Math.Min(dragRect.Bottom, lines[i].OriginalBounds.Bottom) - num) / 2;
						lines[i].y1 = (lines[i].y2 = num);
						if (lines[i].LineType == DragAssistanceManager.LineType.Margin)
						{
							lines[i].x1 = Math.Min(dragRect.Right, lines[i].OriginalBounds.Right);
							lines[i].x2 = Math.Max(dragRect.Left, lines[i].OriginalBounds.Left);
						}
						else if (lines[i].PaddingLineType == DragAssistanceManager.PaddingLineType.PaddingLeft)
						{
							lines[i].x1 = lines[i].OriginalBounds.Left;
							lines[i].x2 = dragRect.Left;
						}
						else
						{
							lines[i].x1 = dragRect.Right;
							lines[i].x2 = lines[i].OriginalBounds.Right;
						}
						lines[i].x2--;
					}
					else
					{
						int num2 = Math.Max(dragRect.Left, lines[i].OriginalBounds.Left);
						num2 += (Math.Min(dragRect.Right, lines[i].OriginalBounds.Right) - num2) / 2;
						lines[i].x1 = (lines[i].x2 = num2);
						if (lines[i].LineType == DragAssistanceManager.LineType.Margin)
						{
							lines[i].y1 = Math.Min(dragRect.Bottom, lines[i].OriginalBounds.Bottom);
							lines[i].y2 = Math.Max(dragRect.Top, lines[i].OriginalBounds.Top);
						}
						else if (lines[i].PaddingLineType == DragAssistanceManager.PaddingLineType.PaddingTop)
						{
							lines[i].y1 = lines[i].OriginalBounds.Top;
							lines[i].y2 = dragRect.Top;
						}
						else
						{
							lines[i].y1 = dragRect.Bottom;
							lines[i].y2 = lines[i].OriginalBounds.Bottom;
						}
						lines[i].y2--;
					}
				}
				else if (lines[i].LineType == DragAssistanceManager.LineType.Baseline)
				{
					pen = this.baselinePen;
					lines[i].x2--;
				}
				else
				{
					pen = this.edgePen;
					if (lines[i].x1 == lines[i].x2)
					{
						lines[i].y2--;
					}
					else
					{
						lines[i].x2--;
					}
				}
				this.graphics.DrawLine(pen, lines[i].x1, lines[i].y1, lines[i].x2, lines[i].y2);
			}
		}

		private static void CombineSnaplines(DragAssistanceManager.Line snapLine, ArrayList currentLines)
		{
			bool flag = false;
			for (int i = 0; i < currentLines.Count; i++)
			{
				DragAssistanceManager.Line line = (DragAssistanceManager.Line)currentLines[i];
				DragAssistanceManager.Line line2 = DragAssistanceManager.Line.Overlap(snapLine, line);
				if (line2 != null)
				{
					currentLines[i] = line2;
					flag = true;
				}
			}
			if (!flag)
			{
				currentLines.Add(snapLine);
			}
		}

		private void StoreSnapLine(SnapLine snapLine, Rectangle dragBounds)
		{
			Rectangle rectangle = (Rectangle)this.snapLineToBounds[snapLine];
			DragAssistanceManager.LineType lineType = DragAssistanceManager.LineType.Standard;
			if (DragAssistanceManager.IsMarginOrPaddingSnapLine(snapLine))
			{
				lineType = (snapLine.Filter.StartsWith("Margin") ? DragAssistanceManager.LineType.Margin : DragAssistanceManager.LineType.Padding);
			}
			else if (snapLine.SnapLineType == SnapLineType.Baseline)
			{
				lineType = DragAssistanceManager.LineType.Baseline;
			}
			DragAssistanceManager.Line line;
			if (snapLine.IsVertical)
			{
				line = new DragAssistanceManager.Line(snapLine.Offset, Math.Min(dragBounds.Top + ((this.snapPointY != 4369) ? this.snapPointY : 0), rectangle.Top), snapLine.Offset, Math.Max(dragBounds.Bottom + ((this.snapPointY != 4369) ? this.snapPointY : 0), rectangle.Bottom));
				line.LineType = lineType;
				DragAssistanceManager.CombineSnaplines(line, this.tempVertLines);
			}
			else
			{
				line = new DragAssistanceManager.Line(Math.Min(dragBounds.Left + ((this.snapPointX != 4369) ? this.snapPointX : 0), rectangle.Left), snapLine.Offset, Math.Max(dragBounds.Right + ((this.snapPointX != 4369) ? this.snapPointX : 0), rectangle.Right), snapLine.Offset);
				line.LineType = lineType;
				DragAssistanceManager.CombineSnaplines(line, this.tempHorzLines);
			}
			if (DragAssistanceManager.IsMarginOrPaddingSnapLine(snapLine))
			{
				line.OriginalBounds = rectangle;
				string filter;
				if (line.LineType == DragAssistanceManager.LineType.Padding && (filter = snapLine.Filter) != null)
				{
					if (filter == "Padding.Right")
					{
						line.PaddingLineType = DragAssistanceManager.PaddingLineType.PaddingRight;
						return;
					}
					if (filter == "Padding.Left")
					{
						line.PaddingLineType = DragAssistanceManager.PaddingLineType.PaddingLeft;
						return;
					}
					if (filter == "Padding.Top")
					{
						line.PaddingLineType = DragAssistanceManager.PaddingLineType.PaddingTop;
						return;
					}
					if (!(filter == "Padding.Bottom"))
					{
						return;
					}
					line.PaddingLineType = DragAssistanceManager.PaddingLineType.PaddingBottom;
				}
			}
		}

		private bool ValidateMarginOrPaddingLine(SnapLine snapLine, Rectangle dragBounds)
		{
			Rectangle rectangle = (Rectangle)this.snapLineToBounds[snapLine];
			if (snapLine.IsVertical)
			{
				if (rectangle.Top < dragBounds.Top)
				{
					if (rectangle.Top + rectangle.Height < dragBounds.Top)
					{
						return false;
					}
				}
				else if (dragBounds.Top + dragBounds.Height < rectangle.Top)
				{
					return false;
				}
			}
			else if (rectangle.Left < dragBounds.Left)
			{
				if (rectangle.Left + rectangle.Width < dragBounds.Left)
				{
					return false;
				}
			}
			else if (dragBounds.Left + dragBounds.Width < rectangle.Left)
			{
				return false;
			}
			return true;
		}

		internal Point OnMouseMove(Rectangle dragBounds, SnapLine[] snapLines)
		{
			bool flag = false;
			return this.OnMouseMove(dragBounds, snapLines, ref flag, true);
		}

		internal Point OnMouseMove(Rectangle dragBounds, SnapLine[] snapLines, ref bool didSnap, bool shouldSnapHorizontally)
		{
			if (snapLines == null || snapLines.Length == 0)
			{
				return Point.Empty;
			}
			this.targetHorizontalSnapLines.Clear();
			this.targetVerticalSnapLines.Clear();
			foreach (SnapLine snapLine in snapLines)
			{
				if (snapLine.IsHorizontal)
				{
					this.targetHorizontalSnapLines.Add(snapLine);
				}
				else
				{
					this.targetVerticalSnapLines.Add(snapLine);
				}
			}
			return this.OnMouseMove(dragBounds, false, ref didSnap, shouldSnapHorizontally);
		}

		internal Point OnMouseMove(Rectangle dragBounds)
		{
			bool flag = false;
			return this.OnMouseMove(dragBounds, true, ref flag, true);
		}

		internal Point OnMouseMove(Control targetControl, SnapLine[] snapLines, ref bool didSnap, bool shouldSnapHorizontally)
		{
			Rectangle rectangle = new Rectangle(this.behaviorService.ControlToAdornerWindow(targetControl), targetControl.Size);
			didSnap = false;
			return this.OnMouseMove(rectangle, snapLines, ref didSnap, shouldSnapHorizontally);
		}

		private Point OnMouseMove(Rectangle dragBounds, bool offsetSnapLines, ref bool didSnap, bool shouldSnapHorizontally)
		{
			this.tempVertLines.Clear();
			this.tempHorzLines.Clear();
			this.dragOffset = new Point(dragBounds.X - this.dragOffset.X, dragBounds.Y - this.dragOffset.Y);
			if (offsetSnapLines)
			{
				for (int i = 0; i < this.targetHorizontalSnapLines.Count; i++)
				{
					((SnapLine)this.targetHorizontalSnapLines[i]).AdjustOffset(this.dragOffset.Y);
				}
				for (int j = 0; j < this.targetVerticalSnapLines.Count; j++)
				{
					((SnapLine)this.targetVerticalSnapLines[j]).AdjustOffset(this.dragOffset.X);
				}
			}
			int num = this.BuildDistanceArray(this.verticalSnapLines, this.targetVerticalSnapLines, this.verticalDistances, dragBounds);
			int num2 = 4369;
			if (shouldSnapHorizontally)
			{
				num2 = this.BuildDistanceArray(this.horizontalSnapLines, this.targetHorizontalSnapLines, this.horizontalDistances, dragBounds);
			}
			this.snapPointX = ((Math.Abs(num) <= 8) ? (-num) : 4369);
			this.snapPointY = ((Math.Abs(num2) <= 8) ? (-num2) : 4369);
			didSnap = false;
			if (this.snapPointX != 4369)
			{
				this.IdentifyAndStoreValidLines(this.verticalSnapLines, this.verticalDistances, dragBounds, num);
				didSnap = true;
			}
			if (this.snapPointY != 4369)
			{
				this.IdentifyAndStoreValidLines(this.horizontalSnapLines, this.horizontalDistances, dragBounds, num2);
				didSnap = true;
			}
			Point point = new Point((this.snapPointX != 4369) ? this.snapPointX : 0, (this.snapPointY != 4369) ? this.snapPointY : 0);
			Rectangle rectangle = new Rectangle(dragBounds.Left + point.X, dragBounds.Top + point.Y, dragBounds.Width, dragBounds.Height);
			this.vertLines = this.EraseOldSnapLines(this.vertLines, this.tempVertLines);
			this.horzLines = this.EraseOldSnapLines(this.horzLines, this.tempHorzLines);
			this.cachedDragRect = rectangle;
			this.dragOffset = dragBounds.Location;
			return point;
		}

		internal void RenderSnapLinesInternal(Rectangle dragRect)
		{
			this.cachedDragRect = dragRect;
			this.RenderSnapLinesInternal();
		}

		internal void RenderSnapLinesInternal()
		{
			this.RenderSnapLines(this.vertLines, this.cachedDragRect);
			this.RenderSnapLines(this.horzLines, this.cachedDragRect);
			this.recentLines = new DragAssistanceManager.Line[this.vertLines.Length + this.horzLines.Length];
			this.vertLines.CopyTo(this.recentLines, 0);
			this.horzLines.CopyTo(this.recentLines, this.vertLines.Length);
		}

		internal void OnMouseUp()
		{
			if (this.behaviorService != null)
			{
				DragAssistanceManager.Line[] array = this.GetRecentLines();
				string[] array2 = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i].ToString();
				}
				this.behaviorService.RecentSnapLines = array2;
			}
			this.EraseSnapLines();
			this.graphics.Dispose();
			if (this.disposeEdgePen && this.edgePen != null)
			{
				this.edgePen.Dispose();
			}
			if (this.disposeMarginPen && this.marginAndPaddingPen != null)
			{
				this.marginAndPaddingPen.Dispose();
			}
			if (this.baselinePen != null)
			{
				this.baselinePen.Dispose();
			}
			if (this.backgroundImage != null)
			{
				this.backgroundImage.Dispose();
			}
		}

		private const int snapDistance = 8;

		private const int INVALID_VALUE = 4369;

		private BehaviorService behaviorService;

		private IServiceProvider serviceProvider;

		private Graphics graphics;

		private IntPtr rootComponentHandle;

		private Point dragOffset;

		private Rectangle cachedDragRect;

		private Pen edgePen = SystemPens.Highlight;

		private bool disposeEdgePen;

		private Pen marginAndPaddingPen = SystemPens.InactiveCaption;

		private bool disposeMarginPen;

		private Pen baselinePen = new Pen(Color.Fuchsia);

		private ArrayList verticalSnapLines = new ArrayList();

		private ArrayList horizontalSnapLines = new ArrayList();

		private ArrayList targetVerticalSnapLines = new ArrayList();

		private ArrayList targetHorizontalSnapLines = new ArrayList();

		private ArrayList targetSnapLineTypes = new ArrayList();

		private int[] verticalDistances;

		private int[] horizontalDistances;

		private ArrayList tempVertLines = new ArrayList();

		private ArrayList tempHorzLines = new ArrayList();

		private DragAssistanceManager.Line[] vertLines = new DragAssistanceManager.Line[0];

		private DragAssistanceManager.Line[] horzLines = new DragAssistanceManager.Line[0];

		private Hashtable snapLineToBounds = new Hashtable();

		private DragAssistanceManager.Line[] recentLines;

		private Image backgroundImage;

		private int snapPointX;

		private int snapPointY;

		private bool resizing;

		private bool ctrlDrag;

		internal class Line
		{
			public DragAssistanceManager.LineType LineType
			{
				get
				{
					return this.lineType;
				}
				set
				{
					this.lineType = value;
				}
			}

			public Rectangle OriginalBounds
			{
				get
				{
					return this.originalBounds;
				}
				set
				{
					this.originalBounds = value;
				}
			}

			public DragAssistanceManager.PaddingLineType PaddingLineType
			{
				get
				{
					return this.paddingLineType;
				}
				set
				{
					this.paddingLineType = value;
				}
			}

			public Line(int x1, int y1, int x2, int y2)
			{
				this.x1 = x1;
				this.y1 = y1;
				this.x2 = x2;
				this.y2 = y2;
				this.lineType = DragAssistanceManager.LineType.Standard;
			}

			private Line(int x1, int y1, int x2, int y2, DragAssistanceManager.LineType type)
			{
				this.x1 = x1;
				this.y1 = y1;
				this.x2 = x2;
				this.y2 = y2;
				this.lineType = type;
			}

			public static DragAssistanceManager.Line[] GetDiffs(DragAssistanceManager.Line l1, DragAssistanceManager.Line l2)
			{
				if (l1.x1 == l1.x2 && l1.x1 == l2.x1)
				{
					return new DragAssistanceManager.Line[]
					{
						new DragAssistanceManager.Line(l1.x1, Math.Min(l1.y1, l2.y1), l1.x1, Math.Max(l1.y1, l2.y1)),
						new DragAssistanceManager.Line(l1.x1, Math.Min(l1.y2, l2.y2), l1.x1, Math.Max(l1.y2, l2.y2))
					};
				}
				if (l1.y1 == l1.y2 && l1.y1 == l2.y1)
				{
					return new DragAssistanceManager.Line[]
					{
						new DragAssistanceManager.Line(Math.Min(l1.x1, l2.x1), l1.y1, Math.Max(l1.x1, l2.x1), l1.y1),
						new DragAssistanceManager.Line(Math.Min(l1.x2, l2.x2), l1.y1, Math.Max(l1.x2, l2.x2), l1.y1)
					};
				}
				return null;
			}

			public static DragAssistanceManager.Line Overlap(DragAssistanceManager.Line l1, DragAssistanceManager.Line l2)
			{
				if (l1.LineType != l2.LineType)
				{
					return null;
				}
				if (l1.LineType != DragAssistanceManager.LineType.Standard && l1.LineType != DragAssistanceManager.LineType.Baseline)
				{
					return null;
				}
				if (l1.x1 == l1.x2 && l2.x1 == l2.x2 && l1.x1 == l2.x1)
				{
					return new DragAssistanceManager.Line(l1.x1, Math.Min(l1.y1, l2.y1), l1.x2, Math.Max(l1.y2, l2.y2), l1.LineType);
				}
				if (l1.y1 == l1.y2 && l2.y1 == l2.y2 && l1.y1 == l2.y2)
				{
					return new DragAssistanceManager.Line(Math.Min(l1.x1, l2.x1), l1.y1, Math.Max(l1.x2, l2.x2), l1.y2, l1.LineType);
				}
				return null;
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"Line, type = ", this.lineType, ", dims =(", this.x1, ", ", this.y1, ")->(", this.x2, ", ", this.y2,
					")"
				});
			}

			public int x1;

			public int y1;

			public int x2;

			public int y2;

			private DragAssistanceManager.LineType lineType;

			private DragAssistanceManager.PaddingLineType paddingLineType;

			private Rectangle originalBounds;
		}

		internal enum LineType
		{
			Standard,
			Margin,
			Padding,
			Baseline
		}

		internal enum PaddingLineType
		{
			None,
			PaddingRight,
			PaddingLeft,
			PaddingTop,
			PaddingBottom
		}
	}
}
