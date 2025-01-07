using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	internal sealed class SelectionUIService : Control, ISelectionUIService
	{
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

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style &= -100663297;
				return createParams;
			}
		}

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

		private ISelectionUIHandler GetHandler(object component)
		{
			return (ISelectionUIHandler)this.selectionHandlers[component];
		}

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

		private void OnTransactionOpened(object sender, EventArgs e)
		{
			this.batchMode = true;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.UpdateWindowRegion();
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs ccevent)
		{
			if (!this.batchMode)
			{
				((ISelectionUIService)this).SyncSelection();
				return;
			}
			this.batchChanged = true;
		}

		private void OnComponentRemove(object sender, ComponentEventArgs ce)
		{
			this.selectionHandlers.Remove(ce.Component);
			this.selectionItems.Remove(ce.Component);
			((ISelectionUIService)this).SyncComponent(ce.Component);
		}

		private void OnContainerSelectorActive(ContainerSelectorActiveEventArgs e)
		{
			if (this.containerSelectorActive != null)
			{
				this.containerSelectorActive(this, e);
			}
		}

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

		private void OnSystemSettingChanged(object sender, EventArgs e)
		{
			base.Invalidate();
		}

		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			base.Invalidate();
		}

		protected override void OnDragEnter(DragEventArgs devent)
		{
			base.OnDragEnter(devent);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragEnter(devent);
			}
		}

		protected override void OnDragOver(DragEventArgs devent)
		{
			base.OnDragOver(devent);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragOver(devent);
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragLeave();
			}
		}

		protected override void OnDragDrop(DragEventArgs devent)
		{
			base.OnDragDrop(devent);
			if (this.dragHandler != null)
			{
				this.dragHandler.OleDragDrop(devent);
			}
		}

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

		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			base.Invalidate();
		}

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

		bool ISelectionUIService.Dragging
		{
			get
			{
				return this.dragHandler != null;
			}
		}

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

		void ISelectionUIService.ClearSelectionUIHandler(object component, ISelectionUIHandler handler)
		{
			ISelectionUIHandler selectionUIHandler = (ISelectionUIHandler)this.selectionHandlers[component];
			if (selectionUIHandler == handler)
			{
				this.selectionHandlers[component] = null;
			}
		}

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

		bool ISelectionUIService.GetAdornmentHitTest(object component, Point value)
		{
			return this.GetHitTest(value, 3).hitTest != 256;
		}

		bool ISelectionUIService.GetContainerSelected(object component)
		{
			return component != null && this.selectionItems[component] is SelectionUIService.ContainerSelectionUIItem;
		}

		SelectionRules ISelectionUIService.GetSelectionRules(object component)
		{
			SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
			if (selectionUIItem == null)
			{
				throw new InvalidOperationException();
			}
			return selectionUIItem.GetRules();
		}

		SelectionStyles ISelectionUIService.GetSelectionStyle(object component)
		{
			SelectionUIService.SelectionUIItem selectionUIItem = (SelectionUIService.SelectionUIItem)this.selectionItems[component];
			if (selectionUIItem == null)
			{
				return SelectionStyles.None;
			}
			return selectionUIItem.Style;
		}

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

		private const int HITTEST_CONTAINER_SELECTOR = 1;

		private const int HITTEST_NORMAL_SELECTION = 2;

		private const int HITTEST_DEFAULT = 3;

		private static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		private ISelectionUIHandler dragHandler;

		private object[] dragComponents;

		private SelectionRules dragRules;

		private bool dragMoved;

		private object containerDrag;

		private bool ignoreCaptureChanged;

		private bool mouseDown;

		private int mouseDragHitTest;

		private Point mouseDragAnchor = SelectionUIService.InvalidPoint;

		private Rectangle mouseDragOffset = Rectangle.Empty;

		private Point lastMoveScreenCoord = Point.Empty;

		private bool ctrlSelect;

		private bool mouseDragging;

		private ContainerSelectorActiveEventHandler containerSelectorActive;

		private Hashtable selectionItems;

		private Hashtable selectionHandlers;

		private bool savedVisible;

		private bool batchMode;

		private bool batchChanged;

		private bool batchSync;

		private ISelectionService selSvc;

		private IDesignerHost host;

		private DesignerTransaction dragTransaction;

		private class SelectionUIItem
		{
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

			public SelectionRules GetRules()
			{
				return this.selectionRules;
			}

			public void Dispose()
			{
				if (this.region != null)
				{
					this.region.Dispose();
					this.region = null;
				}
			}

			public void Invalidate()
			{
				if (!this.outerRect.IsEmpty && !this.selUIsvc.Disposing)
				{
					this.selUIsvc.Invalidate(this.outerRect);
				}
			}

			protected bool PointWithinSelection(Point pt)
			{
				return (this.GetRules() & SelectionRules.Visible) != SelectionRules.None && !this.outerRect.IsEmpty && !this.innerRect.IsEmpty && pt.X >= this.outerRect.X && pt.X <= this.outerRect.X + this.outerRect.Width && pt.Y >= this.outerRect.Y && pt.Y <= this.outerRect.Y + this.outerRect.Height && (pt.X <= this.innerRect.X || pt.X >= this.innerRect.X + this.innerRect.Width || pt.Y <= this.innerRect.Y || pt.Y >= this.innerRect.Y + this.innerRect.Height);
			}

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

			public const int SIZE_X = 1;

			public const int SIZE_Y = 2;

			public const int SIZE_MASK = 3;

			public const int MOVE_X = 4;

			public const int MOVE_Y = 8;

			public const int MOVE_MASK = 12;

			public const int POS_LEFT = 16;

			public const int POS_TOP = 32;

			public const int POS_RIGHT = 64;

			public const int POS_BOTTOM = 128;

			public const int POS_MASK = 240;

			public const int NOHIT = 256;

			public const int CONTAINER_SELECTOR = 512;

			public const int GRABHANDLE_WIDTH = 7;

			public const int GRABHANDLE_HEIGHT = 7;

			internal static readonly int[] activeSizeArray = new int[] { 51, 34, 99, 17, 65, 147, 130, 195 };

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

			internal static readonly int[] inactiveSizeArray;

			internal static readonly Cursor[] inactiveCursorArray;

			internal int[] sizes;

			internal Cursor[] cursors;

			internal SelectionUIService selUIsvc;

			internal Rectangle innerRect = Rectangle.Empty;

			internal Rectangle outerRect = Rectangle.Empty;

			internal Region region;

			internal object component;

			private Control control;

			private SelectionStyles selectionStyle;

			private SelectionRules selectionRules;

			private ISelectionUIHandler handler;
		}

		private class ContainerSelectionUIItem : SelectionUIService.SelectionUIItem
		{
			public ContainerSelectionUIItem(SelectionUIService selUIsvc, object component)
				: base(selUIsvc, component)
			{
			}

			public override Cursor GetCursorAtPoint(Point pt)
			{
				if ((this.GetHitTest(pt) & 512) != 0 && (base.GetRules() & SelectionRules.Moveable) != SelectionRules.None)
				{
					return Cursors.SizeAll;
				}
				return null;
			}

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

			public override void DoPaint(Graphics gr)
			{
				if ((base.GetRules() & SelectionRules.Visible) == SelectionRules.None)
				{
					return;
				}
				Rectangle rectangle = new Rectangle(this.outerRect.X, this.outerRect.Y, 13, 13);
				ControlPaint.DrawContainerGrabHandle(gr, rectangle);
			}

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

			public const int CONTAINER_WIDTH = 13;

			public const int CONTAINER_HEIGHT = 13;
		}

		private struct HitTestInfo
		{
			public HitTestInfo(int hitTest, SelectionUIService.SelectionUIItem selectionUIHit)
			{
				this.hitTest = hitTest;
				this.selectionUIHit = selectionUIHit;
				this.containerSelector = false;
			}

			public HitTestInfo(int hitTest, SelectionUIService.SelectionUIItem selectionUIHit, bool containerSelector)
			{
				this.hitTest = hitTest;
				this.selectionUIHit = selectionUIHit;
				this.containerSelector = containerSelector;
			}

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

			public static bool operator ==(SelectionUIService.HitTestInfo left, SelectionUIService.HitTestInfo right)
			{
				return left.hitTest == right.hitTest && left.selectionUIHit == right.selectionUIHit && left.containerSelector == right.containerSelector;
			}

			public static bool operator !=(SelectionUIService.HitTestInfo left, SelectionUIService.HitTestInfo right)
			{
				return !(left == right);
			}

			public override int GetHashCode()
			{
				int num = this.hitTest | this.selectionUIHit.GetHashCode();
				if (this.containerSelector)
				{
					num |= 65536;
				}
				return num;
			}

			public readonly int hitTest;

			public readonly SelectionUIService.SelectionUIItem selectionUIHit;

			public readonly bool containerSelector;
		}
	}
}
