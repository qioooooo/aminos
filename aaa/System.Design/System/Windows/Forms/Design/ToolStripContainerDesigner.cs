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
	// Token: 0x020002B7 RID: 695
	internal class ToolStripContainerDesigner : ParentControlDesigner
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0008CB83 File Offset: 0x0008BB83
		// (set) Token: 0x06001A00 RID: 6656 RVA: 0x0008CB9A File Offset: 0x0008BB9A
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

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0008CBC3 File Offset: 0x0008BBC3
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x0008CBDA File Offset: 0x0008BBDA
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

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0008CC03 File Offset: 0x0008BC03
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x0008CC1A File Offset: 0x0008BC1A
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

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0008CC43 File Offset: 0x0008BC43
		// (set) Token: 0x06001A06 RID: 6662 RVA: 0x0008CC5A File Offset: 0x0008BC5A
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

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0008CC84 File Offset: 0x0008BC84
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

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0008CCB3 File Offset: 0x0008BCB3
		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0008CCB6 File Offset: 0x0008BCB6
		protected override bool DrawGrid
		{
			get
			{
				return !this.disableDrawGrid && base.DrawGrid;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x0008CCC8 File Offset: 0x0008BCC8
		public override IList SnapLines
		{
			get
			{
				return base.SnapLinesInternal() as ArrayList;
			}
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0008CCE2 File Offset: 0x0008BCE2
		public override int NumberOfInternalControlDesigners()
		{
			return this.panels.Length;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0008CCEC File Offset: 0x0008BCEC
		public override ControlDesigner InternalControlDesigner(int internalControlIndex)
		{
			if (internalControlIndex < this.panels.Length && internalControlIndex >= 0)
			{
				Control control = this.panels[internalControlIndex];
				return this.designerHost.GetDesigner(control) as ControlDesigner;
			}
			return null;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x0008CD24 File Offset: 0x0008BD24
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

		// Token: 0x06001A0E RID: 6670 RVA: 0x0008CDD8 File Offset: 0x0008BDD8
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

		// Token: 0x06001A0F RID: 6671 RVA: 0x0008CE54 File Offset: 0x0008BE54
		public override bool CanParent(Control control)
		{
			return false;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0008CE57 File Offset: 0x0008BE57
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (this.selectionSvc != null)
			{
				this.selectionSvc = null;
			}
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0008CE6F File Offset: 0x0008BE6F
		private ToolStripPanelDesigner GetDesigner(ToolStripPanel panel)
		{
			return this.designerHost.GetDesigner(panel) as ToolStripPanelDesigner;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0008CE82 File Offset: 0x0008BE82
		private PanelDesigner GetDesigner(ToolStripContentPanel panel)
		{
			return this.designerHost.GetDesigner(panel) as PanelDesigner;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0008CE98 File Offset: 0x0008BE98
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

		// Token: 0x06001A14 RID: 6676 RVA: 0x0008CEE0 File Offset: 0x0008BEE0
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

		// Token: 0x06001A15 RID: 6677 RVA: 0x0008D014 File Offset: 0x0008C014
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

		// Token: 0x06001A16 RID: 6678 RVA: 0x0008D054 File Offset: 0x0008C054
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

		// Token: 0x06001A17 RID: 6679 RVA: 0x0008D094 File Offset: 0x0008C094
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

		// Token: 0x06001A18 RID: 6680 RVA: 0x0008D130 File Offset: 0x0008C130
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

		// Token: 0x06001A19 RID: 6681 RVA: 0x0008D17C File Offset: 0x0008C17C
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

		// Token: 0x06001A1A RID: 6682 RVA: 0x0008D435 File Offset: 0x0008C435
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0008D440 File Offset: 0x0008C440
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

		// Token: 0x06001A1C RID: 6684 RVA: 0x0008D478 File Offset: 0x0008C478
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

		// Token: 0x06001A1D RID: 6685 RVA: 0x0008D4FC File Offset: 0x0008C4FC
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

		// Token: 0x040014DA RID: 5338
		private const string topToolStripPanelName = "TopToolStripPanel";

		// Token: 0x040014DB RID: 5339
		private const string bottomToolStripPanelName = "BottomToolStripPanel";

		// Token: 0x040014DC RID: 5340
		private const string leftToolStripPanelName = "LeftToolStripPanel";

		// Token: 0x040014DD RID: 5341
		private const string rightToolStripPanelName = "RightToolStripPanel";

		// Token: 0x040014DE RID: 5342
		private const string contentToolStripPanelName = "ContentPanel";

		// Token: 0x040014DF RID: 5343
		private ToolStripPanel topToolStripPanel;

		// Token: 0x040014E0 RID: 5344
		private ToolStripPanel bottomToolStripPanel;

		// Token: 0x040014E1 RID: 5345
		private ToolStripPanel leftToolStripPanel;

		// Token: 0x040014E2 RID: 5346
		private ToolStripPanel rightToolStripPanel;

		// Token: 0x040014E3 RID: 5347
		private ToolStripContentPanel contentToolStripPanel;

		// Token: 0x040014E4 RID: 5348
		private Control[] panels;

		// Token: 0x040014E5 RID: 5349
		private IDesignerHost designerHost;

		// Token: 0x040014E6 RID: 5350
		private ISelectionService selectionSvc;

		// Token: 0x040014E7 RID: 5351
		private ToolStripContainer toolStripContainer;

		// Token: 0x040014E8 RID: 5352
		private bool disableDrawGrid;
	}
}
