using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ToolStripPanelDesigner : ScrollableControlDesigner
	{
		private Pen BorderPen
		{
			get
			{
				Color color = (((double)this.Control.BackColor.GetBrightness() < 0.5) ? ControlPaint.Light(this.Control.BackColor) : ControlPaint.Dark(this.Control.BackColor));
				return new Pen(color)
				{
					DashStyle = DashStyle.Dash
				};
			}
		}

		private ContextMenuStrip DesignerContextMenu
		{
			get
			{
				if (this.contextMenu == null)
				{
					this.contextMenu = new BaseContextMenuStrip(base.Component.Site, base.Component as Component);
					this.contextMenu.GroupOrdering.Clear();
					this.contextMenu.GroupOrdering.AddRange(new string[] { "Code", "Verbs", "Custom", "Selection", "Edit", "Properties" });
					this.contextMenu.Text = "CustomContextMenu";
				}
				return this.contextMenu;
			}
		}

		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (this.panel != null && this.panel.Parent is ToolStripContainer && base.InheritanceAttribute == InheritanceAttribute.Inherited)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		private Padding Padding
		{
			get
			{
				return (Padding)base.ShadowProperties["Padding"];
			}
			set
			{
				base.ShadowProperties["Padding"] = value;
			}
		}

		public override bool ParticipatesWithSnapLines
		{
			get
			{
				return false;
			}
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				if (this.panel != null && this.panel.Parent is ToolStripContainer)
				{
					selectionRules = SelectionRules.Locked;
				}
				return selectionRules;
			}
		}

		public ToolStripPanelSelectionGlyph ToolStripPanelSelectorGlyph
		{
			get
			{
				return this.containerSelectorGlyph;
			}
		}

		private bool Visible
		{
			get
			{
				return (bool)base.ShadowProperties["Visible"];
			}
			set
			{
				base.ShadowProperties["Visible"] = value;
				this.panel.Visible = value;
			}
		}

		public override bool CanParent(Control control)
		{
			return control is ToolStrip;
		}

		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return this.panel != null && !(this.panel.Parent is ToolStripContainer);
		}

		private void ComponentChangeSvc_ComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (this.containerSelectorGlyph != null)
			{
				this.containerSelectorGlyph.UpdateGlyph();
			}
		}

		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			if (tool != null)
			{
				Type type = tool.GetType(this.designerHost);
				if (!typeof(ToolStrip).IsAssignableFrom(type))
				{
					ToolStripContainer toolStripContainer = this.panel.Parent as ToolStripContainer;
					if (toolStripContainer != null)
					{
						ToolStripContentPanel contentPanel = toolStripContainer.ContentPanel;
						if (contentPanel != null)
						{
							PanelDesigner panelDesigner = this.designerHost.GetDesigner(contentPanel) as PanelDesigner;
							if (panelDesigner != null)
							{
								ParentControlDesigner.InvokeCreateTool(panelDesigner, tool);
							}
						}
					}
				}
				else
				{
					base.CreateToolCore(tool, x, y, width, height, hasLocation, hasSize);
				}
			}
			return null;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				base.Dispose(disposing);
			}
			finally
			{
				if (disposing && this.contextMenu != null)
				{
					this.contextMenu.Dispose();
				}
				if (this.selectionSvc != null)
				{
					this.selectionSvc.SelectionChanging -= this.OnSelectionChanging;
					this.selectionSvc.SelectionChanged -= this.OnSelectionChanged;
					this.selectionSvc = null;
				}
				if (this.componentChangeSvc != null)
				{
					this.componentChangeSvc.ComponentChanged -= this.ComponentChangeSvc_ComponentChanged;
				}
				this.panel.ControlAdded -= this.OnControlAdded;
				this.panel.ControlRemoved -= this.OnControlRemoved;
			}
		}

		private void DrawBorder(Graphics graphics)
		{
			Pen borderPen = this.BorderPen;
			Rectangle clientRectangle = this.Control.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(borderPen, clientRectangle);
			borderPen.Dispose();
		}

		internal void ExpandTopPanel()
		{
			if (this.containerSelectorGlyph == null)
			{
				this.behavior = new ToolStripPanelSelectionBehavior(this.panel, base.Component.Site);
				this.containerSelectorGlyph = new ToolStripPanelSelectionGlyph(Rectangle.Empty, Cursors.Default, this.panel, base.Component.Site, this.behavior);
			}
			if (this.panel != null && this.panel.Dock == DockStyle.Top)
			{
				this.panel.Padding = new Padding(0, 0, 25, 25);
				this.containerSelectorGlyph.IsExpanded = true;
			}
		}

		private void OnKeyShowDesignerActions(object sender, EventArgs e)
		{
			if (this.containerSelectorGlyph != null)
			{
				this.behavior.OnMouseDown(this.containerSelectorGlyph, MouseButtons.Left, Point.Empty);
			}
		}

		internal Glyph GetGlyph()
		{
			if (this.panel != null)
			{
				if (this.containerSelectorGlyph == null)
				{
					this.behavior = new ToolStripPanelSelectionBehavior(this.panel, base.Component.Site);
					this.containerSelectorGlyph = new ToolStripPanelSelectionGlyph(Rectangle.Empty, Cursors.Default, this.panel, base.Component.Site, this.behavior);
				}
				if (this.panel.Visible)
				{
					return this.containerSelectorGlyph;
				}
			}
			return null;
		}

		protected override Control GetParentForComponent(IComponent component)
		{
			Type type = component.GetType();
			if (typeof(ToolStrip).IsAssignableFrom(type))
			{
				return this.panel;
			}
			ToolStripContainer toolStripContainer = this.panel.Parent as ToolStripContainer;
			if (toolStripContainer != null)
			{
				return toolStripContainer.ContentPanel;
			}
			return null;
		}

		public override void Initialize(IComponent component)
		{
			this.panel = component as ToolStripPanel;
			base.Initialize(component);
			this.Padding = this.panel.Padding;
			this.designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
			if (this.selectionSvc == null)
			{
				this.selectionSvc = (ISelectionService)this.GetService(typeof(ISelectionService));
				this.selectionSvc.SelectionChanging += this.OnSelectionChanging;
				this.selectionSvc.SelectionChanged += this.OnSelectionChanged;
			}
			if (this.designerHost != null)
			{
				this.componentChangeSvc = (IComponentChangeService)this.designerHost.GetService(typeof(IComponentChangeService));
			}
			if (this.componentChangeSvc != null)
			{
				this.componentChangeSvc.ComponentChanged += this.ComponentChangeSvc_ComponentChanged;
			}
			this.panel.ControlAdded += this.OnControlAdded;
			this.panel.ControlRemoved += this.OnControlRemoved;
		}

		internal void InvalidateGlyph()
		{
			if (this.containerSelectorGlyph != null)
			{
				base.BehaviorService.Invalidate(this.containerSelectorGlyph.Bounds);
			}
		}

		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control is ToolStrip)
			{
				this.panel.Padding = new Padding(0);
				if (this.containerSelectorGlyph != null)
				{
					this.containerSelectorGlyph.IsExpanded = false;
				}
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(e.Control)["Dock"];
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(e.Control, DockStyle.None);
				}
				if (this.designerHost != null && !this.designerHost.Loading)
				{
					SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
					if (selectionManager != null)
					{
						selectionManager.Refresh();
					}
				}
			}
		}

		private void OnControlRemoved(object sender, ControlEventArgs e)
		{
			if (this.panel.Controls.Count == 0)
			{
				if (this.containerSelectorGlyph != null)
				{
					this.containerSelectorGlyph.IsExpanded = false;
				}
				if (this.designerHost != null && !this.designerHost.Loading)
				{
					SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
					if (selectionManager != null)
					{
						selectionManager.Refresh();
					}
				}
			}
		}

		protected override void OnContextMenu(int x, int y)
		{
			if (this.panel != null && this.panel.Parent is ToolStripContainer)
			{
				this.DesignerContextMenu.Show(x, y);
				return;
			}
			base.OnContextMenu(x, y);
		}

		private void OnSelectionChanging(object sender, EventArgs e)
		{
			if (this.designerShortCutCommand != null)
			{
				IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
				if (menuCommandService != null)
				{
					menuCommandService.RemoveCommand(this.designerShortCutCommand);
					if (this.oldShortCutCommand != null)
					{
						menuCommandService.AddCommand(this.oldShortCutCommand);
					}
				}
				this.designerShortCutCommand = null;
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (this.selectionSvc.PrimarySelection == this.panel)
			{
				this.designerShortCutCommand = new MenuCommand(new EventHandler(this.OnKeyShowDesignerActions), MenuCommands.KeyInvokeSmartTag);
				IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
				if (menuCommandService != null)
				{
					this.oldShortCutCommand = menuCommandService.FindCommand(MenuCommands.KeyInvokeSmartTag);
					if (this.oldShortCutCommand != null)
					{
						menuCommandService.RemoveCommand(this.oldShortCutCommand);
					}
					menuCommandService.AddCommand(this.designerShortCutCommand);
				}
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			if (!ToolStripDesignerUtils.DisplayInformation.TerminalServer && !ToolStripDesignerUtils.DisplayInformation.HighContrast && !ToolStripDesignerUtils.DisplayInformation.LowResolution)
			{
				using (Brush brush = new SolidBrush(Color.FromArgb(50, Color.White)))
				{
					pe.Graphics.FillRectangle(brush, this.panel.ClientRectangle);
				}
			}
			this.DrawBorder(pe.Graphics);
		}

		protected override void PreFilterEvents(IDictionary events)
		{
			base.PreFilterEvents(events);
			if (this.panel.Parent is ToolStripContainer)
			{
				string[] array = new string[]
				{
					"AutoSizeChanged", "BindingContextChanged", "CausesValidationChanged", "ChangeUICues", "DockChanged", "DragDrop", "DragEnter", "DragLeave", "DragOver", "EnabledChanged",
					"FontChanged", "ForeColorChanged", "GiveFeedback", "ImeModeChanged", "KeyDown", "KeyPress", "KeyUp", "LocationChanged", "MarginChanged", "MouseCaptureChanged",
					"Move", "QueryAccessibilityHelp", "QueryContinueDrag", "RegionChanged", "Scroll", "Validated", "Validating"
				};
				for (int i = 0; i < array.Length; i++)
				{
					EventDescriptor eventDescriptor = (EventDescriptor)events[array[i]];
					if (eventDescriptor != null)
					{
						events[array[i]] = TypeDescriptor.CreateEvent(eventDescriptor.ComponentType, eventDescriptor, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (this.panel.Parent is ToolStripContainer)
			{
				properties.Remove("Modifiers");
				properties.Remove("Locked");
				properties.Remove("GenerateMember");
				string[] array = new string[]
				{
					"Anchor", "AutoSize", "Dock", "DockPadding", "Height", "Location", "Name", "Orientation", "Renderer", "RowMargin",
					"Size", "Visible", "Width"
				};
				for (int i = 0; i < array.Length; i++)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
					if (propertyDescriptor != null)
					{
						properties[array[i]] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[]
						{
							BrowsableAttribute.No,
							DesignerSerializationVisibilityAttribute.Hidden
						});
					}
				}
			}
			string[] array2 = new string[] { "Padding", "Visible" };
			Attribute[] array3 = new Attribute[0];
			for (int j = 0; j < array2.Length; j++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array2[j]];
				if (propertyDescriptor != null)
				{
					properties[array2[j]] = TypeDescriptor.CreateProperty(typeof(ToolStripPanelDesigner), propertyDescriptor, array3);
				}
			}
		}

		private bool ShouldSerializePadding()
		{
			return !((Padding)base.ShadowProperties["Padding"]).Equals(ToolStripPanelDesigner._defaultPadding);
		}

		private bool ShouldSerializeVisible()
		{
			return !this.Visible;
		}

		private ToolStripPanel panel;

		private IComponentChangeService componentChangeSvc;

		private static Padding _defaultPadding = new Padding(0);

		private IDesignerHost designerHost;

		private ToolStripPanelSelectionGlyph containerSelectorGlyph;

		private ToolStripPanelSelectionBehavior behavior;

		private BaseContextMenuStrip contextMenu;

		private ISelectionService selectionSvc;

		private MenuCommand designerShortCutCommand;

		private MenuCommand oldShortCutCommand;
	}
}
