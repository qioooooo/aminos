using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000211 RID: 529
	internal class DesignerActionUI : IDisposable
	{
		// Token: 0x060013E6 RID: 5094 RVA: 0x00065224 File Offset: 0x00064224
		public DesignerActionUI(IServiceProvider serviceProvider, Adorner containerAdorner)
		{
			this.serviceProvider = serviceProvider;
			this.designerActionAdorner = containerAdorner;
			this.behaviorService = (BehaviorService)serviceProvider.GetService(typeof(BehaviorService));
			this.menuCommandService = (IMenuCommandService)serviceProvider.GetService(typeof(IMenuCommandService));
			this.selSvc = (ISelectionService)serviceProvider.GetService(typeof(ISelectionService));
			if (this.behaviorService == null || this.selSvc == null)
			{
				return;
			}
			this.designerActionService = (DesignerActionService)serviceProvider.GetService(typeof(DesignerActionService));
			if (this.designerActionService == null)
			{
				this.designerActionService = new DesignerActionService(serviceProvider);
				this.disposeActionService = true;
			}
			this.designerActionUIService = (DesignerActionUIService)serviceProvider.GetService(typeof(DesignerActionUIService));
			if (this.designerActionUIService == null)
			{
				this.designerActionUIService = new DesignerActionUIService(serviceProvider);
				this.disposeActionUIService = true;
			}
			this.designerActionUIService.DesignerActionUIStateChange += this.OnDesignerActionUIStateChange;
			this.designerActionService.DesignerActionListsChanged += this.OnDesignerActionsChanged;
			this.lastPanelComponent = null;
			IComponentChangeService componentChangeService = (IComponentChangeService)serviceProvider.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
			if (this.menuCommandService != null)
			{
				this.cmdShowDesignerActions = new MenuCommand(new EventHandler(this.OnKeyShowDesignerActions), MenuCommands.KeyInvokeSmartTag);
				this.menuCommandService.AddCommand(this.cmdShowDesignerActions);
			}
			this.uiService = (IUIService)serviceProvider.GetService(typeof(IUIService));
			if (this.uiService != null)
			{
				this.mainParentWindow = this.uiService.GetDialogOwnerWindow();
			}
			this.componentToGlyph = new Hashtable();
			this.marshalingControl = new Control();
			this.marshalingControl.CreateControl();
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00065400 File Offset: 0x00064400
		public void Dispose()
		{
			if (this.marshalingControl != null)
			{
				this.marshalingControl.Dispose();
				this.marshalingControl = null;
			}
			if (this.serviceProvider != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.serviceProvider.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
				}
				if (this.cmdShowDesignerActions != null)
				{
					IMenuCommandService menuCommandService = (IMenuCommandService)this.serviceProvider.GetService(typeof(IMenuCommandService));
					if (menuCommandService != null)
					{
						menuCommandService.RemoveCommand(this.cmdShowDesignerActions);
					}
				}
			}
			this.serviceProvider = null;
			this.behaviorService = null;
			this.selSvc = null;
			if (this.designerActionService != null)
			{
				this.designerActionService.DesignerActionListsChanged -= this.OnDesignerActionsChanged;
				if (this.disposeActionService)
				{
					this.designerActionService.Dispose();
				}
			}
			this.designerActionService = null;
			if (this.designerActionUIService != null)
			{
				this.designerActionUIService.DesignerActionUIStateChange -= this.OnDesignerActionUIStateChange;
				if (this.disposeActionUIService)
				{
					this.designerActionUIService.Dispose();
				}
			}
			this.designerActionUIService = null;
			this.designerActionAdorner = null;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0006551F File Offset: 0x0006451F
		public DesignerActionGlyph GetDesignerActionGlyph(IComponent comp)
		{
			return this.GetDesignerActionGlyph(comp, null);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0006552C File Offset: 0x0006452C
		internal DesignerActionGlyph GetDesignerActionGlyph(IComponent comp, DesignerActionListCollection dalColl)
		{
			InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(comp)[typeof(InheritanceAttribute)];
			if (inheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				return null;
			}
			if (dalColl == null)
			{
				dalColl = this.designerActionService.GetComponentActions(comp);
			}
			if (dalColl != null && dalColl.Count > 0)
			{
				DesignerActionGlyph designerActionGlyph = null;
				if (this.componentToGlyph[comp] == null)
				{
					DesignerActionBehavior designerActionBehavior = new DesignerActionBehavior(this.serviceProvider, comp, dalColl, this);
					if (!(comp is Control) || comp is ToolStripDropDown)
					{
						ComponentTray componentTray = this.serviceProvider.GetService(typeof(ComponentTray)) as ComponentTray;
						if (componentTray != null)
						{
							ComponentTray.TrayControl trayControlFromComponent = componentTray.GetTrayControlFromComponent(comp);
							if (trayControlFromComponent != null)
							{
								Rectangle bounds = trayControlFromComponent.Bounds;
								designerActionGlyph = new DesignerActionGlyph(designerActionBehavior, bounds, componentTray);
							}
						}
					}
					if (designerActionGlyph == null)
					{
						designerActionGlyph = new DesignerActionGlyph(designerActionBehavior, this.designerActionAdorner);
					}
					if (designerActionGlyph != null)
					{
						this.componentToGlyph.Add(comp, designerActionGlyph);
					}
				}
				else
				{
					designerActionGlyph = this.componentToGlyph[comp] as DesignerActionGlyph;
					if (designerActionGlyph != null)
					{
						DesignerActionBehavior designerActionBehavior2 = designerActionGlyph.Behavior as DesignerActionBehavior;
						if (designerActionBehavior2 != null)
						{
							designerActionBehavior2.ActionLists = dalColl;
						}
						designerActionGlyph.Invalidate();
					}
				}
				return designerActionGlyph;
			}
			this.RemoveActionGlyph(comp);
			return null;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00065650 File Offset: 0x00064650
		private void OnComponentChanged(object source, ComponentChangedEventArgs ce)
		{
			if (ce.Component == null || ce.Member == null || !this.IsDesignerActionPanelVisible)
			{
				return;
			}
			if (this.lastPanelComponent != null && !this.lastPanelComponent.Equals(ce.Component))
			{
				return;
			}
			DesignerActionGlyph designerActionGlyph = this.componentToGlyph[ce.Component] as DesignerActionGlyph;
			if (designerActionGlyph != null)
			{
				designerActionGlyph.Invalidate();
				if (ce.Member.Name.Equals("Dock"))
				{
					this.RecreatePanel(ce.Component as IComponent);
				}
				if (ce.Member.Name.Equals("Location") || ce.Member.Name.Equals("Width") || ce.Member.Name.Equals("Height"))
				{
					this.UpdateDAPLocation(ce.Component as IComponent, designerActionGlyph);
				}
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00065738 File Offset: 0x00064738
		private void RecreatePanel(IComponent comp)
		{
			if (this.inTransaction || comp != this.selSvc.PrimarySelection)
			{
				return;
			}
			IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null)
			{
				bool flag = false;
				IDesignerHostTransactionState designerHostTransactionState = designerHost as IDesignerHostTransactionState;
				if (designerHostTransactionState != null)
				{
					flag = designerHostTransactionState.IsClosingTransaction;
				}
				if (designerHost.InTransaction && !flag)
				{
					designerHost.TransactionClosed += this.DesignerTransactionClosed;
					this.inTransaction = true;
					this.relatedComponentTransaction = comp;
					return;
				}
			}
			this.RecreateInternal(comp);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000657C0 File Offset: 0x000647C0
		private void DesignerTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction && this.relatedComponentTransaction != null)
			{
				this.inTransaction = false;
				IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
				designerHost.TransactionClosed -= this.DesignerTransactionClosed;
				this.RecreateInternal(this.relatedComponentTransaction);
				this.relatedComponentTransaction = null;
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00065824 File Offset: 0x00064824
		private void RecreateInternal(IComponent comp)
		{
			DesignerActionGlyph designerActionGlyph = this.GetDesignerActionGlyph(comp);
			if (designerActionGlyph != null)
			{
				this.VerifyGlyphIsInAdorner(designerActionGlyph);
				this.RecreatePanel(designerActionGlyph);
				this.UpdateDAPLocation(comp, designerActionGlyph);
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00065854 File Offset: 0x00064854
		private void RecreatePanel(Glyph glyphWithPanelToRegen)
		{
			if (!this.IsDesignerActionPanelVisible)
			{
				return;
			}
			if (glyphWithPanelToRegen != null)
			{
				DesignerActionBehavior designerActionBehavior = glyphWithPanelToRegen.Behavior as DesignerActionBehavior;
				if (designerActionBehavior != null)
				{
					DesignerActionPanel currentPanel = this.designerActionHost.CurrentPanel;
					currentPanel.UpdateTasks(designerActionBehavior.ActionLists, new DesignerActionListCollection(), SR.GetString("DesignerActionPanel_DefaultPanelTitle", new object[] { designerActionBehavior.RelatedComponent.GetType().Name }), null);
					this.designerActionHost.UpdateContainerSize();
				}
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x000658CC File Offset: 0x000648CC
		private void VerifyGlyphIsInAdorner(DesignerActionGlyph glyph)
		{
			if (glyph.IsInComponentTray)
			{
				ComponentTray componentTray = this.serviceProvider.GetService(typeof(ComponentTray)) as ComponentTray;
				if (componentTray.SelectionGlyphs != null && !componentTray.SelectionGlyphs.Contains(glyph))
				{
					componentTray.SelectionGlyphs.Insert(0, glyph);
				}
			}
			else if (this.designerActionAdorner != null && this.designerActionAdorner.Glyphs != null && !this.designerActionAdorner.Glyphs.Contains(glyph))
			{
				this.designerActionAdorner.Glyphs.Insert(0, glyph);
			}
			glyph.InvalidateOwnerLocation();
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00065964 File Offset: 0x00064964
		private void OnDesignerActionsChanged(object sender, DesignerActionListsChangedEventArgs e)
		{
			if (this.marshalingControl != null && this.marshalingControl.IsHandleCreated)
			{
				this.marshalingControl.BeginInvoke(new DesignerActionUI.ActionChangedEventHandler(this.OnInvokedDesignerActionChanged), new object[] { sender, e });
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000659B0 File Offset: 0x000649B0
		private void OnDesignerActionUIStateChange(object sender, DesignerActionUIStateChangeEventArgs e)
		{
			IComponent component = e.RelatedObject as IComponent;
			if (component != null)
			{
				DesignerActionGlyph designerActionGlyph = this.GetDesignerActionGlyph(component);
				if (designerActionGlyph != null)
				{
					if (e.ChangeType == DesignerActionUIStateChangeType.Show)
					{
						DesignerActionBehavior designerActionBehavior = designerActionGlyph.Behavior as DesignerActionBehavior;
						if (designerActionBehavior != null)
						{
							designerActionBehavior.ShowUI(designerActionGlyph);
							return;
						}
					}
					else if (e.ChangeType == DesignerActionUIStateChangeType.Hide)
					{
						DesignerActionBehavior designerActionBehavior2 = designerActionGlyph.Behavior as DesignerActionBehavior;
						if (designerActionBehavior2 != null)
						{
							designerActionBehavior2.HideUI();
							return;
						}
					}
					else if (e.ChangeType == DesignerActionUIStateChangeType.Refresh)
					{
						designerActionGlyph.Invalidate();
						this.RecreatePanel((IComponent)e.RelatedObject);
						return;
					}
				}
			}
			else if (e.ChangeType == DesignerActionUIStateChangeType.Hide)
			{
				this.HideDesignerActionPanel();
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00065A48 File Offset: 0x00064A48
		private void OnInvokedDesignerActionChanged(object sender, DesignerActionListsChangedEventArgs e)
		{
			IComponent component = e.RelatedObject as IComponent;
			DesignerActionGlyph designerActionGlyph = null;
			if (e.ChangeType == DesignerActionListsChangedType.ActionListsAdded)
			{
				if (component == null)
				{
					return;
				}
				IComponent component2 = this.selSvc.PrimarySelection as IComponent;
				if (component2 == e.RelatedObject)
				{
					designerActionGlyph = this.GetDesignerActionGlyph(component, e.ActionLists);
					if (designerActionGlyph != null)
					{
						this.VerifyGlyphIsInAdorner(designerActionGlyph);
					}
					else
					{
						this.RemoveActionGlyph(e.RelatedObject);
					}
				}
			}
			if (e.ChangeType == DesignerActionListsChangedType.ActionListsRemoved && e.ActionLists.Count == 0)
			{
				this.RemoveActionGlyph(e.RelatedObject);
				return;
			}
			if (designerActionGlyph != null)
			{
				this.RecreatePanel(component);
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00065ADC File Offset: 0x00064ADC
		private void OnKeyShowDesignerActions(object sender, EventArgs e)
		{
			this.ShowDesignerActionPanelForPrimarySelection();
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00065AE8 File Offset: 0x00064AE8
		internal bool ShowDesignerActionPanelForPrimarySelection()
		{
			if (this.selSvc == null)
			{
				return false;
			}
			object primarySelection = this.selSvc.PrimarySelection;
			if (primarySelection == null || !this.componentToGlyph.Contains(primarySelection))
			{
				return false;
			}
			DesignerActionGlyph designerActionGlyph = (DesignerActionGlyph)this.componentToGlyph[primarySelection];
			if (designerActionGlyph != null && designerActionGlyph.Behavior is DesignerActionBehavior)
			{
				DesignerActionBehavior designerActionBehavior = designerActionGlyph.Behavior as DesignerActionBehavior;
				if (designerActionBehavior != null)
				{
					if (!this.IsDesignerActionPanelVisible)
					{
						designerActionBehavior.ShowUI(designerActionGlyph);
						return true;
					}
					designerActionBehavior.HideUI();
					return false;
				}
			}
			return false;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00065B6C File Offset: 0x00064B6C
		internal void RemoveActionGlyph(object relatedObject)
		{
			if (relatedObject == null)
			{
				return;
			}
			if (this.IsDesignerActionPanelVisible && relatedObject == this.lastPanelComponent)
			{
				this.HideDesignerActionPanel();
			}
			DesignerActionGlyph designerActionGlyph = (DesignerActionGlyph)this.componentToGlyph[relatedObject];
			if (designerActionGlyph != null)
			{
				ComponentTray componentTray = this.serviceProvider.GetService(typeof(ComponentTray)) as ComponentTray;
				if (componentTray != null && componentTray.SelectionGlyphs != null && componentTray != null && componentTray.SelectionGlyphs.Contains(designerActionGlyph))
				{
					componentTray.SelectionGlyphs.Remove(designerActionGlyph);
				}
				if (this.designerActionAdorner.Glyphs.Contains(designerActionGlyph))
				{
					this.designerActionAdorner.Glyphs.Remove(designerActionGlyph);
				}
				this.componentToGlyph.Remove(relatedObject);
				IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null && designerHost.InTransaction)
				{
					designerHost.TransactionClosed += this.InvalidateGlyphOnLastTransaction;
					this.relatedGlyphTransaction = designerActionGlyph;
				}
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00065C60 File Offset: 0x00064C60
		private void InvalidateGlyphOnLastTransaction(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction)
			{
				IDesignerHost designerHost = ((this.serviceProvider != null) ? (this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost) : null);
				if (designerHost != null)
				{
					designerHost.TransactionClosed -= this.InvalidateGlyphOnLastTransaction;
				}
				if (this.relatedGlyphTransaction != null)
				{
					this.relatedGlyphTransaction.InvalidateOwnerLocation();
				}
				this.relatedGlyphTransaction = null;
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00065CCA File Offset: 0x00064CCA
		internal void HideDesignerActionPanel()
		{
			if (this.IsDesignerActionPanelVisible)
			{
				this.designerActionHost.Close();
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00065CDF File Offset: 0x00064CDF
		internal bool IsDesignerActionPanelVisible
		{
			get
			{
				return this.designerActionHost != null && this.designerActionHost.Visible;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00065CF6 File Offset: 0x00064CF6
		internal IComponent LastPanelComponent
		{
			get
			{
				if (!this.IsDesignerActionPanelVisible)
				{
					return null;
				}
				return this.lastPanelComponent;
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00065D08 File Offset: 0x00064D08
		private void toolStripDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (this.cancelClose || e.Cancel)
			{
				e.Cancel = true;
				return;
			}
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
			{
				e.Cancel = true;
			}
			if (e.CloseReason == ToolStripDropDownCloseReason.Keyboard)
			{
				e.Cancel = false;
			}
			if (!e.Cancel)
			{
				if (this.lastPanelComponent == null)
				{
					return;
				}
				Point lastCursorPoint = DesignerUtils.LastCursorPoint;
				DesignerActionGlyph designerActionGlyph = this.componentToGlyph[this.lastPanelComponent] as DesignerActionGlyph;
				if (designerActionGlyph != null)
				{
					Point glyphLocationScreenCoord = this.GetGlyphLocationScreenCoord(this.lastPanelComponent, designerActionGlyph);
					if (new Rectangle(glyphLocationScreenCoord, new Size(designerActionGlyph.Bounds.Width, designerActionGlyph.Bounds.Height)).Contains(lastCursorPoint))
					{
						DesignerActionBehavior designerActionBehavior = designerActionGlyph.Behavior as DesignerActionBehavior;
						designerActionBehavior.IgnoreNextMouseUp = true;
					}
					designerActionGlyph.InvalidateOwnerLocation();
				}
				this.lastPanelComponent = null;
				this.behaviorService.PopBehavior(this.dapkb);
			}
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00065DF8 File Offset: 0x00064DF8
		internal Point UpdateDAPLocation(IComponent component, DesignerActionGlyph glyph)
		{
			if (component == null)
			{
				component = this.lastPanelComponent;
			}
			if (this.designerActionHost == null)
			{
				return Point.Empty;
			}
			if (component == null || glyph == null)
			{
				return this.designerActionHost.Location;
			}
			if (this.behaviorService != null && !this.behaviorService.AdornerWindowControl.DisplayRectangle.IntersectsWith(glyph.Bounds))
			{
				this.HideDesignerActionPanel();
				return this.designerActionHost.Location;
			}
			Point glyphLocationScreenCoord = this.GetGlyphLocationScreenCoord(component, glyph);
			Rectangle rectangle = new Rectangle(glyphLocationScreenCoord, glyph.Bounds.Size);
			DockStyle dockStyle;
			Point point = DesignerActionPanel.ComputePreferredDesktopLocation(rectangle, this.designerActionHost.Size, out dockStyle);
			glyph.DockEdge = dockStyle;
			this.designerActionHost.Location = point;
			return point;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00065EB4 File Offset: 0x00064EB4
		private Point GetGlyphLocationScreenCoord(IComponent relatedComponent, Glyph glyph)
		{
			Point point = new Point(0, 0);
			if (relatedComponent is Control && !(relatedComponent is ToolStripDropDown))
			{
				point = this.behaviorService.AdornerWindowPointToScreen(glyph.Bounds.Location);
			}
			else if (relatedComponent is ToolStripItem)
			{
				ToolStripItem toolStripItem = relatedComponent as ToolStripItem;
				if (toolStripItem != null && toolStripItem.Owner != null)
				{
					point = this.behaviorService.AdornerWindowPointToScreen(glyph.Bounds.Location);
				}
			}
			else if (relatedComponent != null)
			{
				ComponentTray componentTray = this.serviceProvider.GetService(typeof(ComponentTray)) as ComponentTray;
				if (componentTray != null)
				{
					point = componentTray.PointToScreen(glyph.Bounds.Location);
				}
			}
			return point;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00065F68 File Offset: 0x00064F68
		internal void ShowDesignerActionPanel(IComponent relatedComponent, DesignerActionPanel panel, DesignerActionGlyph glyph)
		{
			if (this.designerActionHost == null)
			{
				this.designerActionHost = new DesignerActionToolStripDropDown(this, this.mainParentWindow);
				this.designerActionHost.AutoSize = false;
				this.designerActionHost.Padding = Padding.Empty;
				this.designerActionHost.Renderer = new NoBorderRenderer();
				this.designerActionHost.Text = "DesignerActionTopLevelForm";
				this.designerActionHost.Closing += this.toolStripDropDown_Closing;
			}
			this.designerActionHost.AccessibleName = SR.GetString("DesignerActionPanel_DefaultPanelTitle", new object[] { relatedComponent.GetType().Name });
			panel.AccessibleName = SR.GetString("DesignerActionPanel_DefaultPanelTitle", new object[] { relatedComponent.GetType().Name });
			this.designerActionHost.SetDesignerActionPanel(panel, glyph);
			Point point = this.UpdateDAPLocation(relatedComponent, glyph);
			if (this.behaviorService != null && this.behaviorService.AdornerWindowControl.DisplayRectangle.IntersectsWith(glyph.Bounds))
			{
				if (this.mainParentWindow != null && this.mainParentWindow.Handle != IntPtr.Zero)
				{
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this.designerActionHost, this.designerActionHost.Handle), -8, new HandleRef(this.mainParentWindow, this.mainParentWindow.Handle));
				}
				this.cancelClose = true;
				this.designerActionHost.Show(point);
				this.designerActionHost.Focus();
				this.designerActionHost.BeginInvoke(new EventHandler(this.OnShowComplete));
				glyph.InvalidateOwnerLocation();
				this.lastPanelComponent = relatedComponent;
				this.dapkb = new DesignerActionKeyboardBehavior(this.designerActionHost.CurrentPanel, this.serviceProvider, this.behaviorService);
				this.behaviorService.PushBehavior(this.dapkb);
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00066144 File Offset: 0x00065144
		private void OnShowComplete(object sender, EventArgs e)
		{
			this.cancelClose = false;
			if (this.designerActionHost != null && this.designerActionHost.Handle != IntPtr.Zero && this.designerActionHost.Visible)
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this, this.designerActionHost.Handle));
				this.designerActionHost.CheckFocusIsRight();
			}
		}

		// Token: 0x040011D1 RID: 4561
		private static TraceSwitch DesigneActionPanelTraceSwitch = new TraceSwitch("DesigneActionPanelTrace", "DesignerActionPanel tracing");

		// Token: 0x040011D2 RID: 4562
		private Adorner designerActionAdorner;

		// Token: 0x040011D3 RID: 4563
		private IServiceProvider serviceProvider;

		// Token: 0x040011D4 RID: 4564
		private ISelectionService selSvc;

		// Token: 0x040011D5 RID: 4565
		private DesignerActionService designerActionService;

		// Token: 0x040011D6 RID: 4566
		private DesignerActionUIService designerActionUIService;

		// Token: 0x040011D7 RID: 4567
		private BehaviorService behaviorService;

		// Token: 0x040011D8 RID: 4568
		private IMenuCommandService menuCommandService;

		// Token: 0x040011D9 RID: 4569
		private DesignerActionKeyboardBehavior dapkb;

		// Token: 0x040011DA RID: 4570
		private Hashtable componentToGlyph;

		// Token: 0x040011DB RID: 4571
		private Control marshalingControl;

		// Token: 0x040011DC RID: 4572
		private IComponent lastPanelComponent;

		// Token: 0x040011DD RID: 4573
		private IUIService uiService;

		// Token: 0x040011DE RID: 4574
		private IWin32Window mainParentWindow;

		// Token: 0x040011DF RID: 4575
		internal DesignerActionToolStripDropDown designerActionHost;

		// Token: 0x040011E0 RID: 4576
		private MenuCommand cmdShowDesignerActions;

		// Token: 0x040011E1 RID: 4577
		private bool inTransaction;

		// Token: 0x040011E2 RID: 4578
		private IComponent relatedComponentTransaction;

		// Token: 0x040011E3 RID: 4579
		private DesignerActionGlyph relatedGlyphTransaction;

		// Token: 0x040011E4 RID: 4580
		private bool disposeActionService;

		// Token: 0x040011E5 RID: 4581
		private bool disposeActionUIService;

		// Token: 0x040011E6 RID: 4582
		internal static readonly TraceSwitch DropDownVisibilityDebug;

		// Token: 0x040011E7 RID: 4583
		private bool cancelClose;

		// Token: 0x02000212 RID: 530
		// (Invoke) Token: 0x06001401 RID: 5121
		private delegate void ActionChangedEventHandler(object sender, DesignerActionListsChangedEventArgs e);
	}
}
