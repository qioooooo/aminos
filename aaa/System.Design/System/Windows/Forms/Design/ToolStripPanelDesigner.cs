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
	// Token: 0x020002D0 RID: 720
	internal class ToolStripPanelDesigner : ScrollableControlDesigner
	{
		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x0009A6BC File Offset: 0x000996BC
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

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x0009A71C File Offset: 0x0009971C
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

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x0009A7C3 File Offset: 0x000997C3
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

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x0009A7F8 File Offset: 0x000997F8
		// (set) Token: 0x06001B93 RID: 7059 RVA: 0x0009A80F File Offset: 0x0009980F
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

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x0009A827 File Offset: 0x00099827
		public override bool ParticipatesWithSnapLines
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001B95 RID: 7061 RVA: 0x0009A82C File Offset: 0x0009982C
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

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x0009A861 File Offset: 0x00099861
		public ToolStripPanelSelectionGlyph ToolStripPanelSelectorGlyph
		{
			get
			{
				return this.containerSelectorGlyph;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x0009A869 File Offset: 0x00099869
		// (set) Token: 0x06001B98 RID: 7064 RVA: 0x0009A880 File Offset: 0x00099880
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

		// Token: 0x06001B99 RID: 7065 RVA: 0x0009A8A4 File Offset: 0x000998A4
		public override bool CanParent(Control control)
		{
			return control is ToolStrip;
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x0009A8AF File Offset: 0x000998AF
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return this.panel != null && !(this.panel.Parent is ToolStripContainer);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x0009A8D1 File Offset: 0x000998D1
		private void ComponentChangeSvc_ComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (this.containerSelectorGlyph != null)
			{
				this.containerSelectorGlyph.UpdateGlyph();
			}
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0009A8E8 File Offset: 0x000998E8
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

		// Token: 0x06001B9D RID: 7069 RVA: 0x0009A968 File Offset: 0x00099968
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

		// Token: 0x06001B9E RID: 7070 RVA: 0x0009AA34 File Offset: 0x00099A34
		private void DrawBorder(Graphics graphics)
		{
			Pen borderPen = this.BorderPen;
			Rectangle clientRectangle = this.Control.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(borderPen, clientRectangle);
			borderPen.Dispose();
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0009AA80 File Offset: 0x00099A80
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

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0009AB15 File Offset: 0x00099B15
		private void OnKeyShowDesignerActions(object sender, EventArgs e)
		{
			if (this.containerSelectorGlyph != null)
			{
				this.behavior.OnMouseDown(this.containerSelectorGlyph, MouseButtons.Left, Point.Empty);
			}
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0009AB3C File Offset: 0x00099B3C
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

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0009ABB8 File Offset: 0x00099BB8
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

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0009AC04 File Offset: 0x00099C04
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

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0009AD1B File Offset: 0x00099D1B
		internal void InvalidateGlyph()
		{
			if (this.containerSelectorGlyph != null)
			{
				base.BehaviorService.Invalidate(this.containerSelectorGlyph.Bounds);
			}
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0009AD3C File Offset: 0x00099D3C
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

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0009ADE0 File Offset: 0x00099DE0
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

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0009AE47 File Offset: 0x00099E47
		protected override void OnContextMenu(int x, int y)
		{
			if (this.panel != null && this.panel.Parent is ToolStripContainer)
			{
				this.DesignerContextMenu.Show(x, y);
				return;
			}
			base.OnContextMenu(x, y);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0009AE7C File Offset: 0x00099E7C
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

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0009AED4 File Offset: 0x00099ED4
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

		// Token: 0x06001BAA RID: 7082 RVA: 0x0009AF5C File Offset: 0x00099F5C
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

		// Token: 0x06001BAB RID: 7083 RVA: 0x0009AFD0 File Offset: 0x00099FD0
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

		// Token: 0x06001BAC RID: 7084 RVA: 0x0009B138 File Offset: 0x0009A138
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

		// Token: 0x06001BAD RID: 7085 RVA: 0x0009B2C0 File Offset: 0x0009A2C0
		private bool ShouldSerializePadding()
		{
			return !((Padding)base.ShadowProperties["Padding"]).Equals(ToolStripPanelDesigner._defaultPadding);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0009B2FD File Offset: 0x0009A2FD
		private bool ShouldSerializeVisible()
		{
			return !this.Visible;
		}

		// Token: 0x0400158A RID: 5514
		private ToolStripPanel panel;

		// Token: 0x0400158B RID: 5515
		private IComponentChangeService componentChangeSvc;

		// Token: 0x0400158C RID: 5516
		private static Padding _defaultPadding = new Padding(0);

		// Token: 0x0400158D RID: 5517
		private IDesignerHost designerHost;

		// Token: 0x0400158E RID: 5518
		private ToolStripPanelSelectionGlyph containerSelectorGlyph;

		// Token: 0x0400158F RID: 5519
		private ToolStripPanelSelectionBehavior behavior;

		// Token: 0x04001590 RID: 5520
		private BaseContextMenuStrip contextMenu;

		// Token: 0x04001591 RID: 5521
		private ISelectionService selectionSvc;

		// Token: 0x04001592 RID: 5522
		private MenuCommand designerShortCutCommand;

		// Token: 0x04001593 RID: 5523
		private MenuCommand oldShortCutCommand;
	}
}
