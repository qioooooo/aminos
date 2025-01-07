using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ToolStripContainerActionList : DesignerActionList
	{
		public ToolStripContainerActionList(ToolStripContainer control)
			: base(control)
		{
			this.container = control;
			this.provider = this.container.Site;
			this.host = this.provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
		}

		private object GetProperty(Component comp, string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(comp)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(comp);
			}
			return null;
		}

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

		private bool IsDockFilled
		{
			get
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.container)["Dock"];
				return propertyDescriptor == null || (DockStyle)propertyDescriptor.GetValue(this.container) == DockStyle.Fill;
			}
		}

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

		private ToolStripContainer container;

		private IServiceProvider provider;

		private IDesignerHost host;
	}
}
