using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class ChangeToolStripParentVerb
	{
		internal ChangeToolStripParentVerb(string text, ToolStripDesigner designer)
		{
			this._designer = designer;
			this._provider = designer.Component.Site;
			this._host = (IDesignerHost)this._provider.GetService(typeof(IDesignerHost));
			this.componentChangeSvc = (IComponentChangeService)this._provider.GetService(typeof(IComponentChangeService));
		}

		public void ChangeParent()
		{
			Cursor cursor = Cursor.Current;
			DesignerTransaction designerTransaction = this._host.CreateTransaction("Add ToolStripContainer Transaction");
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Control control = this._host.RootComponent as Control;
				ParentControlDesigner parentControlDesigner = this._host.GetDesigner(control) as ParentControlDesigner;
				if (parentControlDesigner != null)
				{
					ToolStrip toolStrip = this._designer.Component as ToolStrip;
					if (toolStrip != null && this._designer != null && this._designer.Component != null && this._provider != null)
					{
						DesignerActionUIService designerActionUIService = this._provider.GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
						designerActionUIService.HideUI(toolStrip);
					}
					ToolboxItem toolboxItem = new ToolboxItem(typeof(ToolStripContainer));
					OleDragDropHandler oleDragHandler = parentControlDesigner.GetOleDragHandler();
					if (oleDragHandler != null)
					{
						IComponent[] array = oleDragHandler.CreateTool(toolboxItem, control, 0, 0, 0, 0, false, false);
						ToolStripContainer toolStripContainer = array[0] as ToolStripContainer;
						if (toolStripContainer != null && toolStrip != null)
						{
							IComponentChangeService componentChangeService = this._provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							Control parent = this.GetParent(toolStripContainer, toolStrip);
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parent)["Controls"];
							Control parent2 = toolStrip.Parent;
							if (parent2 != null)
							{
								componentChangeService.OnComponentChanging(parent2, propertyDescriptor);
								parent2.Controls.Remove(toolStrip);
							}
							if (parent != null)
							{
								componentChangeService.OnComponentChanging(parent, propertyDescriptor);
								parent.Controls.Add(toolStrip);
							}
							if (componentChangeService != null && parent2 != null && parent != null)
							{
								componentChangeService.OnComponentChanged(parent2, propertyDescriptor, null, null);
								componentChangeService.OnComponentChanged(parent, propertyDescriptor, null, null);
							}
							ISelectionService selectionService = this._provider.GetService(typeof(ISelectionService)) as ISelectionService;
							if (selectionService != null)
							{
								selectionService.SetSelectedComponents(new IComponent[] { toolStripContainer });
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is InvalidOperationException)
				{
					IUIService iuiservice = (IUIService)this._provider.GetService(typeof(IUIService));
					iuiservice.ShowError(ex.Message);
				}
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
				Cursor.Current = cursor;
			}
		}

		private Control GetParent(ToolStripContainer container, Control c)
		{
			Control control = container.ContentPanel;
			DockStyle dockStyle = c.Dock;
			if (c.Parent is ToolStripPanel)
			{
				dockStyle = c.Parent.Dock;
			}
			foreach (object obj in container.Controls)
			{
				Control control2 = (Control)obj;
				if (control2 is ToolStripPanel && control2.Dock == dockStyle)
				{
					control = control2;
					break;
				}
			}
			return control;
		}

		private ToolStripDesigner _designer;

		private IDesignerHost _host;

		private IComponentChangeService componentChangeSvc;

		private IServiceProvider _provider;
	}
}
