using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001A9 RID: 425
	internal class ChangeToolStripParentVerb
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x0004A540 File Offset: 0x00049540
		internal ChangeToolStripParentVerb(string text, ToolStripDesigner designer)
		{
			this._designer = designer;
			this._provider = designer.Component.Site;
			this._host = (IDesignerHost)this._provider.GetService(typeof(IDesignerHost));
			this.componentChangeSvc = (IComponentChangeService)this._provider.GetService(typeof(IComponentChangeService));
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0004A5AC File Offset: 0x000495AC
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

		// Token: 0x0600104E RID: 4174 RVA: 0x0004A810 File Offset: 0x00049810
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

		// Token: 0x04001054 RID: 4180
		private ToolStripDesigner _designer;

		// Token: 0x04001055 RID: 4181
		private IDesignerHost _host;

		// Token: 0x04001056 RID: 4182
		private IComponentChangeService componentChangeSvc;

		// Token: 0x04001057 RID: 4183
		private IServiceProvider _provider;
	}
}
