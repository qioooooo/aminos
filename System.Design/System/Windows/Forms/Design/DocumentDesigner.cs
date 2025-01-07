using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms.Design.Behavior;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	[ToolboxItemFilter("System.Windows.Forms")]
	public class DocumentDesigner : ScrollableControlDesigner, IRootDesigner, IDesigner, IDisposable, IToolboxUser, IOleDragClient
	{
		private SizeF AutoScaleDimensions
		{
			get
			{
				ContainerControl containerControl = this.Control as ContainerControl;
				if (containerControl != null)
				{
					return containerControl.CurrentAutoScaleDimensions;
				}
				return SizeF.Empty;
			}
			set
			{
				ContainerControl containerControl = this.Control as ContainerControl;
				if (containerControl != null)
				{
					containerControl.AutoScaleDimensions = value;
				}
			}
		}

		private AutoScaleMode AutoScaleMode
		{
			get
			{
				ContainerControl containerControl = this.Control as ContainerControl;
				if (containerControl != null)
				{
					return containerControl.AutoScaleMode;
				}
				return AutoScaleMode.Inherit;
			}
			set
			{
				base.ShadowProperties["AutoScaleMode"] = value;
				ContainerControl containerControl = this.Control as ContainerControl;
				if (containerControl != null && containerControl.AutoScaleMode != value)
				{
					containerControl.AutoScaleMode = value;
					IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if (designerHost != null && !designerHost.Loading)
					{
						containerControl.AutoScaleDimensions = containerControl.CurrentAutoScaleDimensions;
					}
				}
			}
		}

		private Color BackColor
		{
			get
			{
				return this.Control.BackColor;
			}
			set
			{
				base.ShadowProperties["BackColor"] = value;
				if (value.IsEmpty)
				{
					value = SystemColors.Control;
				}
				this.Control.BackColor = value;
			}
		}

		[DefaultValue(typeof(Point), "0, 0")]
		private Point Location
		{
			get
			{
				return (Point)base.ShadowProperties["Location"];
			}
			set
			{
				base.ShadowProperties["Location"] = value;
			}
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				return selectionRules & ~(SelectionRules.Moveable | SelectionRules.TopSizeable | SelectionRules.LeftSizeable);
			}
		}

		private bool TabOrderActive
		{
			get
			{
				if (!this.queriedTabOrder)
				{
					this.queriedTabOrder = true;
					IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
					if (menuCommandService != null)
					{
						this.tabOrderCommand = menuCommandService.FindCommand(StandardCommands.TabOrder);
					}
				}
				return this.tabOrderCommand != null && this.tabOrderCommand.Checked;
			}
		}

		[DefaultValue(true)]
		private bool TrayAutoArrange
		{
			get
			{
				return this.trayAutoArrange;
			}
			set
			{
				this.trayAutoArrange = value;
				if (this.componentTray != null)
				{
					this.componentTray.AutoArrange = this.trayAutoArrange;
				}
			}
		}

		[DefaultValue(false)]
		private bool TrayLargeIcon
		{
			get
			{
				return this.trayLargeIcon;
			}
			set
			{
				this.trayLargeIcon = value;
				if (this.componentTray != null)
				{
					this.componentTray.ShowLargeIcons = this.trayLargeIcon;
				}
			}
		}

		[DefaultValue(80)]
		private int TrayHeight
		{
			get
			{
				if (this.componentTray != null)
				{
					return this.componentTray.Height;
				}
				return this.trayHeight;
			}
			set
			{
				this.trayHeight = value;
				if (this.componentTray != null)
				{
					this.componentTray.Height = this.trayHeight;
				}
			}
		}

		Control IOleDragClient.GetControlForComponent(object component)
		{
			Control control = base.GetControl(component);
			if (control != null)
			{
				return control;
			}
			if (this.componentTray != null)
			{
				return ((IOleDragClient)this.componentTray).GetControlForComponent(component);
			}
			return null;
		}

		internal virtual bool CanDropComponents(DragEventArgs de)
		{
			if (this.componentTray == null)
			{
				return true;
			}
			OleDragDropHandler oleDragHandler = base.GetOleDragHandler();
			object[] draggingObjects = oleDragHandler.GetDraggingObjects(de);
			if (draggingObjects != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				for (int i = 0; i < draggingObjects.Length; i++)
				{
					IComponent component = draggingObjects[i] as IComponent;
					if (designerHost != null && draggingObjects[i] != null && component != null && this.componentTray.IsTrayComponent(component))
					{
						return false;
					}
				}
			}
			return !(de.Data is ToolStripItemDataObject);
		}

		private ToolboxItem CreateAxToolboxItem(IDataObject dataObject)
		{
			MemoryStream memoryStream = (MemoryStream)dataObject.GetData(DocumentDesigner.axClipFormat, true);
			int num = (int)memoryStream.Length;
			byte[] array = new byte[num];
			memoryStream.Read(array, 0, num);
			string text = Encoding.Default.GetString(array);
			int num2 = text.IndexOf("}");
			text = text.Substring(0, num2 + 1);
			if (this.IsSupportedActiveXControl(text))
			{
				DocumentDesigner.AxToolboxItem axToolboxItem;
				if (this.axTools != null)
				{
					axToolboxItem = (DocumentDesigner.AxToolboxItem)this.axTools[text];
					if (axToolboxItem != null)
					{
						bool traceVerbose = DocumentDesigner.AxToolSwitch.TraceVerbose;
						return axToolboxItem;
					}
				}
				axToolboxItem = new DocumentDesigner.AxToolboxItem(text);
				if (this.axTools == null)
				{
					this.axTools = new Hashtable();
				}
				this.axTools.Add(text, axToolboxItem);
				return axToolboxItem;
			}
			return null;
		}

		private ToolboxItem CreateCfCodeToolboxItem(IDataObject dataObject)
		{
			object obj = dataObject.GetData(OleDragDropHandler.NestedToolboxItemFormat, false);
			if (obj != null)
			{
				return (ToolboxItem)obj;
			}
			obj = dataObject.GetData(OleDragDropHandler.DataFormat, false);
			if (obj != null)
			{
				return new OleDragDropHandler.CfCodeToolboxItem(obj);
			}
			return null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ToolStripAdornerWindowService toolStripAdornerWindowService = (ToolStripAdornerWindowService)this.GetService(typeof(ToolStripAdornerWindowService));
					if (toolStripAdornerWindowService != null)
					{
						toolStripAdornerWindowService.Dispose();
						designerHost.RemoveService(typeof(ToolStripAdornerWindowService));
					}
					designerHost.Activated -= this.OnDesignerActivate;
					designerHost.Deactivated -= this.OnDesignerDeactivate;
					if (this.componentTray != null)
					{
						ISplitWindowService splitWindowService = (ISplitWindowService)this.GetService(typeof(ISplitWindowService));
						if (splitWindowService != null)
						{
							splitWindowService.RemoveSplitWindow(this.componentTray);
							this.componentTray.Dispose();
							this.componentTray = null;
						}
						designerHost.RemoveService(typeof(ComponentTray));
					}
					IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentAdded -= this.OnComponentAdded;
						componentChangeService.ComponentChanged -= this.OnComponentChanged;
						componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
					}
					if (this.undoEngine != null)
					{
						this.undoEngine.Undoing -= this.OnUndoing;
						this.undoEngine.Undone -= this.OnUndone;
					}
					if (this.toolboxCreator != null)
					{
						IToolboxService toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
						if (toolboxService != null)
						{
							toolboxService.RemoveCreator(DocumentDesigner.axClipFormat, designerHost);
							toolboxService.RemoveCreator(OleDragDropHandler.DataFormat, designerHost);
							toolboxService.RemoveCreator(OleDragDropHandler.NestedToolboxItemFormat, designerHost);
						}
						this.toolboxCreator = null;
					}
				}
				ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SelectionChanged -= this.OnSelectionChanged;
				}
				if (this.behaviorService != null)
				{
					this.behaviorService.Dispose();
					this.behaviorService = null;
				}
				if (this.selectionManager != null)
				{
					this.selectionManager.Dispose();
					this.selectionManager = null;
				}
				if (this.componentTray != null)
				{
					if (designerHost != null)
					{
						ISplitWindowService splitWindowService2 = (ISplitWindowService)this.GetService(typeof(ISplitWindowService));
						if (splitWindowService2 != null)
						{
							splitWindowService2.RemoveSplitWindow(this.componentTray);
						}
					}
					this.componentTray.Dispose();
					this.componentTray = null;
				}
				if (this.pbrsFwd != null)
				{
					this.pbrsFwd.Dispose();
					this.pbrsFwd = null;
				}
				if (this.frame != null)
				{
					this.frame.Dispose();
					this.frame = null;
				}
				if (this.commandSet != null)
				{
					this.commandSet.Dispose();
					this.commandSet = null;
				}
				if (this.inheritanceService != null)
				{
					this.inheritanceService.Dispose();
					this.inheritanceService = null;
				}
				if (this.inheritanceUI != null)
				{
					this.inheritanceUI.Dispose();
					this.inheritanceUI = null;
				}
				if (this.designBindingValueUIHandler != null)
				{
					IPropertyValueUIService propertyValueUIService = (IPropertyValueUIService)this.GetService(typeof(IPropertyValueUIService));
					if (propertyValueUIService != null)
					{
						propertyValueUIService.RemovePropertyValueUIHandler(new PropertyValueUIHandler(this.designBindingValueUIHandler.OnGetUIValueItem));
					}
					this.designBindingValueUIHandler.Dispose();
					this.designBindingValueUIHandler = null;
				}
				if (this.designerExtenders != null)
				{
					this.designerExtenders.Dispose();
					this.designerExtenders = null;
				}
				if (this.axTools != null)
				{
					this.axTools.Clear();
				}
				if (designerHost != null)
				{
					designerHost.RemoveService(typeof(BehaviorService));
					designerHost.RemoveService(typeof(ToolStripAdornerWindowService));
					designerHost.RemoveService(typeof(SelectionManager));
					designerHost.RemoveService(typeof(IInheritanceService));
					designerHost.RemoveService(typeof(IEventHandlerService));
					designerHost.RemoveService(typeof(IOverlayService));
					designerHost.RemoveService(typeof(ISplitWindowService));
					designerHost.RemoveService(typeof(InheritanceUI));
				}
			}
			base.Dispose(disposing);
		}

		public override GlyphCollection GetGlyphs(GlyphSelectionType selectionType)
		{
			GlyphCollection glyphCollection = new GlyphCollection();
			if (selectionType != GlyphSelectionType.NotSelected)
			{
				Point point = base.BehaviorService.ControlToAdornerWindow((Control)base.Component);
				Rectangle rectangle = new Rectangle(point, ((Control)base.Component).Size);
				bool flag = selectionType == GlyphSelectionType.SelectedPrimary;
				bool flag2 = false;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Locked"];
				if (propertyDescriptor != null)
				{
					flag2 = (bool)propertyDescriptor.GetValue(base.Component);
				}
				bool flag3 = false;
				propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["AutoSize"];
				if (propertyDescriptor != null)
				{
					flag3 = (bool)propertyDescriptor.GetValue(base.Component);
				}
				AutoSizeMode autoSizeMode = AutoSizeMode.GrowOnly;
				propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["AutoSizeMode"];
				if (propertyDescriptor != null)
				{
					autoSizeMode = (AutoSizeMode)propertyDescriptor.GetValue(base.Component);
				}
				SelectionRules selectionRules = this.SelectionRules;
				if (flag2)
				{
					glyphCollection.Add(new LockedHandleGlyph(rectangle, flag));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Top));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Bottom));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Left));
					glyphCollection.Add(new LockedBorderGlyph(rectangle, SelectionBorderGlyphType.Right));
				}
				else if (flag3 && autoSizeMode == AutoSizeMode.GrowAndShrink && !(this.Control is Form))
				{
					glyphCollection.Add(new NoResizeHandleGlyph(rectangle, selectionRules, flag, null));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Top, null));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Bottom, null));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Left, null));
					glyphCollection.Add(new NoResizeSelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Right, null));
				}
				else
				{
					glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.MiddleRight, this.StandardBehavior, flag));
					glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.LowerRight, this.StandardBehavior, flag));
					glyphCollection.Add(new GrabHandleGlyph(rectangle, GrabHandleGlyphType.MiddleBottom, this.StandardBehavior, flag));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Top, null));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Bottom, this.StandardBehavior));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Left, null));
					glyphCollection.Add(new SelectionBorderGlyph(rectangle, selectionRules, SelectionBorderGlyphType.Right, this.StandardBehavior));
				}
			}
			return glyphCollection;
		}

		private ParentControlDesigner GetSelectedParentControlDesigner()
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			ParentControlDesigner parentControlDesigner = null;
			if (selectionService != null)
			{
				object obj = selectionService.PrimarySelection;
				if (obj == null || !(obj is Control))
				{
					obj = null;
					ICollection selectedComponents = selectionService.GetSelectedComponents();
					foreach (object obj2 in selectedComponents)
					{
						if (obj2 is Control)
						{
							obj = obj2;
							break;
						}
					}
				}
				if (obj != null)
				{
					Control control = (Control)obj;
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						while (control != null)
						{
							ParentControlDesigner parentControlDesigner2 = designerHost.GetDesigner(control) as ParentControlDesigner;
							if (parentControlDesigner2 != null)
							{
								parentControlDesigner = parentControlDesigner2;
								break;
							}
							control = control.Parent;
						}
					}
				}
			}
			if (parentControlDesigner == null)
			{
				parentControlDesigner = this;
			}
			return parentControlDesigner;
		}

		protected virtual bool GetToolSupported(ToolboxItem tool)
		{
			return true;
		}

		public override void Initialize(IComponent component)
		{
			this.initializing = true;
			base.Initialize(component);
			this.initializing = false;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component.GetType())["BackColor"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(Color) && !propertyDescriptor.ShouldSerializeValue(base.Component))
			{
				this.Control.BackColor = SystemColors.Control;
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			IExtenderProviderService extenderProviderService = (IExtenderProviderService)this.GetService(typeof(IExtenderProviderService));
			if (extenderProviderService != null)
			{
				this.designerExtenders = new DesignerExtenders(extenderProviderService);
			}
			if (designerHost != null)
			{
				designerHost.Activated += this.OnDesignerActivate;
				designerHost.Deactivated += this.OnDesignerDeactivate;
				ServiceCreatorCallback serviceCreatorCallback = new ServiceCreatorCallback(this.OnCreateService);
				designerHost.AddService(typeof(IEventHandlerService), serviceCreatorCallback);
				this.frame = new DesignerFrame(component.Site);
				IOverlayService overlayService = this.frame;
				designerHost.AddService(typeof(IOverlayService), overlayService);
				designerHost.AddService(typeof(ISplitWindowService), this.frame);
				this.behaviorService = new BehaviorService(base.Component.Site, this.frame);
				designerHost.AddService(typeof(BehaviorService), this.behaviorService);
				this.selectionManager = new SelectionManager(designerHost, this.behaviorService);
				designerHost.AddService(typeof(SelectionManager), this.selectionManager);
				designerHost.AddService(typeof(ToolStripAdornerWindowService), serviceCreatorCallback);
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded += this.OnComponentAdded;
					componentChangeService.ComponentChanged += this.OnComponentChanged;
					componentChangeService.ComponentRemoved += this.OnComponentRemoved;
				}
				this.inheritanceUI = new InheritanceUI();
				designerHost.AddService(typeof(InheritanceUI), this.inheritanceUI);
				InheritanceService inheritanceService = new DocumentDesigner.DocumentInheritanceService(this);
				designerHost.AddService(typeof(IInheritanceService), inheritanceService);
				inheritanceService.AddInheritedComponents(component, component.Site.Container);
				this.inheritanceService = inheritanceService;
				if (this.Control.IsHandleCreated)
				{
					this.OnCreateHandle();
				}
				IPropertyValueUIService propertyValueUIService = (IPropertyValueUIService)component.Site.GetService(typeof(IPropertyValueUIService));
				if (propertyValueUIService != null)
				{
					this.designBindingValueUIHandler = new DesignBindingValueUIHandler();
					propertyValueUIService.AddPropertyValueUIHandler(new PropertyValueUIHandler(this.designBindingValueUIHandler.OnGetUIValueItem));
				}
				IToolboxService toolboxService = (IToolboxService)designerHost.GetService(typeof(IToolboxService));
				if (toolboxService != null)
				{
					this.toolboxCreator = new ToolboxItemCreatorCallback(this.OnCreateToolboxItem);
					toolboxService.AddCreator(this.toolboxCreator, DocumentDesigner.axClipFormat, designerHost);
					toolboxService.AddCreator(this.toolboxCreator, OleDragDropHandler.DataFormat, designerHost);
					toolboxService.AddCreator(this.toolboxCreator, OleDragDropHandler.NestedToolboxItemFormat, designerHost);
				}
				designerHost.LoadComplete += this.OnLoadComplete;
			}
			this.commandSet = new ControlCommandSet(component.Site);
			this.frame.Initialize(this.Control);
			this.pbrsFwd = new PbrsForward(this.frame, component.Site);
			this.Location = new Point(0, 0);
		}

		private bool IsSupportedActiveXControl(string clsid)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			bool flag;
			try
			{
				string text = "CLSID\\" + clsid + "\\Control";
				registryKey = Registry.ClassesRoot.OpenSubKey(text);
				if (registryKey != null)
				{
					string text2 = string.Concat(new string[]
					{
						"CLSID\\",
						clsid,
						"\\Implemented Categories\\{",
						DocumentDesigner.htmlDesignTime.ToString(),
						"}"
					});
					registryKey2 = Registry.ClassesRoot.OpenSubKey(text2);
					flag = registryKey2 == null;
				}
				else
				{
					flag = false;
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
			}
			return flag;
		}

		private void OnUndone(object source, EventArgs e)
		{
			if (this.suspendedComponents != null)
			{
				foreach (object obj in this.suspendedComponents)
				{
					Control control = (Control)obj;
					control.ResumeLayout(false);
					control.PerformLayout();
				}
			}
		}

		private void OnUndoing(object source, EventArgs e)
		{
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null)
			{
				IContainer container = designerHost.Container;
				if (container != null)
				{
					this.suspendedComponents = new ArrayList(container.Components.Count + 1);
					foreach (object obj in container.Components)
					{
						IComponent component = (IComponent)obj;
						Control control = component as Control;
						if (control != null)
						{
							control.SuspendLayout();
							this.suspendedComponents.Add(control);
						}
					}
					Control control2 = designerHost.RootComponent as Control;
					if (control2 != null)
					{
						Control parent = control2.Parent;
						if (parent != null)
						{
							parent.SuspendLayout();
							this.suspendedComponents.Add(parent);
						}
					}
				}
			}
		}

		private void OnComponentAdded(object source, ComponentEventArgs ce)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				IComponent component = ce.Component;
				this.EnsureMenuEditorService(ce.Component);
				bool flag = true;
				if (!(designerHost.GetDesigner(component) is ToolStripDesigner))
				{
					ControlDesigner controlDesigner = designerHost.GetDesigner(component) as ControlDesigner;
					if (controlDesigner != null)
					{
						Form form = controlDesigner.Control as Form;
						if (form == null || !form.TopLevel)
						{
							flag = false;
						}
					}
				}
				if (flag && TypeDescriptor.GetAttributes(component).Contains(DesignTimeVisibleAttribute.Yes))
				{
					if (this.componentTray == null)
					{
						ISplitWindowService splitWindowService = (ISplitWindowService)this.GetService(typeof(ISplitWindowService));
						if (splitWindowService != null)
						{
							this.componentTray = new ComponentTray(this, base.Component.Site);
							splitWindowService.AddSplitWindow(this.componentTray);
							this.componentTray.Height = this.trayHeight;
							this.componentTray.ShowLargeIcons = this.trayLargeIcon;
							this.componentTray.AutoArrange = this.trayAutoArrange;
							designerHost.AddService(typeof(ComponentTray), this.componentTray);
						}
					}
					if (this.componentTray != null)
					{
						if (designerHost != null && designerHost.Loading && !this.trayLayoutSuspended)
						{
							this.trayLayoutSuspended = true;
							this.componentTray.SuspendLayout();
						}
						this.componentTray.AddComponent(component);
					}
				}
			}
		}

		private void OnComponentRemoved(object source, ComponentEventArgs ce)
		{
			if ((!(ce.Component is Control) || ce.Component is ToolStrip || (ce.Component is Form && ((Form)ce.Component).TopLevel)) && this.componentTray != null)
			{
				this.componentTray.RemoveComponent(ce.Component);
				if (this.componentTray.ComponentCount == 0)
				{
					ISplitWindowService splitWindowService = (ISplitWindowService)this.GetService(typeof(ISplitWindowService));
					if (splitWindowService != null)
					{
						splitWindowService.RemoveSplitWindow(this.componentTray);
						IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
						if (designerHost != null)
						{
							designerHost.RemoveService(typeof(ComponentTray));
						}
						this.componentTray.Dispose();
						this.componentTray = null;
					}
				}
			}
		}

		protected override void OnContextMenu(int x, int y)
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					if (selectionService.SelectionCount == 1 && selectionService.GetComponentSelected(base.Component))
					{
						menuCommandService.ShowContextMenu(MenuCommands.ContainerMenu, x, y);
						return;
					}
					Component component = selectionService.PrimarySelection as Component;
					if (component != null)
					{
						IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
						if (designerHost != null)
						{
							ComponentDesigner componentDesigner = designerHost.GetDesigner(component) as ComponentDesigner;
							if (componentDesigner != null)
							{
								componentDesigner.ShowContextMenu(x, y);
								return;
							}
						}
					}
					menuCommandService.ShowContextMenu(MenuCommands.SelectionMenu, x, y);
				}
			}
		}

		protected override void OnCreateHandle()
		{
			if (this.inheritanceService != null)
			{
				base.OnCreateHandle();
			}
		}

		private object OnCreateService(IServiceContainer container, Type serviceType)
		{
			if (serviceType == typeof(IEventHandlerService))
			{
				if (this.eventHandlerService == null)
				{
					this.eventHandlerService = new EventHandlerService(this.frame);
				}
				return this.eventHandlerService;
			}
			if (serviceType == typeof(ToolStripAdornerWindowService))
			{
				return new ToolStripAdornerWindowService(base.Component.Site, this.frame);
			}
			return null;
		}

		private ToolboxItem OnCreateToolboxItem(object serializedData, string format)
		{
			IDataObject dataObject = serializedData as IDataObject;
			if (dataObject == null)
			{
				return null;
			}
			if (format.Equals(DocumentDesigner.axClipFormat))
			{
				return this.CreateAxToolboxItem(dataObject);
			}
			if (format.Equals(OleDragDropHandler.DataFormat) || format.Equals(OleDragDropHandler.NestedToolboxItemFormat))
			{
				return this.CreateCfCodeToolboxItem(dataObject);
			}
			return null;
		}

		private void OnDesignerActivate(object source, EventArgs evevent)
		{
			if (this.undoEngine == null)
			{
				this.undoEngine = this.GetService(typeof(UndoEngine)) as UndoEngine;
				if (this.undoEngine != null)
				{
					this.undoEngine.Undoing += this.OnUndoing;
					this.undoEngine.Undone += this.OnUndone;
				}
			}
		}

		private void OnDesignerDeactivate(object sender, EventArgs e)
		{
			Control control = this.Control;
			if (control != null && control.IsHandleCreated)
			{
				NativeMethods.SendMessage(control.Handle, 134, 0, 0);
				SafeNativeMethods.RedrawWindow(control.Handle, null, IntPtr.Zero, 1024);
			}
		}

		private void OnLoadComplete(object sender, EventArgs e)
		{
			((IDesignerHost)sender).LoadComplete -= this.OnLoadComplete;
			if (this.trayLayoutSuspended && this.componentTray != null)
			{
				this.componentTray.ResumeLayout();
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += this.OnSelectionChanged;
				selectionService.SetSelectedComponents(new object[] { base.Component }, SelectionTypes.Replace);
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && control.IsHandleCreated)
			{
				UnsafeNativeMethods.NotifyWinEvent(32779, new HandleRef(control, control.Handle), -4, 0);
				if (this.frame.Focused)
				{
					UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(control, control.Handle), -4, 0);
				}
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				foreach (object obj in selectedComponents)
				{
					Control control = obj as Control;
					if (control != null)
					{
						UnsafeNativeMethods.NotifyWinEvent(32775, new HandleRef(control, control.Handle), -4, 0);
					}
				}
				Control control2 = selectionService.PrimarySelection as Control;
				if (control2 != null)
				{
					UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(control2, control2.Handle), -4, 0);
				}
				IHelpService helpService = (IHelpService)this.GetService(typeof(IHelpService));
				if (helpService != null)
				{
					ushort num = 0;
					string[] array = new string[] { "VisualSelection", "NonVisualSelection", "MixedSelection" };
					foreach (object obj2 in selectedComponents)
					{
						if (obj2 is Control)
						{
							if (obj2 != base.Component)
							{
								num |= 1;
							}
						}
						else
						{
							num |= 2;
						}
						if (num == 3)
						{
							break;
						}
					}
					for (int i = 0; i < array.Length; i++)
					{
						helpService.RemoveContextAttribute("Keyword", array[i]);
					}
					if (num != 0)
					{
						helpService.AddContextAttribute("Keyword", array[(int)(num - 1)], HelpKeywordType.GeneralKeyword);
					}
				}
				if (this.menuEditorService != null)
				{
					this.DoProperMenuSelection(selectedComponents);
				}
			}
		}

		internal virtual void DoProperMenuSelection(ICollection selComponents)
		{
			foreach (object obj in selComponents)
			{
				ContextMenu contextMenu = obj as ContextMenu;
				if (contextMenu != null)
				{
					this.menuEditorService.SetMenu((Menu)obj);
				}
				else
				{
					MenuItem menuItem = obj as MenuItem;
					if (menuItem != null)
					{
						MenuItem menuItem2 = menuItem;
						while (menuItem2.Parent is MenuItem)
						{
							menuItem2 = (MenuItem)menuItem2.Parent;
						}
						if (this.menuEditorService.GetMenu() != menuItem2.Parent)
						{
							this.menuEditorService.SetMenu(menuItem2.Parent);
						}
						if (selComponents.Count == 1)
						{
							this.menuEditorService.SetSelection(menuItem);
						}
					}
					else
					{
						this.menuEditorService.SetMenu(null);
					}
				}
			}
		}

		protected virtual void EnsureMenuEditorService(IComponent c)
		{
			if (this.menuEditorService == null && c is ContextMenu)
			{
				this.menuEditorService = (IMenuEditorService)this.GetService(typeof(IMenuEditorService));
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			properties["TrayHeight"] = TypeDescriptor.CreateProperty(typeof(DocumentDesigner), "TrayHeight", typeof(int), new Attribute[]
			{
				BrowsableAttribute.No,
				DesignOnlyAttribute.Yes,
				new SRDescriptionAttribute("FormDocumentDesignerTraySizeDescr"),
				CategoryAttribute.Design
			});
			properties["TrayLargeIcon"] = TypeDescriptor.CreateProperty(typeof(DocumentDesigner), "TrayLargeIcon", typeof(bool), new Attribute[]
			{
				BrowsableAttribute.No,
				DesignOnlyAttribute.Yes,
				CategoryAttribute.Design
			});
			properties["DoubleBuffered"] = TypeDescriptor.CreateProperty(typeof(Control), "DoubleBuffered", typeof(bool), new Attribute[]
			{
				BrowsableAttribute.Yes,
				DesignOnlyAttribute.No
			});
			string[] array = new string[] { "Location", "BackColor" };
			string[] array2 = new string[] { "Anchor", "Dock", "TabIndex", "TabStop", "Visible" };
			Attribute[] array3 = new Attribute[0];
			PropertyDescriptor propertyDescriptor;
			for (int i = 0; i < array.Length; i++)
			{
				propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(DocumentDesigner), propertyDescriptor, array3);
				}
			}
			propertyDescriptor = (PropertyDescriptor)properties["AutoScaleDimensions"];
			if (propertyDescriptor != null)
			{
				properties["AutoScaleDimensions"] = TypeDescriptor.CreateProperty(typeof(DocumentDesigner), propertyDescriptor, new Attribute[] { DesignerSerializationVisibilityAttribute.Visible });
			}
			propertyDescriptor = (PropertyDescriptor)properties["AutoScaleMode"];
			if (propertyDescriptor != null)
			{
				properties["AutoScaleMode"] = TypeDescriptor.CreateProperty(typeof(DocumentDesigner), propertyDescriptor, new Attribute[]
				{
					DesignerSerializationVisibilityAttribute.Visible,
					BrowsableAttribute.Yes
				});
			}
			for (int j = 0; j < array2.Length; j++)
			{
				propertyDescriptor = (PropertyDescriptor)properties[array2[j]];
				if (propertyDescriptor != null)
				{
					properties[array2[j]] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[]
					{
						BrowsableAttribute.No,
						DesignerSerializationVisibilityAttribute.Hidden
					});
				}
			}
		}

		private void ResetBackColor()
		{
			this.BackColor = Color.Empty;
		}

		private bool ShouldSerializeAutoScaleDimensions()
		{
			return !this.initializing && this.AutoScaleMode != AutoScaleMode.None && this.AutoScaleMode != AutoScaleMode.Inherit;
		}

		private bool ShouldSerializeAutoScaleMode()
		{
			return !this.initializing && base.ShadowProperties.Contains("AutoScaleMode");
		}

		private bool ShouldSerializeBackColor()
		{
			return base.ShadowProperties.Contains("BackColor") && !((Color)base.ShadowProperties["BackColor"]).IsEmpty;
		}

		protected virtual void ToolPicked(ToolboxItem tool)
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				MenuCommand menuCommand = menuCommandService.FindCommand(StandardCommands.TabOrder);
				if (menuCommand != null && menuCommand.Checked)
				{
					return;
				}
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				designerHost.Activate();
			}
			try
			{
				ParentControlDesigner selectedParentControlDesigner = this.GetSelectedParentControlDesigner();
				if (!base.InvokeGetInheritanceAttribute(selectedParentControlDesigner).Equals(InheritanceAttribute.InheritedReadOnly))
				{
					ParentControlDesigner.InvokeCreateTool(selectedParentControlDesigner, tool);
					IToolboxService toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
					if (toolboxService != null)
					{
						toolboxService.SelectedToolboxItemUsed();
					}
				}
			}
			catch (Exception ex)
			{
				base.DisplayError(ex);
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			catch
			{
			}
		}

		ViewTechnology[] IRootDesigner.SupportedTechnologies
		{
			get
			{
				return new ViewTechnology[]
				{
					ViewTechnology.Default,
					ViewTechnology.WindowsForms
				};
			}
		}

		object IRootDesigner.GetView(ViewTechnology technology)
		{
			if (technology != ViewTechnology.Default && technology != ViewTechnology.WindowsForms)
			{
				throw new ArgumentException();
			}
			return this.frame;
		}

		bool IToolboxUser.GetToolSupported(ToolboxItem tool)
		{
			return this.GetToolSupported(tool);
		}

		void IToolboxUser.ToolPicked(ToolboxItem tool)
		{
			this.ToolPicked(tool);
		}

		private unsafe void WmWindowPosChanged(ref Message m)
		{
			NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)(void*)m.LParam;
			if ((ptr->flags & 1) == 0 && this.menuEditorService != null)
			{
				base.BehaviorService.SyncSelection();
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (this.menuEditorService != null && (!this.TabOrderActive || (m.Msg != 161 && m.Msg != 164)) && this.menuEditorService.MessageFilter(ref m))
			{
				return;
			}
			base.WndProc(ref m);
			if (m.Msg == 71)
			{
				this.WmWindowPosChanged(ref m);
			}
		}

		private DesignerFrame frame;

		private ControlCommandSet commandSet;

		private InheritanceService inheritanceService;

		private EventHandlerService eventHandlerService;

		private DesignBindingValueUIHandler designBindingValueUIHandler;

		private BehaviorService behaviorService;

		private SelectionManager selectionManager;

		private DesignerExtenders designerExtenders;

		private InheritanceUI inheritanceUI;

		private PbrsForward pbrsFwd;

		private ArrayList suspendedComponents;

		private UndoEngine undoEngine;

		private bool initializing;

		private bool queriedTabOrder;

		private MenuCommand tabOrderCommand;

		protected IMenuEditorService menuEditorService;

		private ComponentTray componentTray;

		private int trayHeight = 80;

		private bool trayLargeIcon;

		private bool trayAutoArrange;

		private bool trayLayoutSuspended;

		private static Guid htmlDesignTime = new Guid("73CEF3DD-AE85-11CF-A406-00AA00C00940");

		private Hashtable axTools;

		private static TraceSwitch AxToolSwitch = new TraceSwitch("AxTool", "ActiveX Toolbox Tracing");

		private static readonly string axClipFormat = "CLSID";

		private ToolboxItemCreatorCallback toolboxCreator;

		[Serializable]
		private class AxToolboxItem : ToolboxItem
		{
			public AxToolboxItem(string clsid)
				: base(typeof(AxHost))
			{
				this.clsid = clsid;
				base.Company = null;
				this.LoadVersionInfo();
			}

			private AxToolboxItem(SerializationInfo info, StreamingContext context)
			{
				this.Deserialize(info, context);
			}

			public override string ComponentType
			{
				get
				{
					return SR.GetString("Ax_Control");
				}
			}

			public override string Version
			{
				get
				{
					return this.version;
				}
			}

			private void LoadVersionInfo()
			{
				string text = "CLSID\\" + this.clsid;
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text);
				if (registryKey != null)
				{
					RegistryKey registryKey2 = registryKey.OpenSubKey("Version");
					if (registryKey2 != null)
					{
						this.version = (string)registryKey2.GetValue("");
					}
				}
			}

			protected override IComponent[] CreateComponentsCore(IDesignerHost host)
			{
				IComponent[] array = null;
				object references = this.GetReferences(host);
				if (references != null)
				{
					try
					{
						global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR typeLibAttr = this.GetTypeLibAttr();
						object[] array2 = new object[]
						{
							"{" + typeLibAttr.guid.ToString() + "}",
							(int)typeLibAttr.wMajorVerNum,
							(int)typeLibAttr.wMinorVerNum,
							typeLibAttr.lcid,
							""
						};
						references.GetType().InvokeMember("AddActiveX", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, references, array2, CultureInfo.InvariantCulture);
						array2[4] = "aximp";
						object obj = references.GetType().InvokeMember("AddActiveX", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, references, array2, CultureInfo.InvariantCulture);
						this.axctlType = this.GetAxTypeFromReference(obj, host);
					}
					catch (TargetInvocationException ex)
					{
						throw ex.InnerException;
					}
					catch (Exception ex2)
					{
						throw ex2;
					}
					catch
					{
						throw;
					}
				}
				if (this.axctlType == null)
				{
					IUIService iuiservice = (IUIService)host.GetService(typeof(IUIService));
					if (iuiservice == null)
					{
						RTLAwareMessageBox.Show(null, SR.GetString("AxImportFailed"), null, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
					else
					{
						iuiservice.ShowError(SR.GetString("AxImportFailed"));
					}
					return new IComponent[0];
				}
				array = new IComponent[1];
				try
				{
					array[0] = host.CreateComponent(this.axctlType);
				}
				catch (Exception ex3)
				{
					throw ex3;
				}
				catch
				{
					throw;
				}
				return array;
			}

			protected override void Deserialize(SerializationInfo info, StreamingContext context)
			{
				base.Deserialize(info, context);
				this.clsid = info.GetString("Clsid");
			}

			private Type GetAxTypeFromReference(object reference, IDesignerHost host)
			{
				string text = (string)reference.GetType().InvokeMember("Path", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, reference, null, CultureInfo.InvariantCulture);
				if (text == null || text.Length <= 0)
				{
					return null;
				}
				FileInfo fileInfo = new FileInfo(text);
				string fullName = fileInfo.FullName;
				ITypeResolutionService typeResolutionService = (ITypeResolutionService)host.GetService(typeof(ITypeResolutionService));
				Assembly assembly = typeResolutionService.GetAssembly(AssemblyName.GetAssemblyName(fullName));
				return this.GetAxTypeFromAssembly(assembly);
			}

			private Type GetAxTypeFromAssembly(Assembly a)
			{
				Type[] types = a.GetTypes();
				int num = types.Length;
				for (int i = 0; i < num; i++)
				{
					Type type = types[i];
					if (typeof(AxHost).IsAssignableFrom(type))
					{
						object[] customAttributes = type.GetCustomAttributes(typeof(AxHost.ClsidAttribute), false);
						AxHost.ClsidAttribute clsidAttribute = (AxHost.ClsidAttribute)customAttributes[0];
						if (string.Equals(clsidAttribute.Value, this.clsid, StringComparison.OrdinalIgnoreCase))
						{
							return type;
						}
					}
				}
				return null;
			}

			private object GetReferences(IDesignerHost host)
			{
				Type type = Type.GetType("EnvDTE.ProjectItem, EnvDTE, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				if (type == null)
				{
					return null;
				}
				object service = host.GetService(type);
				if (service == null)
				{
					return null;
				}
				service.GetType().InvokeMember("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, service, null, CultureInfo.InvariantCulture).ToString();
				object obj = service.GetType().InvokeMember("ContainingProject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, service, null, CultureInfo.InvariantCulture);
				object obj2 = obj.GetType().InvokeMember("Object", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, obj, null, CultureInfo.InvariantCulture);
				return obj2.GetType().InvokeMember("References", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, obj2, null, CultureInfo.InvariantCulture);
			}

			private global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR GetTypeLibAttr()
			{
				string text = "CLSID\\" + this.clsid;
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text);
				if (registryKey == null)
				{
					bool traceVerbose = DocumentDesigner.AxToolSwitch.TraceVerbose;
					throw new ArgumentException(SR.GetString("AXNotRegistered", new object[] { text.ToString() }));
				}
				ITypeLib typeLib = null;
				Guid guid = Guid.Empty;
				RegistryKey registryKey2 = registryKey.OpenSubKey("TypeLib");
				if (registryKey2 != null)
				{
					RegistryKey registryKey3 = registryKey.OpenSubKey("Version");
					string text2 = (string)registryKey3.GetValue("");
					int num = text2.IndexOf('.');
					short num2;
					short num3;
					if (num == -1)
					{
						num2 = short.Parse(text2, CultureInfo.InvariantCulture);
						num3 = 0;
					}
					else
					{
						num2 = short.Parse(text2.Substring(0, num), CultureInfo.InvariantCulture);
						num3 = short.Parse(text2.Substring(num + 1, text2.Length - num - 1), CultureInfo.InvariantCulture);
					}
					registryKey3.Close();
					object value = registryKey2.GetValue("");
					guid = new Guid((string)value);
					registryKey2.Close();
					try
					{
						typeLib = NativeMethods.LoadRegTypeLib(ref guid, num2, num3, Application.CurrentCulture.LCID);
					}
					catch (Exception ex)
					{
						bool enabled = AxWrapperGen.AxWrapper.Enabled;
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					catch
					{
					}
				}
				if (typeLib == null)
				{
					RegistryKey registryKey4 = registryKey.OpenSubKey("InprocServer32");
					if (registryKey4 != null)
					{
						string text3 = (string)registryKey4.GetValue("");
						registryKey4.Close();
						typeLib = NativeMethods.LoadTypeLib(text3);
					}
				}
				registryKey.Close();
				if (typeLib != null)
				{
					try
					{
						IntPtr invalidIntPtr = NativeMethods.InvalidIntPtr;
						typeLib.GetLibAttr(out invalidIntPtr);
						if (invalidIntPtr == NativeMethods.InvalidIntPtr)
						{
							throw new ArgumentException(SR.GetString("AXNotRegistered", new object[] { text.ToString() }));
						}
						global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR)Marshal.PtrToStructure(invalidIntPtr, typeof(global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR));
						typeLib.ReleaseTLibAttr(invalidIntPtr);
						return typelibattr;
					}
					finally
					{
						Marshal.ReleaseComObject(typeLib);
					}
				}
				throw new ArgumentException(SR.GetString("AXNotRegistered", new object[] { text.ToString() }));
			}

			protected override void Serialize(SerializationInfo info, StreamingContext context)
			{
				bool traceVerbose = DocumentDesigner.AxToolSwitch.TraceVerbose;
				base.Serialize(info, context);
				info.AddValue("Clsid", this.clsid);
			}

			private string clsid;

			private Type axctlType;

			private string version = string.Empty;
		}

		private class DocumentInheritanceService : InheritanceService
		{
			public DocumentInheritanceService(DocumentDesigner designer)
			{
				this.designer = designer;
			}

			protected override bool IgnoreInheritedMember(MemberInfo member, IComponent component)
			{
				FieldInfo fieldInfo = member as FieldInfo;
				MethodInfo methodInfo = member as MethodInfo;
				bool flag;
				Type type;
				if (fieldInfo != null)
				{
					flag = fieldInfo.IsPrivate || fieldInfo.IsAssembly;
					type = fieldInfo.FieldType;
				}
				else
				{
					if (methodInfo == null)
					{
						return true;
					}
					flag = methodInfo.IsPrivate || methodInfo.IsAssembly;
					type = methodInfo.ReturnType;
				}
				if (flag)
				{
					if (typeof(Control).IsAssignableFrom(type))
					{
						Control control = null;
						if (fieldInfo != null)
						{
							control = (Control)fieldInfo.GetValue(component);
						}
						else if (methodInfo != null)
						{
							control = (Control)methodInfo.Invoke(component, null);
						}
						Control control2 = this.designer.Control;
						while (control != null && control != control2)
						{
							control = control.Parent;
						}
						if (control != null)
						{
							return false;
						}
					}
					else if (typeof(Menu).IsAssignableFrom(type))
					{
						object obj = null;
						if (fieldInfo != null)
						{
							obj = fieldInfo.GetValue(component);
						}
						else if (methodInfo != null)
						{
							obj = methodInfo.Invoke(component, null);
						}
						if (obj != null)
						{
							return false;
						}
					}
				}
				return base.IgnoreInheritedMember(member, component);
			}

			private DocumentDesigner designer;
		}
	}
}
