using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002F4 RID: 756
	internal sealed class DropSourceBehavior : Behavior, IComparer
	{
		// Token: 0x06001D4F RID: 7503 RVA: 0x000A40E4 File Offset: 0x000A30E4
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

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001D50 RID: 7504 RVA: 0x000A41EE File Offset: 0x000A31EE
		internal DragDropEffects AllowedEffects
		{
			get
			{
				return this.allowedEffects;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x000A41F6 File Offset: 0x000A31F6
		internal DataObject DataObject
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x000A4200 File Offset: 0x000A3200
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

		// Token: 0x06001D53 RID: 7507 RVA: 0x000A42C7 File Offset: 0x000A32C7
		private Point MapPointFromSourceToTarget(Point pt)
		{
			if (this.srcHost != this.destHost && this.destHost != null)
			{
				pt = this.behaviorServiceSource.AdornerWindowPointToScreen(pt);
				return this.behaviorServiceTarget.MapAdornerWindowPoint(IntPtr.Zero, pt);
			}
			return pt;
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000A4300 File Offset: 0x000A3300
		private Point MapPointFromTargetToSource(Point pt)
		{
			if (this.srcHost != this.destHost && this.destHost != null)
			{
				pt = this.behaviorServiceTarget.AdornerWindowPointToScreen(pt);
				return this.behaviorServiceSource.MapAdornerWindowPoint(IntPtr.Zero, pt);
			}
			return pt;
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000A433C File Offset: 0x000A333C
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

		// Token: 0x06001D56 RID: 7510 RVA: 0x000A43B4 File Offset: 0x000A33B4
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

		// Token: 0x06001D57 RID: 7511 RVA: 0x000A443C File Offset: 0x000A343C
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

		// Token: 0x06001D58 RID: 7512 RVA: 0x000A44FC File Offset: 0x000A34FC
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

		// Token: 0x06001D59 RID: 7513 RVA: 0x000A45AC File Offset: 0x000A35AC
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

		// Token: 0x06001D5A RID: 7514 RVA: 0x000A4D78 File Offset: 0x000A3D78
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

		// Token: 0x06001D5B RID: 7515 RVA: 0x000A5500 File Offset: 0x000A4500
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

		// Token: 0x06001D5C RID: 7516 RVA: 0x000A5540 File Offset: 0x000A4540
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

		// Token: 0x06001D5D RID: 7517 RVA: 0x000A5618 File Offset: 0x000A4618
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

		// Token: 0x06001D5E RID: 7518 RVA: 0x000A56B4 File Offset: 0x000A46B4
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

		// Token: 0x06001D5F RID: 7519 RVA: 0x000A5B64 File Offset: 0x000A4B64
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

		// Token: 0x06001D60 RID: 7520 RVA: 0x000A5BC4 File Offset: 0x000A4BC4
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

		// Token: 0x06001D61 RID: 7521 RVA: 0x000A5C20 File Offset: 0x000A4C20
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

		// Token: 0x06001D62 RID: 7522 RVA: 0x000A5C7C File Offset: 0x000A4C7C
		internal void CleanupDrag()
		{
			this.CleanupDrag(true);
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000A5C88 File Offset: 0x000A4C88
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

		// Token: 0x0400165F RID: 5727
		private DropSourceBehavior.DragComponent[] dragComponents;

		// Token: 0x04001660 RID: 5728
		private ArrayList dragObjects;

		// Token: 0x04001661 RID: 5729
		private DropSourceBehavior.BehaviorDataObject data;

		// Token: 0x04001662 RID: 5730
		private DragDropEffects allowedEffects;

		// Token: 0x04001663 RID: 5731
		private DragDropEffects lastEffect;

		// Token: 0x04001664 RID: 5732
		private bool targetAllowsSnapLines;

		// Token: 0x04001665 RID: 5733
		private IComponent lastDropTarget;

		// Token: 0x04001666 RID: 5734
		private Point lastSnapOffset;

		// Token: 0x04001667 RID: 5735
		private BehaviorService behaviorServiceSource;

		// Token: 0x04001668 RID: 5736
		private BehaviorService behaviorServiceTarget;

		// Token: 0x04001669 RID: 5737
		private DragAssistanceManager dragAssistanceManager;

		// Token: 0x0400166A RID: 5738
		private Graphics graphicsTarget;

		// Token: 0x0400166B RID: 5739
		private IServiceProvider serviceProviderSource;

		// Token: 0x0400166C RID: 5740
		private IServiceProvider serviceProviderTarget;

		// Token: 0x0400166D RID: 5741
		private Point initialMouseLoc;

		// Token: 0x0400166E RID: 5742
		private Image dragImage;

		// Token: 0x0400166F RID: 5743
		private Rectangle dragImageRect;

		// Token: 0x04001670 RID: 5744
		private Rectangle clearDragImageRect;

		// Token: 0x04001671 RID: 5745
		private Point originalDragImageLocation;

		// Token: 0x04001672 RID: 5746
		private Region dragImageRegion;

		// Token: 0x04001673 RID: 5747
		private Point lastFeedbackLocation;

		// Token: 0x04001674 RID: 5748
		private Control suspendedParent;

		// Token: 0x04001675 RID: 5749
		private Size parentGridSize;

		// Token: 0x04001676 RID: 5750
		private Point parentLocation;

		// Token: 0x04001677 RID: 5751
		private bool shareParent = true;

		// Token: 0x04001678 RID: 5752
		private bool cleanedUpDrag;

		// Token: 0x04001679 RID: 5753
		private StatusCommandUI statusCommandUITarget;

		// Token: 0x0400167A RID: 5754
		private IDesignerHost srcHost;

		// Token: 0x0400167B RID: 5755
		private IDesignerHost destHost;

		// Token: 0x0400167C RID: 5756
		private bool currentShowState = true;

		// Token: 0x0400167D RID: 5757
		private int primaryComponentIndex = -1;

		// Token: 0x020002F5 RID: 757
		private struct DragComponent
		{
			// Token: 0x0400167E RID: 5758
			public object dragComponent;

			// Token: 0x0400167F RID: 5759
			public int zorderIndex;

			// Token: 0x04001680 RID: 5760
			public Point originalControlLocation;

			// Token: 0x04001681 RID: 5761
			public Point draggedLocation;

			// Token: 0x04001682 RID: 5762
			public Image dragImage;

			// Token: 0x04001683 RID: 5763
			public Point positionOffset;
		}

		// Token: 0x020002F6 RID: 758
		internal class BehaviorDataObject : DataObject
		{
			// Token: 0x06001D64 RID: 7524 RVA: 0x000A5E80 File Offset: 0x000A4E80
			public BehaviorDataObject(ICollection dragComponents, Control source, DropSourceBehavior sourceBehavior)
			{
				this.dragComponents = dragComponents;
				this.source = source;
				this.sourceBehavior = sourceBehavior;
				this.target = null;
			}

			// Token: 0x17000519 RID: 1305
			// (get) Token: 0x06001D65 RID: 7525 RVA: 0x000A5EA4 File Offset: 0x000A4EA4
			public Control Source
			{
				get
				{
					return this.source;
				}
			}

			// Token: 0x1700051A RID: 1306
			// (get) Token: 0x06001D66 RID: 7526 RVA: 0x000A5EAC File Offset: 0x000A4EAC
			public ICollection DragComponents
			{
				get
				{
					return this.dragComponents;
				}
			}

			// Token: 0x1700051B RID: 1307
			// (get) Token: 0x06001D67 RID: 7527 RVA: 0x000A5EB4 File Offset: 0x000A4EB4
			// (set) Token: 0x06001D68 RID: 7528 RVA: 0x000A5EBC File Offset: 0x000A4EBC
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

			// Token: 0x06001D69 RID: 7529 RVA: 0x000A5EC5 File Offset: 0x000A4EC5
			internal void EndDragDrop(bool allowSetChildIndexOnDrop)
			{
				this.sourceBehavior.EndDragDrop(allowSetChildIndexOnDrop);
			}

			// Token: 0x06001D6A RID: 7530 RVA: 0x000A5ED3 File Offset: 0x000A4ED3
			internal void CleanupDrag()
			{
				this.sourceBehavior.CleanupDrag();
			}

			// Token: 0x06001D6B RID: 7531 RVA: 0x000A5EE0 File Offset: 0x000A4EE0
			internal ArrayList GetSortedDragControls(ref int primaryControlIndex)
			{
				return this.sourceBehavior.GetSortedDragControls(ref primaryControlIndex);
			}

			// Token: 0x04001684 RID: 5764
			private ICollection dragComponents;

			// Token: 0x04001685 RID: 5765
			private Control source;

			// Token: 0x04001686 RID: 5766
			private IComponent target;

			// Token: 0x04001687 RID: 5767
			private DropSourceBehavior sourceBehavior;
		}
	}
}
