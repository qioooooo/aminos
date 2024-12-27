using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002B6 RID: 694
	internal class ToolStripContainerActionList : DesignerActionList
	{
		// Token: 0x060019EE RID: 6638 RVA: 0x0008C3AC File Offset: 0x0008B3AC
		public ToolStripContainerActionList(ToolStripContainer control)
			: base(control)
		{
			this.container = control;
			this.provider = this.container.Site;
			this.host = this.provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0008C3F8 File Offset: 0x0008B3F8
		private object GetProperty(Component comp, string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(comp)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(comp);
			}
			return null;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0008C420 File Offset: 0x0008B420
		private void ChangeProperty(Component comp, string propertyName, object value)
		{
			if (this.host != null)
			{
				ToolStripPanel toolStripPanel = comp as ToolStripPanel;
				ToolStripPanelDesigner toolStripPanelDesigner = this.host.GetDesigner(comp) as ToolStripPanelDesigner;
				if (propertyName.Equals("Visible"))
				{
					foreach (object obj in toolStripPanel.Controls)
					{
						Control control = (Control)obj;
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["Visible"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(control, value);
						}
					}
					if (!(bool)value)
					{
						if (toolStripPanel != null)
						{
							toolStripPanel.Padding = new Padding(0);
						}
						if (toolStripPanelDesigner != null && toolStripPanelDesigner.ToolStripPanelSelectorGlyph != null)
						{
							toolStripPanelDesigner.ToolStripPanelSelectorGlyph.IsExpanded = false;
						}
					}
				}
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(comp)[propertyName];
				if (propertyDescriptor2 != null)
				{
					propertyDescriptor2.SetValue(comp, value);
				}
				SelectionManager selectionManager = (SelectionManager)this.provider.GetService(typeof(SelectionManager));
				if (selectionManager != null)
				{
					selectionManager.Refresh();
				}
				if (toolStripPanelDesigner != null)
				{
					toolStripPanelDesigner.InvalidateGlyph();
				}
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x0008C544 File Offset: 0x0008B544
		private bool IsDockFilled
		{
			get
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.container)["Dock"];
				return propertyDescriptor == null || (DockStyle)propertyDescriptor.GetValue(this.container) == DockStyle.Fill;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060019F2 RID: 6642 RVA: 0x0008C584 File Offset: 0x0008B584
		private bool ProvideReparent
		{
			get
			{
				if (this.host != null)
				{
					Control control = this.host.RootComponent as Control;
					if (control != null && this.container.Parent == control && this.IsDockFilled && control.Controls.Count > 1)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0008C5D4 File Offset: 0x0008B5D4
		public void SetDockToForm()
		{
			if (this.host != null)
			{
				Control control = this.host.RootComponent as Control;
				if (control != null && this.container.Parent != control)
				{
					control.Controls.Add(this.container);
				}
				if (!this.IsDockFilled)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.container)["Dock"];
					if (propertyDescriptor != null)
					{
						propertyDescriptor.SetValue(this.container, DockStyle.Fill);
					}
				}
			}
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0008C650 File Offset: 0x0008B650
		public void ReparentControls()
		{
			if (this.host != null)
			{
				Control control = this.host.RootComponent as Control;
				if (control != null && this.container.Parent == control && control.Controls.Count > 1)
				{
					Control control2 = this.container.ContentPanel;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control2)["AutoScroll"];
					if (propertyDescriptor != null)
					{
						propertyDescriptor.SetValue(control2, true);
					}
					DesignerTransaction designerTransaction = this.host.CreateTransaction("Reparent Transaction");
					try
					{
						Control[] array = new Control[control.Controls.Count];
						control.Controls.CopyTo(array, 0);
						foreach (Control control3 in array)
						{
							if (control3 != this.container && !(control3 is MdiClient))
							{
								InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(control3)[typeof(InheritanceAttribute)];
								if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
								{
									IComponentChangeService componentChangeService = this.provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
									if (control3 is ToolStrip)
									{
										control2 = this.GetParent(control3);
									}
									else
									{
										control2 = this.container.ContentPanel;
									}
									PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control2)["Controls"];
									Control parent = control3.Parent;
									if (parent != null)
									{
										if (componentChangeService != null)
										{
											componentChangeService.OnComponentChanging(parent, propertyDescriptor2);
										}
										parent.Controls.Remove(control3);
									}
									if (componentChangeService != null)
									{
										componentChangeService.OnComponentChanging(control2, propertyDescriptor2);
									}
									control2.Controls.Add(control3);
									if (componentChangeService != null && parent != null)
									{
										componentChangeService.OnComponentChanged(parent, propertyDescriptor2, null, null);
									}
									if (componentChangeService != null)
									{
										componentChangeService.OnComponentChanged(control2, propertyDescriptor2, null, null);
									}
								}
							}
						}
					}
					catch
					{
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
							designerTransaction = null;
						}
					}
					finally
					{
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
							designerTransaction = null;
						}
						ISelectionService selectionService = this.provider.GetService(typeof(ISelectionService)) as ISelectionService;
						if (selectionService != null)
						{
							selectionService.SetSelectedComponents(new IComponent[] { control2 });
						}
					}
				}
			}
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0008C8B0 File Offset: 0x0008B8B0
		private Control GetParent(Control c)
		{
			Control control = this.container.ContentPanel;
			DockStyle dock = c.Dock;
			foreach (object obj in this.container.Controls)
			{
				Control control2 = (Control)obj;
				if (control2 is ToolStripPanel && control2.Dock == dock)
				{
					control = control2;
					break;
				}
			}
			return control;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0008C934 File Offset: 0x0008B934
		// (set) Token: 0x060019F7 RID: 6647 RVA: 0x0008C94C File Offset: 0x0008B94C
		public bool TopVisible
		{
			get
			{
				return (bool)this.GetProperty(this.container, "TopToolStripPanelVisible");
			}
			set
			{
				if (value != this.TopVisible)
				{
					this.ChangeProperty(this.container, "TopToolStripPanelVisible", value);
				}
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x0008C96E File Offset: 0x0008B96E
		// (set) Token: 0x060019F9 RID: 6649 RVA: 0x0008C986 File Offset: 0x0008B986
		public bool BottomVisible
		{
			get
			{
				return (bool)this.GetProperty(this.container, "BottomToolStripPanelVisible");
			}
			set
			{
				if (value != this.BottomVisible)
				{
					this.ChangeProperty(this.container, "BottomToolStripPanelVisible", value);
				}
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x0008C9A8 File Offset: 0x0008B9A8
		// (set) Token: 0x060019FB RID: 6651 RVA: 0x0008C9C0 File Offset: 0x0008B9C0
		public bool LeftVisible
		{
			get
			{
				return (bool)this.GetProperty(this.container, "LeftToolStripPanelVisible");
			}
			set
			{
				if (value != this.LeftVisible)
				{
					this.ChangeProperty(this.container, "LeftToolStripPanelVisible", value);
				}
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0008C9E2 File Offset: 0x0008B9E2
		// (set) Token: 0x060019FD RID: 6653 RVA: 0x0008C9FA File Offset: 0x0008B9FA
		public bool RightVisible
		{
			get
			{
				return (bool)this.GetProperty(this.container, "RightToolStripPanelVisible");
			}
			set
			{
				if (value != this.RightVisible)
				{
					this.ChangeProperty(this.container, "RightToolStripPanelVisible", value);
				}
			}
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0008CA1C File Offset: 0x0008BA1C
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			designerActionItemCollection.Add(new DesignerActionHeaderItem(SR.GetString("ToolStripContainerActionList_Visible"), SR.GetString("ToolStripContainerActionList_Show")));
			designerActionItemCollection.Add(new DesignerActionPropertyItem("TopVisible", SR.GetString("ToolStripContainerActionList_Top"), SR.GetString("ToolStripContainerActionList_Show"), SR.GetString("ToolStripContainerActionList_TopDesc")));
			designerActionItemCollection.Add(new DesignerActionPropertyItem("BottomVisible", SR.GetString("ToolStripContainerActionList_Bottom"), SR.GetString("ToolStripContainerActionList_Show"), SR.GetString("ToolStripContainerActionList_BottomDesc")));
			designerActionItemCollection.Add(new DesignerActionPropertyItem("LeftVisible", SR.GetString("ToolStripContainerActionList_Left"), SR.GetString("ToolStripContainerActionList_Show"), SR.GetString("ToolStripContainerActionList_LeftDesc")));
			designerActionItemCollection.Add(new DesignerActionPropertyItem("RightVisible", SR.GetString("ToolStripContainerActionList_Right"), SR.GetString("ToolStripContainerActionList_Show"), SR.GetString("ToolStripContainerActionList_RightDesc")));
			if (!this.IsDockFilled)
			{
				bool flag = true;
				if (this.host != null)
				{
					Control control = this.host.RootComponent as UserControl;
					if (control != null)
					{
						flag = false;
					}
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "SetDockToForm", flag ? SR.GetString("DesignerShortcutDockInForm") : SR.GetString("DesignerShortcutDockInUserControl")));
			}
			if (this.ProvideReparent)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "ReparentControls", SR.GetString("DesignerShortcutReparentControls")));
			}
			return designerActionItemCollection;
		}

		// Token: 0x040014D7 RID: 5335
		private ToolStripContainer container;

		// Token: 0x040014D8 RID: 5336
		private IServiceProvider provider;

		// Token: 0x040014D9 RID: 5337
		private IDesignerHost host;
	}
}
