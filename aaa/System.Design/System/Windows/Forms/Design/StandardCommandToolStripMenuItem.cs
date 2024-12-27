using System;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000292 RID: 658
	internal class StandardCommandToolStripMenuItem : ToolStripMenuItem
	{
		// Token: 0x06001876 RID: 6262 RVA: 0x0008070C File Offset: 0x0007F70C
		public StandardCommandToolStripMenuItem(CommandID menuID, string text, string imageName, IServiceProvider serviceProvider)
		{
			this.menuID = menuID;
			this.serviceProvider = serviceProvider;
			try
			{
				this.menuCommand = this.MenuService.FindCommand(menuID);
			}
			catch
			{
				this.Enabled = false;
			}
			this.Text = text;
			this.name = imageName;
			this.RefreshItem();
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00080770 File Offset: 0x0007F770
		public void RefreshItem()
		{
			if (this.menuCommand != null)
			{
				base.Visible = this.menuCommand.Visible;
				this.Enabled = this.menuCommand.Enabled;
				base.Checked = this.menuCommand.Checked;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001878 RID: 6264 RVA: 0x000807AD File Offset: 0x0007F7AD
		public IMenuCommandService MenuService
		{
			get
			{
				if (this.menuCommandService == null)
				{
					this.menuCommandService = (IMenuCommandService)this.serviceProvider.GetService(typeof(IMenuCommandService));
				}
				return this.menuCommandService;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001879 RID: 6265 RVA: 0x000807E0 File Offset: 0x0007F7E0
		// (set) Token: 0x0600187A RID: 6266 RVA: 0x00080868 File Offset: 0x0007F868
		public override Image Image
		{
			get
			{
				if (!this._cachedImage)
				{
					this._cachedImage = true;
					try
					{
						if (this.name != null)
						{
							this._image = new Bitmap(typeof(ToolStripMenuItem), this.name + ".bmp");
						}
						base.ImageTransparentColor = Color.Magenta;
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					catch
					{
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

		// Token: 0x0600187B RID: 6267 RVA: 0x00080878 File Offset: 0x0007F878
		protected override void OnClick(EventArgs e)
		{
			if (this.menuCommand != null)
			{
				this.menuCommand.Invoke();
				return;
			}
			if (this.MenuService == null || this.MenuService.GlobalInvoke(this.menuID))
			{
			}
		}

		// Token: 0x04001429 RID: 5161
		private bool _cachedImage;

		// Token: 0x0400142A RID: 5162
		private Image _image;

		// Token: 0x0400142B RID: 5163
		private CommandID menuID;

		// Token: 0x0400142C RID: 5164
		private IMenuCommandService menuCommandService;

		// Token: 0x0400142D RID: 5165
		private IServiceProvider serviceProvider;

		// Token: 0x0400142E RID: 5166
		private string name;

		// Token: 0x0400142F RID: 5167
		private MenuCommand menuCommand;
	}
}
