using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	internal abstract partial class TaskFormBase : DesignerForm
	{
		public TaskFormBase(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.InitializeComponent();
			this.InitializeUI();
		}

		protected Label CaptionLabel
		{
			get
			{
				return this._captionLabel;
			}
		}

		public Image Glyph
		{
			get
			{
				return this._glyphPictureBox.Image;
			}
			set
			{
				this._glyphPictureBox.Image = value;
			}
		}

		protected Panel TaskPanel
		{
			get
			{
				return this._taskPanel;
			}
		}

		private void InitializeUI()
		{
			this.UpdateFonts();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		private void UpdateFonts()
		{
			this._captionLabel.Font = new Font(this.Font.FontFamily, this.Font.Size + 2f, FontStyle.Bold, this.Font.Unit);
		}
	}
}
