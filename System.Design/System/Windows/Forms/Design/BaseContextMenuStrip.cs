using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class BaseContextMenuStrip : GroupedContextMenuStrip
	{
		public BaseContextMenuStrip(IServiceProvider provider, Component component)
		{
			this.serviceProvider = provider;
			this.component = component;
			this.InitializeContextMenu();
		}

		private void AddCodeMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.ViewCode, SR.GetString("ContextMenuViewCode"), "viewcode", this.serviceProvider);
			base.Groups["Code"].Items.Add(standardCommandToolStripMenuItem);
		}

		private void AddZorderMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.BringToFront, SR.GetString("ContextMenuBringToFront"), "bringToFront", this.serviceProvider);
			base.Groups["ZOrder"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.SendToBack, SR.GetString("ContextMenuSendToBack"), "sendToBack", this.serviceProvider);
			base.Groups["ZOrder"].Items.Add(standardCommandToolStripMenuItem);
		}

		private void AddGridMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.AlignToGrid, SR.GetString("ContextMenuAlignToGrid"), "alignToGrid", this.serviceProvider);
			base.Groups["Grid"].Items.Add(standardCommandToolStripMenuItem);
		}

		private void AddLockMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.LockControls, SR.GetString("ContextMenuLockControls"), "lockControls", this.serviceProvider);
			base.Groups["Lock"].Items.Add(standardCommandToolStripMenuItem);
		}

		private void RefreshSelectionMenuItem()
		{
			int num = -1;
			if (this.selectionMenuItem != null)
			{
				num = this.Items.IndexOf(this.selectionMenuItem);
				base.Groups["Selection"].Items.Remove(this.selectionMenuItem);
				this.Items.Remove(this.selectionMenuItem);
			}
			ArrayList arrayList = new ArrayList();
			int num2 = 0;
			ISelectionService selectionService = this.serviceProvider.GetService(typeof(ISelectionService)) as ISelectionService;
			IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (selectionService != null && designerHost != null)
			{
				IComponent rootComponent = designerHost.RootComponent;
				Control control = selectionService.PrimarySelection as Control;
				if (control != null && rootComponent != null && control != rootComponent)
				{
					for (Control control2 = control.Parent; control2 != null; control2 = control2.Parent)
					{
						if (control2.Site != null)
						{
							arrayList.Add(control2);
							num2++;
						}
						if (control2 == rootComponent)
						{
							break;
						}
					}
				}
				else if (selectionService.PrimarySelection is ToolStripItem)
				{
					ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
					ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(toolStripItem) as ToolStripItemDesigner;
					if (toolStripItemDesigner != null)
					{
						arrayList = toolStripItemDesigner.AddParentTree();
						num2 = arrayList.Count;
					}
				}
			}
			if (num2 > 0)
			{
				this.selectionMenuItem = new ToolStripMenuItem();
				IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
				if (iuiservice != null)
				{
					this.selectionMenuItem.DropDown.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
					this.selectionMenuItem.DropDown.Font = (Font)iuiservice.Styles["DialogFont"];
				}
				this.selectionMenuItem.Text = SR.GetString("ContextMenuSelect");
				foreach (object obj in arrayList)
				{
					Component component = (Component)obj;
					ToolStripMenuItem toolStripMenuItem = new BaseContextMenuStrip.SelectToolStripMenuItem(component, this.serviceProvider);
					this.selectionMenuItem.DropDownItems.Add(toolStripMenuItem);
				}
				base.Groups["Selection"].Items.Add(this.selectionMenuItem);
				if (num != -1)
				{
					this.Items.Insert(num, this.selectionMenuItem);
				}
			}
		}

		private void AddVerbMenuItem()
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)this.serviceProvider.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				DesignerVerbCollection verbs = menuCommandService.Verbs;
				foreach (object obj in verbs)
				{
					DesignerVerb designerVerb = (DesignerVerb)obj;
					DesignerVerbToolStripMenuItem designerVerbToolStripMenuItem = new DesignerVerbToolStripMenuItem(designerVerb);
					base.Groups["Verbs"].Items.Add(designerVerbToolStripMenuItem);
				}
			}
		}

		private void AddEditMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.Cut, SR.GetString("ContextMenuCut"), "cut", this.serviceProvider);
			base.Groups["Edit"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.Copy, SR.GetString("ContextMenuCopy"), "copy", this.serviceProvider);
			base.Groups["Edit"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.Paste, SR.GetString("ContextMenuPaste"), "paste", this.serviceProvider);
			base.Groups["Edit"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.Delete, SR.GetString("ContextMenuDelete"), "delete", this.serviceProvider);
			base.Groups["Edit"].Items.Add(standardCommandToolStripMenuItem);
		}

		private void AddPropertiesMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.DocumentOutline, SR.GetString("ContextMenuDocumentOutline"), "", this.serviceProvider);
			base.Groups["Properties"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(MenuCommands.DesignerProperties, SR.GetString("ContextMenuProperties"), "properties", this.serviceProvider);
			base.Groups["Properties"].Items.Add(standardCommandToolStripMenuItem);
		}

		private void InitializeContextMenu()
		{
			base.Name = "designerContextMenuStrip";
			IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				base.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
			}
			base.GroupOrdering.AddRange(new string[] { "Code", "ZOrder", "Grid", "Lock", "Verbs", "Custom", "Selection", "Edit", "Properties" });
			this.AddCodeMenuItem();
			this.AddZorderMenuItem();
			this.AddGridMenuItem();
			this.AddLockMenuItem();
			this.AddVerbMenuItem();
			this.RefreshSelectionMenuItem();
			this.AddEditMenuItem();
			this.AddPropertiesMenuItem();
		}

		public override void RefreshItems()
		{
			IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				this.Font = (Font)iuiservice.Styles["DialogFont"];
			}
			foreach (object obj in this.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = toolStripItem as StandardCommandToolStripMenuItem;
				if (standardCommandToolStripMenuItem != null)
				{
					standardCommandToolStripMenuItem.RefreshItem();
				}
			}
			this.RefreshSelectionMenuItem();
		}

		private IServiceProvider serviceProvider;

		private Component component;

		private ToolStripMenuItem selectionMenuItem;

		private class SelectToolStripMenuItem : ToolStripMenuItem
		{
			public SelectToolStripMenuItem(Component c, IServiceProvider provider)
			{
				this.comp = c;
				this.serviceProvider = provider;
				string text = null;
				if (this.comp != null)
				{
					ISite site = this.comp.Site;
					if (site != null)
					{
						INestedSite nestedSite = site as INestedSite;
						if (nestedSite != null && !string.IsNullOrEmpty(nestedSite.FullName))
						{
							text = nestedSite.FullName;
						}
						else if (!string.IsNullOrEmpty(site.Name))
						{
							text = site.Name;
						}
					}
				}
				this.Text = SR.GetString("ToolStripSelectMenuItem", new object[] { text });
				this._itemType = c.GetType();
			}

			public override Image Image
			{
				get
				{
					if (!this._cachedImage)
					{
						this._cachedImage = true;
						ToolboxItem toolboxItem = ToolboxService.GetToolboxItem(this._itemType);
						if (toolboxItem != null)
						{
							this._image = toolboxItem.Bitmap;
						}
						else if (this._itemType.Namespace == BaseContextMenuStrip.SelectToolStripMenuItem.systemWindowsFormsNamespace)
						{
							this._image = ToolboxBitmapAttribute.GetImageFromResource(this._itemType, null, false);
						}
						if (this._image == null)
						{
							this._image = ToolboxBitmapAttribute.GetImageFromResource(this.comp.GetType(), null, false);
						}
					}
					return this._image;
				}
				set
				{
					this._image = value;
					this._cachedImage = true;
				}
			}

			protected override void OnClick(EventArgs e)
			{
				ISelectionService selectionService = this.serviceProvider.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { this.comp }, SelectionTypes.Replace);
				}
			}

			private Component comp;

			private IServiceProvider serviceProvider;

			private Type _itemType;

			private bool _cachedImage;

			private Image _image;

			private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;
		}
	}
}
