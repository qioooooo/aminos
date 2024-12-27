using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200029B RID: 667
	internal class TabControlDesigner : ParentControlDesigner
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x0008405C File Offset: 0x0008305C
		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0008405F File Offset: 0x0008305F
		protected override bool DrawGrid
		{
			get
			{
				return !this.disableDrawGrid && base.DrawGrid;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x00084074 File Offset: 0x00083074
		public override bool ParticipatesWithSnapLines
		{
			get
			{
				if (!this.forwardOnDrag)
				{
					return false;
				}
				TabPageDesigner selectedTabPageDesigner = this.GetSelectedTabPageDesigner();
				return selectedTabPageDesigner == null || selectedTabPageDesigner.ParticipatesWithSnapLines;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x0008409D File Offset: 0x0008309D
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x000840A5 File Offset: 0x000830A5
		private int SelectedIndex
		{
			get
			{
				return this.persistedSelectedIndex;
			}
			set
			{
				this.persistedSelectedIndex = value;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x000840B0 File Offset: 0x000830B0
		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.removeVerb = new DesignerVerb(SR.GetString("TabControlRemove"), new EventHandler(this.OnRemove));
					this.verbs = new DesignerVerbCollection();
					this.verbs.Add(new DesignerVerb(SR.GetString("TabControlAdd"), new EventHandler(this.OnAdd)));
					this.verbs.Add(this.removeVerb);
				}
				if (this.Control != null)
				{
					this.removeVerb.Enabled = this.Control.Controls.Count > 0;
				}
				return this.verbs;
			}
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x00084158 File Offset: 0x00083158
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			try
			{
				this.addingOnInitialize = true;
				this.OnAdd(this, EventArgs.Empty);
				this.OnAdd(this, EventArgs.Empty);
			}
			finally
			{
				this.addingOnInitialize = false;
			}
			MemberDescriptor memberDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
			base.RaiseComponentChanging(memberDescriptor);
			base.RaiseComponentChanged(memberDescriptor, null, null);
			TabControl tabControl = (TabControl)base.Component;
			if (tabControl != null)
			{
				tabControl.SelectedIndex = 0;
			}
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x000841E4 File Offset: 0x000831E4
		public override bool CanParent(Control control)
		{
			return control is TabPage && !this.Control.Contains(control);
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x000841FF File Offset: 0x000831FF
		private void CheckVerbStatus()
		{
			if (this.removeVerb != null)
			{
				this.removeVerb.Enabled = this.Control.Controls.Count > 0;
			}
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00084228 File Offset: 0x00083228
		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			TabControl tabControl = (TabControl)this.Control;
			if (tabControl.SelectedTab == null)
			{
				throw new ArgumentException(SR.GetString("TabControlInvalidTabPageType", new object[] { tool.DisplayName }));
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				TabPageDesigner tabPageDesigner = (TabPageDesigner)designerHost.GetDesigner(tabControl.SelectedTab);
				ParentControlDesigner.InvokeCreateTool(tabPageDesigner, tool);
			}
			return null;
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x000842A0 File Offset: 0x000832A0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SelectionChanged -= this.OnSelectionChanged;
				}
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
				}
				TabControl tabControl = this.Control as TabControl;
				if (tabControl != null)
				{
					tabControl.SelectedIndexChanged -= this.OnTabSelectedIndexChanged;
					tabControl.GotFocus -= this.OnGotFocus;
					tabControl.RightToLeftLayoutChanged -= this.OnRightToLeftLayoutChanged;
					tabControl.ControlAdded -= this.OnControlAdded;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00084368 File Offset: 0x00083368
		protected override bool GetHitTest(Point point)
		{
			TabControl tabControl = (TabControl)this.Control;
			if (this.tabControlSelected)
			{
				Point point2 = this.Control.PointToClient(point);
				return !tabControl.DisplayRectangle.Contains(point2);
			}
			return false;
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x000843AC File Offset: 0x000833AC
		internal static TabPage GetTabPageOfComponent(object comp)
		{
			if (!(comp is Control))
			{
				return null;
			}
			Control control = (Control)comp;
			while (control != null && !(control is TabPage))
			{
				control = control.Parent;
			}
			return (TabPage)control;
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x000843E4 File Offset: 0x000833E4
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			TabControl tabControl = component as TabControl;
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += this.OnSelectionChanged;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
			if (tabControl != null)
			{
				tabControl.SelectedIndexChanged += this.OnTabSelectedIndexChanged;
				tabControl.GotFocus += this.OnGotFocus;
				tabControl.RightToLeftLayoutChanged += this.OnRightToLeftLayoutChanged;
				tabControl.ControlAdded += this.OnControlAdded;
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000844A8 File Offset: 0x000834A8
		private void OnAdd(object sender, EventArgs eevent)
		{
			TabControl tabControl = (TabControl)base.Component;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				DesignerTransaction designerTransaction = null;
				try
				{
					try
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("TabControlAddTab", new object[] { base.Component.Site.Name }));
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return;
						}
						throw ex;
					}
					MemberDescriptor memberDescriptor = TypeDescriptor.GetProperties(tabControl)["Controls"];
					TabPage tabPage = (TabPage)designerHost.CreateComponent(typeof(TabPage));
					if (!this.addingOnInitialize)
					{
						base.RaiseComponentChanging(memberDescriptor);
					}
					tabPage.Padding = new Padding(3);
					string text = null;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(tabPage)["Name"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
					{
						text = (string)propertyDescriptor.GetValue(tabPage);
					}
					if (text != null)
					{
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(tabPage)["Text"];
						if (propertyDescriptor2 != null)
						{
							propertyDescriptor2.SetValue(tabPage, text);
						}
					}
					PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(tabPage)["UseVisualStyleBackColor"];
					if (propertyDescriptor3 != null && propertyDescriptor3.PropertyType == typeof(bool) && !propertyDescriptor3.IsReadOnly && propertyDescriptor3.IsBrowsable)
					{
						propertyDescriptor3.SetValue(tabPage, true);
					}
					tabControl.Controls.Add(tabPage);
					tabControl.SelectedIndex = tabControl.TabCount - 1;
					if (!this.addingOnInitialize)
					{
						base.RaiseComponentChanged(memberDescriptor, null, null);
					}
				}
				finally
				{
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
					}
				}
			}
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x00084684 File Offset: 0x00083684
		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			this.CheckVerbStatus();
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0008468C File Offset: 0x0008368C
		private void OnGotFocus(object sender, EventArgs e)
		{
			IEventHandlerService eventHandlerService = (IEventHandlerService)this.GetService(typeof(IEventHandlerService));
			if (eventHandlerService != null)
			{
				Control focusWindow = eventHandlerService.FocusWindow;
				if (focusWindow != null)
				{
					focusWindow.Focus();
				}
			}
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x000846C4 File Offset: 0x000836C4
		private void OnRemove(object sender, EventArgs eevent)
		{
			TabControl tabControl = (TabControl)base.Component;
			if (tabControl == null || tabControl.TabPages.Count == 0)
			{
				return;
			}
			MemberDescriptor memberDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
			TabPage selectedTab = tabControl.SelectedTab;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				DesignerTransaction designerTransaction = null;
				try
				{
					try
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("TabControlRemoveTab", new object[]
						{
							((IComponent)selectedTab).Site.Name,
							base.Component.Site.Name
						}));
						base.RaiseComponentChanging(memberDescriptor);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return;
						}
						throw ex;
					}
					designerHost.DestroyComponent(selectedTab);
					base.RaiseComponentChanged(memberDescriptor, null, null);
				}
				finally
				{
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
					}
				}
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x000847BC File Offset: 0x000837BC
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			try
			{
				this.disableDrawGrid = true;
				base.OnPaintAdornments(pe);
			}
			finally
			{
				this.disableDrawGrid = false;
			}
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x000847F4 File Offset: 0x000837F4
		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control != null && !e.Control.IsHandleCreated)
			{
				IntPtr handle = e.Control.Handle;
			}
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00084817 File Offset: 0x00083817
		private void OnRightToLeftLayoutChanged(object sender, EventArgs e)
		{
			if (base.BehaviorService != null)
			{
				base.BehaviorService.SyncSelection();
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0008482C File Offset: 0x0008382C
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			this.tabControlSelected = false;
			if (selectionService != null)
			{
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				TabControl tabControl = (TabControl)base.Component;
				foreach (object obj in selectedComponents)
				{
					if (obj == tabControl)
					{
						this.tabControlSelected = true;
					}
					TabPage tabPageOfComponent = TabControlDesigner.GetTabPageOfComponent(obj);
					if (tabPageOfComponent != null && tabPageOfComponent.Parent == tabControl)
					{
						this.tabControlSelected = false;
						tabControl.SelectedTab = tabPageOfComponent;
						SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
						selectionManager.Refresh();
						break;
					}
				}
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00084904 File Offset: 0x00083904
		private void OnTabSelectedIndexChanged(object sender, EventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				TabControl tabControl = (TabControl)base.Component;
				bool flag = false;
				foreach (object obj in selectedComponents)
				{
					TabPage tabPageOfComponent = TabControlDesigner.GetTabPageOfComponent(obj);
					if (tabPageOfComponent != null && tabPageOfComponent.Parent == tabControl && tabPageOfComponent == tabControl.SelectedTab)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					selectionService.SetSelectedComponents(new object[] { base.Component });
				}
			}
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x000849C8 File Offset: 0x000839C8
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "SelectedIndex" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(TabControlDesigner), propertyDescriptor, array2);
				}
			}
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00084A34 File Offset: 0x00083A34
		private TabPageDesigner GetSelectedTabPageDesigner()
		{
			TabPageDesigner tabPageDesigner = null;
			TabPage selectedTab = ((TabControl)base.Component).SelectedTab;
			if (selectedTab != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					tabPageDesigner = designerHost.GetDesigner(selectedTab) as TabPageDesigner;
				}
			}
			return tabPageDesigner;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x00084A80 File Offset: 0x00083A80
		protected override void OnDragEnter(DragEventArgs de)
		{
			this.forwardOnDrag = false;
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				int num = -1;
				ArrayList sortedDragControls = behaviorDataObject.GetSortedDragControls(ref num);
				if (sortedDragControls != null)
				{
					for (int i = 0; i < sortedDragControls.Count; i++)
					{
						if (!(sortedDragControls[i] is Control) || (sortedDragControls[i] is Control && !(sortedDragControls[i] is TabPage)))
						{
							this.forwardOnDrag = true;
							break;
						}
					}
				}
			}
			else
			{
				this.forwardOnDrag = true;
			}
			if (this.forwardOnDrag)
			{
				TabPageDesigner selectedTabPageDesigner = this.GetSelectedTabPageDesigner();
				if (selectedTabPageDesigner != null)
				{
					selectedTabPageDesigner.OnDragEnterInternal(de);
					return;
				}
			}
			else
			{
				base.OnDragEnter(de);
			}
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00084B24 File Offset: 0x00083B24
		protected override void OnDragDrop(DragEventArgs de)
		{
			if (this.forwardOnDrag)
			{
				TabPageDesigner selectedTabPageDesigner = this.GetSelectedTabPageDesigner();
				if (selectedTabPageDesigner != null)
				{
					selectedTabPageDesigner.OnDragDropInternal(de);
				}
			}
			else
			{
				base.OnDragDrop(de);
			}
			this.forwardOnDrag = false;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00084B5C File Offset: 0x00083B5C
		protected override void OnDragLeave(EventArgs e)
		{
			if (this.forwardOnDrag)
			{
				TabPageDesigner selectedTabPageDesigner = this.GetSelectedTabPageDesigner();
				if (selectedTabPageDesigner != null)
				{
					selectedTabPageDesigner.OnDragLeaveInternal(e);
				}
			}
			else
			{
				base.OnDragLeave(e);
			}
			this.forwardOnDrag = false;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00084B94 File Offset: 0x00083B94
		protected override void OnDragOver(DragEventArgs de)
		{
			if (this.forwardOnDrag)
			{
				TabControl tabControl = (TabControl)this.Control;
				Point point = this.Control.PointToClient(new Point(de.X, de.Y));
				if (!tabControl.DisplayRectangle.Contains(point))
				{
					de.Effect = DragDropEffects.None;
					return;
				}
				TabPageDesigner selectedTabPageDesigner = this.GetSelectedTabPageDesigner();
				if (selectedTabPageDesigner != null)
				{
					selectedTabPageDesigner.OnDragOverInternal(de);
					return;
				}
			}
			else
			{
				base.OnDragOver(de);
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00084C04 File Offset: 0x00083C04
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			if (this.forwardOnDrag)
			{
				TabPageDesigner selectedTabPageDesigner = this.GetSelectedTabPageDesigner();
				if (selectedTabPageDesigner != null)
				{
					selectedTabPageDesigner.OnGiveFeedbackInternal(e);
					return;
				}
			}
			else
			{
				base.OnGiveFeedback(e);
			}
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00084C34 File Offset: 0x00083C34
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 123)
			{
				if (msg != 132)
				{
					switch (msg)
					{
					case 276:
					case 277:
						base.BehaviorService.Invalidate(base.BehaviorService.ControlRectInAdornerWindow(this.Control));
						base.WndProc(ref m);
						return;
					default:
						base.WndProc(ref m);
						break;
					}
				}
				else
				{
					base.WndProc(ref m);
					if ((int)m.Result == -1)
					{
						m.Result = (IntPtr)1;
						return;
					}
				}
				return;
			}
			int num = NativeMethods.Util.SignedLOWORD((int)m.LParam);
			int num2 = NativeMethods.Util.SignedHIWORD((int)m.LParam);
			if (num == -1 && num2 == -1)
			{
				Point position = Cursor.Position;
				num = position.X;
				num2 = position.Y;
			}
			this.OnContextMenu(num, num2);
		}

		// Token: 0x0400146C RID: 5228
		private bool tabControlSelected;

		// Token: 0x0400146D RID: 5229
		private DesignerVerbCollection verbs;

		// Token: 0x0400146E RID: 5230
		private DesignerVerb removeVerb;

		// Token: 0x0400146F RID: 5231
		private bool disableDrawGrid;

		// Token: 0x04001470 RID: 5232
		private int persistedSelectedIndex;

		// Token: 0x04001471 RID: 5233
		private bool addingOnInitialize;

		// Token: 0x04001472 RID: 5234
		private bool forwardOnDrag;
	}
}
