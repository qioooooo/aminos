using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class DropSourceBehavior : Behavior, IComparer
	{
		internal DropSourceBehavior(ICollection dragComponents, Control source, Point initialMouseLocation)
		{
			this.serviceProviderSource = source.Site;
			if (this.serviceProviderSource == null)
			{
				return;
			}
			this.behaviorServiceSource = (BehaviorService)this.serviceProviderSource.GetService(typeof(BehaviorService));
			if (this.behaviorServiceSource == null)
			{
				return;
			}
			if (dragComponents == null || dragComponents.Count <= 0)
			{
				return;
			}
			this.srcHost = (IDesignerHost)this.serviceProviderSource.GetService(typeof(IDesignerHost));
			if (this.srcHost == null)
			{
				return;
			}
			this.data = new DropSourceBehavior.BehaviorDataObject(dragComponents, source, this);
			this.allowedEffects = DragDropEffects.Copy | DragDropEffects.Move;
			this.dragComponents = new DropSourceBehavior.DragComponent[dragComponents.Count];
			this.parentGridSize = Size.Empty;
			this.lastEffect = DragDropEffects.None;
			this.lastFeedbackLocation = new Point(-1, -1);
			this.lastSnapOffset = Point.Empty;
			this.dragImageRect = Rectangle.Empty;
			this.clearDragImageRect = Rectangle.Empty;
			this.InitiateDrag(initialMouseLocation, dragComponents);
		}

		internal DragDropEffects AllowedEffects
		{
			get
			{
				return this.allowedEffects;
			}
		}

		internal DataObject DataObject
		{
			get
			{
				return this.data;
			}
		}

		private Point AdjustToGrid(Point dragLoc)
		{
			Point point = new Point(dragLoc.X - this.parentLocation.X, dragLoc.Y - this.parentLocation.Y);
			Point empty = Point.Empty;
			int num = point.X % this.parentGridSize.Width;
			int num2 = point.Y % this.parentGridSize.Height;
			if (num > this.parentGridSize.Width / 2)
			{
				empty.X = this.parentGridSize.Width - num;
			}
			else
			{
				empty.X = -num;
			}
			if (num2 > this.parentGridSize.Height / 2)
			{
				empty.Y = this.parentGridSize.Height - num2;
			}
			else
			{
				empty.Y = -num2;
			}
			return empty;
		}

		private Point MapPointFromSourceToTarget(Point pt)
		{
			if (this.srcHost != this.destHost && this.destHost != null)
			{
				pt = this.behaviorServiceSource.AdornerWindowPointToScreen(pt);
				return this.behaviorServiceTarget.MapAdornerWindowPoint(IntPtr.Zero, pt);
			}
			return pt;
		}

		private Point MapPointFromTargetToSource(Point pt)
		{
			if (this.srcHost != this.destHost && this.destHost != null)
			{
				pt = this.behaviorServiceTarget.AdornerWindowPointToScreen(pt);
				return this.behaviorServiceSource.MapAdornerWindowPoint(IntPtr.Zero, pt);
			}
			return pt;
		}

		private void ClearAllDragImages()
		{
			if (this.dragImageRect != Rectangle.Empty)
			{
				Rectangle rectangle = this.dragImageRect;
				rectangle.Location = this.MapPointFromSourceToTarget(rectangle.Location);
				if (this.graphicsTarget != null)
				{
					this.graphicsTarget.SetClip(rectangle);
				}
				if (this.behaviorServiceTarget != null)
				{
					this.behaviorServiceTarget.Invalidate(rectangle);
				}
				if (this.graphicsTarget != null)
				{
					this.graphicsTarget.ResetClip();
				}
			}
		}

		private void SetDesignerHost(Control c)
		{
			foreach (object obj in c.Controls)
			{
				Control control = (Control)obj;
				this.SetDesignerHost(control);
			}
			if (c.Site != null && !(c.Site is INestedSite) && this.destHost != null)
			{
				this.destHost.Container.Add(c);
			}
		}

		private void DropControl(int dragComponentIndex, Control dragTarget, Control dragSource, bool localDrag)
		{
			Control control = this.dragComponents[dragComponentIndex].dragComponent as Control;
			if (this.lastEffect == DragDropEffects.Copy || (this.srcHost != this.destHost && this.destHost != null))
			{
				control.Visible = true;
				bool flag = true;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["Visible"];
				if (propertyDescriptor != null)
				{
					flag = (bool)propertyDescriptor.GetValue(control);
				}
				this.SetDesignerHost(control);
				control.Parent = dragTarget;
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(control, flag);
					return;
				}
			}
			else if (!localDrag && control.Parent.Equals(dragSource))
			{
				dragSource.Controls.Remove(control);
				control.Visible = true;
				dragTarget.Controls.Add(control);
			}
		}

		private void SetLocationPropertyAndChildIndex(int dragComponentIndex, Control dragTarget, Point dropPoint, int newIndex, bool allowSetChildIndexOnDrop)
		{
			Control control = this.dragComponents[dragComponentIndex].dragComponent as Control;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.dragComponents[dragComponentIndex].dragComponent)["Location"];
			if (propertyDescriptor != null && control != null)
			{
				Point point = new Point(dropPoint.X, dropPoint.Y);
				ScrollableControl scrollableControl = control.Parent as ScrollableControl;
				if (scrollableControl != null)
				{
					Point autoScrollPosition = scrollableControl.AutoScrollPosition;
					point.Offset(-autoScrollPosition.X, -autoScrollPosition.Y);
				}
				propertyDescriptor.SetValue(control, point);
				if (allowSetChildIndexOnDrop)
				{
					dragTarget.Controls.SetChildIndex(control, newIndex);
				}
			}
		}

		private void EndDragDrop(bool allowSetChildIndexOnDrop)
		{
			Control control = this.data.Target as Control;
			if (control == null)
			{
				return;
			}
			if (this.serviceProviderTarget == null)
			{
				this.serviceProviderTarget = control.Site;
				if (this.serviceProviderTarget == null)
				{
					return;
				}
			}
			if (this.destHost == null)
			{
				this.destHost = (IDesignerHost)this.serviceProviderTarget.GetService(typeof(IDesignerHost));
				if (this.destHost == null)
				{
					return;
				}
			}
			if (this.behaviorServiceTarget == null)
			{
				this.behaviorServiceTarget = (BehaviorService)this.serviceProviderTarget.GetService(typeof(BehaviorService));
				if (this.behaviorServiceTarget == null)
				{
					return;
				}
			}
			ArrayList arrayList = null;
			bool flag = this.lastEffect == DragDropEffects.Copy;
			Control source = this.data.Source;
			bool flag2 = source.Equals(control);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["Controls"];
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(source)["Controls"];
			IComponentChangeService componentChangeService = (IComponentChangeService)this.serviceProviderSource.GetService(typeof(IComponentChangeService));
			IComponentChangeService componentChangeService2 = (IComponentChangeService)this.serviceProviderTarget.GetService(typeof(IComponentChangeService));
			if (this.dragAssistanceManager != null)
			{
				this.dragAssistanceManager.OnMouseUp();
			}
			ISelectionService selectionService = null;
			if (flag || (this.srcHost != this.destHost && this.destHost != null))
			{
				selectionService = (ISelectionService)this.serviceProviderTarget.GetService(typeof(ISelectionService));
			}
			try
			{
				if (this.dragComponents != null && this.dragComponents.Length > 0)
				{
					DesignerTransaction designerTransaction = null;
					DesignerTransaction designerTransaction2 = null;
					string text2;
					if (this.dragComponents.Length == 1)
					{
						string text = TypeDescriptor.GetComponentName(this.dragComponents[0].dragComponent);
						if (text == null || text.Length == 0)
						{
							text = this.dragComponents[0].dragComponent.GetType().Name;
						}
						text2 = SR.GetString(flag ? "BehaviorServiceCopyControl" : "BehaviorServiceMoveControl", new object[] { text });
					}
					else
					{
						text2 = SR.GetString(flag ? "BehaviorServiceCopyControls" : "BehaviorServiceMoveControls", new object[] { this.dragComponents.Length });
					}
					if (this.srcHost != null && (this.srcHost == this.destHost || this.destHost == null || !flag))
					{
						designerTransaction = this.srcHost.CreateTransaction(text2);
					}
					if (this.srcHost != this.destHost && this.destHost != null)
					{
						designerTransaction2 = this.destHost.CreateTransaction(text2);
					}
					try
					{
						ComponentTray componentTray = null;
						int num = 0;
						if (flag)
						{
							componentTray = this.serviceProviderTarget.GetService(typeof(ComponentTray)) as ComponentTray;
							num = ((componentTray != null) ? componentTray.Controls.Count : 0);
							ArrayList arrayList2 = new ArrayList();
							for (int i = 0; i < this.dragComponents.Length; i++)
							{
								arrayList2.Add(this.dragComponents[i].dragComponent);
							}
							arrayList2 = DesignerUtils.CopyDragObjects(arrayList2, this.serviceProviderTarget) as ArrayList;
							if (arrayList2 == null)
							{
								return;
							}
							arrayList = new ArrayList();
							for (int j = 0; j < arrayList2.Count; j++)
							{
								arrayList.Add(this.dragComponents[j].dragComponent);
								this.dragComponents[j].dragComponent = arrayList2[j];
							}
						}
						if ((!flag2 || flag) && componentChangeService != null && componentChangeService2 != null)
						{
							componentChangeService2.OnComponentChanging(control, propertyDescriptor);
							if (!flag)
							{
								componentChangeService.OnComponentChanging(source, propertyDescriptor2);
							}
						}
						int num2 = ParentControlDesigner.DetermineTopChildIndex(control);
						this.DropControl(this.primaryComponentIndex, control, source, flag2);
						Point point = this.behaviorServiceSource.AdornerWindowPointToScreen(this.dragComponents[this.primaryComponentIndex].draggedLocation);
						point = ((Control)this.dragComponents[this.primaryComponentIndex].dragComponent).Parent.PointToClient(point);
						if (((Control)this.dragComponents[this.primaryComponentIndex].dragComponent).Parent.IsMirrored)
						{
							point.Offset(-((Control)this.dragComponents[this.primaryComponentIndex].dragComponent).Width, 0);
						}
						Control control2 = this.dragComponents[this.primaryComponentIndex].dragComponent as Control;
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(control2)["Location"];
						if (control2 != null && propertyDescriptor3 != null)
						{
							try
							{
								componentChangeService2.OnComponentChanging(control2, propertyDescriptor3);
							}
							catch (CheckoutException ex)
							{
								if (ex == CheckoutException.Canceled)
								{
									return;
								}
								throw;
							}
						}
						this.SetLocationPropertyAndChildIndex(this.primaryComponentIndex, control, point, this.shareParent ? (num2 + this.dragComponents[this.primaryComponentIndex].zorderIndex) : num2, allowSetChildIndexOnDrop);
						if (selectionService != null)
						{
							selectionService.SetSelectedComponents(new object[] { this.dragComponents[this.primaryComponentIndex].dragComponent }, SelectionTypes.Replace | SelectionTypes.Click);
						}
						for (int k = 0; k < this.dragComponents.Length; k++)
						{
							if (k != this.primaryComponentIndex)
							{
								this.DropControl(k, control, source, flag2);
								Point point2 = new Point(point.X + this.dragComponents[k].positionOffset.X, point.Y + this.dragComponents[k].positionOffset.Y);
								this.SetLocationPropertyAndChildIndex(k, control, point2, this.shareParent ? (num2 + this.dragComponents[k].zorderIndex) : num2, allowSetChildIndexOnDrop);
								if (selectionService != null)
								{
									selectionService.SetSelectedComponents(new object[] { this.dragComponents[k].dragComponent }, SelectionTypes.Add);
								}
							}
						}
						if ((!flag2 || flag) && componentChangeService != null && componentChangeService2 != null)
						{
							componentChangeService2.OnComponentChanged(control, propertyDescriptor, control.Controls, control.Controls);
							if (!flag)
							{
								componentChangeService.OnComponentChanged(source, propertyDescriptor2, source.Controls, source.Controls);
							}
						}
						if (arrayList != null)
						{
							for (int l = 0; l < arrayList.Count; l++)
							{
								this.dragComponents[l].dragComponent = arrayList[l];
							}
							arrayList = null;
						}
						if (flag)
						{
							if (componentTray == null)
							{
								componentTray = this.serviceProviderTarget.GetService(typeof(ComponentTray)) as ComponentTray;
							}
							if (componentTray != null)
							{
								int num3 = componentTray.Controls.Count - num;
								if (num3 > 0)
								{
									ArrayList arrayList3 = new ArrayList();
									for (int m = 0; m < num3; m++)
									{
										arrayList3.Add(componentTray.Controls[num + m]);
									}
									componentTray.UpdatePastePositions(arrayList3);
								}
							}
						}
						this.CleanupDrag(false);
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
							designerTransaction = null;
						}
						if (designerTransaction2 != null)
						{
							designerTransaction2.Commit();
							designerTransaction2 = null;
						}
					}
					finally
					{
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
						}
						if (designerTransaction2 != null)
						{
							designerTransaction2.Cancel();
						}
					}
				}
			}
			finally
			{
				if (arrayList != null)
				{
					for (int n = 0; n < arrayList.Count; n++)
					{
						this.dragComponents[n].dragComponent = arrayList[n];
					}
				}
				this.CleanupDrag(false);
				if (this.statusCommandUITarget != null)
				{
					this.statusCommandUITarget.SetStatusInformation((selectionService == null) ? (this.dragComponents[this.primaryComponentIndex].dragComponent as Component) : (selectionService.PrimarySelection as Component));
				}
			}
			this.lastFeedbackLocation = new Point(-1, -1);
		}

		internal void GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			this.lastEffect = e.Effect;
			if (this.data.Target == null || e.Effect == DragDropEffects.None)
			{
				if (this.clearDragImageRect != this.dragImageRect)
				{
					this.ClearAllDragImages();
					this.clearDragImageRect = this.dragImageRect;
				}
				if (this.dragAssistanceManager != null)
				{
					this.dragAssistanceManager.EraseSnapLines();
				}
				return;
			}
			bool flag = false;
			Point mousePosition = Control.MousePosition;
			bool flag2 = Control.ModifierKeys == Keys.Alt;
			if (flag2 && this.dragAssistanceManager != null)
			{
				this.dragAssistanceManager.EraseSnapLines();
			}
			if (this.data.Target.Equals(this.data.Source) && this.lastEffect != DragDropEffects.Copy)
			{
				e.UseDefaultCursors = false;
				Cursor.Current = Cursors.Default;
			}
			else
			{
				e.UseDefaultCursors = true;
			}
			Control control = this.data.Target as Control;
			if (mousePosition != this.lastFeedbackLocation || (flag2 && this.dragAssistanceManager != null))
			{
				if (!this.data.Target.Equals(this.lastDropTarget))
				{
					this.serviceProviderTarget = control.Site;
					if (this.serviceProviderTarget == null)
					{
						return;
					}
					IDesignerHost designerHost = (IDesignerHost)this.serviceProviderTarget.GetService(typeof(IDesignerHost));
					if (designerHost == null)
					{
						return;
					}
					this.targetAllowsSnapLines = true;
					ControlDesigner controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
					if (controlDesigner != null && !controlDesigner.ParticipatesWithSnapLines)
					{
						this.targetAllowsSnapLines = false;
					}
					this.statusCommandUITarget = new StatusCommandUI(this.serviceProviderTarget);
					if (this.lastDropTarget == null || designerHost != this.destHost)
					{
						if (this.destHost != null && this.destHost != this.srcHost)
						{
							foreach (Adorner adorner in this.behaviorServiceTarget.Adorners)
							{
								adorner.Enabled = true;
							}
						}
						this.behaviorServiceTarget = (BehaviorService)this.serviceProviderTarget.GetService(typeof(BehaviorService));
						if (this.behaviorServiceTarget == null)
						{
							return;
						}
						this.GetParentSnapInfo(control, this.behaviorServiceTarget);
						if (designerHost != this.srcHost)
						{
							this.DisableAdorners(this.serviceProviderTarget, this.behaviorServiceTarget, true);
						}
						this.ClearAllDragImages();
						if (this.lastDropTarget != null)
						{
							for (int i = 0; i < this.dragObjects.Count; i++)
							{
								Control control2 = (Control)this.dragObjects[i];
								Rectangle rectangle = this.behaviorServiceSource.ControlRectInAdornerWindow(control2);
								rectangle.Location = this.behaviorServiceSource.AdornerWindowPointToScreen(rectangle.Location);
								rectangle.Location = this.behaviorServiceTarget.MapAdornerWindowPoint(IntPtr.Zero, rectangle.Location);
								if (i == 0)
								{
									if (this.dragImageRegion != null)
									{
										this.dragImageRegion.Dispose();
									}
									this.dragImageRegion = new Region(rectangle);
								}
								else
								{
									this.dragImageRegion.Union(rectangle);
								}
							}
						}
						if (this.graphicsTarget != null)
						{
							this.graphicsTarget.Dispose();
						}
						this.graphicsTarget = this.behaviorServiceTarget.AdornerWindowGraphics;
						flag = true;
						this.destHost = designerHost;
					}
					this.lastDropTarget = this.data.Target;
				}
				if (this.ShowHideDragControls(this.lastEffect == DragDropEffects.Copy) && !flag)
				{
					flag = true;
				}
				if (flag && this.behaviorServiceTarget.UseSnapLines)
				{
					if (this.dragAssistanceManager != null)
					{
						this.dragAssistanceManager.EraseSnapLines();
					}
					this.dragAssistanceManager = new DragAssistanceManager(this.serviceProviderTarget, this.graphicsTarget, this.dragObjects, null, this.lastEffect == DragDropEffects.Copy);
				}
				Point point = new Point(mousePosition.X - this.initialMouseLoc.X + this.dragComponents[this.primaryComponentIndex].originalControlLocation.X, mousePosition.Y - this.initialMouseLoc.Y + this.dragComponents[this.primaryComponentIndex].originalControlLocation.Y);
				point = this.MapPointFromSourceToTarget(point);
				Rectangle rectangle2 = new Rectangle(point.X, point.Y, this.dragComponents[this.primaryComponentIndex].dragImage.Width, this.dragComponents[this.primaryComponentIndex].dragImage.Height);
				if (this.dragAssistanceManager != null)
				{
					if (this.targetAllowsSnapLines && !flag2)
					{
						this.lastSnapOffset = this.dragAssistanceManager.OnMouseMove(rectangle2);
					}
					else
					{
						this.dragAssistanceManager.OnMouseMove(new Rectangle(-100, -100, 0, 0));
					}
				}
				else if (!this.parentGridSize.IsEmpty)
				{
					this.lastSnapOffset = this.AdjustToGrid(point);
				}
				point.X += this.lastSnapOffset.X;
				point.Y += this.lastSnapOffset.Y;
				this.dragComponents[this.primaryComponentIndex].draggedLocation = this.MapPointFromTargetToSource(point);
				Rectangle rectangle3 = this.dragImageRect;
				point = new Point(mousePosition.X - this.initialMouseLoc.X + this.originalDragImageLocation.X, mousePosition.Y - this.initialMouseLoc.Y + this.originalDragImageLocation.Y);
				point.X += this.lastSnapOffset.X;
				point.Y += this.lastSnapOffset.Y;
				this.dragImageRect.Location = point;
				rectangle3.Location = this.MapPointFromSourceToTarget(rectangle3.Location);
				Rectangle rectangle4 = this.dragImageRect;
				rectangle4.Location = this.MapPointFromSourceToTarget(rectangle4.Location);
				Rectangle rectangle5 = Rectangle.Union(rectangle4, rectangle3);
				Region region = new Region(rectangle5);
				region.Exclude(rectangle4);
				using (Region region2 = this.dragImageRegion.Clone())
				{
					region2.Translate(mousePosition.X - this.initialMouseLoc.X + this.lastSnapOffset.X, mousePosition.Y - this.initialMouseLoc.Y + this.lastSnapOffset.Y);
					region2.Complement(rectangle4);
					region2.Union(region);
					this.behaviorServiceTarget.Invalidate(region2);
				}
				region.Dispose();
				if (this.graphicsTarget != null)
				{
					this.graphicsTarget.SetClip(rectangle4);
					this.graphicsTarget.DrawImage(this.dragImage, rectangle4.X, rectangle4.Y);
					this.graphicsTarget.ResetClip();
				}
				Control control3 = this.dragComponents[this.primaryComponentIndex].dragComponent as Control;
				if (control3 != null)
				{
					Point point2 = this.behaviorServiceSource.AdornerWindowPointToScreen(this.dragComponents[this.primaryComponentIndex].draggedLocation);
					point2 = control.PointToClient(point2);
					if (control.IsMirrored && control3.IsMirrored)
					{
						point2.Offset(-control3.Width, 0);
					}
					if (this.statusCommandUITarget != null)
					{
						this.statusCommandUITarget.SetStatusInformation(control3, point2);
					}
				}
				if (this.dragAssistanceManager != null && !flag2 && this.targetAllowsSnapLines)
				{
					this.dragAssistanceManager.RenderSnapLinesInternal();
				}
				this.lastFeedbackLocation = mousePosition;
			}
			this.data.Target = null;
		}

		int IComparer.Compare(object x, object y)
		{
			DropSourceBehavior.DragComponent dragComponent = (DropSourceBehavior.DragComponent)x;
			DropSourceBehavior.DragComponent dragComponent2 = (DropSourceBehavior.DragComponent)y;
			if (dragComponent.zorderIndex > dragComponent2.zorderIndex)
			{
				return -1;
			}
			if (dragComponent.zorderIndex < dragComponent2.zorderIndex)
			{
				return 1;
			}
			return 0;
		}

		private void GetParentSnapInfo(Control parentControl, BehaviorService bhvSvc)
		{
			this.parentGridSize = Size.Empty;
			if (bhvSvc != null && !bhvSvc.UseSnapLines)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parentControl)["SnapToGrid"];
				if (propertyDescriptor != null && (bool)propertyDescriptor.GetValue(parentControl))
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(parentControl)["GridSize"];
					if (propertyDescriptor2 != null)
					{
						Control control = this.dragComponents[this.primaryComponentIndex].dragComponent as Control;
						if (control != null)
						{
							this.parentGridSize = (Size)propertyDescriptor2.GetValue(parentControl);
							this.parentLocation = bhvSvc.MapAdornerWindowPoint(parentControl.Handle, Point.Empty);
							if (parentControl.Parent != null && parentControl.Parent.IsMirrored)
							{
								this.parentLocation.Offset(-parentControl.Width, 0);
							}
						}
					}
				}
			}
		}

		private void DisableAdorners(IServiceProvider serviceProvider, BehaviorService behaviorService, bool hostChange)
		{
			Adorner adorner = null;
			SelectionManager selectionManager = (SelectionManager)serviceProvider.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				adorner = selectionManager.BodyGlyphAdorner;
			}
			foreach (Adorner adorner2 in behaviorService.Adorners)
			{
				if (adorner == null || !adorner2.Equals(adorner))
				{
					adorner2.Enabled = false;
				}
			}
			if (hostChange)
			{
				selectionManager.OnBeginDrag(new BehaviorDragDropEventArgs(this.dragObjects));
			}
		}

		private void InitiateDrag(Point initialMouseLocation, ICollection dragComps)
		{
			this.dragObjects = new ArrayList(dragComps);
			this.DisableAdorners(this.serviceProviderSource, this.behaviorServiceSource, false);
			Control control = this.dragObjects[0] as Control;
			Control control2 = ((control != null) ? control.Parent : null);
			Color color = ((control2 != null) ? control2.BackColor : Color.Empty);
			this.dragImageRect = Rectangle.Empty;
			this.clearDragImageRect = Rectangle.Empty;
			this.initialMouseLoc = initialMouseLocation;
			for (int i = 0; i < this.dragObjects.Count; i++)
			{
				Control control3 = (Control)this.dragObjects[i];
				this.dragComponents[i].dragComponent = this.dragObjects[i];
				this.dragComponents[i].positionOffset = new Point(control3.Location.X - control.Location.X, control3.Location.Y - control.Location.Y);
				Rectangle rectangle = this.behaviorServiceSource.ControlRectInAdornerWindow(control3);
				if (this.dragImageRect.IsEmpty)
				{
					this.dragImageRect = rectangle;
					this.dragImageRegion = new Region(rectangle);
				}
				else
				{
					this.dragImageRect = Rectangle.Union(this.dragImageRect, rectangle);
					this.dragImageRegion.Union(rectangle);
				}
				this.dragComponents[i].draggedLocation = rectangle.Location;
				this.dragComponents[i].originalControlLocation = this.dragComponents[i].draggedLocation;
				DesignerUtils.GenerateSnapShot(control3, ref this.dragComponents[i].dragImage, (i == 0) ? 2 : 1, 1.0, color);
				if (control2 != null && this.shareParent)
				{
					this.dragComponents[i].zorderIndex = control2.Controls.GetChildIndex(control3, false);
					if (this.dragComponents[i].zorderIndex == -1)
					{
						this.shareParent = false;
					}
				}
			}
			if (this.shareParent)
			{
				Array.Sort(this.dragComponents, this);
			}
			for (int j = 0; j < this.dragComponents.Length; j++)
			{
				if (control.Equals(this.dragComponents[j].dragComponent as Control))
				{
					this.primaryComponentIndex = j;
					break;
				}
			}
			if (control2 != null)
			{
				this.suspendedParent = control2;
				this.suspendedParent.SuspendLayout();
				this.GetParentSnapInfo(this.suspendedParent, this.behaviorServiceSource);
			}
			int num = this.dragImageRect.Width;
			if (num == 0)
			{
				num = 1;
			}
			int num2 = this.dragImageRect.Height;
			if (num2 == 0)
			{
				num2 = 1;
			}
			this.dragImage = new Bitmap(num, num2, PixelFormat.Format32bppPArgb);
			using (Graphics graphics = Graphics.FromImage(this.dragImage))
			{
				graphics.Clear(Color.Chartreuse);
			}
			((Bitmap)this.dragImage).MakeTransparent(Color.Chartreuse);
			using (Graphics graphics2 = Graphics.FromImage(this.dragImage))
			{
				using (SolidBrush solidBrush = new SolidBrush(control.BackColor))
				{
					for (int k = 0; k < this.dragComponents.Length; k++)
					{
						Rectangle rectangle2 = new Rectangle(this.dragComponents[k].draggedLocation.X - this.dragImageRect.X, this.dragComponents[k].draggedLocation.Y - this.dragImageRect.Y, this.dragComponents[k].dragImage.Width, this.dragComponents[k].dragImage.Height);
						graphics2.FillRectangle(solidBrush, rectangle2);
						graphics2.DrawImage(this.dragComponents[k].dragImage, rectangle2, new Rectangle(0, 0, this.dragComponents[k].dragImage.Width, this.dragComponents[k].dragImage.Height), GraphicsUnit.Pixel);
					}
				}
			}
			this.originalDragImageLocation = new Point(this.dragImageRect.X, this.dragImageRect.Y);
			this.ShowHideDragControls(false);
			this.cleanedUpDrag = false;
		}

		internal ArrayList GetSortedDragControls(ref int primaryControlIndex)
		{
			ArrayList arrayList = new ArrayList();
			primaryControlIndex = -1;
			if (this.dragComponents != null && this.dragComponents.Length > 0)
			{
				primaryControlIndex = this.primaryComponentIndex;
				for (int i = 0; i < this.dragComponents.Length; i++)
				{
					arrayList.Add(this.dragComponents[i].dragComponent);
				}
			}
			return arrayList;
		}

		internal void QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (this.behaviorServiceSource != null && this.behaviorServiceSource.CancelDrag)
			{
				e.Action = DragAction.Cancel;
				this.CleanupDrag(true);
				return;
			}
			if (e.Action == DragAction.Continue)
			{
				return;
			}
			if (e.Action == DragAction.Cancel || this.lastEffect == DragDropEffects.None)
			{
				this.CleanupDrag(true);
				e.Action = DragAction.Cancel;
			}
		}

		internal bool ShowHideDragControls(bool show)
		{
			if (this.currentShowState == show)
			{
				return false;
			}
			this.currentShowState = show;
			if (this.dragComponents != null)
			{
				for (int i = 0; i < this.dragComponents.Length; i++)
				{
					Control control = this.dragComponents[i].dragComponent as Control;
					if (control != null)
					{
						control.Visible = show;
					}
				}
			}
			return true;
		}

		internal void CleanupDrag()
		{
			this.CleanupDrag(true);
		}

		internal void CleanupDrag(bool clearImages)
		{
			if (!this.cleanedUpDrag)
			{
				if (clearImages)
				{
					this.ClearAllDragImages();
				}
				this.ShowHideDragControls(true);
				try
				{
					if (this.suspendedParent != null)
					{
						this.suspendedParent.ResumeLayout();
					}
				}
				finally
				{
					this.suspendedParent = null;
					foreach (Adorner adorner in this.behaviorServiceSource.Adorners)
					{
						adorner.Enabled = true;
					}
					if (this.destHost != this.srcHost && this.destHost != null)
					{
						foreach (Adorner adorner2 in this.behaviorServiceTarget.Adorners)
						{
							adorner2.Enabled = true;
						}
						this.behaviorServiceTarget.SyncSelection();
					}
					if (this.behaviorServiceSource != null)
					{
						this.behaviorServiceSource.SyncSelection();
					}
					if (this.dragImageRegion != null)
					{
						this.dragImageRegion.Dispose();
						this.dragImageRegion = null;
					}
					if (this.dragImage != null)
					{
						this.dragImage.Dispose();
						this.dragImage = null;
					}
					if (this.dragComponents != null)
					{
						for (int i = 0; i < this.dragComponents.Length; i++)
						{
							if (this.dragComponents[i].dragImage != null)
							{
								this.dragComponents[i].dragImage.Dispose();
								this.dragComponents[i].dragImage = null;
							}
						}
					}
					if (this.graphicsTarget != null)
					{
						this.graphicsTarget.Dispose();
						this.graphicsTarget = null;
					}
					this.cleanedUpDrag = true;
				}
			}
		}

		private DropSourceBehavior.DragComponent[] dragComponents;

		private ArrayList dragObjects;

		private DropSourceBehavior.BehaviorDataObject data;

		private DragDropEffects allowedEffects;

		private DragDropEffects lastEffect;

		private bool targetAllowsSnapLines;

		private IComponent lastDropTarget;

		private Point lastSnapOffset;

		private BehaviorService behaviorServiceSource;

		private BehaviorService behaviorServiceTarget;

		private DragAssistanceManager dragAssistanceManager;

		private Graphics graphicsTarget;

		private IServiceProvider serviceProviderSource;

		private IServiceProvider serviceProviderTarget;

		private Point initialMouseLoc;

		private Image dragImage;

		private Rectangle dragImageRect;

		private Rectangle clearDragImageRect;

		private Point originalDragImageLocation;

		private Region dragImageRegion;

		private Point lastFeedbackLocation;

		private Control suspendedParent;

		private Size parentGridSize;

		private Point parentLocation;

		private bool shareParent = true;

		private bool cleanedUpDrag;

		private StatusCommandUI statusCommandUITarget;

		private IDesignerHost srcHost;

		private IDesignerHost destHost;

		private bool currentShowState = true;

		private int primaryComponentIndex = -1;

		private struct DragComponent
		{
			public object dragComponent;

			public int zorderIndex;

			public Point originalControlLocation;

			public Point draggedLocation;

			public Image dragImage;

			public Point positionOffset;
		}

		internal class BehaviorDataObject : DataObject
		{
			public BehaviorDataObject(ICollection dragComponents, Control source, DropSourceBehavior sourceBehavior)
			{
				this.dragComponents = dragComponents;
				this.source = source;
				this.sourceBehavior = sourceBehavior;
				this.target = null;
			}

			public Control Source
			{
				get
				{
					return this.source;
				}
			}

			public ICollection DragComponents
			{
				get
				{
					return this.dragComponents;
				}
			}

			public IComponent Target
			{
				get
				{
					return this.target;
				}
				set
				{
					this.target = value;
				}
			}

			internal void EndDragDrop(bool allowSetChildIndexOnDrop)
			{
				this.sourceBehavior.EndDragDrop(allowSetChildIndexOnDrop);
			}

			internal void CleanupDrag()
			{
				this.sourceBehavior.CleanupDrag();
			}

			internal ArrayList GetSortedDragControls(ref int primaryControlIndex)
			{
				return this.sourceBehavior.GetSortedDragControls(ref primaryControlIndex);
			}

			private ICollection dragComponents;

			private Control source;

			private IComponent target;

			private DropSourceBehavior sourceBehavior;
		}
	}
}
