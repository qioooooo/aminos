using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CC RID: 972
	internal abstract partial class TaskFormBase : DesignerForm
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x000BF53E File Offset: 0x000BE53E
		public TaskFormBase(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.InitializeComponent();
			this.InitializeUI();
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000BF553 File Offset: 0x000BE553
		protected Label CaptionLabel
		{
			get
			{
				return this._captionLabel;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000BF55B File Offset: 0x000BE55B
		// (set) Token: 0x060023B1 RID: 9137 RVA: 0x000BF568 File Offset: 0x000BE568
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

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x000BF576 File Offset: 0x000BE576
		protected Panel TaskPanel
		{
			get
			{
				return this._taskPanel;
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000BF864 File Offset: 0x000BE864
		private void InitializeUI()
		{
			this.UpdateFonts();
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000BF86C File Offset: 0x000BE86C
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000BF87B File Offset: 0x000BE87B
		private void UpdateFonts()
		{
			this._captionLabel.Font = new Font(this.Font.FontFamily, this.Font.Size + 2f, FontStyle.Bold, this.Font.Unit);
		}
	}
}
