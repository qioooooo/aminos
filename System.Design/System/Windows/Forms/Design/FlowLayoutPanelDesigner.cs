using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class FlowLayoutPanelDesigner : FlowPanelDesigner
	{
		public FlowLayoutPanelDesigner()
		{
			this.commonSizes = new ArrayList();
			this.oldP1 = (this.oldP2 = Point.Empty);
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
		}

		protected override bool AllowGenericDragBox
		{
			get
			{
				return false;
			}
		}

		protected internal override bool AllowSetChildIndexOnDrop
		{
			get
			{
				return false;
			}
		}

		private new FlowLayoutPanel Control
		{
			get
			{
				return base.Control as FlowLayoutPanel;
			}
		}

		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.InheritanceAttribute == InheritanceAttribute.Inherited || base.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		private FlowDirection FlowDirection
		{
			get
			{
				return this.Control.FlowDirection;
			}
			set
			{
				if (value != this.Control.FlowDirection)
				{
					base.BehaviorService.Invalidate(base.BehaviorService.ControlRectInAdornerWindow(this.Control));
					this.Control.FlowDirection = value;
				}
			}
		}

		private bool HorizontalFlow
		{
			get
			{
				return this.Control.FlowDirection == FlowDirection.RightToLeft || this.Control.FlowDirection == FlowDirection.LeftToRight;
			}
		}

		private FlowDirection RTLTranslateFlowDirection(FlowDirection direction)
		{
			if (this.Control.RightToLeft == RightToLeft.No)
			{
				return direction;
			}
			switch (direction)
			{
			case FlowDirection.LeftToRight:
				return FlowDirection.RightToLeft;
			case FlowDirection.TopDown:
			case FlowDirection.BottomUp:
				return direction;
			case FlowDirection.RightToLeft:
				return FlowDirection.LeftToRight;
			default:
				return direction;
			}
		}

		private Rectangle GetMarginBounds(Control control)
		{
			return new Rectangle(control.Bounds.Left - ((this.Control.RightToLeft == RightToLeft.No) ? control.Margin.Left : control.Margin.Right), control.Bounds.Top - control.Margin.Top, control.Bounds.Width + control.Margin.Horizontal, control.Bounds.Height + control.Margin.Vertical);
		}

		private void CreateMarginBoundsList()
		{
			this.commonSizes.Clear();
			if (this.Control.Controls.Count == 0)
			{
				this.childInfo = new FlowLayoutPanelDesigner.ChildInfo[0];
				return;
			}
			this.childInfo = new FlowLayoutPanelDesigner.ChildInfo[this.Control.Controls.Count];
			Point point = this.Control.PointToScreen(Point.Empty);
			FlowDirection flowDirection = this.RTLTranslateFlowDirection(this.Control.FlowDirection);
			bool horizontalFlow = this.HorizontalFlow;
			int num = int.MaxValue;
			int num2 = -1;
			int num3 = -1;
			if ((horizontalFlow && flowDirection == FlowDirection.RightToLeft) || (!horizontalFlow && flowDirection == FlowDirection.BottomUp))
			{
				num3 = int.MaxValue;
			}
			bool flag = this.Control.RightToLeft == RightToLeft.Yes;
			int i;
			for (i = 0; i < this.Control.Controls.Count; i++)
			{
				Control control = this.Control.Controls[i];
				Rectangle marginBounds = this.GetMarginBounds(control);
				Rectangle bounds = control.Bounds;
				if (horizontalFlow)
				{
					bounds.X -= ((!flag) ? control.Margin.Left : control.Margin.Right);
					bounds.Width += control.Margin.Horizontal;
					bounds.Height--;
				}
				else
				{
					bounds.Y -= control.Margin.Top;
					bounds.Height += control.Margin.Vertical;
					bounds.Width--;
				}
				marginBounds.Offset(point.X, point.Y);
				bounds.Offset(point.X, point.Y);
				this.childInfo[i].marginBounds = marginBounds;
				this.childInfo[i].controlBounds = bounds;
				this.childInfo[i].inSelectionColl = false;
				if (this.dragControls != null && this.dragControls.Contains(control))
				{
					this.childInfo[i].inSelectionColl = true;
				}
				if (horizontalFlow)
				{
					if (((flowDirection == FlowDirection.LeftToRight) ? (marginBounds.X < num3) : (marginBounds.X > num3)) && num > 0 && num2 > 0)
					{
						this.commonSizes.Add(new Rectangle(num, num2, num2 - num, i));
						num = int.MaxValue;
						num2 = -1;
					}
					num3 = marginBounds.X;
					if (marginBounds.Top < num)
					{
						num = marginBounds.Top;
					}
					if (marginBounds.Bottom > num2)
					{
						num2 = marginBounds.Bottom;
					}
				}
				else
				{
					if (((flowDirection == FlowDirection.TopDown) ? (marginBounds.Y < num3) : (marginBounds.Y > num3)) && num > 0 && num2 > 0)
					{
						this.commonSizes.Add(new Rectangle(num, num2, num2 - num, i));
						num = int.MaxValue;
						num2 = -1;
					}
					num3 = marginBounds.Y;
					if (marginBounds.Left < num)
					{
						num = marginBounds.Left;
					}
					if (marginBounds.Right > num2)
					{
						num2 = marginBounds.Right;
					}
				}
			}
			if (num > 0 && num2 > 0)
			{
				this.commonSizes.Add(new Rectangle(num, num2, num2 - num, i));
			}
			int j = 0;
			for (i = 0; i < this.commonSizes.Count; i++)
			{
				while (j < ((Rectangle)this.commonSizes[i]).Height)
				{
					if (horizontalFlow)
					{
						this.childInfo[j].marginBounds.Y = ((Rectangle)this.commonSizes[i]).X;
						this.childInfo[j].marginBounds.Height = ((Rectangle)this.commonSizes[i]).Width;
					}
					else
					{
						this.childInfo[j].marginBounds.X = ((Rectangle)this.commonSizes[i]).X;
						this.childInfo[j].marginBounds.Width = ((Rectangle)this.commonSizes[i]).Width;
					}
					j++;
				}
			}
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				for (int i = 0; i < this.Control.Controls.Count; i++)
				{
					TypeDescriptor.AddAttributes(this.Control.Controls[i], new Attribute[] { InheritanceAttribute.InheritedReadOnly });
				}
			}
		}

		private void OnChildControlAdded(object sender, ControlEventArgs e)
		{
			if (this.insertIndex != FlowLayoutPanelDesigner.InvalidIndex)
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Control)["Controls"];
					if (componentChangeService != null && propertyDescriptor != null)
					{
						componentChangeService.OnComponentChanging(this.Control, propertyDescriptor);
						this.Control.Controls.SetChildIndex(e.Control, this.insertIndex);
						this.insertIndex++;
						componentChangeService.OnComponentChanged(this.Control, propertyDescriptor, null, null);
					}
				}
			}
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			bool flag = false;
			if (this.dragControls != null && this.primaryDragControl != null && this.Control.Controls.Contains(this.primaryDragControl))
			{
				flag = true;
			}
			if (!flag)
			{
				if (this.Control != null)
				{
					this.Control.ControlAdded += this.OnChildControlAdded;
				}
				try
				{
					base.OnDragDrop(de);
					goto IL_0500;
				}
				finally
				{
					if (this.Control != null)
					{
						this.Control.ControlAdded -= this.OnChildControlAdded;
					}
				}
			}
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null)
			{
				DesignerTransaction designerTransaction = null;
				bool flag2 = de.Effect == DragDropEffects.Copy;
				ArrayList arrayList = null;
				ISelectionService selectionService = null;
				string text2;
				if (this.dragControls.Count == 1)
				{
					string text = TypeDescriptor.GetComponentName(this.dragControls[0]);
					if (text == null || text.Length == 0)
					{
						text = this.dragControls[0].GetType().Name;
					}
					text2 = SR.GetString(flag2 ? "BehaviorServiceCopyControl" : "BehaviorServiceMoveControl", new object[] { text });
				}
				else
				{
					text2 = SR.GetString(flag2 ? "BehaviorServiceCopyControls" : "BehaviorServiceMoveControls", new object[] { this.dragControls.Count });
				}
				designerTransaction = designerHost.CreateTransaction(text2);
				try
				{
					while (this.insertIndex < this.childInfo.Length - 1 && this.childInfo[this.insertIndex].inSelectionColl)
					{
						this.insertIndex++;
					}
					IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Control)["Controls"];
					Control control = null;
					if (this.insertIndex != this.childInfo.Length)
					{
						control = this.Control.Controls[this.insertIndex];
					}
					else
					{
						this.insertIndex = -1;
					}
					if (componentChangeService != null && propertyDescriptor != null)
					{
						componentChangeService.OnComponentChanging(this.Control, propertyDescriptor);
					}
					if (!flag2)
					{
						for (int i = 0; i < this.dragControls.Count; i++)
						{
							this.Control.Controls.Remove(this.dragControls[i] as Control);
						}
						if (control != null)
						{
							this.insertIndex = this.Control.Controls.GetChildIndex(control, false);
						}
					}
					else
					{
						ArrayList arrayList2 = new ArrayList();
						for (int j = 0; j < this.dragControls.Count; j++)
						{
							arrayList2.Add(this.dragControls[j]);
						}
						arrayList2 = DesignerUtils.CopyDragObjects(arrayList2, base.Component.Site) as ArrayList;
						if (arrayList2 == null)
						{
							return;
						}
						arrayList = new ArrayList();
						for (int k = 0; k < arrayList2.Count; k++)
						{
							arrayList.Add(this.dragControls[k]);
							if (this.primaryDragControl.Equals(this.dragControls[k] as Control))
							{
								this.primaryDragControl = arrayList2[k] as Control;
							}
							this.dragControls[k] = arrayList2[k];
						}
						selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					}
					if (this.insertIndex == -1)
					{
						this.insertIndex = this.Control.Controls.Count;
					}
					this.Control.Controls.Add(this.primaryDragControl);
					this.Control.Controls.SetChildIndex(this.primaryDragControl, this.insertIndex);
					this.insertIndex++;
					if (selectionService != null)
					{
						selectionService.SetSelectedComponents(new object[] { this.primaryDragControl }, SelectionTypes.Replace | SelectionTypes.Click);
					}
					for (int l = this.dragControls.Count - 1; l >= 0; l--)
					{
						if (!this.primaryDragControl.Equals(this.dragControls[l] as Control))
						{
							this.Control.Controls.Add(this.dragControls[l] as Control);
							this.Control.Controls.SetChildIndex(this.dragControls[l] as Control, this.insertIndex);
							this.insertIndex++;
							if (selectionService != null)
							{
								selectionService.SetSelectedComponents(new object[] { this.dragControls[l] }, SelectionTypes.Add);
							}
						}
					}
					if (componentChangeService != null && propertyDescriptor != null)
					{
						componentChangeService.OnComponentChanged(this.Control, propertyDescriptor, null, null);
					}
					if (arrayList != null)
					{
						for (int m = 0; m < arrayList.Count; m++)
						{
							this.dragControls[m] = arrayList[m];
						}
					}
					base.OnDragComplete(de);
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
						designerTransaction = null;
					}
				}
				finally
				{
					if (designerTransaction != null)
					{
						designerTransaction.Cancel();
					}
				}
			}
			IL_0500:
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
		}

		protected override void OnDragLeave(EventArgs e)
		{
			this.EraseIBar();
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
			this.primaryDragControl = null;
			if (this.dragControls != null)
			{
				this.dragControls.Clear();
			}
			base.OnDragLeave(e);
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			base.OnDragEnter(de);
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
			this.lastMouseLoc = Point.Empty;
			this.primaryDragControl = null;
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				int num = -1;
				this.dragControls = behaviorDataObject.GetSortedDragControls(ref num);
				this.primaryDragControl = this.dragControls[num] as Control;
			}
			this.CreateMarginBoundsList();
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			base.OnDragOver(de);
			Point mousePosition = global::System.Windows.Forms.Control.MousePosition;
			if (mousePosition.Equals(this.lastMouseLoc) || this.childInfo == null || this.childInfo.Length == 0 || this.commonSizes.Count == 0)
			{
				return;
			}
			Rectangle rectangle = Rectangle.Empty;
			this.lastMouseLoc = mousePosition;
			Point point = this.Control.PointToScreen(new Point(0, 0));
			if (this.Control.RightToLeft == RightToLeft.Yes)
			{
				point.X += this.Control.Width;
			}
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
			int i;
			for (i = 0; i < this.childInfo.Length; i++)
			{
				if (this.childInfo[i].marginBounds.Contains(mousePosition))
				{
					rectangle = this.childInfo[i].controlBounds;
					break;
				}
			}
			if (!rectangle.IsEmpty)
			{
				this.insertIndex = i;
				if (this.childInfo[i].inSelectionColl)
				{
					this.EraseIBar();
				}
				else
				{
					FlowDirection flowDirection = this.RTLTranslateFlowDirection(this.Control.FlowDirection);
					if (flowDirection == FlowDirection.LeftToRight)
					{
						this.ReDrawIBar(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Left, rectangle.Bottom));
					}
					else if (flowDirection == FlowDirection.RightToLeft)
					{
						this.ReDrawIBar(new Point(rectangle.Right, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
					}
					else if (flowDirection == FlowDirection.TopDown)
					{
						this.ReDrawIBar(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Top));
					}
					else if (flowDirection == FlowDirection.BottomUp)
					{
						this.ReDrawIBar(new Point(rectangle.Left, rectangle.Bottom), new Point(rectangle.Right, rectangle.Bottom));
					}
				}
			}
			else
			{
				int num = (this.HorizontalFlow ? point.Y : point.X);
				bool flag = this.Control.RightToLeft == RightToLeft.Yes;
				i = 0;
				while (i < this.commonSizes.Count)
				{
					if (flag)
					{
						num -= ((Rectangle)this.commonSizes[i]).Width;
					}
					else
					{
						num += ((Rectangle)this.commonSizes[i]).Width;
					}
					bool flag2;
					if (!flag)
					{
						flag2 = (this.HorizontalFlow ? mousePosition.Y : mousePosition.X) <= num;
					}
					else
					{
						flag2 = (this.HorizontalFlow && mousePosition.Y <= num) || (!this.HorizontalFlow && mousePosition.X >= num);
					}
					if (flag2)
					{
						this.insertIndex = ((Rectangle)this.commonSizes[i]).Height;
						rectangle = this.childInfo[this.insertIndex - 1].controlBounds;
						if (this.childInfo[this.insertIndex - 1].inSelectionColl)
						{
							this.EraseIBar();
							break;
						}
						FlowDirection flowDirection2 = this.RTLTranslateFlowDirection(this.Control.FlowDirection);
						if (flowDirection2 == FlowDirection.LeftToRight)
						{
							this.ReDrawIBar(new Point(rectangle.Right, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
							break;
						}
						if (flowDirection2 == FlowDirection.RightToLeft)
						{
							this.ReDrawIBar(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Left, rectangle.Bottom));
							break;
						}
						if (flowDirection2 == FlowDirection.TopDown)
						{
							this.ReDrawIBar(new Point(rectangle.Left, rectangle.Bottom), new Point(rectangle.Right, rectangle.Bottom));
							break;
						}
						if (flowDirection2 == FlowDirection.BottomUp)
						{
							this.ReDrawIBar(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Top));
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			if (this.insertIndex == FlowLayoutPanelDesigner.InvalidIndex)
			{
				this.insertIndex = this.Control.Controls.Count;
				this.EraseIBar();
			}
		}

		private void EraseIBar()
		{
			this.ReDrawIBar(Point.Empty, Point.Empty);
		}

		private void ReDrawIBar(Point p1, Point p2)
		{
			Point point = base.BehaviorService.AdornerWindowToScreen();
			Pen pen = SystemPens.ControlText;
			if (this.Control.BackColor != Color.Empty && (double)this.Control.BackColor.GetBrightness() < 0.5)
			{
				pen = SystemPens.ControlLight;
			}
			if (p1 != Point.Empty)
			{
				p1.Offset(-point.X, -point.Y);
				p2.Offset(-point.X, -point.Y);
			}
			if (p1 != this.oldP1 && p2 != this.oldP2 && this.oldP1 != Point.Empty)
			{
				Rectangle rectangle = new Rectangle(this.oldP1.X, this.oldP1.Y, this.oldP2.X - this.oldP1.X + 1, this.oldP2.Y - this.oldP1.Y + 1);
				rectangle.Inflate(this.maxIBarWidth, this.maxIBarWidth);
				base.BehaviorService.Invalidate(rectangle);
			}
			this.oldP1 = p1;
			this.oldP2 = p2;
			if (p1 != Point.Empty)
			{
				using (Graphics adornerWindowGraphics = base.BehaviorService.AdornerWindowGraphics)
				{
					if (this.HorizontalFlow)
					{
						if (Math.Abs(p1.Y - p2.Y) <= 10)
						{
							adornerWindowGraphics.DrawLine(pen, p1, p2);
							adornerWindowGraphics.DrawLine(pen, p1.X - 2, p1.Y, p1.X + 2, p1.Y);
							adornerWindowGraphics.DrawLine(pen, p2.X - 2, p2.Y, p2.X + 2, p2.Y);
						}
						else
						{
							for (int i = 0; i < 2; i++)
							{
								adornerWindowGraphics.DrawLine(pen, p1.X - (4 - i * 2) / 2, p1.Y + i, p1.X + (4 - i * 2) / 2, p1.Y + i);
								adornerWindowGraphics.DrawLine(pen, p2.X - (4 - i * 2) / 2, p2.Y - i, p2.X + (4 - i * 2) / 2, p2.Y - i);
							}
							adornerWindowGraphics.DrawLine(pen, p1.X, p1.Y, p1.X, p1.Y + 3 - 1);
							adornerWindowGraphics.DrawLine(pen, p2.X, p2.Y, p2.X, p2.Y - 3 + 1);
							adornerWindowGraphics.DrawLine(pen, p1.X, p1.Y + 5, p2.X, p2.Y - 5);
						}
					}
					else if (Math.Abs(p1.X - p2.X) <= 10)
					{
						adornerWindowGraphics.DrawLine(pen, p1, p2);
						adornerWindowGraphics.DrawLine(pen, p1.X, p1.Y - 2, p1.X, p1.Y + 2);
						adornerWindowGraphics.DrawLine(pen, p2.X, p2.Y - 2, p2.X, p2.Y + 2);
					}
					else
					{
						for (int j = 0; j < 2; j++)
						{
							adornerWindowGraphics.DrawLine(pen, p1.X + j, p1.Y - (4 - j * 2) / 2, p1.X + j, p1.Y + (4 - j * 2) / 2);
							adornerWindowGraphics.DrawLine(pen, p2.X - j, p2.Y - (4 - j * 2) / 2, p2.X - j, p2.Y + (4 - j * 2) / 2);
						}
						adornerWindowGraphics.DrawLine(pen, p1.X, p1.Y, p1.X + 3 - 1, p1.Y);
						adornerWindowGraphics.DrawLine(pen, p2.X, p2.Y, p2.X - 3 + 1, p2.Y);
						adornerWindowGraphics.DrawLine(pen, p1.X + 5, p1.Y, p2.X - 5, p2.Y);
					}
				}
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "FlowDirection" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(FlowLayoutPanelDesigner), propertyDescriptor, array2);
				}
			}
		}

		private const int iBarHalfSize = 2;

		private const int minIBar = 10;

		private const int iBarHatHeight = 3;

		private const int iBarSpace = 2;

		private const int iBarLineOffset = 5;

		private const int iBarHatWidth = 5;

		private FlowLayoutPanelDesigner.ChildInfo[] childInfo;

		private ArrayList dragControls;

		private Control primaryDragControl;

		private ArrayList commonSizes;

		private int insertIndex;

		private Point lastMouseLoc;

		private Point oldP1;

		private Point oldP2;

		private static readonly int InvalidIndex = -1;

		private int maxIBarWidth = Math.Max(2, 2);

		private struct ChildInfo
		{
			public Rectangle marginBounds;

			public Rectangle controlBounds;

			public bool inSelectionColl;
		}
	}
}
