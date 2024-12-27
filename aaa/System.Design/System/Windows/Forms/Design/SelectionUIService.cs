using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000287 RID: 647
	internal sealed class SelectionUIService : Control, ISelectionUIService
	{
		// Token: 0x060017DD RID: 6109 RVA: 0x0007BFE8 File Offset: 0x0007AFE8
		public SelectionUIService(IDesignerHost host)
		{
			base.SetStyle(ControlStyles.Opaque | ControlStyles.StandardClick | ControlStyles.OptimizedDoubleBuffer, true);
			this.host = host;
			this.dragHandler = null;
			this.dragComponents = null;
			this.selectionItems = new Hashtable();
			this.selectionHandlers = new Hashtable();
			this.AllowDrop = true;
			this.Text = "SelectionUIOverlay";
			this.selSvc = (ISelectionService)host.GetService(typeof(ISelectionService));
			if (this.selSvc != null)
			{
				this.selSvc.SelectionChanged += this.OnSelectionChanged;
			}
			host.TransactionOpened += this.OnTransactionOpened;
			host.TransactionClosed += this.OnTransactionClosed;
			if (host.InTransaction)
			{
				this.OnTransactionOpened(host, EventArgs.Empty);
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)host.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved += this.OnComponentRemove;
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
			SystemEvents.DisplaySettingsChanged += this.OnSystemSettingChanged;
			SystemEvents.InstalledFontsChanged += this.OnSystemSettingChanged;
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x0007C148 File Offset: 0x0007B148
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style &= -100663297;
				return createParams;
			}
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0007C16F File Offset: 0x0007B16F
		private void BeginMouseDrag(Point anchor, int hitTest)
		{
			base.Capture = true;
			this.ignoreCaptureChanged = false;
			this.mouseDragAnchor = anchor;
			this.mouseDragging = true;
			this.mouseDragHitTest = hitTest;
			this.mouseDragOffset = default(Rectangle);
			this.savedVisible = base.Visible;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0007C1AC File Offset: 0x0007B1AC
		private void DisplayError(Exception e)
		{
			IUIService iuiservice = (IUIService)this.host.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				iuiservice.ShowError(e);
				return;
			}
			string text = e.Message;
			if (text == null || text.Length == 0)
			{
				text = e.ToString();
			}
			RTLAwareMessageBox.Show(null, text, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0007C208 File Offset: 0x0007B208
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.selSvc != null)
				{
					this.selSvc.SelectionChanged -= this.OnSelectionChanged;
				}
				if (this.host != null)
				{
					this.host.TransactionOpened -= this.OnTransactionOpened;
					this.host.TransactionClosed -= this.OnTransactionClosed;
					if (this.host.InTransaction)
					{
						this.OnTransactionClosed(this.host, new DesignerTransactionCloseEventArgs(true, true));
					}
					IComponentChangeService componentChangeService = (IComponentChangeService)this.host.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentRemoved -= this.OnComponentRemove;
						componentChangeService.ComponentChanged -= this.OnComponentChanged;
					}
				}
				foreach (object obj in this.selectionItems.Values)
				{
					SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
					selectionUIItem.Dispose();
				}
				this.selectionHandlers.Clear();
				this.selectionItems.Clear();
				SystemEvents.DisplaySettingsChanged -= this.OnSystemSettingChanged;
				SystemEvents.InstalledFontsChanged -= this.OnSystemSettingChanged;
				SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0007C378 File Offset: 0x0007B378
		private void EndMouseDrag(Point position)
		{
			if (base.IsDisposed)
			{
				return;
			}
			this.ignoreCaptureChanged = true;
			base.Capture = false;
			this.mouseDragAnchor = SelectionUIService.InvalidPoint;
			this.mouseDragOffset = Rectangle.Empty;
			this.mouseDragHitTest = 0;
			this.dragMoved = false;
			this.SetSelectionCursor(position);
			this.mouseDragging = (this.ctrlSelect = false);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0007C3D8 File Offset: 0x0007B3D8
		private SelectionUIService.HitTestInfo GetHitTest(Point value, int flags)
		{
			Point point = base.PointToClient(value);
			foreach (object obj in this.selectionItems.Values)
			{
				SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
				if ((flags & 1) != 0 && selectionUIItem is SelectionUIService.ContainerSelectionUIItem && (selectionUIItem.GetRules() & SelectionRules.Visible) != SelectionRules.None)
				{
					int hitTest = selectionUIItem.GetHitTest(point);
					if ((hitTest & 512) != 0)
					{
						return new SelectionUIService.HitTestInfo(hitTest, selectionUIItem, true);
					}
				}
				if ((flags & 2) != 0 && !(selectionUIItem is SelectionUIService.ContainerSelectionUIItem) && (selectionUIItem.GetRules() & SelectionRules.Visible) != SelectionRules.None)
				{
					int hitTest2 = selectionUIItem.GetHitTest(point);
					if (hitTest2 != 256)
					{
						if (hitTest2 != 0)
						{
							return new SelectionUIService.HitTestInfo(hitTest2, selectionUIItem);
						}
						return new SelectionUIService.HitTestInfo(256, selectionUIItem);
					}
				}
			}
			return new SelectionUIService.HitTestInfo(256, null);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0007C4D4 File Offset: 0x0007B4D4
		private ISelectionUIHandler GetHandler(object component)
		{
			return (ISelectionUIHandler)this.selectionHandlers[component];
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0007C4E8 File Offset: 0x0007B4E8
		public static string GetTransactionName(SelectionRules rules, object[] objects)
		{
			string text;
			if ((rules & SelectionRules.Moveable) != SelectionRules.None)
			{
				if (objects.Length > 1)
				{
					text = SR.GetString("DragDropMoveComponents", new object[] { objects.Length });
				}
				else
				{
					string text2 = string.Empty;
					if (objects.Length > 0)
					{
						IComponent component = objects[0] as IComponent;
						if (component != null && component.Site != null)
						{
							text2 = component.Site.Name;
						}
						else
						{
							text2 = objects[0].GetType().Name;
						}
					}
					text = SR.GetString("DragDropMoveComponent", new object[] { text2 });
				}
			}
			else if ((rules & SelectionRules.AllSizeable) != SelectionRules.None)
			{
				if (objects.Length > 1)
				{
					text = SR.GetString("DragDropSizeComponents", new object[] { objects.Length });
				}
				else
				{
					string text3 = string.Empty;
					if (objects.Length > 0)
					{
						IComponent component2 = objects[0] as IComponent;
						if (component2 != null && component2.Site != null)
						{
							text3 = component2.Site.Name;
						}
						else
						{
							text3 = objects[0].GetType().Name;
						}
					}
					text = SR.GetString("DragDropSizeComponent", new object[] { text3 });
				}
			}
			else
			{
				text = SR.GetString("DragDropDragComponents", new object[] { objects.Length });
			}
			return text;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0007C638 File Offset: 0x0007B638
		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction)
			{
				this.batchMode = false;
				if (this.batchChanged)
				{
					this.batchChanged = false;
					((ISelectionUIService)this).SyncSelection();
				}
				if (this.batchSync)
				{
					this.batchSync = false;
					((ISelectionUIService)this).SyncComponent(null);
				}
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0007C674 File Offset: 0x0007B674
		private void OnTransactionOpened(object sender, EventArgs e)
		{
			this.batchMode = true;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0007C67D File Offset: 0x0007B67D
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.UpdateWindowRegion();
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0007C68C File Offset: 0x0007B68C
		private void OnComponentChanged(object sender, ComponentChangedEventArgs ccevent)
		{
			if (!this.batchMode)
			{
				((ISelectionUIService)this).SyncSelection();
				return;
			}
			this.batchChanged = true;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0007C6A4 File Offset: 0x0007B6A4
		private void OnComponentRemove(object sender, ComponentEventArgs ce)
		{
			this.selectionHandlers.Remove(ce.Component);
			this.selectionItems.Remove(ce.Component);
			((ISelectionUIService)this).SyncComponent(ce.Component);
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0007C6D4 File Offset: 0x0007B6D4
		private void OnContainerSelectorActive(ContainerSelectorActiveEventArgs e)
		{
			if (this.containerSelectorActive != null)
			{
				this.containerSelectorActive(this, e);
			}
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0007C6EC File Offset: 0x0007B6EC
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			ICollection selectedComponents = this.selSvc.GetSelectedComponents();
			Hashtable hashtable = new Hashtable(selectedComponents.Count);
			bool flag = false;
			foreach (object obj in selectedComponents)
			{
				object obj2 = this.selectionItems[obj];
				bool flag2 = true;
				if (obj2 != null)
				{
					SelectionUIService.ContainerSelectionUIItem containerSelectionUIItem = obj2 as SelectionUIService.ContainerSelectionUIItem;
					if (containerSelectionUIItem != null)
					{
						containerSelectionUIItem.Dispose();
						flag = true;
					}
					else
					{
						hashtable[obj] = obj2;
						flag2 = false;
					}
				}
				if (flag2)
				{
					flag = true;
					hashtable[obj] = new SelectionUIService.SelectionUIItem(this, obj);
				}
			}
			if (!flag)
			{
				flag = this.selectionItems.Keys.Count != hashtable.Keys.Count;
			}
			this.selectionItems = hashtable;
			if (flag)
			{
				this.UpdateWindowRegion();
			}
			base.Invalidate();
			base.Update();
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0007C7E4 File Offset: 0x0007B7E4
		private void OnSystemSettingChanged(object sender, EventArgs e)
		{
			base.Invalidate();
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0007C7EC File Offset: 0x0007B7EC
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			base.Invalidate();
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0007C7F4 File Offset: 0x0007B7F4
		protected override void OnDragEnter(DragEventArgs devent)
		{
			base.OnDragEnter(devent);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragEnter(devent);
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0007C811 File Offset: 0x0007B811
		protected override void OnDragOver(DragEventArgs devent)
		{
			base.OnDragOver(devent);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragOver(devent);
			}
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0007C82E File Offset: 0x0007B82E
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragLeave();
			}
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0007C84A File Offset: 0x0007B84A
		protected override void OnDragDrop(DragEventArgs devent)
		{
			base.OnDragDrop(devent);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragDrop(devent);
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0007C868 File Offset: 0x0007B868
		protected override void OnDoubleClick(EventArgs devent)
		{
			base.OnDoubleClick(devent);
			if (this.selSvc != null)
			{
				object primarySelection = this.selSvc.PrimarySelection;
				if (primarySelection != null)
				{
					ISelectionUIHandler handler = this.GetHandler(primarySelection);
					if (handler != null)
					{
						handler.OnSelectionDoubleClick((IComponent)primarySelection);
					}
				}
			}
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0007C8AC File Offset: 0x0007B8AC
		protected override void OnMouseDown(MouseEventArgs me)
		{
			if (this.dragHandler == null && this.selSvc != null)
			{
				try
				{
					this.mouseDown = true;
					Point point = base.PointToScreen(new Point(me.X, me.Y));
					SelectionUIService.HitTestInfo hitTest = this.GetHitTest(point, 3);
					int hitTest2 = hitTest.hitTest;
					if ((hitTest2 & 512) != 0)
					{
						this.selSvc.SetSelectedComponents(new object[] { hitTest.selectionUIHit.component }, SelectionTypes.Auto);
						SelectionRules selectionRules = SelectionRules.Moveable;
						if (((ISelectionUIService)this).BeginDrag(selectionRules, point.X, point.Y))
						{
							base.Visible = false;
							this.containerDrag = hitTest.selectionUIHit.component;
							this.BeginMouseDrag(point, hitTest2);
						}
					}
					else if (hitTest2 != 256 && me.Button == MouseButtons.Left)
					{
						SelectionRules selectionRules2 = SelectionRules.None;
						this.ctrlSelect = (Control.ModifierKeys & Keys.Control) != Keys.None;
						if (!this.ctrlSelect)
						{
							this.selSvc.SetSelectedComponents(new object[] { hitTest.selectionUIHit.component }, SelectionTypes.Click);
						}
						if ((hitTest2 & 12) != 0)
						{
							selectionRules2 |= SelectionRules.Moveable;
						}
						if ((hitTest2 & 3) != 0)
						{
							if ((hitTest2 & 65) == 65)
							{
								selectionRules2 |= SelectionRules.RightSizeable;
							}
							if ((hitTest2 & 17) == 17)
							{
								selectionRules2 |= SelectionRules.LeftSizeable;
							}
							if ((hitTest2 & 34) == 34)
							{
								selectionRules2 |= SelectionRules.TopSizeable;
							}
							if ((hitTest2 & 130) == 130)
							{
								selectionRules2 |= SelectionRules.BottomSizeable;
							}
							if (((ISelectionUIService)this).BeginDrag(selectionRules2, point.X, point.Y))
							{
								this.BeginMouseDrag(point, hitTest2);
							}
						}
						else
						{
							this.dragRules = selectionRules2;
							this.BeginMouseDrag(point, hitTest2);
						}
					}
					else if (hitTest2 == 256)
					{
						this.dragRules = SelectionRules.None;
						this.mouseDragAnchor = SelectionUIService.InvalidPoint;
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
					if (ex != CheckoutException.Canceled)
					{
						this.DisplayError(ex);
					}
				}
			}
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0007CABC File Offset: 0x0007BABC
		protected override void OnMouseMove(MouseEventArgs me)
		{
			base.OnMouseMove(me);
			Point point = base.PointToScreen(new Point(me.X, me.Y));
			SelectionUIService.HitTestInfo hitTest = this.GetHitTest(point, 1);
			int hitTest2 = hitTest.hitTest;
			if (hitTest2 != 512 && hitTest.selectionUIHit != null)
			{
				this.OnContainerSelectorActive(new ContainerSelectorActiveEventArgs(hitTest.selectionUIHit.component));
			}
			if (this.lastMoveScreenCoord == point)
			{
				return;
			}
			if (!this.mouseDragging)
			{
				this.SetSelectionCursor(point);
				return;
			}
			if (!((ISelectionUIService)this).Dragging && (this.mouseDragHitTest & 12) != 0)
			{
				Size dragSize = SystemInformation.DragSize;
				if (Math.Abs(point.X - this.mouseDragAnchor.X) < dragSize.Width && Math.Abs(point.Y - this.mouseDragAnchor.Y) < dragSize.Height)
				{
					return;
				}
				this.ignoreCaptureChanged = true;
				if (!((ISelectionUIService)this).BeginDrag(this.dragRules, this.mouseDragAnchor.X, this.mouseDragAnchor.Y))
				{
					this.EndMouseDrag(Control.MousePosition);
					return;
				}
				this.ctrlSelect = false;
			}
			Rectangle rectangle = this.mouseDragOffset;
			if ((this.mouseDragHitTest & 4) != 0)
			{
				this.mouseDragOffset.X = point.X - this.mouseDragAnchor.X;
			}
			if ((this.mouseDragHitTest & 8) != 0)
			{
				this.mouseDragOffset.Y = point.Y - this.mouseDragAnchor.Y;
			}
			if ((this.mouseDragHitTest & 1) != 0)
			{
				if ((this.mouseDragHitTest & 16) != 0)
				{
					this.mouseDragOffset.X = point.X - this.mouseDragAnchor.X;
					this.mouseDragOffset.Width = this.mouseDragAnchor.X - point.X;
				}
				else
				{
					this.mouseDragOffset.Width = point.X - this.mouseDragAnchor.X;
				}
			}
			if ((this.mouseDragHitTest & 2) != 0)
			{
				if ((this.mouseDragHitTest & 32) != 0)
				{
					this.mouseDragOffset.Y = point.Y - this.mouseDragAnchor.Y;
					this.mouseDragOffset.Height = this.mouseDragAnchor.Y - point.Y;
				}
				else
				{
					this.mouseDragOffset.Height = point.Y - this.mouseDragAnchor.Y;
				}
			}
			if (!rectangle.Equals(this.mouseDragOffset))
			{
				Rectangle rectangle2 = this.mouseDragOffset;
				rectangle2.X -= rectangle.X;
				rectangle2.Y -= rectangle.Y;
				rectangle2.Width -= rectangle.Width;
				rectangle2.Height -= rectangle.Height;
				if (rectangle2.X != 0 || rectangle2.Y != 0 || rectangle2.Width != 0 || rectangle2.Height != 0)
				{
					if ((this.mouseDragHitTest & 4) != 0 || (this.mouseDragHitTest & 8) != 0)
					{
						this.Cursor = Cursors.Default;
					}
					((ISelectionUIService)this).DragMoved(rectangle2);
				}
			}
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0007CDE0 File Offset: 0x0007BDE0
		protected override void OnMouseUp(MouseEventArgs me)
		{
			try
			{
				this.mouseDown = false;
				Point point = base.PointToScreen(new Point(me.X, me.Y));
				if (this.ctrlSelect && !this.mouseDragging && this.selSvc != null)
				{
					SelectionUIService.HitTestInfo hitTest = this.GetHitTest(point, 3);
					this.selSvc.SetSelectedComponents(new object[] { hitTest.selectionUIHit.component }, SelectionTypes.Click);
				}
				if (this.mouseDragging)
				{
					object obj = this.containerDrag;
					bool flag = this.dragMoved;
					this.EndMouseDrag(point);
					if (((ISelectionUIService)this).Dragging)
					{
						((ISelectionUIService)this).EndDrag(false);
					}
					if (me.Button == MouseButtons.Right && obj != null && !flag)
					{
						this.OnContainerSelectorActive(new ContainerSelectorActiveEventArgs(obj, ContainerSelectorActiveEventArgsType.Contextmenu));
					}
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
				if (ex != CheckoutException.Canceled)
				{
					this.DisplayError(ex);
				}
			}
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0007CED4 File Offset: 0x0007BED4
		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			base.Invalidate();
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0007CEE4 File Offset: 0x0007BEE4
		protected override void OnPaint(PaintEventArgs e)
		{
			foreach (object obj in this.selectionItems.Values)
			{
				SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
				if (!(selectionUIItem is SelectionUIService.ContainerSelectionUIItem))
				{
					selectionUIItem.DoPaint(e.Graphics);
				}
			}
			foreach (object obj2 in this.selectionItems.Values)
			{
				SelectionUIService.SelectionUIItem selectionUIItem2 = (SelectionUIService.SelectionUIItem)obj2;
				if (selectionUIItem2 is SelectionUIService.ContainerSelectionUIItem)
				{
					selectionUIItem2.DoPaint(e.Graphics);
				}
			}
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0007CFB0 File Offset: 0x0007BFB0
		private void SetSelectionCursor(Point pt)
		{
			Point point = base.PointToClient(pt);
			foreach (object obj in this.selectionItems.Values)
			{
				SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
				if (!(selectionUIItem is SelectionUIService.ContainerSelectionUIItem))
				{
					Cursor cursorAtPoint = selectionUIItem.GetCursorAtPoint(point);
					if (cursorAtPoint != null)
					{
						if (cursorAtPoint == Cursors.Default)
						{
							this.Cursor = null;
						}
						else
						{
							this.Cursor = cursorAtPoint;
						}
						return;
					}
				}
			}
			foreach (object obj2 in this.selectionItems.Values)
			{
				SelectionUIService.SelectionUIItem selectionUIItem2 = (SelectionUIService.SelectionUIItem)obj2;
				if (selectionUIItem2 is SelectionUIService.ContainerSelectionUIItem)
				{
					Cursor cursorAtPoint2 = selectionUIItem2.GetCursorAtPoint(point);
					if (cursorAtPoint2 != null)
					{
						if (cursorAtPoint2 == Cursors.Default)
						{
							this.Cursor = null;
						}
						else
						{
							this.Cursor = cursorAtPoint2;
						}
						return;
					}
				}
			}
			this.Cursor = null;
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0007D0E4 File Offset: 0x0007C0E4
		private void UpdateWindowRegion()
		{
			Region region = new Region(new Rectangle(0, 0, 0, 0));
			foreach (object obj in this.selectionItems.Values)
			{
				SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
				region.Union(selectionUIItem.GetRegion());
			}
			base.Region = region;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0007D160 File Offset: 0x0007C160
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 514 && msg != 517)
			{
				if (msg == 533)
				{
					if (!this.ignoreCaptureChanged && this.mouseDragAnchor != SelectionUIService.InvalidPoint)
					{
						this.EndMouseDrag(Control.MousePosition);
						if (((ISelectionUIService)this).Dragging)
						{
							((ISelectionUIService)this).EndDrag(true);
						}
					}
					this.ignoreCaptureChanged = false;
				}
			}
			else if (this.mouseDragAnchor != SelectionUIService.InvalidPoint)
			{
				this.ignoreCaptureChanged = true;
			}
			base.WndProc(ref m);
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x0007D1EB File Offset: 0x0007C1EB
		bool ISelectionUIService.Dragging
		{
			get
			{
				return this.dragHandler != null;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x0007D1F9 File Offset: 0x0007C1F9
		// (set) Token: 0x060017FE RID: 6142 RVA: 0x0007D201 File Offset: 0x0007C201
		bool ISelectionUIService.Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060017FF RID: 6143 RVA: 0x0007D20A File Offset: 0x0007C20A
		// (remove) Token: 0x06001800 RID: 6144 RVA: 0x0007D223 File Offset: 0x0007C223
		event ContainerSelectorActiveEventHandler ISelectionUIService.ContainerSelectorActive
		{
			add
			{
				this.containerSelectorActive = (ContainerSelectorActiveEventHandler)Delegate.Combine(this.containerSelectorActive, value);
			}
			remove
			{
				this.containerSelectorActive = (ContainerSelectorActiveEventHandler)Delegate.Remove(this.containerSelectorActive, value);
			}
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0007D23C File Offset: 0x0007C23C
		void ISelectionUIService.AssignSelectionUIHandler(object component, ISelectionUIHandler handler)
		{
			ISelectionUIHandler selectionUIHandler = (ISelectionUIHandler)this.selectionHandlers[component];
			if (selectionUIHandler == null)
			{
				this.selectionHandlers[component] = handler;
				if (this.selSvc != null && this.selSvc.GetComponentSelected(component))
				{
					SelectionUIService.SelectionUIItem selectionUIItem = new SelectionUIService.SelectionUIItem(this, component);
					this.selectionItems[component] = selectionUIItem;
					this.UpdateWindowRegion();
					selectionUIItem.Invalidate();
				}
				return;
			}
			if (handler == selectionUIHandler)
			{
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0007D2B0 File Offset: 0x0007C2B0
		void ISelectionUIService.ClearSelectionUIHandler(object component, ISelectionUIHandler handler)
		{
			ISelectionUIHandler selectionUIHandler = (ISelectionUIHandler)this.selectionHandlers[component];
			if (selectionUIHandler == handler)
			{
				this.selectionHandlers[component] = null;
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0007D2E0 File Offset: 0x0007C2E0
		bool ISelectionUIService.BeginDrag(SelectionRules rules, int initialX, int initialY)
		{
			if (this.dragHandler != null)
			{
				return false;
			}
			if (rules == SelectionRules.None)
			{
				return false;
			}
			if (this.selSvc == null)
			{
				return false;
			}
			this.savedVisible = base.Visible;
			ICollection selectedComponents = this.selSvc.GetSelectedComponents();
			object[] array = new object[selectedComponents.Count];
			selectedComponents.CopyTo(array, 0);
			array = ((ISelectionUIService)this).FilterSelection(array, rules);
			if (array.Length == 0)
			{
				return false;
			}
			ISelectionUIHandler selectionUIHandler = null;
			object primarySelection = this.selSvc.PrimarySelection;
			if (primarySelection != null)
			{
				selectionUIHandler = this.GetHandler(primarySelection);
			}
			if (selectionUIHandler == null)
			{
				return false;
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (this.GetHandler(array[i]) == selectionUIHandler)
				{
					SelectionRules componentRules = selectionUIHandler.GetComponentRules(array[i]);
					if ((componentRules & rules) == rules)
					{
						arrayList.Add(array[i]);
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return false;
			}
			array = arrayList.ToArray();
			bool flag = false;
			this.dragComponents = array;
			this.dragRules = rules;
			this.dragHandler = selectionUIHandler;
			string transactionName = SelectionUIService.GetTransactionName(rules, array);
			this.dragTransaction = this.host.CreateTransaction(transactionName);
			try
			{
				if (selectionUIHandler.QueryBeginDrag(array, rules, initialX, initialY) && this.dragHandler != null)
				{
					try
					{
						flag = selectionUIHandler.BeginDrag(array, rules, initialX, initialY);
					}
					catch (Exception)
					{
						flag = false;
					}
				}
			}
			finally
			{
				if (!flag)
				{
					this.dragComponents = null;
					this.dragRules = SelectionRules.None;
					this.dragHandler = null;
					if (this.dragTransaction != null)
					{
						this.dragTransaction.Commit();
						this.dragTransaction = null;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0007D468 File Offset: 0x0007C468
		void ISelectionUIService.DragMoved(Rectangle offset)
		{
			Rectangle empty = Rectangle.Empty;
			if (this.dragHandler == null)
			{
				throw new Exception(SR.GetString("DesignerBeginDragNotCalled"));
			}
			if ((this.dragRules & SelectionRules.Moveable) == SelectionRules.None && (this.dragRules & (SelectionRules.TopSizeable | SelectionRules.LeftSizeable)) == SelectionRules.None)
			{
				empty = new Rectangle(0, 0, offset.Width, offset.Height);
			}
			if ((this.dragRules & SelectionRules.AllSizeable) == SelectionRules.None)
			{
				if (empty.IsEmpty)
				{
					empty = new Rectangle(offset.X, offset.Y, 0, 0);
				}
				else
				{
					empty.Width = (empty.Height = 0);
				}
			}
			if (!empty.IsEmpty)
			{
				offset = empty;
			}
			base.Visible = false;
			this.dragMoved = true;
			this.dragHandler.DragMoved(this.dragComponents, offset);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0007D530 File Offset: 0x0007C530
		void ISelectionUIService.EndDrag(bool cancel)
		{
			this.containerDrag = null;
			ISelectionUIHandler selectionUIHandler = this.dragHandler;
			object[] array = this.dragComponents;
			this.dragHandler = null;
			this.dragComponents = null;
			this.dragRules = SelectionRules.None;
			if (selectionUIHandler == null)
			{
				throw new InvalidOperationException();
			}
			DesignerTransaction designerTransaction = null;
			try
			{
				IComponent component = array[0] as IComponent;
				if (array.Length > 1 || (array.Length == 1 && component != null && component.Site == null))
				{
					designerTransaction = this.host.CreateTransaction(SR.GetString("DragDropMoveComponents", new object[] { array.Length }));
				}
				else if (array.Length == 1 && component != null)
				{
					designerTransaction = this.host.CreateTransaction(SR.GetString("DragDropMoveComponent", new object[] { component.Site.Name }));
				}
				try
				{
					selectionUIHandler.EndDrag(array, cancel);
				}
				catch (Exception)
				{
				}
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
				base.Visible = this.savedVisible;
				((ISelectionUIService)this).SyncSelection();
				if (this.dragTransaction != null)
				{
					this.dragTransaction.Commit();
					this.dragTransaction = null;
				}
				this.EndMouseDrag(Control.MousePosition);
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0007D664 File Offset: 0x0007C664
		object[] ISelectionUIService.FilterSelection(object[] components, SelectionRules selectionRules)
		{
			object[] array = null;
			if (components == null)
			{
				return new object[0];
			}
			if (selectionRules != SelectionRules.None)
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in components)
				{
					SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[obj];
					if (selectionUIItem != null && !(selectionUIItem is SelectionUIService.ContainerSelectionUIItem) && (selectionUIItem.GetRules() & selectionRules) == selectionRules)
					{
						arrayList.Add(obj);
					}
				}
				array = arrayList.ToArray();
			}
			if (array != null)
			{
				return array;
			}
			return new object[0];
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0007D6E4 File Offset: 0x0007C6E4
		Size ISelectionUIService.GetAdornmentDimensions(AdornmentType adornmentType)
		{
			switch (adornmentType)
			{
			case AdornmentType.GrabHandle:
				return new Size(7, 7);
			case AdornmentType.ContainerSelector:
			case AdornmentType.Maximum:
				return new Size(13, 13);
			default:
				return new Size(0, 0);
			}
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0007D722 File Offset: 0x0007C722
		bool ISelectionUIService.GetAdornmentHitTest(object component, Point value)
		{
			return this.GetHitTest(value, 3).hitTest != 256;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0007D73B File Offset: 0x0007C73B
		bool ISelectionUIService.GetContainerSelected(object component)
		{
			return component != null && this.selectionItems[component] is SelectionUIService.ContainerSelectionUIItem;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0007D758 File Offset: 0x0007C758
		SelectionRules ISelectionUIService.GetSelectionRules(object component)
		{
			SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
			if (selectionUIItem == null)
			{
				throw new InvalidOperationException();
			}
			return selectionUIItem.GetRules();
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0007D788 File Offset: 0x0007C788
		SelectionStyles ISelectionUIService.GetSelectionStyle(object component)
		{
			SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
			if (selectionUIItem == null)
			{
				return SelectionStyles.None;
			}
			return selectionUIItem.Style;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0007D7B4 File Offset: 0x0007C7B4
		void ISelectionUIService.SetContainerSelected(object component, bool selected)
		{
			if (selected)
			{
				SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
				if (!(selectionUIItem is SelectionUIService.ContainerSelectionUIItem))
				{
					if (selectionUIItem != null)
					{
						selectionUIItem.Dispose();
					}
					SelectionUIService.SelectionUIItem selectionUIItem2 = new SelectionUIService.ContainerSelectionUIItem(this, component);
					this.selectionItems[component] = selectionUIItem2;
					this.UpdateWindowRegion();
					if (selectionUIItem != null)
					{
						selectionUIItem.Invalidate();
					}
					selectionUIItem2.Invalidate();
					return;
				}
			}
			else
			{
				SelectionUIService.SelectionUIItem selectionUIItem3 = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
				if (selectionUIItem3 == null || selectionUIItem3 is SelectionUIService.ContainerSelectionUIItem)
				{
					this.selectionItems.Remove(component);
					if (selectionUIItem3 != null)
					{
						selectionUIItem3.Dispose();
					}
					this.UpdateWindowRegion();
					selectionUIItem3.Invalidate();
				}
			}
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0007D850 File Offset: 0x0007C850
		void ISelectionUIService.SetSelectionStyle(object component, SelectionStyles style)
		{
			SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
			if (this.selSvc != null && this.selSvc.GetComponentSelected(component))
			{
				selectionUIItem = new SelectionUIService.SelectionUIItem(this, component);
				this.selectionItems[component] = selectionUIItem;
			}
			if (selectionUIItem != null)
			{
				selectionUIItem.Style = style;
				this.UpdateWindowRegion();
				selectionUIItem.Invalidate();
			}
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0007D8B0 File Offset: 0x0007C8B0
		void ISelectionUIService.SyncSelection()
		{
			if (this.batchMode)
			{
				this.batchChanged = true;
				return;
			}
			if (base.IsHandleCreated)
			{
				bool flag = false;
				foreach (object obj in this.selectionItems.Values)
				{
					SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
					flag |= selectionUIItem.UpdateSize();
					selectionUIItem.UpdateRules();
				}
				if (flag)
				{
					this.UpdateWindowRegion();
					base.Update();
				}
			}
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0007D940 File Offset: 0x0007C940
		void ISelectionUIService.SyncComponent(object component)
		{
			if (this.batchMode)
			{
				this.batchSync = true;
				return;
			}
			if (base.IsHandleCreated)
			{
				foreach (object obj in this.selectionItems.Values)
				{
					SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)obj;
					selectionUIItem.UpdateRules();
					selectionUIItem.Dispose();
				}
				this.UpdateWindowRegion();
				base.Invalidate();
				base.Update();
			}
		}

		// Token: 0x040013C4 RID: 5060
		private const int HITTEST_CONTAINER_SELECTOR = 1;

		// Token: 0x040013C5 RID: 5061
		private const int HITTEST_NORMAL_SELECTION = 2;

		// Token: 0x040013C6 RID: 5062
		private const int HITTEST_DEFAULT = 3;

		// Token: 0x040013C7 RID: 5063
		private static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		// Token: 0x040013C8 RID: 5064
		private ISelectionUIHandler dragHandler;

		// Token: 0x040013C9 RID: 5065
		private object[] dragComponents;

		// Token: 0x040013CA RID: 5066
		private SelectionRules dragRules;

		// Token: 0x040013CB RID: 5067
		private bool dragMoved;

		// Token: 0x040013CC RID: 5068
		private object containerDrag;

		// Token: 0x040013CD RID: 5069
		private bool ignoreCaptureChanged;

		// Token: 0x040013CE RID: 5070
		private bool mouseDown;

		// Token: 0x040013CF RID: 5071
		private int mouseDragHitTest;

		// Token: 0x040013D0 RID: 5072
		private Point mouseDragAnchor = SelectionUIService.InvalidPoint;

		// Token: 0x040013D1 RID: 5073
		private Rectangle mouseDragOffset = Rectangle.Empty;

		// Token: 0x040013D2 RID: 5074
		private Point lastMoveScreenCoord = Point.Empty;

		// Token: 0x040013D3 RID: 5075
		private bool ctrlSelect;

		// Token: 0x040013D4 RID: 5076
		private bool mouseDragging;

		// Token: 0x040013D5 RID: 5077
		private ContainerSelectorActiveEventHandler containerSelectorActive;

		// Token: 0x040013D6 RID: 5078
		private Hashtable selectionItems;

		// Token: 0x040013D7 RID: 5079
		private Hashtable selectionHandlers;

		// Token: 0x040013D8 RID: 5080
		private bool savedVisible;

		// Token: 0x040013D9 RID: 5081
		private bool batchMode;

		// Token: 0x040013DA RID: 5082
		private bool batchChanged;

		// Token: 0x040013DB RID: 5083
		private bool batchSync;

		// Token: 0x040013DC RID: 5084
		private ISelectionService selSvc;

		// Token: 0x040013DD RID: 5085
		private IDesignerHost host;

		// Token: 0x040013DE RID: 5086
		private DesignerTransaction dragTransaction;

		// Token: 0x02000288 RID: 648
		private class SelectionUIItem
		{
			// Token: 0x06001811 RID: 6161 RVA: 0x0007D9E8 File Offset: 0x0007C9E8
			public SelectionUIItem(SelectionUIService selUIsvc, object component)
			{
				this.selUIsvc = selUIsvc;
				this.component = component;
				this.selectionStyle = SelectionStyles.Selected;
				this.handler = selUIsvc.GetHandler(component);
				this.sizes = SelectionUIService.SelectionUIItem.inactiveSizeArray;
				this.cursors = SelectionUIService.SelectionUIItem.inactiveCursorArray;
				IComponent component2 = component as IComponent;
				if (component2 != null)
				{
					ControlDesigner controlDesigner = selUIsvc.host.GetDesigner(component2) as ControlDesigner;
					if (controlDesigner != null)
					{
						this.control = controlDesigner.Control;
					}
				}
				this.UpdateRules();
				this.UpdateGrabSettings();
				this.UpdateSize();
			}

			// Token: 0x17000427 RID: 1063
			// (get) Token: 0x06001812 RID: 6162 RVA: 0x0007DA87 File Offset: 0x0007CA87
			// (set) Token: 0x06001813 RID: 6163 RVA: 0x0007DA8F File Offset: 0x0007CA8F
			public virtual SelectionStyles Style
			{
				get
				{
					return this.selectionStyle;
				}
				set
				{
					if (value != this.selectionStyle)
					{
						this.selectionStyle = value;
						if (this.region != null)
						{
							this.region.Dispose();
							this.region = null;
						}
					}
				}
			}

			// Token: 0x06001814 RID: 6164 RVA: 0x0007DABC File Offset: 0x0007CABC
			public virtual void DoPaint(Graphics gr)
			{
				if ((this.GetRules() & SelectionRules.Visible) == SelectionRules.None)
				{
					return;
				}
				bool flag = false;
				if (this.selUIsvc.selSvc != null)
				{
					flag = this.component == this.selUIsvc.selSvc.PrimarySelection;
					flag = flag == this.selUIsvc.selSvc.SelectionCount <= 1;
				}
				Rectangle rectangle = new Rectangle(this.outerRect.X, this.outerRect.Y, 7, 7);
				Rectangle rectangle2 = this.innerRect;
				Rectangle rectangle3 = this.outerRect;
				Region clip = gr.Clip;
				Color backColor = SystemColors.Control;
				if (this.control != null && this.control.Parent != null)
				{
					Control parent = this.control.Parent;
					backColor = parent.BackColor;
				}
				Brush brush = new SolidBrush(backColor);
				gr.ExcludeClip(rectangle2);
				gr.FillRectangle(brush, rectangle3);
				gr.Clip = clip;
				ControlPaint.DrawSelectionFrame(gr, false, rectangle3, rectangle2, backColor);
				if ((this.GetRules() & SelectionRules.Locked) == SelectionRules.None && (this.GetRules() & SelectionRules.AllSizeable) != SelectionRules.None)
				{
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[0] != 0);
					rectangle.X = rectangle2.X + rectangle2.Width;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[2] != 0);
					rectangle.Y = rectangle2.Y + rectangle2.Height;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[7] != 0);
					rectangle.X = rectangle3.X;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[5] != 0);
					rectangle.X += (rectangle3.Width - 7) / 2;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[6] != 0);
					rectangle.Y = rectangle3.Y;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[1] != 0);
					rectangle.X = rectangle3.X;
					rectangle.Y = rectangle2.Y + (rectangle2.Height - 7) / 2;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[3] != 0);
					rectangle.X = rectangle2.X + rectangle2.Width;
					ControlPaint.DrawGrabHandle(gr, rectangle, flag, this.sizes[4] != 0);
					return;
				}
				ControlPaint.DrawLockedFrame(gr, rectangle3, flag);
			}

			// Token: 0x06001815 RID: 6165 RVA: 0x0007DD24 File Offset: 0x0007CD24
			public virtual Cursor GetCursorAtPoint(Point pt)
			{
				Cursor cursor = null;
				if (this.PointWithinSelection(pt))
				{
					int num = -1;
					if ((this.GetRules() & SelectionRules.AllSizeable) != SelectionRules.None)
					{
						num = this.GetHandleIndexOfPoint(pt);
					}
					if (-1 == num)
					{
						if ((this.GetRules() & SelectionRules.Moveable) == SelectionRules.None)
						{
							cursor = Cursors.Default;
						}
						else
						{
							cursor = Cursors.SizeAll;
						}
					}
					else
					{
						cursor = this.cursors[num];
					}
				}
				return cursor;
			}

			// Token: 0x06001816 RID: 6166 RVA: 0x0007DD80 File Offset: 0x0007CD80
			public virtual int GetHitTest(Point pt)
			{
				if (!this.PointWithinSelection(pt))
				{
					return 256;
				}
				int handleIndexOfPoint = this.GetHandleIndexOfPoint(pt);
				if (-1 != handleIndexOfPoint && this.sizes[handleIndexOfPoint] != 0)
				{
					return this.sizes[handleIndexOfPoint];
				}
				if ((this.GetRules() & SelectionRules.Moveable) != SelectionRules.None)
				{
					return 12;
				}
				return 0;
			}

			// Token: 0x06001817 RID: 6167 RVA: 0x0007DDD0 File Offset: 0x0007CDD0
			private int GetHandleIndexOfPoint(Point pt)
			{
				if (pt.X >= this.outerRect.X && pt.X <= this.innerRect.X)
				{
					if (pt.Y >= this.outerRect.Y && pt.Y <= this.innerRect.Y)
					{
						return 0;
					}
					if (pt.Y >= this.innerRect.Y + this.innerRect.Height && pt.Y <= this.outerRect.Y + this.outerRect.Height)
					{
						return 5;
					}
					if (pt.Y >= this.outerRect.Y + (this.outerRect.Height - 7) / 2 && pt.Y <= this.outerRect.Y + (this.outerRect.Height + 7) / 2)
					{
						return 3;
					}
					return -1;
				}
				else if (pt.Y >= this.outerRect.Y && pt.Y <= this.innerRect.Y)
				{
					if (pt.X >= this.innerRect.X + this.innerRect.Width && pt.X <= this.outerRect.X + this.outerRect.Width)
					{
						return 2;
					}
					if (pt.X >= this.outerRect.X + (this.outerRect.Width - 7) / 2 && pt.X <= this.outerRect.X + (this.outerRect.Width + 7) / 2)
					{
						return 1;
					}
					return -1;
				}
				else if (pt.X >= this.innerRect.X + this.innerRect.Width && pt.X <= this.outerRect.X + this.outerRect.Width)
				{
					if (pt.Y >= this.innerRect.Y + this.innerRect.Height && pt.Y <= this.outerRect.Y + this.outerRect.Height)
					{
						return 7;
					}
					if (pt.Y >= this.outerRect.Y + (this.outerRect.Height - 7) / 2 && pt.Y <= this.outerRect.Y + (this.outerRect.Height + 7) / 2)
					{
						return 4;
					}
					return -1;
				}
				else
				{
					if (pt.Y < this.innerRect.Y + this.innerRect.Height || pt.Y > this.outerRect.Y + this.outerRect.Height)
					{
						return -1;
					}
					if (pt.X >= this.outerRect.X + (this.outerRect.Width - 7) / 2 && pt.X <= this.outerRect.X + (this.outerRect.Width + 7) / 2)
					{
						return 6;
					}
					return -1;
				}
			}

			// Token: 0x06001818 RID: 6168 RVA: 0x0007E0E0 File Offset: 0x0007D0E0
			public virtual Region GetRegion()
			{
				if (this.region == null)
				{
					if ((this.GetRules() & SelectionRules.Visible) != SelectionRules.None && !this.outerRect.IsEmpty)
					{
						this.region = new Region(this.outerRect);
						this.region.Exclude(this.innerRect);
					}
					else
					{
						this.region = new Region(new Rectangle(0, 0, 0, 0));
					}
					if (this.handler != null)
					{
						Rectangle selectionClipRect = this.handler.GetSelectionClipRect(this.component);
						if (!selectionClipRect.IsEmpty)
						{
							this.region.Intersect(this.selUIsvc.RectangleToClient(selectionClipRect));
						}
					}
				}
				return this.region;
			}

			// Token: 0x06001819 RID: 6169 RVA: 0x0007E18B File Offset: 0x0007D18B
			public SelectionRules GetRules()
			{
				return this.selectionRules;
			}

			// Token: 0x0600181A RID: 6170 RVA: 0x0007E193 File Offset: 0x0007D193
			public void Dispose()
			{
				if (this.region != null)
				{
					this.region.Dispose();
					this.region = null;
				}
			}

			// Token: 0x0600181B RID: 6171 RVA: 0x0007E1AF File Offset: 0x0007D1AF
			public void Invalidate()
			{
				if (!this.outerRect.IsEmpty && !this.selUIsvc.Disposing)
				{
					this.selUIsvc.Invalidate(this.outerRect);
				}
			}

			// Token: 0x0600181C RID: 6172 RVA: 0x0007E1DC File Offset: 0x0007D1DC
			protected bool PointWithinSelection(Point pt)
			{
				return (this.GetRules() & SelectionRules.Visible) != SelectionRules.None && !this.outerRect.IsEmpty && !this.innerRect.IsEmpty && pt.X >= this.outerRect.X && pt.X <= this.outerRect.X + this.outerRect.Width && pt.Y >= this.outerRect.Y && pt.Y <= this.outerRect.Y + this.outerRect.Height && (pt.X <= this.innerRect.X || pt.X >= this.innerRect.X + this.innerRect.Width || pt.Y <= this.innerRect.Y || pt.Y >= this.innerRect.Y + this.innerRect.Height);
			}

			// Token: 0x0600181D RID: 6173 RVA: 0x0007E2EC File Offset: 0x0007D2EC
			private void UpdateGrabSettings()
			{
				SelectionRules rules = this.GetRules();
				if ((rules & SelectionRules.AllSizeable) == SelectionRules.None)
				{
					this.sizes = SelectionUIService.SelectionUIItem.inactiveSizeArray;
					this.cursors = SelectionUIService.SelectionUIItem.inactiveCursorArray;
					return;
				}
				this.sizes = new int[8];
				this.cursors = new Cursor[8];
				Array.Copy(SelectionUIService.SelectionUIItem.activeCursorArrays, this.cursors, this.cursors.Length);
				Array.Copy(SelectionUIService.SelectionUIItem.activeSizeArray, this.sizes, this.sizes.Length);
				if ((rules & SelectionRules.TopSizeable) != SelectionRules.TopSizeable)
				{
					this.sizes[0] = 0;
					this.sizes[1] = 0;
					this.sizes[2] = 0;
					this.cursors[0] = Cursors.Arrow;
					this.cursors[1] = Cursors.Arrow;
					this.cursors[2] = Cursors.Arrow;
				}
				if ((rules & SelectionRules.LeftSizeable) != SelectionRules.LeftSizeable)
				{
					this.sizes[0] = 0;
					this.sizes[3] = 0;
					this.sizes[5] = 0;
					this.cursors[0] = Cursors.Arrow;
					this.cursors[3] = Cursors.Arrow;
					this.cursors[5] = Cursors.Arrow;
				}
				if ((rules & SelectionRules.BottomSizeable) != SelectionRules.BottomSizeable)
				{
					this.sizes[5] = 0;
					this.sizes[6] = 0;
					this.sizes[7] = 0;
					this.cursors[5] = Cursors.Arrow;
					this.cursors[6] = Cursors.Arrow;
					this.cursors[7] = Cursors.Arrow;
				}
				if ((rules & SelectionRules.RightSizeable) != SelectionRules.RightSizeable)
				{
					this.sizes[2] = 0;
					this.sizes[4] = 0;
					this.sizes[7] = 0;
					this.cursors[2] = Cursors.Arrow;
					this.cursors[4] = Cursors.Arrow;
					this.cursors[7] = Cursors.Arrow;
				}
			}

			// Token: 0x0600181E RID: 6174 RVA: 0x0007E488 File Offset: 0x0007D488
			public void UpdateRules()
			{
				if (this.handler == null)
				{
					this.selectionRules = SelectionRules.None;
					return;
				}
				SelectionRules selectionRules = this.selectionRules;
				this.selectionRules = this.handler.GetComponentRules(this.component);
				if (this.selectionRules != selectionRules)
				{
					this.UpdateGrabSettings();
					this.Invalidate();
				}
			}

			// Token: 0x0600181F RID: 6175 RVA: 0x0007E4D8 File Offset: 0x0007D4D8
			public virtual bool UpdateSize()
			{
				bool flag = false;
				if (this.handler == null)
				{
					return false;
				}
				if ((this.GetRules() & SelectionRules.Visible) == SelectionRules.None)
				{
					return false;
				}
				this.innerRect = this.handler.GetComponentBounds(this.component);
				if (!this.innerRect.IsEmpty)
				{
					this.innerRect = this.selUIsvc.RectangleToClient(this.innerRect);
					Rectangle rectangle = new Rectangle(this.innerRect.X - 7, this.innerRect.Y - 7, this.innerRect.Width + 14, this.innerRect.Height + 14);
					if (this.outerRect.IsEmpty || !this.outerRect.Equals(rectangle))
					{
						if (!this.outerRect.IsEmpty)
						{
							this.Invalidate();
						}
						this.outerRect = rectangle;
						this.Invalidate();
						if (this.region != null)
						{
							this.region.Dispose();
							this.region = null;
						}
						flag = true;
					}
				}
				else
				{
					Rectangle rectangle2 = new Rectangle(0, 0, 0, 0);
					flag = this.outerRect.IsEmpty || !this.outerRect.Equals(rectangle2);
					this.innerRect = (this.outerRect = rectangle2);
				}
				return flag;
			}

			// Token: 0x06001820 RID: 6176 RVA: 0x0007E650 File Offset: 0x0007D650
			// Note: this type is marked as 'beforefieldinit'.
			static SelectionUIItem()
			{
				int[] array = new int[8];
				SelectionUIService.SelectionUIItem.inactiveSizeArray = array;
				SelectionUIService.SelectionUIItem.inactiveCursorArray = new Cursor[]
				{
					Cursors.Arrow,
					Cursors.Arrow,
					Cursors.Arrow,
					Cursors.Arrow,
					Cursors.Arrow,
					Cursors.Arrow,
					Cursors.Arrow,
					Cursors.Arrow
				};
			}

			// Token: 0x040013DF RID: 5087
			public const int SIZE_X = 1;

			// Token: 0x040013E0 RID: 5088
			public const int SIZE_Y = 2;

			// Token: 0x040013E1 RID: 5089
			public const int SIZE_MASK = 3;

			// Token: 0x040013E2 RID: 5090
			public const int MOVE_X = 4;

			// Token: 0x040013E3 RID: 5091
			public const int MOVE_Y = 8;

			// Token: 0x040013E4 RID: 5092
			public const int MOVE_MASK = 12;

			// Token: 0x040013E5 RID: 5093
			public const int POS_LEFT = 16;

			// Token: 0x040013E6 RID: 5094
			public const int POS_TOP = 32;

			// Token: 0x040013E7 RID: 5095
			public const int POS_RIGHT = 64;

			// Token: 0x040013E8 RID: 5096
			public const int POS_BOTTOM = 128;

			// Token: 0x040013E9 RID: 5097
			public const int POS_MASK = 240;

			// Token: 0x040013EA RID: 5098
			public const int NOHIT = 256;

			// Token: 0x040013EB RID: 5099
			public const int CONTAINER_SELECTOR = 512;

			// Token: 0x040013EC RID: 5100
			public const int GRABHANDLE_WIDTH = 7;

			// Token: 0x040013ED RID: 5101
			public const int GRABHANDLE_HEIGHT = 7;

			// Token: 0x040013EE RID: 5102
			internal static readonly int[] activeSizeArray = new int[] { 51, 34, 99, 17, 65, 147, 130, 195 };

			// Token: 0x040013EF RID: 5103
			internal static readonly Cursor[] activeCursorArrays = new Cursor[]
			{
				Cursors.SizeNWSE,
				Cursors.SizeNS,
				Cursors.SizeNESW,
				Cursors.SizeWE,
				Cursors.SizeWE,
				Cursors.SizeNESW,
				Cursors.SizeNS,
				Cursors.SizeNWSE
			};

			// Token: 0x040013F0 RID: 5104
			internal static readonly int[] inactiveSizeArray;

			// Token: 0x040013F1 RID: 5105
			internal static readonly Cursor[] inactiveCursorArray;

			// Token: 0x040013F2 RID: 5106
			internal int[] sizes;

			// Token: 0x040013F3 RID: 5107
			internal Cursor[] cursors;

			// Token: 0x040013F4 RID: 5108
			internal SelectionUIService selUIsvc;

			// Token: 0x040013F5 RID: 5109
			internal Rectangle innerRect = Rectangle.Empty;

			// Token: 0x040013F6 RID: 5110
			internal Rectangle outerRect = Rectangle.Empty;

			// Token: 0x040013F7 RID: 5111
			internal Region region;

			// Token: 0x040013F8 RID: 5112
			internal object component;

			// Token: 0x040013F9 RID: 5113
			private Control control;

			// Token: 0x040013FA RID: 5114
			private SelectionStyles selectionStyle;

			// Token: 0x040013FB RID: 5115
			private SelectionRules selectionRules;

			// Token: 0x040013FC RID: 5116
			private ISelectionUIHandler handler;
		}

		// Token: 0x02000289 RID: 649
		private class ContainerSelectionUIItem : SelectionUIService.SelectionUIItem
		{
			// Token: 0x06001821 RID: 6177 RVA: 0x0007E71A File Offset: 0x0007D71A
			public ContainerSelectionUIItem(SelectionUIService selUIsvc, object component)
				: base(selUIsvc, component)
			{
			}

			// Token: 0x06001822 RID: 6178 RVA: 0x0007E724 File Offset: 0x0007D724
			public override Cursor GetCursorAtPoint(Point pt)
			{
				if ((this.GetHitTest(pt) & 512) != 0 && (base.GetRules() & SelectionRules.Moveable) != SelectionRules.None)
				{
					return Cursors.SizeAll;
				}
				return null;
			}

			// Token: 0x06001823 RID: 6179 RVA: 0x0007E74C File Offset: 0x0007D74C
			public override int GetHitTest(Point pt)
			{
				int num = 256;
				if ((base.GetRules() & SelectionRules.Visible) != SelectionRules.None && !this.outerRect.IsEmpty)
				{
					Rectangle rectangle = new Rectangle(this.outerRect.X, this.outerRect.Y, 13, 13);
					if (rectangle.Contains(pt))
					{
						num = 512;
						if ((base.GetRules() & SelectionRules.Moveable) != SelectionRules.None)
						{
							num |= 12;
						}
					}
				}
				return num;
			}

			// Token: 0x06001824 RID: 6180 RVA: 0x0007E7C0 File Offset: 0x0007D7C0
			public override void DoPaint(Graphics gr)
			{
				if ((base.GetRules() & SelectionRules.Visible) == SelectionRules.None)
				{
					return;
				}
				Rectangle rectangle = new Rectangle(this.outerRect.X, this.outerRect.Y, 13, 13);
				ControlPaint.DrawContainerGrabHandle(gr, rectangle);
			}

			// Token: 0x06001825 RID: 6181 RVA: 0x0007E804 File Offset: 0x0007D804
			public override Region GetRegion()
			{
				if (this.region == null)
				{
					if ((base.GetRules() & SelectionRules.Visible) != SelectionRules.None && !this.outerRect.IsEmpty)
					{
						Rectangle rectangle = new Rectangle(this.outerRect.X, this.outerRect.Y, 13, 13);
						this.region = new Region(rectangle);
					}
					else
					{
						this.region = new Region(new Rectangle(0, 0, 0, 0));
					}
				}
				return this.region;
			}

			// Token: 0x040013FD RID: 5117
			public const int CONTAINER_WIDTH = 13;

			// Token: 0x040013FE RID: 5118
			public const int CONTAINER_HEIGHT = 13;
		}

		// Token: 0x0200028A RID: 650
		private struct HitTestInfo
		{
			// Token: 0x06001826 RID: 6182 RVA: 0x0007E87D File Offset: 0x0007D87D
			public HitTestInfo(int hitTest, SelectionUIService.SelectionUIItem selectionUIHit)
			{
				this.hitTest = hitTest;
				this.selectionUIHit = selectionUIHit;
				this.containerSelector = false;
			}

			// Token: 0x06001827 RID: 6183 RVA: 0x0007E894 File Offset: 0x0007D894
			public HitTestInfo(int hitTest, SelectionUIService.SelectionUIItem selectionUIHit, bool containerSelector)
			{
				this.hitTest = hitTest;
				this.selectionUIHit = selectionUIHit;
				this.containerSelector = containerSelector;
			}

			// Token: 0x06001828 RID: 6184 RVA: 0x0007E8AC File Offset: 0x0007D8AC
			public override bool Equals(object obj)
			{
				try
				{
					SelectionUIService.HitTestInfo hitTestInfo = (SelectionUIService.HitTestInfo)obj;
					return this.hitTest == hitTestInfo.hitTest && this.selectionUIHit == hitTestInfo.selectionUIHit && this.containerSelector == hitTestInfo.containerSelector;
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				return false;
			}

			// Token: 0x06001829 RID: 6185 RVA: 0x0007E914 File Offset: 0x0007D914
			public static bool operator ==(SelectionUIService.HitTestInfo left, SelectionUIService.HitTestInfo right)
			{
				return left.hitTest == right.hitTest && left.selectionUIHit == right.selectionUIHit && left.containerSelector == right.containerSelector;
			}

			// Token: 0x0600182A RID: 6186 RVA: 0x0007E948 File Offset: 0x0007D948
			public static bool operator !=(SelectionUIService.HitTestInfo left, SelectionUIService.HitTestInfo right)
			{
				return !(left == right);
			}

			// Token: 0x0600182B RID: 6187 RVA: 0x0007E954 File Offset: 0x0007D954
			public override int GetHashCode()
			{
				int num = this.hitTest | this.selectionUIHit.GetHashCode();
				if (this.containerSelector)
				{
					num |= 65536;
				}
				return num;
			}

			// Token: 0x040013FF RID: 5119
			public readonly int hitTest;

			// Token: 0x04001400 RID: 5120
			public readonly SelectionUIService.SelectionUIItem selectionUIHit;

			// Token: 0x04001401 RID: 5121
			public readonly bool containerSelector;
		}
	}
}
