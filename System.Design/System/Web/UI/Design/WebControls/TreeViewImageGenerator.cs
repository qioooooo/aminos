using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	internal partial class TreeViewImageGenerator : DesignerForm
	{
		public TreeViewImageGenerator(global::System.Web.UI.WebControls.TreeView treeView)
			: base(treeView.Site)
		{
			this._previewPictureBox = new PictureBox();
			this._previewLabel = new global::System.Windows.Forms.Label();
			this._previewPanel = new global::System.Windows.Forms.Panel();
			this._previewFrameTextBox = new global::System.Windows.Forms.TextBox();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._folderNameLabel = new global::System.Windows.Forms.Label();
			this._folderNameTextBox = new global::System.Windows.Forms.TextBox();
			this._propertiesLabel = new global::System.Windows.Forms.Label();
			this._propertyGrid = new VsPropertyGrid(base.ServiceProvider);
			this._progressBar = new ProgressBar();
			this._progressBarLabel = new global::System.Windows.Forms.Label();
			this._previewPanel.SuspendLayout();
			base.SuspendLayout();
			this._previewPictureBox.Name = "_previewPictureBox";
			this._previewPictureBox.SizeMode = PictureBoxSizeMode.Normal;
			this._previewPictureBox.TabIndex = 10;
			this._previewPictureBox.TabStop = false;
			this._previewPictureBox.BackColor = Color.White;
			this._previewLabel.Location = new Point(12, 12);
			this._previewLabel.Name = "_previewLabel";
			this._previewLabel.Size = new Size(180, 14);
			this._previewLabel.TabIndex = 9;
			this._previewLabel.Text = SR.GetString("TreeViewImageGenerator_Preview");
			this._previewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._previewPanel.AutoScroll = true;
			this._previewPanel.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this._previewPanel.Controls.AddRange(new Control[] { this._previewPictureBox });
			this._previewPanel.Location = new Point(13, 29);
			this._previewPanel.Name = "_previewPanel";
			this._previewPanel.Size = new Size(178, 242);
			this._previewPanel.TabIndex = 11;
			this._previewFrameTextBox.Multiline = true;
			this._previewFrameTextBox.Enabled = false;
			this._previewFrameTextBox.TabStop = false;
			this._previewFrameTextBox.Location = new Point(12, 28);
			this._previewFrameTextBox.Size = new Size(180, 244);
			this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this._okButton.FlatStyle = FlatStyle.System;
			this._okButton.Location = new Point(376, 324);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new Size(75, 23);
			this._okButton.TabIndex = 20;
			this._okButton.Text = SR.GetString("OKCaption");
			this._okButton.Click += this.OnOKButtonClick;
			this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this._cancelButton.FlatStyle = FlatStyle.System;
			this._cancelButton.Location = new Point(456, 324);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new Size(75, 23);
			this._cancelButton.TabIndex = 21;
			this._cancelButton.Text = SR.GetString("CancelCaption");
			this._cancelButton.Click += this.OnCancelButtonClick;
			this._folderNameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this._folderNameLabel.Location = new Point(213, 279);
			this._folderNameLabel.Name = "_folderNameLabel";
			this._folderNameLabel.Size = new Size(315, 14);
			this._folderNameLabel.TabIndex = 17;
			this._folderNameLabel.Text = SR.GetString("TreeViewImageGenerator_FolderName");
			this._folderNameTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._folderNameTextBox.Location = new Point(213, 295);
			this._folderNameTextBox.Name = "_folderNameTextBox";
			this._folderNameTextBox.Size = new Size(315, 20);
			this._folderNameTextBox.TabIndex = 18;
			this._folderNameTextBox.Text = SR.GetString("TreeViewImageGenerator_DefaultFolderName");
			this._folderNameTextBox.WordWrap = false;
			this._folderNameTextBox.TextChanged += this.OnFolderNameTextBoxTextChanged;
			this._progressBarLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this._progressBarLabel.Location = new Point(12, 279);
			this._progressBarLabel.Name = "_progressBarLabel";
			this._progressBarLabel.Size = new Size(180, 14);
			this._progressBarLabel.Text = SR.GetString("TreeViewImageGenerator_ProgressBarName");
			this._progressBarLabel.Visible = false;
			this._progressBar.Location = new Point(12, 295);
			this._progressBar.Size = new Size(180, 16);
			this._progressBar.Maximum = 16;
			this._progressBar.Minimum = 0;
			this._progressBar.Visible = false;
			this._propertiesLabel.Location = new Point(213, 12);
			this._propertiesLabel.Name = "_propertiesLabel";
			this._propertiesLabel.Size = new Size(315, 14);
			this._propertiesLabel.TabIndex = 12;
			this._propertiesLabel.Text = SR.GetString("TreeViewImageGenerator_Properties");
			this._propertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			this._propertyGrid.CommandsVisibleIfAvailable = true;
			this._propertyGrid.LargeButtons = false;
			this._propertyGrid.LineColor = SystemColors.ScrollBar;
			this._propertyGrid.Location = new Point(213, 28);
			this._propertyGrid.Name = "_propertyGrid";
			this._propertyGrid.PropertySort = PropertySort.Alphabetical;
			this._propertyGrid.Size = new Size(315, 244);
			this._propertyGrid.TabIndex = 13;
			this._propertyGrid.ToolbarVisible = true;
			this._propertyGrid.ViewBackColor = SystemColors.Window;
			this._propertyGrid.ViewForeColor = SystemColors.WindowText;
			this._propertyGrid.PropertyValueChanged += this.OnPropertyGridPropertyValueChanged;
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new Size(540, 359);
			base.Controls.AddRange(new Control[]
			{
				this._propertyGrid, this._propertiesLabel, this._progressBar, this._progressBarLabel, this._folderNameTextBox, this._folderNameLabel, this._cancelButton, this._okButton, this._previewPanel, this._previewLabel,
				this._previewFrameTextBox
			});
			this.MinimumSize = new Size(540, 359);
			base.Name = "TreeLineImageGenerator";
			this.Text = SR.GetString("TreeViewImageGenerator_Title");
			base.Resize += this.OnFormResize;
			this._previewPanel.ResumeLayout(false);
			base.InitializeForm();
			base.ResumeLayout(false);
			this._imageInfo = new TreeViewImageGenerator.LineImageInfo();
			this._propertyGrid.SelectedObject = this._imageInfo;
			this._treeView = treeView;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = null;
			this.UpdatePreview();
		}

		private global::System.Drawing.Image DefaultMinusImage
		{
			get
			{
				if (TreeViewImageGenerator.defaultMinusImage == null)
				{
					TreeViewImageGenerator.defaultMinusImage = new Bitmap(typeof(TreeViewImageGenerator), "Minus.gif");
				}
				return TreeViewImageGenerator.defaultMinusImage;
			}
		}

		private global::System.Drawing.Image DefaultPlusImage
		{
			get
			{
				if (TreeViewImageGenerator.defaultPlusImage == null)
				{
					TreeViewImageGenerator.defaultPlusImage = new Bitmap(typeof(TreeViewImageGenerator), "Plus.gif");
				}
				return TreeViewImageGenerator.defaultPlusImage;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.TreeView.ImageGenerator";
			}
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.Close();
		}

		private void OnFormResize(object sender, EventArgs e)
		{
			this.UpdatePreview();
		}

		private void OnOKButtonClick(object sender, EventArgs e)
		{
			string text = this._folderNameTextBox.Text.Trim();
			if (text.Length == 0)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_MissingFolderName"));
				return;
			}
			if (text.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidFolderName", new object[] { text }));
				return;
			}
			IWebApplication webApplication = (IWebApplication)this._treeView.Site.GetService(typeof(IWebApplication));
			if (webApplication == null)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_ErrorWriting"));
				return;
			}
			IFolderProjectItem folderProjectItem = (IFolderProjectItem)webApplication.RootProjectItem;
			IProjectItem projectItemFromUrl = webApplication.GetProjectItemFromUrl(Path.Combine("~/", text));
			if (projectItemFromUrl != null && !(projectItemFromUrl is IFolderProjectItem))
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_DocumentExists", new object[] { text }));
				return;
			}
			IFolderProjectItem folderProjectItem2 = (IFolderProjectItem)projectItemFromUrl;
			if (folderProjectItem2 != null)
			{
				goto IL_015A;
			}
			if (UIServiceHelper.ShowMessage(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_NonExistentFolderName", new object[] { text }), SR.GetString("TreeViewImageGenerator_Title"), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				try
				{
					folderProjectItem2 = folderProjectItem.AddFolder(text);
					goto IL_015A;
				}
				catch
				{
					UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_ErrorCreatingFolder", new object[] { text }));
				}
			}
			return;
			IL_015A:
			global::System.Drawing.Image expandImage = this._imageInfo.ExpandImage;
			if (expandImage == null)
			{
				expandImage = this.DefaultPlusImage;
			}
			global::System.Drawing.Image collapseImage = this._imageInfo.CollapseImage;
			if (collapseImage == null)
			{
				collapseImage = this.DefaultMinusImage;
			}
			global::System.Drawing.Image noExpandImage = this._imageInfo.NoExpandImage;
			int width = this._imageInfo.Width;
			if (width < 1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidValue", new object[] { "Width" }));
				return;
			}
			int height = this._imageInfo.Height;
			if (height < 1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidValue", new object[] { "Height" }));
				return;
			}
			int lineWidth = this._imageInfo.LineWidth;
			if (lineWidth < 1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidValue", new object[] { "LineWidth" }));
				return;
			}
			int lineStyle = (int)this._imageInfo.LineStyle;
			Color lineColor = this._imageInfo.LineColor;
			this._progressBar.Value = 0;
			this._progressBar.Visible = true;
			this._progressBarLabel.Visible = true;
			try
			{
				bool flag = false;
				bool flag2 = false;
				Bitmap bitmap = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.FillRectangle(new SolidBrush(this._imageInfo.TransparentColor), 0, 0, width, height);
				this.RenderImage(graphics, 0, 0, width, height, 'i', lineStyle, lineWidth, lineColor, null);
				flag2 |= this.SaveTransparentGif(bitmap, folderProjectItem2, "i.gif", ref flag);
				this._progressBar.Value++;
				string text2 = "-rtl ";
				for (int i = 0; i < text2.Length; i++)
				{
					bitmap = new Bitmap(width, height);
					graphics = Graphics.FromImage(bitmap);
					graphics.FillRectangle(new SolidBrush(this._imageInfo.TransparentColor), 0, 0, width, height);
					this.RenderImage(graphics, 0, 0, width, height, text2[i], lineStyle, lineWidth, lineColor, collapseImage);
					graphics.Dispose();
					string text3 = "minus.gif";
					if (text2[i] == '-')
					{
						text3 = "dash" + text3;
					}
					else if (text2[i] != ' ')
					{
						text3 = text2[i] + text3;
					}
					flag2 |= this.SaveTransparentGif(bitmap, folderProjectItem2, text3, ref flag);
					this._progressBar.Value++;
				}
				for (int j = 0; j < text2.Length; j++)
				{
					bitmap = new Bitmap(width, height);
					graphics = Graphics.FromImage(bitmap);
					graphics.FillRectangle(new SolidBrush(this._imageInfo.TransparentColor), 0, 0, width, height);
					this.RenderImage(graphics, 0, 0, width, height, text2[j], lineStyle, lineWidth, lineColor, expandImage);
					graphics.Dispose();
					string text3 = "plus.gif";
					if (text2[j] == '-')
					{
						text3 = "dash" + text3;
					}
					else if (text2[j] != ' ')
					{
						text3 = text2[j] + text3;
					}
					flag2 |= this.SaveTransparentGif(bitmap, folderProjectItem2, text3, ref flag);
					this._progressBar.Value++;
				}
				for (int k = 0; k < text2.Length; k++)
				{
					bitmap = new Bitmap(width, height);
					graphics = Graphics.FromImage(bitmap);
					graphics.FillRectangle(new SolidBrush(this._imageInfo.TransparentColor), 0, 0, width, height);
					this.RenderImage(graphics, 0, 0, width, height, text2[k], lineStyle, lineWidth, lineColor, noExpandImage);
					graphics.Dispose();
					string text3 = ".gif";
					if (text2[k] == '-')
					{
						text3 = "dash" + text3;
					}
					else if (text2[k] == ' ')
					{
						text3 = "noexpand" + text3;
					}
					else
					{
						text3 = text2[k] + text3;
					}
					flag2 |= this.SaveTransparentGif(bitmap, folderProjectItem2, text3, ref flag);
					this._progressBar.Value++;
				}
				this._progressBar.Visible = false;
				this._progressBarLabel.Visible = false;
				if (flag2)
				{
					UIServiceHelper.ShowMessage(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_LineImagesGenerated", new object[] { text }));
				}
			}
			catch
			{
				this._progressBar.Visible = false;
				this._progressBarLabel.Visible = false;
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_ErrorWriting", new object[] { text }));
				return;
			}
			this._treeView.LineImagesFolder = "~/" + text;
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnFolderNameTextBoxTextChanged(object sender, EventArgs e)
		{
			if (this._folderNameTextBox.Text.Trim().Length > 0)
			{
				this._okButton.Enabled = true;
				return;
			}
			this._okButton.Enabled = false;
		}

		private void OnPropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.UpdatePreview();
		}

		private void RenderImage(Graphics g, int x, int y, int width, int height, char lineType, int lineStyle, int lineWidth, Color lineColor, global::System.Drawing.Image image)
		{
			Pen pen = new Pen(lineColor, (float)lineWidth);
			switch (lineStyle)
			{
			case 0:
				pen.DashStyle = DashStyle.Dot;
				break;
			case 1:
				pen.DashStyle = DashStyle.Dash;
				break;
			default:
				pen.DashStyle = DashStyle.Solid;
				break;
			}
			if (lineType == 'i')
			{
				g.DrawLine(pen, x + width / 2, y, x + width / 2, y + height);
			}
			else if (lineType == 'r')
			{
				g.DrawLine(pen, x + width / 2, y + height / 2, x + width, y + height / 2);
				g.DrawLine(pen, x + width / 2, y + height / 2, x + width / 2, y + height);
			}
			else if (lineType == 't')
			{
				g.DrawLine(pen, x + width / 2, y, x + width / 2, y + height);
				g.DrawLine(pen, x + width / 2, y + height / 2, x + width, y + height / 2);
			}
			else if (lineType == 'l')
			{
				g.DrawLine(pen, x + width / 2, y, x + width / 2, y + height / 2);
				g.DrawLine(pen, x + width / 2, y + height / 2, x + width, y + height / 2);
			}
			else if (lineType == '-')
			{
				g.DrawLine(pen, x + width / 2, y + height / 2, x + width, y + height / 2);
			}
			if (image != null)
			{
				int num = Math.Min(image.Width, width);
				int num2 = Math.Min(image.Height, height);
				g.DrawImage(image, x + (width - num + 1) / 2, y + (height - num2 + 1) / 2, num, num2);
			}
			pen.Dispose();
		}

		private void UpdatePreview()
		{
			global::System.Drawing.Image expandImage = this._imageInfo.ExpandImage;
			if (expandImage == null)
			{
				expandImage = this.DefaultPlusImage;
			}
			global::System.Drawing.Image collapseImage = this._imageInfo.CollapseImage;
			if (collapseImage == null)
			{
				collapseImage = this.DefaultMinusImage;
			}
			global::System.Drawing.Image noExpandImage = this._imageInfo.NoExpandImage;
			int width = this._imageInfo.Width;
			if (width < 1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidValue", new object[] { "Width" }));
				return;
			}
			int height = this._imageInfo.Height;
			if (height < 1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidValue", new object[] { "Height" }));
				return;
			}
			int lineWidth = this._imageInfo.LineWidth;
			if (lineWidth < 1)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("TreeViewImageGenerator_InvalidValue", new object[] { "LineWidth" }));
				return;
			}
			int lineStyle = (int)this._imageInfo.LineStyle;
			Color lineColor = this._imageInfo.LineColor;
			Font font = new Font("Tahoma", 10f);
			Graphics graphics = Graphics.FromHwnd(base.Handle);
			int num = width * 2 + (int)graphics.MeasureString(SR.GetString("TreeViewImageGenerator_SampleParent", new object[] { 1 }), font).Width;
			int num2 = Math.Max((int)graphics.MeasureString(SR.GetString("TreeViewImageGenerator_SampleParent", new object[] { 1 }), font).Height, height);
			graphics.Dispose();
			int num3 = num2 * 6;
			int num4 = Math.Max(width, this._treeView.NodeIndent);
			Bitmap bitmap = new Bitmap(Math.Max(num, this._previewPanel.Width), Math.Max(num3, this._previewPanel.Height));
			Graphics graphics2 = Graphics.FromImage(bitmap);
			int num5 = 5;
			int num6 = 5;
			graphics2.FillRectangle(Brushes.White, num5, num6, num, num3);
			this.RenderImage(graphics2, num5, num6, width, height, '-', lineStyle, lineWidth, lineColor, expandImage);
			num5 += width;
			graphics2.DrawString(SR.GetString("TreeViewImageGenerator_SampleRoot", new object[] { 1 }), font, Brushes.Black, (float)num5, (float)num6 + ((float)height - graphics2.MeasureString(SR.GetString("TreeViewImageGenerator_SampleRoot", new object[] { 1 }), font).Height + 1f) / 2f);
			num6 += num2;
			num5 -= width;
			this.RenderImage(graphics2, num5, num6, width, height, 'r', lineStyle, lineWidth, lineColor, collapseImage);
			num5 += width;
			graphics2.DrawString(SR.GetString("TreeViewImageGenerator_SampleRoot", new object[] { 2 }), font, Brushes.Black, (float)num5, (float)num6 + ((float)height - graphics2.MeasureString(SR.GetString("TreeViewImageGenerator_SampleRoot", new object[] { 2 }), font).Height + 1f) / 2f);
			num6 += num2;
			num5 -= width;
			this.RenderImage(graphics2, num5, num6, width, height, 'i', lineStyle, lineWidth, lineColor, null);
			num5 += num4;
			this.RenderImage(graphics2, num5, num6, width, height, 't', lineStyle, lineWidth, lineColor, expandImage);
			num5 += width;
			graphics2.DrawString(SR.GetString("TreeViewImageGenerator_SampleParent", new object[] { 1 }), font, Brushes.Black, (float)num5, (float)num6 + ((float)height - graphics2.MeasureString(SR.GetString("TreeViewImageGenerator_SampleParent", new object[] { 1 }), font).Height + 1f) / 2f);
			num6 += num2;
			num5 -= width + num4;
			this.RenderImage(graphics2, num5, num6, width, height, 'i', lineStyle, lineWidth, lineColor, null);
			num5 += num4;
			this.RenderImage(graphics2, num5, num6, width, height, 't', lineStyle, lineWidth, lineColor, noExpandImage);
			num5 += width;
			graphics2.DrawString(SR.GetString("TreeViewImageGenerator_SampleLeaf", new object[] { 1 }), font, Brushes.Black, (float)num5, (float)num6 + ((float)height - graphics2.MeasureString(SR.GetString("TreeViewImageGenerator_SampleLeaf", new object[] { 1 }), font).Height + 1f) / 2f);
			num6 += num2;
			num5 -= width + num4;
			this.RenderImage(graphics2, num5, num6, width, height, 'i', lineStyle, lineWidth, lineColor, null);
			num5 += num4;
			this.RenderImage(graphics2, num5, num6, width, height, 'l', lineStyle, lineWidth, lineColor, noExpandImage);
			num5 += width;
			graphics2.DrawString(SR.GetString("TreeViewImageGenerator_SampleLeaf", new object[] { 2 }), font, Brushes.Black, (float)num5, (float)num6 + ((float)height - graphics2.MeasureString(SR.GetString("TreeViewImageGenerator_SampleLeaf", new object[] { 2 }), font).Height + 1f) / 2f);
			num6 += num2;
			num5 -= width + num4;
			this.RenderImage(graphics2, num5, num6, width, height, 'l', lineStyle, lineWidth, lineColor, expandImage);
			num5 += width;
			graphics2.DrawString(SR.GetString("TreeViewImageGenerator_SampleRoot", new object[] { 3 }), font, Brushes.Black, (float)num5, (float)num6 + ((float)height - graphics2.MeasureString(SR.GetString("TreeViewImageGenerator_SampleRoot", new object[] { 3 }), font).Height + 1f) / 2f);
			graphics2.Dispose();
			bitmap.MakeTransparent(this._imageInfo.TransparentColor);
			this._previewPictureBox.Image = bitmap;
			this._previewPictureBox.Width = Math.Max(num, this._previewPanel.Width);
			this._previewPictureBox.Height = Math.Max(num3, this._previewPanel.Height);
		}

		private unsafe static global::System.Drawing.Image ReduceColors(Bitmap bitmap, int maxColors, int numBits, Color transparentColor)
		{
			if (numBits < 3 || numBits > 8)
			{
				throw new ArgumentOutOfRangeException("numBits");
			}
			if (maxColors < 16)
			{
				throw new ArgumentOutOfRangeException("maxColors");
			}
			int width = bitmap.Width;
			int height = bitmap.Height;
			TreeViewImageGenerator.Octree octree = new TreeViewImageGenerator.Octree(maxColors, numBits, transparentColor);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					octree.AddColor(bitmap.GetPixel(i, j));
				}
			}
			TreeViewImageGenerator.ColorIndexTable colorIndexTable = octree.GetColorIndexTable();
			Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			ColorPalette palette = bitmap2.Palette;
			Rectangle rectangle = new Rectangle(0, 0, width, height);
			BitmapData bitmapData = bitmap2.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			IntPtr scan = bitmapData.Scan0;
			byte* ptr;
			if (bitmapData.Stride > 0)
			{
				ptr = (byte*)scan.ToPointer();
			}
			else
			{
				ptr = (byte*)scan.ToPointer() + (IntPtr)bitmapData.Stride * (IntPtr)(height - 1);
			}
			int num = Math.Abs(bitmapData.Stride);
			for (int k = 0; k < height; k++)
			{
				for (int l = 0; l < width; l++)
				{
					byte* ptr2 = ptr + (IntPtr)k * (IntPtr)num + l;
					Color pixel = bitmap.GetPixel(l, k);
					byte b = (byte)colorIndexTable[pixel];
					*ptr2 = b;
				}
			}
			colorIndexTable.CopyToColorPalette(palette);
			bitmap2.Palette = palette;
			bitmap2.UnlockBits(bitmapData);
			return bitmap2;
		}

		private bool SaveTransparentGif(Bitmap bitmap, IFolderProjectItem folder, string name, ref bool overwrite)
		{
			global::System.Drawing.Image image = TreeViewImageGenerator.ReduceColors(bitmap, 256, 5, this._imageInfo.TransparentColor);
			bool flag = false;
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				image.Save(memoryStream, ImageFormat.Gif);
				memoryStream.Flush();
				memoryStream.Capacity = (int)memoryStream.Length;
				folder.AddDocument(name, memoryStream.GetBuffer());
			}
			finally
			{
				image.Dispose();
			}
			return flag;
		}

		private static global::System.Drawing.Image defaultMinusImage;

		private static global::System.Drawing.Image defaultPlusImage;

		private global::System.Web.UI.WebControls.TreeView _treeView;

		private PictureBox _previewPictureBox;

		private global::System.Windows.Forms.TextBox _previewFrameTextBox;

		private global::System.Windows.Forms.Label _previewLabel;

		private global::System.Windows.Forms.Panel _previewPanel;

		private global::System.Windows.Forms.Button _okButton;

		private global::System.Windows.Forms.Button _cancelButton;

		private global::System.Windows.Forms.Label _folderNameLabel;

		private global::System.Windows.Forms.Label _propertiesLabel;

		private PropertyGrid _propertyGrid;

		private global::System.Windows.Forms.TextBox _folderNameTextBox;

		private ProgressBar _progressBar;

		private global::System.Windows.Forms.Label _progressBarLabel;

		private TreeViewImageGenerator.LineImageInfo _imageInfo;

		private enum LineStyle
		{
			Dotted,
			Dashed,
			Solid
		}

		private class LineImageInfo
		{
			public LineImageInfo()
			{
				this._height = 20;
				this._width = 19;
				this._lineWidth = 1;
				this._lineStyle = TreeViewImageGenerator.LineStyle.Dotted;
				this._lineColor = Color.Black;
				this._transparentColor = Color.Magenta;
			}

			[SRDescription("TreeViewImageGenerator_CollapseImage")]
			[DefaultValue(null)]
			public global::System.Drawing.Image CollapseImage
			{
				get
				{
					return this._collapseImage;
				}
				set
				{
					this._collapseImage = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_ExpandImage")]
			[DefaultValue(null)]
			public global::System.Drawing.Image ExpandImage
			{
				get
				{
					return this._expandImage;
				}
				set
				{
					this._expandImage = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_LineColor")]
			public Color LineColor
			{
				get
				{
					return this._lineColor;
				}
				set
				{
					this._lineColor = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_LineStyle")]
			public TreeViewImageGenerator.LineStyle LineStyle
			{
				get
				{
					return this._lineStyle;
				}
				set
				{
					this._lineStyle = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_LineWidth")]
			public int LineWidth
			{
				get
				{
					return this._lineWidth;
				}
				set
				{
					if (value > 300)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this._lineWidth = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_LineImageHeight")]
			public int Height
			{
				get
				{
					return this._height;
				}
				set
				{
					if (value > 300)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this._height = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_NoExpandImage")]
			[DefaultValue(null)]
			public global::System.Drawing.Image NoExpandImage
			{
				get
				{
					return this._noExpandImage;
				}
				set
				{
					this._noExpandImage = value;
				}
			}

			[DefaultValue(typeof(Color), "Magenta")]
			[SRDescription("TreeViewImageGenerator_TransparentColor")]
			public Color TransparentColor
			{
				get
				{
					return this._transparentColor;
				}
				set
				{
					this._transparentColor = value;
				}
			}

			[SRDescription("TreeViewImageGenerator_LineImageWidth")]
			public int Width
			{
				get
				{
					return this._width;
				}
				set
				{
					if (value > 300)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this._width = value;
				}
			}

			private const int MaxSize = 300;

			private int _height;

			private int _width;

			private int _lineWidth;

			private TreeViewImageGenerator.LineStyle _lineStyle;

			private Color _lineColor;

			private Color _transparentColor;

			private global::System.Drawing.Image _collapseImage;

			private global::System.Drawing.Image _expandImage;

			private global::System.Drawing.Image _noExpandImage;
		}

		private class Octree
		{
			public Octree(int maxColors, int numBits, Color transparentColor)
			{
				this._root = new TreeViewImageGenerator.OctreeNode();
				this._maxColors = maxColors;
				this._leafNodes = new ArrayList();
				this._numBits = numBits;
				this._transparentColor = transparentColor;
				if (!this._transparentColor.IsEmpty)
				{
					this._hasTransparency = true;
					this._maxColors--;
				}
				this._levels = new ArrayList[this._numBits - 1];
				for (int i = 0; i < this._levels.Length; i++)
				{
					this._levels[i] = new ArrayList();
				}
			}

			public void AddColor(Color c)
			{
				if (this._hasTransparency && this._transparentColor.R == c.R && this._transparentColor.G == c.G && this._transparentColor.B == c.B)
				{
					return;
				}
				int i = -1;
				if (this._leafNodes.Count >= this._maxColors)
				{
					TreeViewImageGenerator.OctreeNode octreeNode = null;
					for (int j = this._numBits - 2; j > 0; j--)
					{
						ArrayList arrayList = this._levels[j];
						if (arrayList.Count > 0)
						{
							i = j;
							int num = -1;
							for (int k = 0; k < arrayList.Count; k++)
							{
								TreeViewImageGenerator.OctreeNode octreeNode2 = (TreeViewImageGenerator.OctreeNode)arrayList[k];
								if (octreeNode2.PixelCount > num)
								{
									octreeNode = octreeNode2;
									num = octreeNode2.PixelCount;
								}
							}
							break;
						}
					}
					this.ReduceNode(octreeNode, i);
					this._leafNodes.Add(octreeNode);
				}
				TreeViewImageGenerator.OctreeNode octreeNode3 = this._root;
				i = 0;
				bool flag = false;
				while (i < this._numBits - 1)
				{
					int index = this.GetIndex(c, i);
					TreeViewImageGenerator.OctreeNode octreeNode4 = octreeNode3[index];
					if (octreeNode4 == null)
					{
						octreeNode4 = new TreeViewImageGenerator.OctreeNode();
						octreeNode3[index] = octreeNode4;
						flag = true;
						if (octreeNode3.NodeCount == 2)
						{
							this._levels[i].Add(octreeNode3);
						}
					}
					octreeNode3 = octreeNode4;
					octreeNode3.AddColor(c);
					if (octreeNode3.Reduced)
					{
						break;
					}
					i++;
				}
				if (flag)
				{
					this._leafNodes.Add(octreeNode3);
				}
			}

			public TreeViewImageGenerator.ColorIndexTable GetColorIndexTable()
			{
				Hashtable hashtable = new Hashtable();
				int maxColors = this._maxColors;
				Color[] array = new Color[maxColors];
				int num = 0;
				if (!this._transparentColor.IsEmpty)
				{
					hashtable[TreeViewImageGenerator.ColorIndexTable.GetColorKey(this._transparentColor)] = 0;
					array[0] = Color.FromArgb(0, this._transparentColor);
					num = 1;
				}
				foreach (object obj in this._leafNodes)
				{
					TreeViewImageGenerator.OctreeNode octreeNode = (TreeViewImageGenerator.OctreeNode)obj;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					foreach (object obj2 in octreeNode.Colors)
					{
						Color color = (Color)obj2;
						int colorKey = TreeViewImageGenerator.ColorIndexTable.GetColorKey(color);
						hashtable[colorKey] = num;
						num2 += (int)color.R;
						num3 += (int)color.G;
						num4 += (int)color.B;
					}
					int count = octreeNode.Colors.Count;
					array[num] = Color.FromArgb(255, num2 / count, num3 / count, num4 / count);
					num++;
				}
				return new TreeViewImageGenerator.ColorIndexTable(hashtable, array);
			}

			private void ReduceNode(TreeViewImageGenerator.OctreeNode node, int depth)
			{
				ArrayList arrayList = null;
				if (depth < this._numBits - 2)
				{
					arrayList = this._levels[depth + 1];
				}
				for (int i = 0; i < 8; i++)
				{
					TreeViewImageGenerator.OctreeNode octreeNode = node[i];
					if (octreeNode != null)
					{
						if (depth < this._numBits - 2)
						{
							this.ReduceNode(octreeNode, depth + 1);
						}
						if (arrayList != null)
						{
							arrayList.Remove(octreeNode);
						}
						if (octreeNode.NodeCount == 0)
						{
							this._leafNodes.Remove(octreeNode);
						}
						node[i] = null;
					}
					this._levels[depth].Remove(node);
					node.Reduced = true;
				}
			}

			private int GetIndex(Color c, int depth)
			{
				int num = 7 - depth;
				return (((c.R >> num) & 1) << 2) | (((c.G >> num) & 1) << 1) | ((c.B >> num) & 1);
			}

			private TreeViewImageGenerator.OctreeNode _root;

			private ArrayList _leafNodes;

			private int _maxColors;

			private int _numBits;

			private Color _transparentColor;

			private bool _hasTransparency;

			private ArrayList[] _levels;
		}

		private class OctreeNode
		{
			public OctreeNode()
			{
				this._nodes = new TreeViewImageGenerator.OctreeNode[8];
				this._colors = new ArrayList();
				this._nodeCount = 0;
				this._reduced = false;
			}

			public ICollection Colors
			{
				get
				{
					return this._colors;
				}
			}

			public int NodeCount
			{
				get
				{
					return this._nodeCount;
				}
			}

			public int PixelCount
			{
				get
				{
					return this._colors.Count;
				}
			}

			public bool Reduced
			{
				get
				{
					return this._reduced;
				}
				set
				{
					this._reduced = value;
				}
			}

			public TreeViewImageGenerator.OctreeNode this[int index]
			{
				get
				{
					return this._nodes[index];
				}
				set
				{
					this._nodes[index] = value;
					if (this._nodes[index] == null)
					{
						this._nodeCount--;
						return;
					}
					this._nodeCount++;
				}
			}

			public void AddColor(Color c)
			{
				this._colors.Add(c);
			}

			private TreeViewImageGenerator.OctreeNode[] _nodes;

			private ArrayList _colors;

			private int _nodeCount;

			private bool _reduced;
		}

		private class ColorIndexTable
		{
			internal ColorIndexTable(IDictionary table, Color[] colors)
			{
				this._table = table;
				this._colors = colors;
			}

			public int this[Color c]
			{
				get
				{
					object obj = this._table[TreeViewImageGenerator.ColorIndexTable.GetColorKey(c)];
					if (obj == null)
					{
						return 0;
					}
					return (int)obj;
				}
			}

			public void CopyToColorPalette(ColorPalette palette)
			{
				for (int i = 0; i < this._colors.Length; i++)
				{
					palette.Entries[i] = this._colors[i];
				}
			}

			internal static int GetColorKey(Color c)
			{
				return ((int)(c.R & byte.MaxValue) << 16) | ((int)(c.G & byte.MaxValue) << 8) | (int)(c.B & byte.MaxValue);
			}

			private IDictionary _table;

			private Color[] _colors;
		}
	}
}
