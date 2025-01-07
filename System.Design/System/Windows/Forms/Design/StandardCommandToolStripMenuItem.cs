using System;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class StandardCommandToolStripMenuItem : ToolStripMenuItem
	{
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

		public void RefreshItem()
		{
			if (this.menuCommand != null)
			{
				base.Visible = this.menuCommand.Visible;
				this.Enabled = this.menuCommand.Enabled;
				base.Checked = this.menuCommand.Checked;
			}
		}

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

		private bool _cachedImage;

		private Image _image;

		private CommandID menuID;

		private IMenuCommandService menuCommandService;

		private IServiceProvider serviceProvider;

		private string name;

		private MenuCommand menuCommand;
	}
}
