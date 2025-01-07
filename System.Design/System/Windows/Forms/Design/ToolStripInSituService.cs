using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class ToolStripInSituService : ISupportInSituService, IDisposable
	{
		public ToolStripInSituService(IServiceProvider provider)
		{
			this.sp = provider;
			this.designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
			if (this.designerHost != null)
			{
				this.designerHost.AddService(typeof(ISupportInSituService), this);
			}
			this.componentChangeSvc = (IComponentChangeService)this.designerHost.GetService(typeof(IComponentChangeService));
			if (this.componentChangeSvc != null)
			{
				this.componentChangeSvc.ComponentRemoved += this.OnComponentRemoved;
			}
		}

		public void Dispose()
		{
			if (this.toolDesigner != null)
			{
				this.toolDesigner.Dispose();
				this.toolDesigner = null;
			}
			if (this.toolItemDesigner != null)
			{
				this.toolItemDesigner.Dispose();
				this.toolItemDesigner = null;
			}
			if (this.componentChangeSvc != null)
			{
				this.componentChangeSvc.ComponentRemoved -= this.OnComponentRemoved;
				this.componentChangeSvc = null;
			}
		}

		private ToolStripKeyboardHandlingService ToolStripKeyBoardService
		{
			get
			{
				if (this.toolStripKeyBoardService == null)
				{
					this.toolStripKeyBoardService = (ToolStripKeyboardHandlingService)this.sp.GetService(typeof(ToolStripKeyboardHandlingService));
				}
				return this.toolStripKeyBoardService;
			}
		}

		public bool IgnoreMessages
		{
			get
			{
				ISelectionService selectionService = (ISelectionService)this.sp.GetService(typeof(ISelectionService));
				IDesignerHost designerHost = (IDesignerHost)this.sp.GetService(typeof(IDesignerHost));
				if (selectionService != null && designerHost != null)
				{
					IComponent component = selectionService.PrimarySelection as IComponent;
					if (component == null)
					{
						component = (IComponent)this.ToolStripKeyBoardService.SelectedDesignerControl;
					}
					if (component != null)
					{
						DesignerToolStripControlHost designerToolStripControlHost = component as DesignerToolStripControlHost;
						if (designerToolStripControlHost != null)
						{
							ToolStripDropDown toolStripDropDown = designerToolStripControlHost.GetCurrentParent() as ToolStripDropDown;
							if (toolStripDropDown != null)
							{
								ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
								if (toolStripDropDownItem != null)
								{
									ToolStripOverflowButton toolStripOverflowButton = toolStripDropDownItem as ToolStripOverflowButton;
									if (toolStripOverflowButton != null)
									{
										return false;
									}
									this.toolItemDesigner = designerHost.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
									if (this.toolItemDesigner != null)
									{
										this.toolDesigner = null;
										return true;
									}
								}
							}
							else
							{
								MenuStrip menuStrip = designerToolStripControlHost.GetCurrentParent() as MenuStrip;
								if (menuStrip != null)
								{
									this.toolDesigner = designerHost.GetDesigner(menuStrip) as ToolStripDesigner;
									if (this.toolDesigner != null)
									{
										this.toolItemDesigner = null;
										return true;
									}
								}
							}
						}
						else if (component is ToolStripDropDown)
						{
							ToolStripDropDownDesigner toolStripDropDownDesigner = designerHost.GetDesigner(component) as ToolStripDropDownDesigner;
							if (toolStripDropDownDesigner != null)
							{
								ToolStripMenuItem designerMenuItem = toolStripDropDownDesigner.DesignerMenuItem;
								if (designerMenuItem != null)
								{
									this.toolItemDesigner = designerHost.GetDesigner(designerMenuItem) as ToolStripItemDesigner;
									if (this.toolItemDesigner != null)
									{
										this.toolDesigner = null;
										return true;
									}
								}
							}
						}
						else if (component is MenuStrip)
						{
							this.toolDesigner = designerHost.GetDesigner(component) as ToolStripDesigner;
							if (this.toolDesigner != null)
							{
								this.toolItemDesigner = null;
								return true;
							}
						}
						else if (component is ToolStripMenuItem)
						{
							this.toolItemDesigner = designerHost.GetDesigner(component) as ToolStripItemDesigner;
							if (this.toolItemDesigner != null)
							{
								this.toolDesigner = null;
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		public void HandleKeyChar()
		{
			if (this.toolDesigner != null || this.toolItemDesigner != null)
			{
				if (this.toolDesigner != null)
				{
					this.toolDesigner.ShowEditNode(false);
					return;
				}
				if (this.toolItemDesigner != null)
				{
					ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.toolItemDesigner as ToolStripMenuItemDesigner;
					if (toolStripMenuItemDesigner != null)
					{
						ISelectionService selectionService = (ISelectionService)this.sp.GetService(typeof(ISelectionService));
						if (selectionService != null)
						{
							object obj = selectionService.PrimarySelection;
							if (obj == null)
							{
								obj = this.ToolStripKeyBoardService.SelectedDesignerControl;
							}
							if (obj is DesignerToolStripControlHost || obj is ToolStripDropDown)
							{
								toolStripMenuItemDesigner.EditTemplateNode(false);
								return;
							}
							toolStripMenuItemDesigner.ShowEditNode(false);
							return;
						}
					}
					else
					{
						this.toolItemDesigner.ShowEditNode(false);
					}
				}
			}
		}

		public IntPtr GetEditWindow()
		{
			IntPtr intPtr = IntPtr.Zero;
			if (this.toolDesigner != null && this.toolDesigner.Editor != null && this.toolDesigner.Editor.EditBox != null)
			{
				intPtr = (this.toolDesigner.Editor.EditBox.Visible ? this.toolDesigner.Editor.EditBox.Handle : intPtr);
			}
			else if (this.toolItemDesigner != null && this.toolItemDesigner.Editor != null && this.toolItemDesigner.Editor.EditBox != null)
			{
				intPtr = (this.toolItemDesigner.Editor.EditBox.Visible ? this.toolItemDesigner.Editor.EditBox.Handle : intPtr);
			}
			return intPtr;
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			bool flag = false;
			ComponentCollection components = this.designerHost.Container.Components;
			foreach (object obj in components)
			{
				IComponent component = (IComponent)obj;
				if (component is ToolStrip)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				ToolStripInSituService toolStripInSituService = (ToolStripInSituService)this.sp.GetService(typeof(ISupportInSituService));
				if (toolStripInSituService != null)
				{
					this.designerHost.RemoveService(typeof(ISupportInSituService));
				}
			}
		}

		private IServiceProvider sp;

		private IDesignerHost designerHost;

		private IComponentChangeService componentChangeSvc;

		private ToolStripDesigner toolDesigner;

		private ToolStripItemDesigner toolItemDesigner;

		private ToolStripKeyboardHandlingService toolStripKeyBoardService;
	}
}
