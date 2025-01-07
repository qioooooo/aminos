using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ToolStripContainerDesigner : ParentControlDesigner
	{
		private bool TopToolStripPanelVisible
		{
			get
			{
				return (bool)base.ShadowProperties["TopToolStripPanelVisible"];
			}
			set
			{
				base.ShadowProperties["TopToolStripPanelVisible"] = value;
				((ToolStripContainer)base.Component).TopToolStripPanelVisible = value;
			}
		}

		private bool LeftToolStripPanelVisible
		{
			get
			{
				return (bool)base.ShadowProperties["LeftToolStripPanelVisible"];
			}
			set
			{
				base.ShadowProperties["LeftToolStripPanelVisible"] = value;
				((ToolStripContainer)base.Component).LeftToolStripPanelVisible = value;
			}
		}

		private bool RightToolStripPanelVisible
		{
			get
			{
				return (bool)base.ShadowProperties["RightToolStripPanelVisible"];
			}
			set
			{
				base.ShadowProperties["RightToolStripPanelVisible"] = value;
				((ToolStripContainer)base.Component).RightToolStripPanelVisible = value;
			}
		}

		private bool BottomToolStripPanelVisible
		{
			get
			{
				return (bool)base.ShadowProperties["BottomToolStripPanelVisible"];
			}
			set
			{
				base.ShadowProperties["BottomToolStripPanelVisible"] = value;
				((ToolStripContainer)base.Component).BottomToolStripPanelVisible = value;
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				return new DesignerActionListCollection
				{
					new ToolStripContainerActionList(this.toolStripContainer)
					{
						AutoShow = true
					}
				};
			}
		}

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

		public override IList SnapLines
		{
			get
			{
				return base.SnapLinesInternal() as ArrayList;
			}
		}

		public override int NumberOfInternalControlDesigners()
		{
			return this.panels.Length;
		}

		public override ControlDesigner InternalControlDesigner(int internalControlIndex)
		{
			if (internalControlIndex < this.panels.Length && internalControlIndex >= 0)
			{
				Control control = this.panels[internalControlIndex];
				return this.designerHost.GetDesigner(control) as ControlDesigner;
			}
			return null;
		}

		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this.toolStripContainer.Controls)
				{
					Control control = (Control)obj;
					foreach (object obj2 in control.Controls)
					{
						Control control2 = (Control)obj2;
						arrayList.Add(control2);
					}
				}
				return arrayList;
			}
		}

		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			if (tool != null)
			{
				Type type = tool.GetType(this.designerHost);
				if (typeof(StatusStrip).IsAssignableFrom(type))
				{
					ParentControlDesigner.InvokeCreateTool(this.GetDesigner(this.bottomToolStripPanel), tool);
				}
				else if (typeof(ToolStrip).IsAssignableFrom(type))
				{
					ParentControlDesigner.InvokeCreateTool(this.GetDesigner(this.topToolStripPanel), tool);
				}
				else
				{
					ParentControlDesigner.InvokeCreateTool(this.GetDesigner(this.contentToolStripPanel), tool);
				}
			}
			return null;
		}

		public override bool CanParent(Control control)
		{
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (this.selectionSvc != null)
			{
				this.selectionSvc = null;
			}
		}

		private ToolStripPanelDesigner GetDesigner(ToolStripPanel panel)
		{
			return this.designerHost.GetDesigner(panel) as ToolStripPanelDesigner;
		}

		private PanelDesigner GetDesigner(ToolStripContentPanel panel)
		{
			return this.designerHost.GetDesigner(panel) as PanelDesigner;
		}

		private ToolStripContainer ContainerParent(Control c)
		{
			ToolStripContainer toolStripContainer = null;
			if (c != null && !(c is ToolStripContainer))
			{
				while (c.Parent != null)
				{
					if (c.Parent is ToolStripContainer)
					{
						toolStripContainer = c.Parent as ToolStripContainer;
						break;
					}
					c = c.Parent;
				}
			}
			return toolStripContainer;
		}

		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				for (int i = 0; i <= 4; i++)
				{
					Control control = this.panels[i];
					Rectangle rectangle = base.BehaviorService.ControlRectInAdornerWindow(control);
					ControlDesigner controlDesigner = this.InternalControlDesigner(i);
					this.OnSetCursor();
					if (controlDesigner != null)
					{
						ControlBodyGlyph controlBodyGlyph = new ControlBodyGlyph(rectangle, Cursor.Current, control, controlDesigner);
						selectionManager.BodyGlyphAdorner.Glyphs.Add(controlBodyGlyph);
						bool flag = true;
						ICollection selectedComponents = this.selectionSvc.GetSelectedComponents();
						if (!this.selectionSvc.GetComponentSelected(this.toolStripContainer))
						{
							foreach (object obj in selectedComponents)
							{
								ToolStripContainer toolStripContainer = this.ContainerParent(obj as Control);
								flag = toolStripContainer == this.toolStripContainer;
							}
						}
						if (flag)
						{
							ToolStripPanelDesigner toolStripPanelDesigner = controlDesigner as ToolStripPanelDesigner;
							if (toolStripPanelDesigner != null)
							{
								this.AddPanelSelectionGlyph(toolStripPanelDesigner, selectionManager);
							}
						}
					}
				}
			}
			return base.GetControlGlyph(selectionType);
		}

		private Control GetAssociatedControl(Component c)
		{
			if (c is Control)
			{
				return c as Control;
			}
			if (c is ToolStripItem)
			{
				ToolStripItem toolStripItem = c as ToolStripItem;
				Control control = toolStripItem.GetCurrentParent();
				if (control == null)
				{
					control = toolStripItem.Owner;
				}
				return control;
			}
			return null;
		}

		private bool CheckDropDownBounds(ToolStripDropDownItem dropDownItem, Glyph childGlyph, GlyphCollection glyphs)
		{
			if (dropDownItem != null)
			{
				Rectangle bounds = childGlyph.Bounds;
				Rectangle rectangle = base.BehaviorService.ControlRectInAdornerWindow(dropDownItem.DropDown);
				if (!bounds.IntersectsWith(rectangle))
				{
					glyphs.Insert(0, childGlyph);
				}
				return true;
			}
			return false;
		}

		private bool CheckAssociatedControl(Component c, Glyph childGlyph, GlyphCollection glyphs)
		{
			bool flag = false;
			ToolStripDropDownItem toolStripDropDownItem = c as ToolStripDropDownItem;
			if (toolStripDropDownItem != null)
			{
				flag = this.CheckDropDownBounds(toolStripDropDownItem, childGlyph, glyphs);
			}
			if (!flag)
			{
				Control associatedControl = this.GetAssociatedControl(c);
				if (associatedControl != null && associatedControl != this.toolStripContainer && !UnsafeNativeMethods.IsChild(new HandleRef(this.toolStripContainer, this.toolStripContainer.Handle), new HandleRef(associatedControl, associatedControl.Handle)))
				{
					Rectangle bounds = childGlyph.Bounds;
					Rectangle rectangle = base.BehaviorService.ControlRectInAdornerWindow(associatedControl);
					if (c == this.designerHost.RootComponent || !bounds.IntersectsWith(rectangle))
					{
						glyphs.Insert(0, childGlyph);
					}
					flag = true;
				}
			}
			return flag;
		}

		protected override Control GetParentForComponent(IComponent component)
		{
			Type type = component.GetType();
			if (typeof(StatusStrip).IsAssignableFrom(type))
			{
				return this.bottomToolStripPanel;
			}
			if (typeof(ToolStrip).IsAssignableFrom(type))
			{
				return this.topToolStripPanel;
			}
			return this.contentToolStripPanel;
		}

		public override void Initialize(IComponent component)
		{
			this.toolStripContainer = (ToolStripContainer)component;
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.topToolStripPanel = this.toolStripContainer.TopToolStripPanel;
			this.bottomToolStripPanel = this.toolStripContainer.BottomToolStripPanel;
			this.leftToolStripPanel = this.toolStripContainer.LeftToolStripPanel;
			this.rightToolStripPanel = this.toolStripContainer.RightToolStripPanel;
			this.contentToolStripPanel = this.toolStripContainer.ContentPanel;
			this.panels = new Control[] { this.contentToolStripPanel, this.leftToolStripPanel, this.rightToolStripPanel, this.topToolStripPanel, this.bottomToolStripPanel };
			ToolboxBitmapAttribute toolboxBitmapAttribute = new ToolboxBitmapAttribute(typeof(ToolStripPanel), "ToolStripContainer_BottomToolStripPanel.bmp");
			ToolboxBitmapAttribute toolboxBitmapAttribute2 = new ToolboxBitmapAttribute(typeof(ToolStripPanel), "ToolStripContainer_RightToolStripPanel.bmp");
			ToolboxBitmapAttribute toolboxBitmapAttribute3 = new ToolboxBitmapAttribute(typeof(ToolStripPanel), "ToolStripContainer_TopToolStripPanel.bmp");
			ToolboxBitmapAttribute toolboxBitmapAttribute4 = new ToolboxBitmapAttribute(typeof(ToolStripPanel), "ToolStripContainer_LeftToolStripPanel.bmp");
			TypeDescriptor.AddAttributes(this.bottomToolStripPanel, new Attribute[]
			{
				toolboxBitmapAttribute,
				new DescriptionAttribute("bottom")
			});
			TypeDescriptor.AddAttributes(this.rightToolStripPanel, new Attribute[]
			{
				toolboxBitmapAttribute2,
				new DescriptionAttribute("right")
			});
			TypeDescriptor.AddAttributes(this.leftToolStripPanel, new Attribute[]
			{
				toolboxBitmapAttribute4,
				new DescriptionAttribute("left")
			});
			TypeDescriptor.AddAttributes(this.topToolStripPanel, new Attribute[]
			{
				toolboxBitmapAttribute3,
				new DescriptionAttribute("top")
			});
			base.EnableDesignMode(this.topToolStripPanel, "TopToolStripPanel");
			base.EnableDesignMode(this.bottomToolStripPanel, "BottomToolStripPanel");
			base.EnableDesignMode(this.leftToolStripPanel, "LeftToolStripPanel");
			base.EnableDesignMode(this.rightToolStripPanel, "RightToolStripPanel");
			base.EnableDesignMode(this.contentToolStripPanel, "ContentPanel");
			this.designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (this.selectionSvc == null)
			{
				this.selectionSvc = (ISelectionService)this.GetService(typeof(ISelectionService));
			}
			if (this.topToolStripPanel != null)
			{
				ToolStripPanelDesigner toolStripPanelDesigner = this.designerHost.GetDesigner(this.topToolStripPanel) as ToolStripPanelDesigner;
				toolStripPanelDesigner.ExpandTopPanel();
			}
			this.TopToolStripPanelVisible = this.toolStripContainer.TopToolStripPanelVisible;
			this.LeftToolStripPanelVisible = this.toolStripContainer.LeftToolStripPanelVisible;
			this.RightToolStripPanelVisible = this.toolStripContainer.RightToolStripPanelVisible;
			this.BottomToolStripPanelVisible = this.toolStripContainer.BottomToolStripPanelVisible;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
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

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "TopToolStripPanelVisible", "LeftToolStripPanelVisible", "RightToolStripPanelVisible", "BottomToolStripPanelVisible" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(ToolStripContainerDesigner), propertyDescriptor, array2);
				}
			}
		}

		private void AddPanelSelectionGlyph(ToolStripPanelDesigner designer, SelectionManager selMgr)
		{
			if (designer != null)
			{
				Glyph glyph = designer.GetGlyph();
				if (glyph != null)
				{
					ICollection selectedComponents = this.selectionSvc.GetSelectedComponents();
					foreach (object obj in selectedComponents)
					{
						Component component = obj as Component;
						if (component != null && !this.CheckAssociatedControl(component, glyph, selMgr.BodyGlyphAdorner.Glyphs))
						{
							selMgr.BodyGlyphAdorner.Glyphs.Insert(0, glyph);
						}
					}
				}
			}
		}

		private const string topToolStripPanelName = "TopToolStripPanel";

		private const string bottomToolStripPanelName = "BottomToolStripPanel";

		private const string leftToolStripPanelName = "LeftToolStripPanel";

		private const string rightToolStripPanelName = "RightToolStripPanel";

		private const string contentToolStripPanelName = "ContentPanel";

		private ToolStripPanel topToolStripPanel;

		private ToolStripPanel bottomToolStripPanel;

		private ToolStripPanel leftToolStripPanel;

		private ToolStripPanel rightToolStripPanel;

		private ToolStripContentPanel contentToolStripPanel;

		private Control[] panels;

		private IDesignerHost designerHost;

		private ISelectionService selectionSvc;

		private ToolStripContainer toolStripContainer;

		private bool disableDrawGrid;
	}
}
