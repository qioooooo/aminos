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
	internal class TabControlDesigner : ParentControlDesigner
	{
		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}

		protected override bool DrawGrid
		{
			get
			{
				return !this.disableDrawGrid && base.DrawGrid;
			}
		}

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

		public override bool CanParent(Control control)
		{
			return control is TabPage && !this.Control.Contains(control);
		}

		private void CheckVerbStatus()
		{
			if (this.removeVerb != null)
			{
				this.removeVerb.Enabled = this.Control.Controls.Count > 0;
			}
		}

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

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			this.CheckVerbStatus();
		}

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

		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control != null && !e.Control.IsHandleCreated)
			{
				IntPtr handle = e.Control.Handle;
			}
		}

		private void OnRightToLeftLayoutChanged(object sender, EventArgs e)
		{
			if (base.BehaviorService != null)
			{
				base.BehaviorService.SyncSelection();
			}
		}

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

		private bool tabControlSelected;

		private DesignerVerbCollection verbs;

		private DesignerVerb removeVerb;

		private bool disableDrawGrid;

		private int persistedSelectedIndex;

		private bool addingOnInitialize;

		private bool forwardOnDrag;
	}
}
