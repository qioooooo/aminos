using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	[ToolboxItem(false)]
	[ProvideProperty("Location", typeof(IComponent))]
	[ProvideProperty("TrayLocation", typeof(IComponent))]
	[DesignTimeVisible(false)]
	public class ComponentTray : ScrollableControl, IExtenderProvider, ISelectionUIHandler, IOleDragClient
	{
		public ComponentTray(IDesigner mainDesigner, IServiceProvider serviceProvider)
		{
			this.AutoScroll = true;
			this.mainDesigner = mainDesigner;
			this.serviceProvider = serviceProvider;
			this.AllowDrop = true;
			this.Text = "ComponentTray";
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			this.controls = new ArrayList();
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			IExtenderProviderService extenderProviderService = (IExtenderProviderService)this.GetService(typeof(IExtenderProviderService));
			if (extenderProviderService != null)
			{
				extenderProviderService.AddExtenderProvider(this);
			}
			if (this.GetService(typeof(IEventHandlerService)) == null && designerHost != null)
			{
				this.eventHandlerService = new EventHandlerService(this);
				designerHost.AddService(typeof(IEventHandlerService), this.eventHandlerService);
			}
			IMenuCommandService menuService = this.MenuService;
			if (menuService != null)
			{
				this.menucmdArrangeIcons = new MenuCommand(new EventHandler(this.OnMenuArrangeIcons), StandardCommands.ArrangeIcons);
				this.menucmdLineupIcons = new MenuCommand(new EventHandler(this.OnMenuLineupIcons), StandardCommands.LineupIcons);
				this.menucmdLargeIcons = new MenuCommand(new EventHandler(this.OnMenuShowLargeIcons), StandardCommands.ShowLargeIcons);
				this.menucmdArrangeIcons.Checked = this.AutoArrange;
				this.menucmdLargeIcons.Checked = this.ShowLargeIcons;
				menuService.AddCommand(this.menucmdArrangeIcons);
				menuService.AddCommand(this.menucmdLineupIcons);
				menuService.AddCommand(this.menucmdLargeIcons);
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved += this.OnComponentRemoved;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				Color color;
				if (iuiservice.Styles["VsColorDesignerTray"] is Color)
				{
					color = (Color)iuiservice.Styles["VsColorDesignerTray"];
				}
				else if (iuiservice.Styles["HighlightColor"] is Color)
				{
					color = (Color)iuiservice.Styles["HighlightColor"];
				}
				else
				{
					color = SystemColors.Info;
				}
				this.BackColor = color;
				this.Font = (Font)iuiservice.Styles["DialogFont"];
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += this.OnSelectionChanged;
			}
			SystemEvents.DisplaySettingsChanged += this.OnSystemSettingChanged;
			SystemEvents.InstalledFontsChanged += this.OnSystemSettingChanged;
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
			TypeDescriptor.Refreshed += this.OnComponentRefresh;
			BehaviorService behaviorService = this.GetService(typeof(BehaviorService)) as BehaviorService;
			if (behaviorService != null)
			{
				this.glyphManager = new ComponentTray.ComponentTrayGlyphManager(selectionService, behaviorService);
			}
		}

		public bool AutoArrange
		{
			get
			{
				return this.autoArrange;
			}
			set
			{
				if (this.autoArrange != value)
				{
					this.autoArrange = value;
					this.menucmdArrangeIcons.Checked = value;
					if (this.autoArrange)
					{
						this.DoAutoArrange(true);
					}
				}
			}
		}

		public int ComponentCount
		{
			get
			{
				return base.Controls.Count;
			}
		}

		internal virtual SelectionUIHandler DragHandler
		{
			get
			{
				if (this.dragHandler == null)
				{
					this.dragHandler = new ComponentTray.TraySelectionUIHandler(this);
				}
				return this.dragHandler;
			}
		}

		internal GlyphCollection SelectionGlyphs
		{
			get
			{
				if (this.glyphManager != null)
				{
					return this.glyphManager.SelectionGlyphs;
				}
				return null;
			}
		}

		private InheritanceUI InheritanceUI
		{
			get
			{
				if (this.inheritanceUI == null)
				{
					this.inheritanceUI = new InheritanceUI();
				}
				return this.inheritanceUI;
			}
		}

		private IMenuCommandService MenuService
		{
			get
			{
				if (this.menuCommandService == null)
				{
					this.menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
				}
				return this.menuCommandService;
			}
		}

		public bool ShowLargeIcons
		{
			get
			{
				return this.showLargeIcons;
			}
			set
			{
				if (this.showLargeIcons != value)
				{
					this.showLargeIcons = value;
					this.menucmdLargeIcons.Checked = this.ShowLargeIcons;
					this.ResetTrayControls();
					base.Invalidate(true);
				}
			}
		}

		private bool TabOrderActive
		{
			get
			{
				if (!this.queriedTabOrder)
				{
					this.queriedTabOrder = true;
					IMenuCommandService menuService = this.MenuService;
					if (menuService != null)
					{
						this.tabOrderCommand = menuService.FindCommand(StandardCommands.TabOrder);
					}
				}
				return this.tabOrderCommand != null && this.tabOrderCommand.Checked;
			}
		}

		internal bool IsWindowVisible
		{
			get
			{
				return base.IsHandleCreated && NativeMethods.IsWindowVisible(base.Handle);
			}
		}

		internal Size ParentGridSize
		{
			get
			{
				ParentControlDesigner parentControlDesigner = this.mainDesigner as ParentControlDesigner;
				if (parentControlDesigner != null)
				{
					return parentControlDesigner.ParentGridSize;
				}
				return new Size(8, 8);
			}
		}

		public virtual void AddComponent(IComponent component)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (!this.CanDisplayComponent(component))
			{
				return;
			}
			if (this.selectionUISvc == null)
			{
				this.selectionUISvc = (ISelectionUIService)this.GetService(typeof(ISelectionUIService));
				if (this.selectionUISvc == null)
				{
					this.selectionUISvc = new SelectionUIService(designerHost);
					designerHost.AddService(typeof(ISelectionUIService), this.selectionUISvc);
				}
				this.grabHandle = this.selectionUISvc.GetAdornmentDimensions(AdornmentType.GrabHandle);
			}
			ComponentTray.TrayControl trayControl = new ComponentTray.TrayControl(this, component);
			base.SuspendLayout();
			try
			{
				base.Controls.Add(trayControl);
				this.controls.Add(trayControl);
				TypeDescriptor.Refresh(component);
				if (designerHost != null && !designerHost.Loading)
				{
					this.PositionControl(trayControl);
				}
				if (this.selectionUISvc != null)
				{
					this.selectionUISvc.AssignSelectionUIHandler(component, this);
				}
				InheritanceAttribute inheritanceAttribute = trayControl.InheritanceAttribute;
				if (inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited)
				{
					InheritanceUI inheritanceUI = this.InheritanceUI;
					if (inheritanceUI != null)
					{
						inheritanceUI.AddInheritedControl(trayControl, inheritanceAttribute.InheritanceLevel);
					}
				}
			}
			finally
			{
				base.ResumeLayout();
			}
			if (designerHost != null && !designerHost.Loading)
			{
				base.ScrollControlIntoView(trayControl);
			}
		}

		bool IExtenderProvider.CanExtend(object component)
		{
			IComponent component2 = component as IComponent;
			return component2 != null && ComponentTray.TrayControl.FromComponent(component2) != null;
		}

		protected virtual bool CanCreateComponentFromTool(ToolboxItem tool)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			Type type = designerHost.GetType(tool.TypeName);
			if (type == null)
			{
				return true;
			}
			if (!type.IsSubclassOf(typeof(Control)))
			{
				return true;
			}
			Type designerType = this.GetDesignerType(type, typeof(IDesigner));
			return !typeof(ControlDesigner).IsAssignableFrom(designerType);
		}

		protected virtual bool CanDisplayComponent(IComponent component)
		{
			return TypeDescriptor.GetAttributes(component).Contains(DesignTimeVisibleAttribute.Yes);
		}

		public void CreateComponentFromTool(ToolboxItem tool)
		{
			if (!this.CanCreateComponentFromTool(tool))
			{
				return;
			}
			this.GetOleDragHandler().CreateTool(tool, null, 0, 0, 0, 0, false, false);
		}

		protected void DisplayError(Exception e)
		{
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
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
			if (disposing && this.controls != null)
			{
				IExtenderProviderService extenderProviderService = (IExtenderProviderService)this.GetService(typeof(IExtenderProviderService));
				bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
				if (extenderProviderService != null)
				{
					extenderProviderService.RemoveExtenderProvider(this);
				}
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (this.eventHandlerService != null && designerHost != null)
				{
					designerHost.RemoveService(typeof(IEventHandlerService));
					this.eventHandlerService = null;
				}
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				}
				TypeDescriptor.Refreshed -= this.OnComponentRefresh;
				SystemEvents.DisplaySettingsChanged -= this.OnSystemSettingChanged;
				SystemEvents.InstalledFontsChanged -= this.OnSystemSettingChanged;
				SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
				IMenuCommandService menuService = this.MenuService;
				if (menuService != null)
				{
					menuService.RemoveCommand(this.menucmdArrangeIcons);
					menuService.RemoveCommand(this.menucmdLineupIcons);
					menuService.RemoveCommand(this.menucmdLargeIcons);
				}
				if (this.privateCommandSet != null)
				{
					this.privateCommandSet.Dispose();
					if (designerHost != null)
					{
						designerHost.RemoveService(typeof(ISelectionUIService));
					}
				}
				this.selectionUISvc = null;
				if (this.inheritanceUI != null)
				{
					this.inheritanceUI.Dispose();
					this.inheritanceUI = null;
				}
				this.serviceProvider = null;
				this.controls.Clear();
				this.controls = null;
				if (this.glyphManager != null)
				{
					this.glyphManager.Dispose();
					this.glyphManager = null;
				}
			}
			base.Dispose(disposing);
		}

		private void DoAutoArrange(bool dirtyDesigner)
		{
			if (this.controls == null || this.controls.Count <= 0)
			{
				return;
			}
			this.controls.Sort(new ComponentTray.AutoArrangeComparer());
			base.SuspendLayout();
			base.AutoScrollPosition = new Point(0, 0);
			try
			{
				Control control = null;
				bool flag = true;
				foreach (object obj in this.controls)
				{
					Control control2 = (Control)obj;
					if (control2.Visible)
					{
						if (this.autoArrange)
						{
							this.PositionInNextAutoSlot(control2 as ComponentTray.TrayControl, control, dirtyDesigner);
						}
						else if (!((ComponentTray.TrayControl)control2).Positioned || !flag)
						{
							this.PositionInNextAutoSlot(control2 as ComponentTray.TrayControl, control, false);
							flag = false;
						}
						control = control2;
					}
				}
				if (this.selectionUISvc != null)
				{
					this.selectionUISvc.SyncSelection();
				}
			}
			finally
			{
				base.ResumeLayout();
			}
		}

		private void DoLineupIcons()
		{
			if (this.autoArrange)
			{
				return;
			}
			bool flag = this.autoArrange;
			this.autoArrange = true;
			try
			{
				this.DoAutoArrange(true);
			}
			finally
			{
				this.autoArrange = flag;
			}
		}

		private void DrawRubber(Point start, Point end)
		{
			this.mouseDragWorkspace.X = Math.Min(start.X, end.X);
			this.mouseDragWorkspace.Y = Math.Min(start.Y, end.Y);
			this.mouseDragWorkspace.Width = Math.Abs(end.X - start.X);
			this.mouseDragWorkspace.Height = Math.Abs(end.Y - start.Y);
			this.mouseDragWorkspace = base.RectangleToScreen(this.mouseDragWorkspace);
			ControlPaint.DrawReversibleFrame(this.mouseDragWorkspace, this.BackColor, FrameStyle.Dashed);
		}

		internal void FocusDesigner()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null && designerHost.RootComponent != null)
			{
				IRootDesigner rootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as IRootDesigner;
				if (rootDesigner != null)
				{
					ViewTechnology[] supportedTechnologies = rootDesigner.SupportedTechnologies;
					if (supportedTechnologies.Length > 0)
					{
						Control control = rootDesigner.GetView(supportedTechnologies[0]) as Control;
						if (control != null)
						{
							control.Focus();
						}
					}
				}
			}
		}

		private object[] GetComponentsInRect(Rectangle rect)
		{
			ArrayList arrayList = new ArrayList();
			int count = base.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				Control control = base.Controls[i];
				Rectangle bounds = control.Bounds;
				ComponentTray.TrayControl trayControl = control as ComponentTray.TrayControl;
				if (trayControl != null && bounds.IntersectsWith(rect))
				{
					arrayList.Add(trayControl.Component);
				}
			}
			return arrayList.ToArray();
		}

		private Type GetDesignerType(Type t, Type designerBaseType)
		{
			Type type = null;
			AttributeCollection attributes = TypeDescriptor.GetAttributes(t);
			for (int i = 0; i < attributes.Count; i++)
			{
				DesignerAttribute designerAttribute = attributes[i] as DesignerAttribute;
				if (designerAttribute != null)
				{
					Type type2 = Type.GetType(designerAttribute.DesignerBaseTypeName);
					if (type2 != null && type2 == designerBaseType)
					{
						bool flag = false;
						ITypeResolutionService typeResolutionService = (ITypeResolutionService)this.GetService(typeof(ITypeResolutionService));
						if (typeResolutionService != null)
						{
							flag = true;
							type = typeResolutionService.GetType(designerAttribute.DesignerTypeName);
						}
						if (!flag)
						{
							type = Type.GetType(designerAttribute.DesignerTypeName);
						}
						if (type != null)
						{
							break;
						}
					}
				}
			}
			return type;
		}

		internal Size GetDragDimensions()
		{
			if (this.AutoArrange)
			{
				ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
				IComponent component = null;
				if (selectionService != null)
				{
					component = (IComponent)selectionService.PrimarySelection;
				}
				Control control = null;
				if (component != null)
				{
					control = ((IOleDragClient)this).GetControlForComponent(component);
				}
				if (control == null && this.controls.Count > 0)
				{
					control = (Control)this.controls[0];
				}
				if (control != null)
				{
					Size size = control.Size;
					size.Width += 2 * this.whiteSpace.X;
					size.Height += 2 * this.whiteSpace.Y;
					return size;
				}
			}
			return new Size(10, 10);
		}

		public IComponent GetNextComponent(IComponent component, bool forward)
		{
			int i = 0;
			while (i < this.controls.Count)
			{
				ComponentTray.TrayControl trayControl = (ComponentTray.TrayControl)this.controls[i];
				if (trayControl.Component == component)
				{
					int num = (forward ? (i + 1) : (i - 1));
					if (num >= 0 && num < this.controls.Count)
					{
						return ((ComponentTray.TrayControl)this.controls[num]).Component;
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			if (this.controls.Count > 0)
			{
				int num2 = (forward ? 0 : (this.controls.Count - 1));
				return ((ComponentTray.TrayControl)this.controls[num2]).Component;
			}
			return null;
		}

		internal virtual OleDragDropHandler GetOleDragHandler()
		{
			if (this.oleDragDropHandler == null)
			{
				this.oleDragDropHandler = new ComponentTray.TrayOleDragDropHandler(this.DragHandler, this.serviceProvider, this);
			}
			return this.oleDragDropHandler;
		}

		[DesignOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlLocationDescr")]
		[Category("Layout")]
		[Browsable(false)]
		[Localizable(false)]
		public Point GetLocation(IComponent receiver)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(receiver.GetType())["Location"];
			if (propertyDescriptor != null)
			{
				return (Point)propertyDescriptor.GetValue(receiver);
			}
			return this.GetTrayLocation(receiver);
		}

		[SRDescription("ControlLocationDescr")]
		[DesignOnly(true)]
		[Category("Layout")]
		[Localizable(false)]
		[Browsable(false)]
		public Point GetTrayLocation(IComponent receiver)
		{
			Control control = ComponentTray.TrayControl.FromComponent(receiver);
			if (control == null)
			{
				return default(Point);
			}
			Point location = control.Location;
			Point autoScrollPosition = base.AutoScrollPosition;
			return new Point(location.X - autoScrollPosition.X, location.Y - autoScrollPosition.Y);
		}

		protected override object GetService(Type serviceType)
		{
			object obj = null;
			if (this.serviceProvider != null)
			{
				obj = this.serviceProvider.GetService(serviceType);
			}
			return obj;
		}

		internal ComponentTray.TrayControl GetTrayControlFromComponent(IComponent comp)
		{
			return ComponentTray.TrayControl.FromComponent(comp);
		}

		public bool IsTrayComponent(IComponent comp)
		{
			if (ComponentTray.TrayControl.FromComponent(comp) == null)
			{
				return false;
			}
			foreach (object obj in base.Controls)
			{
				Control control = (Control)obj;
				ComponentTray.TrayControl trayControl = control as ComponentTray.TrayControl;
				if (trayControl != null && trayControl.Component == comp)
				{
					return true;
				}
			}
			return false;
		}

		private void OnComponentRefresh(RefreshEventArgs e)
		{
			IComponent component = e.ComponentChanged as IComponent;
			if (component != null)
			{
				ComponentTray.TrayControl trayControl = ComponentTray.TrayControl.FromComponent(component);
				if (trayControl != null)
				{
					bool flag = this.CanDisplayComponent(component);
					if (flag != trayControl.Visible || !flag)
					{
						trayControl.Visible = flag;
						Rectangle bounds = trayControl.Bounds;
						bounds.Inflate(this.grabHandle);
						bounds.Inflate(this.grabHandle);
						base.Invalidate(bounds);
						base.PerformLayout();
					}
				}
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs cevent)
		{
			this.RemoveComponent(cevent.Component);
		}

		internal void UpdatePastePositions(ArrayList components)
		{
			foreach (object obj in components)
			{
				ComponentTray.TrayControl trayControl = (ComponentTray.TrayControl)obj;
				if (!this.CanDisplayComponent(trayControl.Component))
				{
					break;
				}
				if (this.mouseDropLocation == ComponentTray.InvalidPoint)
				{
					Control control = null;
					if (this.controls.Count > 1)
					{
						control = (Control)this.controls[this.controls.Count - 1];
					}
					this.PositionInNextAutoSlot(trayControl, control, true);
				}
				else
				{
					this.PositionControl(trayControl);
				}
				trayControl.BringToFront();
			}
		}

		private void OnContextMenu(int x, int y, bool useSelection)
		{
			if (!this.TabOrderActive)
			{
				base.Capture = false;
				IMenuCommandService menuService = this.MenuService;
				if (menuService != null)
				{
					base.Capture = false;
					Cursor.Clip = Rectangle.Empty;
					ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					if (useSelection && selectionService != null && (1 != selectionService.SelectionCount || selectionService.PrimarySelection != this.mainDesigner.Component))
					{
						menuService.ShowContextMenu(MenuCommands.TraySelectionMenu, x, y);
						return;
					}
					menuService.ShowContextMenu(MenuCommands.ComponentTrayMenu, x, y);
				}
			}
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (this.glyphManager != null && this.glyphManager.OnMouseDoubleClick(e))
			{
				return;
			}
			base.OnDoubleClick(e);
			if (!this.TabOrderActive)
			{
				this.OnLostCapture();
				IEventBindingService eventBindingService = (IEventBindingService)this.GetService(typeof(IEventBindingService));
				bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
				if (eventBindingService != null)
				{
					eventBindingService.ShowCode();
				}
			}
		}

		protected override void OnGiveFeedback(GiveFeedbackEventArgs gfevent)
		{
			base.OnGiveFeedback(gfevent);
			this.GetOleDragHandler().DoOleGiveFeedback(gfevent);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			this.mouseDropLocation = base.PointToClient(new Point(de.X, de.Y));
			this.autoScrollPosBeforeDragging = base.AutoScrollPosition;
			if (this.mouseDragTool != null)
			{
				ToolboxItem toolboxItem = this.mouseDragTool;
				this.mouseDragTool = null;
				bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
				try
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					IDesigner designer = designerHost.GetDesigner(designerHost.RootComponent);
					IToolboxUser toolboxUser = designer as IToolboxUser;
					if (toolboxUser != null)
					{
						toolboxUser.ToolPicked(toolboxItem);
					}
					else
					{
						this.CreateComponentFromTool(toolboxItem);
					}
				}
				catch (Exception ex)
				{
					this.DisplayError(ex);
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				catch
				{
				}
				de.Effect = DragDropEffects.Copy;
			}
			else
			{
				this.GetOleDragHandler().DoOleDragDrop(de);
			}
			this.mouseDropLocation = ComponentTray.InvalidPoint;
			base.ResumeLayout();
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			if (!this.TabOrderActive)
			{
				base.SuspendLayout();
				if (this.toolboxService == null)
				{
					this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
				}
				OleDragDropHandler oleDragHandler = this.GetOleDragHandler();
				object[] draggingObjects = oleDragHandler.GetDraggingObjects(de);
				if (this.toolboxService != null && draggingObjects == null)
				{
					this.mouseDragTool = this.toolboxService.DeserializeToolboxItem(de.Data, (IDesignerHost)this.GetService(typeof(IDesignerHost)));
				}
				if (this.mouseDragTool != null)
				{
					if ((de.AllowedEffect & DragDropEffects.Move) != DragDropEffects.None)
					{
						de.Effect = DragDropEffects.Move;
						return;
					}
					de.Effect = DragDropEffects.Copy;
					return;
				}
				else
				{
					oleDragHandler.DoOleDragEnter(de);
				}
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			this.mouseDragTool = null;
			this.GetOleDragHandler().DoOleDragLeave();
			base.ResumeLayout();
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			if (this.mouseDragTool != null)
			{
				de.Effect = DragDropEffects.Copy;
				return;
			}
			this.GetOleDragHandler().DoOleDragOver(de);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			this.DoAutoArrange(false);
			base.Invalidate(true);
			base.OnLayout(levent);
		}

		protected virtual void OnLostCapture()
		{
			if (this.mouseDragStart != ComponentTray.InvalidPoint)
			{
				Cursor.Clip = Rectangle.Empty;
				if (this.mouseDragEnd != ComponentTray.InvalidPoint)
				{
					this.DrawRubber(this.mouseDragStart, this.mouseDragEnd);
					this.mouseDragEnd = ComponentTray.InvalidPoint;
				}
				this.mouseDragStart = ComponentTray.InvalidPoint;
			}
		}

		private void OnMenuArrangeIcons(object sender, EventArgs e)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			DesignerTransaction designerTransaction = null;
			try
			{
				designerTransaction = designerHost.CreateTransaction(SR.GetString("TrayAutoArrange"));
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.mainDesigner.Component)["TrayAutoArrange"];
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(this.mainDesigner.Component, !this.AutoArrange);
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

		private void OnMenuShowLargeIcons(object sender, EventArgs e)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			DesignerTransaction designerTransaction = null;
			try
			{
				designerTransaction = designerHost.CreateTransaction(SR.GetString("TrayShowLargeIcons"));
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.mainDesigner.Component)["TrayLargeIcon"];
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(this.mainDesigner.Component, !this.ShowLargeIcons);
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

		private void OnMenuLineupIcons(object sender, EventArgs e)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			DesignerTransaction designerTransaction = null;
			try
			{
				designerTransaction = designerHost.CreateTransaction(SR.GetString("TrayLineUpIcons"));
				this.DoLineupIcons();
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
		}

		internal void OnMessage(ref Message m)
		{
			this.WndProc(ref m);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (this.glyphManager != null && this.glyphManager.OnMouseDown(e))
			{
				return;
			}
			base.OnMouseDown(e);
			if (!this.TabOrderActive)
			{
				if (this.toolboxService == null)
				{
					this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
				}
				this.FocusDesigner();
				if (e.Button == MouseButtons.Left && this.toolboxService != null)
				{
					ToolboxItem selectedToolboxItem = this.toolboxService.GetSelectedToolboxItem((IDesignerHost)this.GetService(typeof(IDesignerHost)));
					if (selectedToolboxItem != null)
					{
						this.mouseDropLocation = new Point(e.X, e.Y);
						try
						{
							this.CreateComponentFromTool(selectedToolboxItem);
							this.toolboxService.SelectedToolboxItemUsed();
						}
						catch (Exception ex)
						{
							this.DisplayError(ex);
							if (ClientUtils.IsCriticalException(ex))
							{
								throw;
							}
						}
						catch
						{
						}
						this.mouseDropLocation = ComponentTray.InvalidPoint;
						return;
					}
				}
				if (e.Button == MouseButtons.Left)
				{
					this.mouseDragStart = new Point(e.X, e.Y);
					base.Capture = true;
					Cursor.Clip = base.RectangleToScreen(base.ClientRectangle);
					return;
				}
				try
				{
					ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
					if (selectionService != null)
					{
						selectionService.SetSelectedComponents(new object[] { this.mainDesigner.Component });
					}
				}
				catch (Exception ex2)
				{
					if (ClientUtils.IsCriticalException(ex2))
					{
						throw;
					}
				}
				catch
				{
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this.glyphManager != null && this.glyphManager.OnMouseMove(e))
			{
				return;
			}
			base.OnMouseMove(e);
			if (this.mouseDragStart != ComponentTray.InvalidPoint)
			{
				if (this.mouseDragEnd != ComponentTray.InvalidPoint)
				{
					this.DrawRubber(this.mouseDragStart, this.mouseDragEnd);
				}
				else
				{
					this.mouseDragEnd = new Point(0, 0);
				}
				this.mouseDragEnd.X = e.X;
				this.mouseDragEnd.Y = e.Y;
				this.DrawRubber(this.mouseDragStart, this.mouseDragEnd);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (this.glyphManager != null && this.glyphManager.OnMouseUp(e))
			{
				return;
			}
			if (this.mouseDragStart != ComponentTray.InvalidPoint && e.Button == MouseButtons.Left)
			{
				base.Capture = false;
				Cursor.Clip = Rectangle.Empty;
				object[] array;
				if (this.mouseDragEnd != ComponentTray.InvalidPoint)
				{
					this.DrawRubber(this.mouseDragStart, this.mouseDragEnd);
					array = this.GetComponentsInRect(new Rectangle
					{
						X = Math.Min(this.mouseDragStart.X, e.X),
						Y = Math.Min(this.mouseDragStart.Y, e.Y),
						Width = Math.Abs(e.X - this.mouseDragStart.X),
						Height = Math.Abs(e.Y - this.mouseDragStart.Y)
					});
					this.mouseDragEnd = ComponentTray.InvalidPoint;
				}
				else
				{
					array = new object[0];
				}
				if (array.Length == 0)
				{
					array = new object[] { this.mainDesigner.Component };
				}
				try
				{
					ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
					bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
					if (selectionService != null)
					{
						selectionService.SetSelectedComponents(array);
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				catch
				{
				}
				this.mouseDragStart = ComponentTray.InvalidPoint;
			}
			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (this.fResetAmbient)
			{
				this.fResetAmbient = false;
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					Color color;
					if (iuiservice.Styles["VsColorDesignerTray"] is Color)
					{
						color = (Color)iuiservice.Styles["VsColorDesignerTray"];
					}
					else if (iuiservice.Styles["HighlightColor"] is Color)
					{
						color = (Color)iuiservice.Styles["HighlightColor"];
					}
					else
					{
						color = SystemColors.Info;
					}
					this.BackColor = color;
					this.Font = (Font)iuiservice.Styles["DialogFont"];
				}
			}
			base.OnPaint(pe);
			Graphics graphics = pe.Graphics;
			if (this.selectedObjects != null)
			{
				bool flag = true;
				foreach (object obj in this.selectedObjects)
				{
					Control controlForComponent = ((IOleDragClient)this).GetControlForComponent(obj);
					if (controlForComponent != null && controlForComponent.Visible)
					{
						Rectangle bounds = controlForComponent.Bounds;
						NoResizeHandleGlyph noResizeHandleGlyph = new NoResizeHandleGlyph(bounds, SelectionRules.None, flag, null);
						DesignerUtils.DrawSelectionBorder(graphics, DesignerUtils.GetBoundsForNoResizeSelectionType(bounds, SelectionBorderGlyphType.Top));
						DesignerUtils.DrawSelectionBorder(graphics, DesignerUtils.GetBoundsForNoResizeSelectionType(bounds, SelectionBorderGlyphType.Bottom));
						DesignerUtils.DrawSelectionBorder(graphics, DesignerUtils.GetBoundsForNoResizeSelectionType(bounds, SelectionBorderGlyphType.Left));
						DesignerUtils.DrawSelectionBorder(graphics, DesignerUtils.GetBoundsForNoResizeSelectionType(bounds, SelectionBorderGlyphType.Right));
						DesignerUtils.DrawNoResizeHandle(graphics, noResizeHandleGlyph.Bounds, flag, noResizeHandleGlyph);
					}
					flag = false;
				}
			}
			if (this.glyphManager != null)
			{
				this.glyphManager.OnPaintGlyphs(pe);
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			this.selectedObjects = ((ISelectionService)sender).GetSelectedComponents();
			object primarySelection = ((ISelectionService)sender).PrimarySelection;
			base.Invalidate();
			foreach (object obj in this.selectedObjects)
			{
				IComponent component = obj as IComponent;
				if (component != null)
				{
					Control control = ComponentTray.TrayControl.FromComponent(component);
					if (control != null)
					{
						UnsafeNativeMethods.NotifyWinEvent(32775, new HandleRef(control, control.Handle), -4, 0);
					}
				}
			}
			IComponent component2 = primarySelection as IComponent;
			if (component2 != null)
			{
				Control control2 = ComponentTray.TrayControl.FromComponent(component2);
				if (control2 != null && base.IsHandleCreated)
				{
					base.ScrollControlIntoView(control2);
					UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(control2, control2.Handle), -4, 0);
				}
				if (this.glyphManager != null)
				{
					this.glyphManager.SelectionGlyphs.Clear();
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					foreach (object obj2 in this.selectedObjects)
					{
						IComponent component3 = obj2 as IComponent;
						if (component3 != null && !(designerHost.GetDesigner(component3) is ControlDesigner))
						{
							GlyphCollection glyphsForComponent = this.glyphManager.GetGlyphsForComponent(component3);
							if (glyphsForComponent != null && glyphsForComponent.Count > 0)
							{
								this.SelectionGlyphs.AddRange(glyphsForComponent);
							}
						}
					}
				}
			}
		}

		protected virtual void OnSetCursor()
		{
			if (this.toolboxService == null)
			{
				this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
			}
			if (this.toolboxService == null || !this.toolboxService.SetCursor())
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void OnSystemSettingChanged(object sender, EventArgs e)
		{
			this.fResetAmbient = true;
			this.ResetTrayControls();
			base.BeginInvoke(new ComponentTray.AsyncInvokeHandler(base.Invalidate), new object[] { true });
		}

		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			this.fResetAmbient = true;
			this.ResetTrayControls();
			base.BeginInvoke(new ComponentTray.AsyncInvokeHandler(base.Invalidate), new object[] { true });
		}

		private void PositionControl(ComponentTray.TrayControl c)
		{
			if (!this.autoArrange)
			{
				if (!(this.mouseDropLocation != ComponentTray.InvalidPoint))
				{
					Control control = null;
					if (this.controls.Count > 1)
					{
						int num = this.controls.IndexOf(c);
						if (num >= 1)
						{
							control = (Control)this.controls[num - 1];
						}
					}
					this.PositionInNextAutoSlot(c, control, true);
					return;
				}
				if (!c.Location.Equals(this.mouseDropLocation))
				{
					c.Location = this.mouseDropLocation;
					return;
				}
			}
			else
			{
				if (this.mouseDropLocation != ComponentTray.InvalidPoint)
				{
					this.RearrangeInAutoSlots(c, this.mouseDropLocation);
					return;
				}
				Control control2 = null;
				if (this.controls.Count > 1)
				{
					int num2 = this.controls.IndexOf(c);
					if (num2 >= 1)
					{
						control2 = (Control)this.controls[num2 - 1];
					}
				}
				this.PositionInNextAutoSlot(c, control2, true);
			}
		}

		private bool PositionInNextAutoSlot(ComponentTray.TrayControl c, Control prevCtl, bool dirtyDesigner)
		{
			if (this.whiteSpace.IsEmpty)
			{
				this.whiteSpace = new Point(this.selectionUISvc.GetAdornmentDimensions(AdornmentType.GrabHandle));
				this.whiteSpace.X = this.whiteSpace.X * 2 + 3;
				this.whiteSpace.Y = this.whiteSpace.Y * 2 + 3;
			}
			if (prevCtl == null)
			{
				Rectangle displayRectangle = this.DisplayRectangle;
				Point point = new Point(displayRectangle.X + this.whiteSpace.X, displayRectangle.Y + this.whiteSpace.Y);
				if (!c.Location.Equals(point))
				{
					c.Location = point;
					if (dirtyDesigner)
					{
						IComponent component = c.Component;
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["TrayLocation"];
						if (propertyDescriptor != null)
						{
							Point autoScrollPosition = base.AutoScrollPosition;
							point = new Point(point.X - autoScrollPosition.X, point.Y - autoScrollPosition.Y);
							propertyDescriptor.SetValue(component, point);
						}
					}
					else
					{
						c.Location = point;
					}
					return true;
				}
			}
			else
			{
				Rectangle bounds = prevCtl.Bounds;
				Point point2 = new Point(bounds.X + bounds.Width + this.whiteSpace.X, bounds.Y);
				if (point2.X + c.Size.Width > base.Size.Width)
				{
					point2.X = this.whiteSpace.X;
					point2.Y += bounds.Height + this.whiteSpace.Y;
				}
				if (!c.Location.Equals(point2))
				{
					if (dirtyDesigner)
					{
						IComponent component2 = c.Component;
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(component2)["TrayLocation"];
						if (propertyDescriptor2 != null)
						{
							Point autoScrollPosition2 = base.AutoScrollPosition;
							point2 = new Point(point2.X - autoScrollPosition2.X, point2.Y - autoScrollPosition2.Y);
							propertyDescriptor2.SetValue(component2, point2);
						}
					}
					else
					{
						c.Location = point2;
					}
					return true;
				}
			}
			return false;
		}

		public virtual void RemoveComponent(IComponent component)
		{
			ComponentTray.TrayControl trayControl = ComponentTray.TrayControl.FromComponent(component);
			if (trayControl != null)
			{
				try
				{
					InheritanceAttribute inheritanceAttribute = trayControl.InheritanceAttribute;
					if (inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited && this.inheritanceUI != null)
					{
						this.inheritanceUI.RemoveInheritedControl(trayControl);
					}
					if (this.controls != null)
					{
						int num = this.controls.IndexOf(trayControl);
						if (num != -1)
						{
							this.controls.RemoveAt(num);
						}
					}
				}
				finally
				{
					trayControl.Dispose();
				}
			}
		}

		private void ResetTrayControls()
		{
			Control.ControlCollection controlCollection = base.Controls;
			if (controlCollection == null)
			{
				return;
			}
			for (int i = 0; i < controlCollection.Count; i++)
			{
				ComponentTray.TrayControl trayControl = controlCollection[i] as ComponentTray.TrayControl;
				if (trayControl != null)
				{
					trayControl.fRecompute = true;
				}
			}
		}

		public void SetLocation(IComponent receiver, Point location)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null && designerHost.Loading)
			{
				this.SetTrayLocation(receiver, location);
				return;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(receiver.GetType())["Location"];
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(receiver, location);
				return;
			}
			this.SetTrayLocation(receiver, location);
		}

		public void SetTrayLocation(IComponent receiver, Point location)
		{
			ComponentTray.TrayControl trayControl = ComponentTray.TrayControl.FromComponent(receiver);
			if (trayControl == null)
			{
				return;
			}
			if (trayControl.Parent == this)
			{
				Point autoScrollPosition = base.AutoScrollPosition;
				location = new Point(location.X + autoScrollPosition.X, location.Y + autoScrollPosition.Y);
				if (trayControl.Visible)
				{
					this.RearrangeInAutoSlots(trayControl, location);
					return;
				}
			}
			else if (!trayControl.Location.Equals(location))
			{
				trayControl.Location = location;
				trayControl.Positioned = true;
			}
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 125)
			{
				switch (msg)
				{
				case 31:
					this.OnLostCapture();
					return;
				case 32:
					this.OnSetCursor();
					return;
				default:
					switch (msg)
					{
					case 123:
					{
						int num = NativeMethods.Util.SignedLOWORD((int)m.LParam);
						int num2 = NativeMethods.Util.SignedHIWORD((int)m.LParam);
						if (num == -1 && num2 == -1)
						{
							Point mousePosition = Control.MousePosition;
							num = mousePosition.X;
							num2 = mousePosition.Y;
						}
						this.OnContextMenu(num, num2, true);
						return;
					}
					case 125:
						base.Invalidate();
						return;
					}
					break;
				}
			}
			else
			{
				if (msg == 132)
				{
					if (this.glyphManager != null)
					{
						Point point = new Point((int)((short)NativeMethods.Util.LOWORD((int)m.LParam)), (int)((short)NativeMethods.Util.HIWORD((int)m.LParam)));
						NativeMethods.POINT point2 = new NativeMethods.POINT();
						point2.x = 0;
						point2.y = 0;
						NativeMethods.MapWindowPoints(IntPtr.Zero, base.Handle, point2, 1);
						point.Offset(point2.x, point2.y);
						this.glyphManager.GetHitTest(point);
					}
					base.WndProc(ref m);
					return;
				}
				switch (msg)
				{
				case 276:
				case 277:
					base.WndProc(ref m);
					if (this.selectionUISvc != null)
					{
						this.selectionUISvc.SyncSelection();
					}
					return;
				}
			}
			base.WndProc(ref m);
		}

		bool IOleDragClient.CanModifyComponents
		{
			get
			{
				return true;
			}
		}

		IComponent IOleDragClient.Component
		{
			get
			{
				return this.mainDesigner.Component;
			}
		}

		bool IOleDragClient.AddComponent(IComponent component, string name, bool firstAdd)
		{
			IOleDragClient oleDragClient = this.mainDesigner as IOleDragClient;
			if (oleDragClient != null)
			{
				try
				{
					oleDragClient.AddComponent(component, name, firstAdd);
					this.PositionControl(ComponentTray.TrayControl.FromComponent(component));
					this.mouseDropLocation = ComponentTray.InvalidPoint;
					return true;
				}
				catch
				{
					return false;
				}
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			try
			{
				if (designerHost != null && designerHost.Container != null)
				{
					if (designerHost.Container.Components[name] != null)
					{
						name = null;
					}
					designerHost.Container.Add(component, name);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		Control IOleDragClient.GetControlForComponent(object component)
		{
			IComponent component2 = component as IComponent;
			if (component2 != null)
			{
				return ComponentTray.TrayControl.FromComponent(component2);
			}
			return null;
		}

		Control IOleDragClient.GetDesignerControl()
		{
			return this;
		}

		bool IOleDragClient.IsDropOk(IComponent component)
		{
			return true;
		}

		bool ISelectionUIHandler.BeginDrag(object[] components, SelectionRules rules, int initialX, int initialY)
		{
			if (this.TabOrderActive)
			{
				return false;
			}
			bool flag = this.DragHandler.BeginDrag(components, rules, initialX, initialY);
			return (!flag || this.GetOleDragHandler().DoBeginDrag(components, rules, initialX, initialY)) && flag;
		}

		void ISelectionUIHandler.DragMoved(object[] components, Rectangle offset)
		{
			this.DragHandler.DragMoved(components, offset);
		}

		void ISelectionUIHandler.EndDrag(object[] components, bool cancel)
		{
			this.DragHandler.EndDrag(components, cancel);
			this.GetOleDragHandler().DoEndDrag(components, cancel);
			if (!this.autoScrollPosBeforeDragging.IsEmpty)
			{
				foreach (IComponent component in components)
				{
					ComponentTray.TrayControl trayControl = ComponentTray.TrayControl.FromComponent(component);
					if (trayControl != null)
					{
						this.SetTrayLocation(component, new Point(trayControl.Location.X - this.autoScrollPosBeforeDragging.X, trayControl.Location.Y - this.autoScrollPosBeforeDragging.Y));
					}
				}
				base.AutoScrollPosition = new Point(-this.autoScrollPosBeforeDragging.X, -this.autoScrollPosBeforeDragging.Y);
			}
		}

		Rectangle ISelectionUIHandler.GetComponentBounds(object component)
		{
			return Rectangle.Empty;
		}

		SelectionRules ISelectionUIHandler.GetComponentRules(object component)
		{
			return SelectionRules.Moveable | SelectionRules.Visible;
		}

		Rectangle ISelectionUIHandler.GetSelectionClipRect(object component)
		{
			if (base.IsHandleCreated)
			{
				return base.RectangleToScreen(base.ClientRectangle);
			}
			return Rectangle.Empty;
		}

		void ISelectionUIHandler.OleDragEnter(DragEventArgs de)
		{
			this.GetOleDragHandler().DoOleDragEnter(de);
		}

		void ISelectionUIHandler.OleDragDrop(DragEventArgs de)
		{
			this.GetOleDragHandler().DoOleDragDrop(de);
		}

		void ISelectionUIHandler.OleDragOver(DragEventArgs de)
		{
			this.GetOleDragHandler().DoOleDragOver(de);
		}

		void ISelectionUIHandler.OleDragLeave()
		{
			this.GetOleDragHandler().DoOleDragLeave();
		}

		void ISelectionUIHandler.OnSelectionDoubleClick(IComponent component)
		{
			if (!this.TabOrderActive)
			{
				ComponentTray.TrayControl trayControl = ((IOleDragClient)this).GetControlForComponent(component) as ComponentTray.TrayControl;
				if (trayControl != null)
				{
					trayControl.ViewDefaultEvent(component);
				}
			}
		}

		bool ISelectionUIHandler.QueryBeginDrag(object[] components, SelectionRules rules, int initialX, int initialY)
		{
			return this.DragHandler.QueryBeginDrag(components, rules, initialX, initialY);
		}

		internal void RearrangeInAutoSlots(Control c, Point pos)
		{
			ComponentTray.TrayControl trayControl = (ComponentTray.TrayControl)c;
			trayControl.Positioned = true;
			trayControl.Location = pos;
		}

		void ISelectionUIHandler.ShowContextMenu(IComponent component)
		{
			Point mousePosition = Control.MousePosition;
			this.OnContextMenu(mousePosition.X, mousePosition.Y, true);
		}

		private static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		private IServiceProvider serviceProvider;

		private Point whiteSpace = Point.Empty;

		private Size grabHandle = Size.Empty;

		private ArrayList controls;

		private SelectionUIHandler dragHandler;

		private ISelectionUIService selectionUISvc;

		private IToolboxService toolboxService;

		internal OleDragDropHandler oleDragDropHandler;

		private IDesigner mainDesigner;

		private IEventHandlerService eventHandlerService;

		private bool queriedTabOrder;

		private MenuCommand tabOrderCommand;

		private ICollection selectedObjects;

		private IMenuCommandService menuCommandService;

		private CommandSet privateCommandSet;

		private InheritanceUI inheritanceUI;

		private Point mouseDragStart = ComponentTray.InvalidPoint;

		private Point mouseDragEnd = ComponentTray.InvalidPoint;

		private Rectangle mouseDragWorkspace = Rectangle.Empty;

		private ToolboxItem mouseDragTool;

		private Point mouseDropLocation = ComponentTray.InvalidPoint;

		private bool showLargeIcons;

		private bool autoArrange;

		private Point autoScrollPosBeforeDragging = Point.Empty;

		private MenuCommand menucmdArrangeIcons;

		private MenuCommand menucmdLineupIcons;

		private MenuCommand menucmdLargeIcons;

		private bool fResetAmbient;

		private ComponentTray.ComponentTrayGlyphManager glyphManager;

		private delegate void AsyncInvokeHandler(bool children);

		private class ComponentTrayGlyphManager
		{
			public ComponentTrayGlyphManager(ISelectionService selSvc, BehaviorService behaviorSvc)
			{
				this.selSvc = selSvc;
				this.behaviorSvc = behaviorSvc;
				this.traySelectionAdorner = new Adorner();
			}

			public GlyphCollection SelectionGlyphs
			{
				get
				{
					return this.traySelectionAdorner.Glyphs;
				}
			}

			public void Dispose()
			{
				if (this.traySelectionAdorner != null)
				{
					this.traySelectionAdorner.Glyphs.Clear();
					this.traySelectionAdorner = null;
				}
			}

			public GlyphCollection GetGlyphsForComponent(IComponent comp)
			{
				GlyphCollection glyphCollection = new GlyphCollection();
				if (this.behaviorSvc != null && comp != null && this.behaviorSvc.DesignerActionUI != null)
				{
					Glyph designerActionGlyph = this.behaviorSvc.DesignerActionUI.GetDesignerActionGlyph(comp);
					if (designerActionGlyph != null)
					{
						glyphCollection.Add(designerActionGlyph);
					}
				}
				return glyphCollection;
			}

			public Cursor GetHitTest(Point p)
			{
				for (int i = 0; i < this.traySelectionAdorner.Glyphs.Count; i++)
				{
					Cursor hitTest = this.traySelectionAdorner.Glyphs[i].GetHitTest(p);
					if (hitTest != null)
					{
						this.hitTestedGlyph = this.traySelectionAdorner.Glyphs[i];
						return hitTest;
					}
				}
				this.hitTestedGlyph = null;
				return null;
			}

			public bool OnMouseDoubleClick(MouseEventArgs e)
			{
				return this.hitTestedGlyph != null && this.hitTestedGlyph.Behavior != null && this.hitTestedGlyph.Behavior.OnMouseDoubleClick(this.hitTestedGlyph, e.Button, new Point(e.X, e.Y));
			}

			public bool OnMouseDown(MouseEventArgs e)
			{
				return this.hitTestedGlyph != null && this.hitTestedGlyph.Behavior != null && this.hitTestedGlyph.Behavior.OnMouseDown(this.hitTestedGlyph, e.Button, new Point(e.X, e.Y));
			}

			public bool OnMouseMove(MouseEventArgs e)
			{
				return this.hitTestedGlyph != null && this.hitTestedGlyph.Behavior != null && this.hitTestedGlyph.Behavior.OnMouseMove(this.hitTestedGlyph, e.Button, new Point(e.X, e.Y));
			}

			public bool OnMouseUp(MouseEventArgs e)
			{
				return this.hitTestedGlyph != null && this.hitTestedGlyph.Behavior != null && this.hitTestedGlyph.Behavior.OnMouseUp(this.hitTestedGlyph, e.Button);
			}

			public void OnPaintGlyphs(PaintEventArgs pe)
			{
				foreach (object obj in this.traySelectionAdorner.Glyphs)
				{
					Glyph glyph = (Glyph)obj;
					glyph.Paint(pe);
				}
			}

			public void UpdateLocation(ComponentTray.TrayControl trayControl)
			{
				foreach (object obj in this.traySelectionAdorner.Glyphs)
				{
					Glyph glyph = (Glyph)obj;
					DesignerActionGlyph designerActionGlyph = glyph as DesignerActionGlyph;
					if (designerActionGlyph != null && ((DesignerActionBehavior)designerActionGlyph.Behavior).RelatedComponent.Equals(trayControl.Component))
					{
						designerActionGlyph.UpdateAlternativeBounds(trayControl.Bounds);
					}
				}
			}

			private Adorner traySelectionAdorner;

			private Glyph hitTestedGlyph;

			private ISelectionService selSvc;

			private BehaviorService behaviorSvc;
		}

		private class TrayOleDragDropHandler : OleDragDropHandler
		{
			public TrayOleDragDropHandler(SelectionUIHandler selectionHandler, IServiceProvider serviceProvider, IOleDragClient client)
				: base(selectionHandler, serviceProvider, client)
			{
			}

			protected override bool CanDropDataObject(IDataObject dataObj)
			{
				ICollection collection = null;
				if (dataObj != null)
				{
					OleDragDropHandler.ComponentDataObjectWrapper componentDataObjectWrapper = dataObj as OleDragDropHandler.ComponentDataObjectWrapper;
					if (componentDataObjectWrapper != null)
					{
						OleDragDropHandler.ComponentDataObject innerData = componentDataObjectWrapper.InnerData;
						collection = innerData.Components;
					}
					else
					{
						try
						{
							object data = dataObj.GetData(OleDragDropHandler.DataFormat, true);
							if (data == null)
							{
								return false;
							}
							IDesignerSerializationService designerSerializationService = (IDesignerSerializationService)base.GetService(typeof(IDesignerSerializationService));
							if (designerSerializationService == null)
							{
								return false;
							}
							collection = designerSerializationService.Deserialize(data);
						}
						catch (Exception ex)
						{
							if (ClientUtils.IsCriticalException(ex))
							{
								throw;
							}
						}
						catch
						{
						}
					}
				}
				if (collection != null && collection.Count > 0)
				{
					foreach (object obj in collection)
					{
						if (!(obj is Point) && (obj is Control || !(obj is IComponent)))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		internal class AutoArrangeComparer : IComparer
		{
			int IComparer.Compare(object o1, object o2)
			{
				Point location = ((Control)o1).Location;
				Point location2 = ((Control)o2).Location;
				int num = ((Control)o1).Width / 2;
				int num2 = ((Control)o1).Height / 2;
				if (location.X == location2.X && location.Y == location2.Y)
				{
					return 0;
				}
				if (location.Y + num2 <= location2.Y)
				{
					return -1;
				}
				if (location2.Y + num2 <= location.Y)
				{
					return 1;
				}
				if (location.X > location2.X)
				{
					return 1;
				}
				return -1;
			}
		}

		internal class TrayControl : Control
		{
			public TrayControl(ComponentTray tray, IComponent component)
			{
				this.tray = tray;
				this.component = component;
				base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
				base.SetStyle(ControlStyles.Selectable, false);
				this.borderWidth = SystemInformation.BorderSize.Width;
				this.UpdateIconInfo();
				IComponentChangeService componentChangeService = (IComponentChangeService)tray.GetService(typeof(IComponentChangeService));
				bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRename += this.OnComponentRename;
				}
				ISite site = component.Site;
				string text = null;
				if (site != null)
				{
					text = site.Name;
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					if (dictionaryService != null)
					{
						dictionaryService.SetValue(base.GetType(), this);
					}
				}
				if (text == null)
				{
					text = component.GetType().Name;
				}
				this.Text = text;
				this.inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(component)[typeof(InheritanceAttribute)];
				base.TabStop = false;
			}

			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			public override Font Font
			{
				get
				{
					return this.tray.Font;
				}
			}

			public InheritanceAttribute InheritanceAttribute
			{
				get
				{
					return this.inheritanceAttribute;
				}
			}

			public bool Positioned
			{
				get
				{
					return this.positioned;
				}
				set
				{
					this.positioned = value;
				}
			}

			private void AdjustSize(bool autoArrange)
			{
				using (Graphics graphics = base.CreateGraphics())
				{
					Size size = Size.Ceiling(graphics.MeasureString(this.Text, this.Font));
					Rectangle bounds = base.Bounds;
					if (this.tray.ShowLargeIcons)
					{
						bounds.Width = Math.Max(this.cxIcon, size.Width) + 4 * this.borderWidth + 10;
						bounds.Height = this.cyIcon + 10 + size.Height + 4 * this.borderWidth;
					}
					else
					{
						bounds.Width = this.cxIcon + size.Width + 4 * this.borderWidth + 10;
						bounds.Height = Math.Max(this.cyIcon, size.Height) + 4 * this.borderWidth;
					}
					base.Bounds = bounds;
					base.Invalidate();
				}
				if (this.tray.glyphManager != null)
				{
					this.tray.glyphManager.UpdateLocation(this);
				}
			}

			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new ComponentTray.TrayControl.TrayControlAccessibleObject(this, this.tray);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					ISite site = this.component.Site;
					if (site != null)
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
						bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
						if (componentChangeService != null)
						{
							componentChangeService.ComponentRename -= this.OnComponentRename;
						}
						IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
						bool enabled2 = CompModSwitches.CommonDesignerServices.Enabled;
						if (dictionaryService != null)
						{
							dictionaryService.SetValue(typeof(ComponentTray.TrayControl), null);
						}
					}
				}
				base.Dispose(disposing);
			}

			public static ComponentTray.TrayControl FromComponent(IComponent component)
			{
				ComponentTray.TrayControl trayControl = null;
				if (component == null)
				{
					return null;
				}
				ISite site = component.Site;
				if (site != null)
				{
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
					if (dictionaryService != null)
					{
						trayControl = (ComponentTray.TrayControl)dictionaryService.GetValue(typeof(ComponentTray.TrayControl));
					}
				}
				return trayControl;
			}

			private void OnComponentRename(object sender, ComponentRenameEventArgs e)
			{
				if (e.Component == this.component)
				{
					this.Text = e.NewName;
					this.AdjustSize(true);
				}
			}

			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
				this.AdjustSize(false);
			}

			protected override void OnDoubleClick(EventArgs e)
			{
				base.OnDoubleClick(e);
				if (!this.tray.TabOrderActive)
				{
					IDesignerHost designerHost = (IDesignerHost)this.tray.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						this.mouseDragLast = ComponentTray.InvalidPoint;
						base.Capture = false;
						IDesigner designer = designerHost.GetDesigner(this.component);
						if (designer == null)
						{
							this.ViewDefaultEvent(this.component);
							return;
						}
						designer.DoDefaultAction();
					}
				}
			}

			private void OnEndDrag(bool cancel)
			{
				this.mouseDragLast = ComponentTray.InvalidPoint;
				if (!this.mouseDragMoved)
				{
					if (this.ctrlSelect)
					{
						ISelectionService selectionService = (ISelectionService)this.tray.GetService(typeof(ISelectionService));
						if (selectionService != null)
						{
							selectionService.SetSelectedComponents(new object[] { this.Component }, SelectionTypes.Click);
						}
						this.ctrlSelect = false;
					}
					return;
				}
				this.mouseDragMoved = false;
				this.ctrlSelect = false;
				base.Capture = false;
				this.OnSetCursor();
				if (this.tray.selectionUISvc != null && this.tray.selectionUISvc.Dragging)
				{
					this.tray.selectionUISvc.EndDrag(cancel);
				}
			}

			protected override void OnMouseDown(MouseEventArgs me)
			{
				base.OnMouseDown(me);
				if (!this.tray.TabOrderActive)
				{
					this.tray.FocusDesigner();
					if (me.Button == MouseButtons.Left)
					{
						base.Capture = true;
						this.mouseDragLast = base.PointToScreen(new Point(me.X, me.Y));
						this.ctrlSelect = NativeMethods.GetKeyState(17) != 0;
						if (!this.ctrlSelect)
						{
							ISelectionService selectionService = (ISelectionService)this.tray.GetService(typeof(ISelectionService));
							bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
							if (selectionService != null)
							{
								selectionService.SetSelectedComponents(new object[] { this.Component }, SelectionTypes.Click);
							}
						}
					}
				}
			}

			protected override void OnMouseMove(MouseEventArgs me)
			{
				base.OnMouseMove(me);
				if (this.mouseDragLast == ComponentTray.InvalidPoint)
				{
					return;
				}
				if (!this.mouseDragMoved)
				{
					Size dragSize = SystemInformation.DragSize;
					Size doubleClickSize = SystemInformation.DoubleClickSize;
					dragSize.Width = Math.Max(dragSize.Width, doubleClickSize.Width);
					dragSize.Height = Math.Max(dragSize.Height, doubleClickSize.Height);
					Point point = base.PointToScreen(new Point(me.X, me.Y));
					if (this.mouseDragLast == ComponentTray.InvalidPoint || (Math.Abs(this.mouseDragLast.X - point.X) < dragSize.Width && Math.Abs(this.mouseDragLast.Y - point.Y) < dragSize.Height))
					{
						return;
					}
					this.mouseDragMoved = true;
					this.ctrlSelect = false;
				}
				try
				{
					ISelectionService selectionService = (ISelectionService)this.tray.GetService(typeof(ISelectionService));
					if (selectionService != null)
					{
						selectionService.SetSelectedComponents(new object[] { this.Component }, SelectionTypes.Click);
					}
					if (this.tray.selectionUISvc != null && this.tray.selectionUISvc.BeginDrag(SelectionRules.Moveable | SelectionRules.Visible, this.mouseDragLast.X, this.mouseDragLast.Y))
					{
						this.OnSetCursor();
					}
				}
				finally
				{
					this.mouseDragMoved = false;
					this.mouseDragLast = ComponentTray.InvalidPoint;
				}
			}

			protected override void OnMouseUp(MouseEventArgs me)
			{
				base.OnMouseUp(me);
				this.OnEndDrag(false);
			}

			private void OnContextMenu(int x, int y)
			{
				if (!this.tray.TabOrderActive)
				{
					base.Capture = false;
					ISelectionService selectionService = (ISelectionService)this.tray.GetService(typeof(ISelectionService));
					if (selectionService != null && !selectionService.GetComponentSelected(this.component))
					{
						selectionService.SetSelectedComponents(new object[] { this.component }, SelectionTypes.Replace);
					}
					IMenuCommandService menuService = this.tray.MenuService;
					if (menuService != null)
					{
						base.Capture = false;
						Cursor.Clip = Rectangle.Empty;
						menuService.ShowContextMenu(MenuCommands.TraySelectionMenu, x, y);
					}
				}
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				if (this.fRecompute)
				{
					this.fRecompute = false;
					this.UpdateIconInfo();
				}
				base.OnPaint(e);
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.X += 5 + this.borderWidth;
				clientRectangle.Y += this.borderWidth;
				clientRectangle.Width -= 2 * this.borderWidth + 5;
				clientRectangle.Height -= 2 * this.borderWidth;
				StringFormat stringFormat = new StringFormat();
				Brush brush = new SolidBrush(this.ForeColor);
				try
				{
					stringFormat.Alignment = StringAlignment.Center;
					if (this.tray.ShowLargeIcons)
					{
						if (this.toolboxBitmap != null)
						{
							int num = clientRectangle.X + (clientRectangle.Width - this.cxIcon) / 2;
							int num2 = clientRectangle.Y + 5;
							e.Graphics.DrawImage(this.toolboxBitmap, new Rectangle(num, num2, this.cxIcon, this.cyIcon));
						}
						clientRectangle.Y += this.cyIcon + 5;
						clientRectangle.Height -= this.cyIcon;
						e.Graphics.DrawString(this.Text, this.Font, brush, clientRectangle, stringFormat);
					}
					else
					{
						if (this.toolboxBitmap != null)
						{
							int num3 = clientRectangle.Y + (clientRectangle.Height - this.cyIcon) / 2;
							e.Graphics.DrawImage(this.toolboxBitmap, new Rectangle(clientRectangle.X, num3, this.cxIcon, this.cyIcon));
						}
						clientRectangle.X += this.cxIcon + this.borderWidth;
						clientRectangle.Width -= this.cxIcon;
						clientRectangle.Y += 3;
						e.Graphics.DrawString(this.Text, this.Font, brush, clientRectangle);
					}
				}
				finally
				{
					if (stringFormat != null)
					{
						stringFormat.Dispose();
					}
					if (brush != null)
					{
						brush.Dispose();
					}
				}
				if (!InheritanceAttribute.NotInherited.Equals(this.inheritanceAttribute))
				{
					InheritanceUI inheritanceUI = this.tray.InheritanceUI;
					if (inheritanceUI != null)
					{
						e.Graphics.DrawImage(inheritanceUI.InheritanceGlyph, 0, 0);
					}
				}
			}

			protected override void OnFontChanged(EventArgs e)
			{
				this.AdjustSize(true);
				base.OnFontChanged(e);
			}

			protected override void OnLocationChanged(EventArgs e)
			{
				if (this.tray.glyphManager != null)
				{
					this.tray.glyphManager.UpdateLocation(this);
				}
			}

			protected override void OnTextChanged(EventArgs e)
			{
				this.AdjustSize(true);
				base.OnTextChanged(e);
			}

			private void OnSetCursor()
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.component)["Locked"];
				if (propertyDescriptor != null && (bool)propertyDescriptor.GetValue(this.component))
				{
					Cursor.Current = Cursors.Default;
					return;
				}
				if (this.tray.TabOrderActive)
				{
					Cursor.Current = Cursors.Default;
					return;
				}
				if (this.mouseDragMoved)
				{
					Cursor.Current = Cursors.Default;
					return;
				}
				if (this.mouseDragLast != ComponentTray.InvalidPoint)
				{
					Cursor.Current = Cursors.Cross;
					return;
				}
				Cursor.Current = Cursors.SizeAll;
			}

			protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
			{
				if (!this.tray.AutoArrange || (specified & BoundsSpecified.Width) == BoundsSpecified.Width || (specified & BoundsSpecified.Height) == BoundsSpecified.Height)
				{
					base.SetBoundsCore(x, y, width, height, specified);
				}
				Rectangle bounds = base.Bounds;
				Size parentGridSize = this.tray.ParentGridSize;
				if (Math.Abs(bounds.X - x) > parentGridSize.Width || Math.Abs(bounds.Y - y) > parentGridSize.Height)
				{
					base.SetBoundsCore(x, y, width, height, specified);
				}
			}

			protected override void SetVisibleCore(bool value)
			{
				if (value && !this.tray.CanDisplayComponent(this.component))
				{
					return;
				}
				base.SetVisibleCore(value);
			}

			public override string ToString()
			{
				return "ComponentTray: " + this.component.ToString();
			}

			internal void UpdateIconInfo()
			{
				ToolboxBitmapAttribute toolboxBitmapAttribute = (ToolboxBitmapAttribute)TypeDescriptor.GetAttributes(this.component)[typeof(ToolboxBitmapAttribute)];
				if (toolboxBitmapAttribute != null)
				{
					this.toolboxBitmap = toolboxBitmapAttribute.GetImage(this.component, this.tray.ShowLargeIcons);
				}
				if (this.toolboxBitmap == null)
				{
					this.cxIcon = 0;
					this.cyIcon = SystemInformation.IconSize.Height;
				}
				else
				{
					Size size = this.toolboxBitmap.Size;
					this.cxIcon = size.Width;
					this.cyIcon = size.Height;
				}
				this.AdjustSize(true);
			}

			public virtual void ViewDefaultEvent(IComponent component)
			{
				EventDescriptor defaultEvent = TypeDescriptor.GetDefaultEvent(component);
				PropertyDescriptor propertyDescriptor = null;
				bool flag = false;
				IEventBindingService eventBindingService = (IEventBindingService)this.GetService(typeof(IEventBindingService));
				bool enabled = CompModSwitches.CommonDesignerServices.Enabled;
				if (eventBindingService != null)
				{
					propertyDescriptor = eventBindingService.GetEventProperty(defaultEvent);
				}
				if (propertyDescriptor == null || propertyDescriptor.IsReadOnly)
				{
					if (eventBindingService != null)
					{
						eventBindingService.ShowCode();
					}
					return;
				}
				string text = (string)propertyDescriptor.GetValue(component);
				if (text == null)
				{
					flag = true;
					text = eventBindingService.CreateUniqueMethodName(component, defaultEvent);
				}
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				DesignerTransaction designerTransaction = null;
				try
				{
					if (designerHost != null)
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("WindowsFormsAddEvent", new object[] { defaultEvent.Name }));
					}
					if (flag && propertyDescriptor != null)
					{
						propertyDescriptor.SetValue(component, text);
					}
					eventBindingService.ShowCode(component, defaultEvent);
				}
				finally
				{
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
					}
				}
			}

			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 32)
				{
					this.OnSetCursor();
					return;
				}
				if (msg == 123)
				{
					int num = NativeMethods.Util.SignedLOWORD((int)m.LParam);
					int num2 = NativeMethods.Util.SignedHIWORD((int)m.LParam);
					if (num == -1 && num2 == -1)
					{
						Point mousePosition = Control.MousePosition;
						num = mousePosition.X;
						num2 = mousePosition.Y;
					}
					this.OnContextMenu(num, num2);
					return;
				}
				if (msg != 132)
				{
					base.WndProc(ref m);
					return;
				}
				if (this.tray.glyphManager != null)
				{
					Point point = new Point((int)((short)NativeMethods.Util.LOWORD((int)m.LParam)), (int)((short)NativeMethods.Util.HIWORD((int)m.LParam)));
					NativeMethods.POINT point2 = new NativeMethods.POINT();
					point2.x = 0;
					point2.y = 0;
					NativeMethods.MapWindowPoints(IntPtr.Zero, base.Handle, point2, 1);
					point.Offset(point2.x, point2.y);
					point.Offset(base.Location.X, base.Location.Y);
					this.tray.glyphManager.GetHitTest(point);
				}
				base.WndProc(ref m);
			}

			private const int whiteSpace = 5;

			private IComponent component;

			private Image toolboxBitmap;

			private int cxIcon;

			private int cyIcon;

			private InheritanceAttribute inheritanceAttribute;

			private ComponentTray tray;

			private Point mouseDragLast = ComponentTray.InvalidPoint;

			private bool mouseDragMoved;

			private bool ctrlSelect;

			private bool positioned;

			private int borderWidth;

			internal bool fRecompute;

			private class TrayControlAccessibleObject : Control.ControlAccessibleObject
			{
				public TrayControlAccessibleObject(ComponentTray.TrayControl owner, ComponentTray tray)
					: base(owner)
				{
					this.tray = tray;
				}

				private IComponent Component
				{
					get
					{
						return ((ComponentTray.TrayControl)base.Owner).Component;
					}
				}

				public override AccessibleStates State
				{
					get
					{
						AccessibleStates accessibleStates = base.State;
						ISelectionService selectionService = (ISelectionService)this.tray.GetService(typeof(ISelectionService));
						if (selectionService != null)
						{
							if (selectionService.GetComponentSelected(this.Component))
							{
								accessibleStates |= AccessibleStates.Selected;
							}
							if (selectionService.PrimarySelection == this.Component)
							{
								accessibleStates |= AccessibleStates.Focused;
							}
						}
						return accessibleStates;
					}
				}

				private ComponentTray tray;
			}
		}

		private class TraySelectionUIHandler : SelectionUIHandler
		{
			public TraySelectionUIHandler(ComponentTray tray)
			{
				this.tray = tray;
				this.snapSize = default(Size);
			}

			public override bool BeginDrag(object[] components, SelectionRules rules, int initialX, int initialY)
			{
				bool flag = base.BeginDrag(components, rules, initialX, initialY);
				this.tray.SuspendLayout();
				return flag;
			}

			public override void EndDrag(object[] components, bool cancel)
			{
				base.EndDrag(components, cancel);
				this.tray.ResumeLayout();
			}

			protected override IComponent GetComponent()
			{
				return this.tray;
			}

			protected override Control GetControl()
			{
				return this.tray;
			}

			protected override Control GetControl(IComponent component)
			{
				return ComponentTray.TrayControl.FromComponent(component);
			}

			protected override Size GetCurrentSnapSize()
			{
				return this.snapSize;
			}

			protected override object GetService(Type serviceType)
			{
				return this.tray.GetService(serviceType);
			}

			protected override bool GetShouldSnapToGrid()
			{
				return false;
			}

			public override Rectangle GetUpdatedRect(Rectangle originalRect, Rectangle dragRect, bool updateSize)
			{
				return dragRect;
			}

			public override void SetCursor()
			{
				this.tray.OnSetCursor();
			}

			private ComponentTray tray;

			private Size snapSize = Size.Empty;
		}
	}
}
