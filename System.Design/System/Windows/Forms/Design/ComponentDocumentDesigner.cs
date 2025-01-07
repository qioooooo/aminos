using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	public class ComponentDocumentDesigner : ComponentDesigner, IRootDesigner, IDesigner, IDisposable, IToolboxUser, IOleDragClient, ITypeDescriptorFilterService
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					designerHost.RemoveService(typeof(IInheritanceService));
					designerHost.RemoveService(typeof(IEventHandlerService));
					designerHost.RemoveService(typeof(ISelectionUIService));
					designerHost.RemoveService(typeof(ComponentTray));
					IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentAdded -= this.OnComponentAdded;
						componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
					}
				}
				if (this.selectionUIService != null)
				{
					this.selectionUIService.Dispose();
					this.selectionUIService = null;
				}
				if (this.commandSet != null)
				{
					this.commandSet.Dispose();
					this.commandSet = null;
				}
				if (this.pbrsFwd != null)
				{
					this.pbrsFwd.Dispose();
					this.pbrsFwd = null;
				}
				if (this.compositionUI != null)
				{
					this.compositionUI.Dispose();
					this.compositionUI = null;
				}
				if (this.designerExtenders != null)
				{
					this.designerExtenders.Dispose();
					this.designerExtenders = null;
				}
				if (this.inheritanceService != null)
				{
					this.inheritanceService.Dispose();
					this.inheritanceService = null;
				}
			}
			base.Dispose(disposing);
		}

		public Control Control
		{
			get
			{
				return this.compositionUI;
			}
		}

		public bool TrayAutoArrange
		{
			get
			{
				return this.autoArrange;
			}
			set
			{
				this.autoArrange = value;
				this.compositionUI.AutoArrange = value;
			}
		}

		public bool TrayLargeIcon
		{
			get
			{
				return this.largeIcons;
			}
			set
			{
				this.largeIcons = value;
				this.compositionUI.ShowLargeIcons = value;
			}
		}

		protected virtual bool GetToolSupported(ToolboxItem tool)
		{
			return true;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.inheritanceService = new InheritanceService();
			ISite site = component.Site;
			IContainer container = null;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			IExtenderProviderService extenderProviderService = (IExtenderProviderService)this.GetService(typeof(IExtenderProviderService));
			if (extenderProviderService != null)
			{
				this.designerExtenders = new DesignerExtenders(extenderProviderService);
			}
			if (designerHost != null)
			{
				this.eventHandlerService = new EventHandlerService(null);
				this.selectionUIService = new SelectionUIService(designerHost);
				designerHost.AddService(typeof(IInheritanceService), this.inheritanceService);
				designerHost.AddService(typeof(IEventHandlerService), this.eventHandlerService);
				designerHost.AddService(typeof(ISelectionUIService), this.selectionUIService);
				this.compositionUI = new ComponentDocumentDesigner.CompositionUI(this, site);
				designerHost.AddService(typeof(ComponentTray), this.compositionUI);
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded += this.OnComponentAdded;
					componentChangeService.ComponentRemoved += this.OnComponentRemoved;
				}
				ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { component }, SelectionTypes.Auto);
				}
			}
			if (site != null)
			{
				this.commandSet = new CompositionCommandSet(this.compositionUI, site);
				container = site.Container;
			}
			this.pbrsFwd = new PbrsForward(this.compositionUI, site);
			this.inheritanceService.AddInheritedComponents(component, container);
			IServiceContainer serviceContainer = (IServiceContainer)this.GetService(typeof(IServiceContainer));
			if (serviceContainer != null)
			{
				this.delegateFilterService = (ITypeDescriptorFilterService)this.GetService(typeof(ITypeDescriptorFilterService));
				if (this.delegateFilterService != null)
				{
					serviceContainer.RemoveService(typeof(ITypeDescriptorFilterService));
				}
				serviceContainer.AddService(typeof(ITypeDescriptorFilterService), this);
			}
		}

		private void OnComponentAdded(object sender, ComponentEventArgs ce)
		{
			if (ce.Component != base.Component)
			{
				this.compositionUI.AddComponent(ce.Component);
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs ce)
		{
			this.compositionUI.RemoveComponent(ce.Component);
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			properties["TrayLargeIcon"] = TypeDescriptor.CreateProperty(base.GetType(), "TrayLargeIcon", typeof(bool), new Attribute[]
			{
				BrowsableAttribute.No,
				DesignOnlyAttribute.Yes,
				CategoryAttribute.Design
			});
		}

		bool IOleDragClient.CanModifyComponents
		{
			get
			{
				return true;
			}
		}

		bool IOleDragClient.AddComponent(IComponent component, string name, bool firstAdd)
		{
			IContainer container = base.Component.Site.Container;
			if (container != null && name != null && container.Components[name] != null)
			{
				name = null;
			}
			IContainer container2 = null;
			bool flag = false;
			if (!firstAdd)
			{
				if (component.Site != null)
				{
					container2 = component.Site.Container;
					if (container2 != container)
					{
						container2.Remove(component);
						flag = true;
					}
				}
				if (container2 != container)
				{
					container.Add(component, name);
				}
			}
			if (flag)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					IComponentInitializer componentInitializer = designerHost.GetDesigner(component) as IComponentInitializer;
					if (componentInitializer != null)
					{
						componentInitializer.InitializeExistingComponent(null);
					}
				}
			}
			return container2 != container;
		}

		Control IOleDragClient.GetControlForComponent(object component)
		{
			if (this.compositionUI != null)
			{
				return ((IOleDragClient)this.compositionUI).GetControlForComponent(component);
			}
			return null;
		}

		Control IOleDragClient.GetDesignerControl()
		{
			if (this.compositionUI != null)
			{
				return ((IOleDragClient)this.compositionUI).GetDesignerControl();
			}
			return null;
		}

		bool IOleDragClient.IsDropOk(IComponent component)
		{
			return true;
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
			return this.compositionUI;
		}

		bool IToolboxUser.GetToolSupported(ToolboxItem tool)
		{
			return true;
		}

		void IToolboxUser.ToolPicked(ToolboxItem tool)
		{
			this.compositionUI.CreateComponentFromTool(tool);
			IToolboxService toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
			if (toolboxService != null)
			{
				toolboxService.SelectedToolboxItemUsed();
			}
		}

		bool ITypeDescriptorFilterService.FilterAttributes(IComponent component, IDictionary attributes)
		{
			return this.delegateFilterService == null || this.delegateFilterService.FilterAttributes(component, attributes);
		}

		bool ITypeDescriptorFilterService.FilterEvents(IComponent component, IDictionary events)
		{
			return this.delegateFilterService == null || this.delegateFilterService.FilterEvents(component, events);
		}

		bool ITypeDescriptorFilterService.FilterProperties(IComponent component, IDictionary properties)
		{
			if (this.delegateFilterService != null)
			{
				this.delegateFilterService.FilterProperties(component, properties);
			}
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["Location"];
			if (propertyDescriptor != null)
			{
				properties["Location"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
			return true;
		}

		private ComponentDocumentDesigner.CompositionUI compositionUI;

		private CompositionCommandSet commandSet;

		private IEventHandlerService eventHandlerService;

		private InheritanceService inheritanceService;

		private SelectionUIService selectionUIService;

		private DesignerExtenders designerExtenders;

		private ITypeDescriptorFilterService delegateFilterService;

		private bool largeIcons;

		private bool autoArrange = true;

		private PbrsForward pbrsFwd;

		private class WatermarkLabel : LinkLabel
		{
			public WatermarkLabel(ComponentDocumentDesigner.CompositionUI compositionUI)
			{
				this.compositionUI = compositionUI;
			}

			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 32)
				{
					if (msg != 132)
					{
						base.WndProc(ref m);
						return;
					}
					Point point = base.PointToClient(new Point((int)m.LParam));
					if (base.PointInLink(point.X, point.Y) == null)
					{
						m.Result = (IntPtr)(-1);
						return;
					}
					base.WndProc(ref m);
					return;
				}
				else
				{
					if (base.OverrideCursor == null)
					{
						this.compositionUI.SetCursor();
						return;
					}
					base.WndProc(ref m);
					return;
				}
			}

			private ComponentDocumentDesigner.CompositionUI compositionUI;
		}

		private class CompositionUI : ComponentTray
		{
			public CompositionUI(ComponentDocumentDesigner compositionDesigner, IServiceProvider provider)
				: base(compositionDesigner, provider)
			{
				this.compositionDesigner = compositionDesigner;
				this.serviceProvider = provider;
				this.watermark = new ComponentDocumentDesigner.WatermarkLabel(this);
				this.watermark.Font = new Font(this.watermark.Font.FontFamily, 11f);
				this.watermark.TextAlign = ContentAlignment.MiddleCenter;
				this.watermark.LinkClicked += this.OnLinkClick;
				this.watermark.Dock = DockStyle.Fill;
				this.watermark.TabStop = false;
				this.watermark.Text = SR.GetString("CompositionDesignerWaterMark");
				try
				{
					string text = SR.GetString("CompositionDesignerWaterMarkFirstLink");
					int num = this.watermark.Text.IndexOf(text);
					int num2 = text.Length;
					this.watermark.Links.Add(num, num2, "Toolbox");
					text = SR.GetString("CompositionDesignerWaterMarkSecondLink");
					num = this.watermark.Text.IndexOf(text);
					num2 = text.Length;
					this.watermark.Links.Add(num, num2, "CodeView");
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
				base.Controls.Add(this.watermark);
			}

			public override void AddComponent(IComponent component)
			{
				base.AddComponent(component);
				if (base.Controls.Count > 0)
				{
					this.watermark.Visible = false;
				}
			}

			protected override bool CanCreateComponentFromTool(ToolboxItem tool)
			{
				return true;
			}

			internal override OleDragDropHandler GetOleDragHandler()
			{
				if (this.oleDragDropHandler == null)
				{
					this.oleDragDropHandler = new OleDragDropHandler(this.DragHandler, this.serviceProvider, this);
				}
				return this.oleDragDropHandler;
			}

			internal override SelectionUIHandler DragHandler
			{
				get
				{
					if (this.dragHandler == null)
					{
						this.dragHandler = new ComponentDocumentDesigner.CompositionUI.CompositionSelectionUIHandler(this.compositionDesigner);
					}
					return this.dragHandler;
				}
			}

			private void OnLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
			{
				IUIService iuiservice = (IUIService)this.compositionDesigner.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					string text = (string)e.Link.LinkData;
					if (text == "ServerExplorer")
					{
						iuiservice.ShowToolWindow(StandardToolWindows.ServerExplorer);
						return;
					}
					if (text == "Toolbox")
					{
						iuiservice.ShowToolWindow(StandardToolWindows.Toolbox);
						return;
					}
					IEventBindingService eventBindingService = (IEventBindingService)this.serviceProvider.GetService(typeof(IEventBindingService));
					if (eventBindingService != null)
					{
						eventBindingService.ShowCode();
					}
				}
			}

			internal void SetCursor()
			{
				if (this.toolboxService == null)
				{
					this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
				}
				if (this.toolboxService == null || !this.toolboxService.SetCursor())
				{
					base.OnSetCursor();
				}
			}

			protected override void OnDragDrop(DragEventArgs de)
			{
				if (base.ClientRectangle.Contains(base.PointToClient(new Point(de.X, de.Y))))
				{
					base.OnDragDrop(de);
					return;
				}
				de.Effect = DragDropEffects.None;
			}

			protected override void OnDragOver(DragEventArgs de)
			{
				if (base.ClientRectangle.Contains(base.PointToClient(new Point(de.X, de.Y))))
				{
					base.OnDragOver(de);
					return;
				}
				de.Effect = DragDropEffects.None;
			}

			protected override void OnResize(EventArgs e)
			{
				base.OnResize(e);
				if (this.watermark != null)
				{
					this.watermark.Location = new Point(0, base.Size.Height / 2);
					this.watermark.Size = new Size(base.Width, base.Size.Height / 2);
				}
			}

			protected override void OnSetCursor()
			{
				this.SetCursor();
			}

			public override void RemoveComponent(IComponent component)
			{
				base.RemoveComponent(component);
				if (base.Controls.Count == 1)
				{
					this.watermark.Visible = true;
				}
			}

			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				base.WndProc(ref m);
			}

			private const int bannerHeight = 40;

			private const int borderWidth = 10;

			private ComponentDocumentDesigner.WatermarkLabel watermark;

			private IToolboxService toolboxService;

			private ComponentDocumentDesigner compositionDesigner;

			private IServiceProvider serviceProvider;

			private SelectionUIHandler dragHandler;

			private class CompositionSelectionUIHandler : SelectionUIHandler
			{
				public CompositionSelectionUIHandler(ComponentDocumentDesigner compositionDesigner)
				{
					this.compositionDesigner = compositionDesigner;
				}

				protected override IComponent GetComponent()
				{
					return this.compositionDesigner.Component;
				}

				protected override Control GetControl()
				{
					return this.compositionDesigner.Control;
				}

				protected override Control GetControl(IComponent component)
				{
					return ComponentTray.TrayControl.FromComponent(component);
				}

				protected override Size GetCurrentSnapSize()
				{
					return new Size(8, 8);
				}

				protected override object GetService(Type serviceType)
				{
					return this.compositionDesigner.GetService(serviceType);
				}

				protected override bool GetShouldSnapToGrid()
				{
					return false;
				}

				public override Rectangle GetUpdatedRect(Rectangle originalRect, Rectangle dragRect, bool updateSize)
				{
					Rectangle rectangle2;
					if (this.GetShouldSnapToGrid())
					{
						Rectangle rectangle = dragRect;
						int x = dragRect.X;
						int y = dragRect.Y;
						int num = dragRect.X + dragRect.Width;
						int num2 = dragRect.Y + dragRect.Height;
						Size size = new Size(8, 8);
						int num3 = size.Width / 2 * ((x < 0) ? (-1) : 1);
						int num4 = size.Height / 2 * ((y < 0) ? (-1) : 1);
						rectangle.X = (x + num3) / size.Width * size.Width;
						rectangle.Y = (y + num4) / size.Height * size.Height;
						num3 = size.Width / 2 * ((num < 0) ? (-1) : 1);
						num4 = size.Height / 2 * ((num2 < 0) ? (-1) : 1);
						if (updateSize)
						{
							rectangle.Width = (num + num3) / size.Width * size.Width - rectangle.X;
							rectangle.Height = (num2 + num4) / size.Height * size.Height - rectangle.Y;
						}
						rectangle2 = rectangle;
					}
					else
					{
						rectangle2 = dragRect;
					}
					return rectangle2;
				}

				public override void SetCursor()
				{
					this.compositionDesigner.compositionUI.OnSetCursor();
				}

				private ComponentDocumentDesigner compositionDesigner;
			}
		}
	}
}
