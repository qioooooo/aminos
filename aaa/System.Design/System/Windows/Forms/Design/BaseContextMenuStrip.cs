using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200019B RID: 411
	internal class BaseContextMenuStrip : GroupedContextMenuStrip
	{
		// Token: 0x06000F5A RID: 3930 RVA: 0x000431CD File Offset: 0x000421CD
		public BaseContextMenuStrip(IServiceProvider provider, Component component)
		{
			this.serviceProvider = provider;
			this.component = component;
			this.InitializeContextMenu();
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x000431EC File Offset: 0x000421EC
		private void AddCodeMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.ViewCode, SR.GetString("ContextMenuViewCode"), "viewcode", this.serviceProvider);
			base.Groups["Code"].Items.Add(standardCommandToolStripMenuItem);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00043234 File Offset: 0x00042234
		private void AddZorderMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.BringToFront, SR.GetString("ContextMenuBringToFront"), "bringToFront", this.serviceProvider);
			base.Groups["ZOrder"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.SendToBack, SR.GetString("ContextMenuSendToBack"), "sendToBack", this.serviceProvider);
			base.Groups["ZOrder"].Items.Add(standardCommandToolStripMenuItem);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x000432B8 File Offset: 0x000422B8
		private void AddGridMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.AlignToGrid, SR.GetString("ContextMenuAlignToGrid"), "alignToGrid", this.serviceProvider);
			base.Groups["Grid"].Items.Add(standardCommandToolStripMenuItem);
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00043300 File Offset: 0x00042300
		private void AddLockMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.LockControls, SR.GetString("ContextMenuLockControls"), "lockControls", this.serviceProvider);
			base.Groups["Lock"].Items.Add(standardCommandToolStripMenuItem);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00043348 File Offset: 0x00042348
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

		// Token: 0x06000F60 RID: 3936 RVA: 0x000435BC File Offset: 0x000425BC
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

		// Token: 0x06000F61 RID: 3937 RVA: 0x00043658 File Offset: 0x00042658
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

		// Token: 0x06000F62 RID: 3938 RVA: 0x00043754 File Offset: 0x00042754
		private void AddPropertiesMenuItem()
		{
			StandardCommandToolStripMenuItem standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(StandardCommands.DocumentOutline, SR.GetString("ContextMenuDocumentOutline"), "", this.serviceProvider);
			base.Groups["Properties"].Items.Add(standardCommandToolStripMenuItem);
			standardCommandToolStripMenuItem = new StandardCommandToolStripMenuItem(MenuCommands.DesignerProperties, SR.GetString("ContextMenuProperties"), "properties", this.serviceProvider);
			base.Groups["Properties"].Items.Add(standardCommandToolStripMenuItem);
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x000437D8 File Offset: 0x000427D8
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

		// Token: 0x06000F64 RID: 3940 RVA: 0x000438B8 File Offset: 0x000428B8
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

		// Token: 0x04000FEC RID: 4076
		private IServiceProvider serviceProvider;

		// Token: 0x04000FED RID: 4077
		private Component component;

		// Token: 0x04000FEE RID: 4078
		private ToolStripMenuItem selectionMenuItem;

		// Token: 0x0200019C RID: 412
		private class SelectToolStripMenuItem : ToolStripMenuItem
		{
			// Token: 0x06000F65 RID: 3941 RVA: 0x0004395C File Offset: 0x0004295C
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

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000F66 RID: 3942 RVA: 0x000439F4 File Offset: 0x000429F4
			// (set) Token: 0x06000F67 RID: 3943 RVA: 0x00043A7D File Offset: 0x00042A7D
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

			// Token: 0x06000F68 RID: 3944 RVA: 0x00043A90 File Offset: 0x00042A90
			protected override void OnClick(EventArgs e)
			{
				ISelectionService selectionService = this.serviceProvider.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { this.comp }, SelectionTypes.Replace);
				}
			}

			// Token: 0x04000FEF RID: 4079
			private Component comp;

			// Token: 0x04000FF0 RID: 4080
			private IServiceProvider serviceProvider;

			// Token: 0x04000FF1 RID: 4081
			private Type _itemType;

			// Token: 0x04000FF2 RID: 4082
			private bool _cachedImage;

			// Token: 0x04000FF3 RID: 4083
			private Image _image;

			// Token: 0x04000FF4 RID: 4084
			private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;
		}
	}
}
